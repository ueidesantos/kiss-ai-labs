# Requires PowerShell 7 and .NET 11. No Ollama or downloaded models needed.
$ErrorActionPreference = 'Stop'
$repo = Split-Path (Split-Path $PSScriptRoot -Parent) -Parent
$project = Join-Path $repo '04-rag-top-k/src/RagTopK.csproj'
$dll = Join-Path $repo '04-rag-top-k/src/bin/Debug/net11.0/RagTopK.dll'
function Assert($condition, $message) { if (-not $condition) { throw $message } }
& dotnet build $project --nologo
Assert ($LASTEXITCODE -eq 0) 'Build failed.'

$portProbe = [System.Net.Sockets.TcpListener]::new([System.Net.IPAddress]::Loopback, 0)
$portProbe.Start()
$port = $portProbe.LocalEndpoint.Port
$portProbe.Stop()
$url = "http://127.0.0.1:$port"
$temp = Join-Path ([System.IO.Path]::GetTempPath()) ('kiss-top-k-' + [guid]::NewGuid())
New-Item -ItemType Directory -Path $temp | Out-Null
$job = $null
Push-Location $repo
try {
    # Scores intentionally differ; file order differs from semantic order.
    Set-Content (Join-Path $temp 'a.md') 'irrelevant'
    Set-Content (Join-Path $temp 'b.md') 'credentials'
    Set-Content (Join-Path $temp 'c.md') 'related'
    $job = Start-Job -ArgumentList $url -ScriptBlock {
        param($url)
        $listener = [System.Net.HttpListener]::new()
        $listener.Prefixes.Add("$url/")
        $listener.Start()
        'READY'
        try {
            for ($i = 0; $i -lt 7; $i++) {
                $pending = $listener.GetContextAsync()
                $deadline = [DateTime]::UtcNow.AddSeconds(30)
                while (-not $pending.IsCompleted) {
                    if ([DateTime]::UtcNow -gt $deadline) { throw "Mock request timeout." }
                    Start-Sleep -Milliseconds 50
                }
                $context = $pending.GetAwaiter().GetResult()
                $reader = [System.IO.StreamReader]::new($context.Request.InputStream)
                $body = $reader.ReadToEnd() | ConvertFrom-Json
                $reader.Dispose()
                $path = $context.Request.Url.AbsolutePath
                if ($path -eq '/api/embed') {
                    $vector = if ($body.input -match 'irrelevant') { @(0, 1) }
                              elseif ($body.input -match 'related') { @(1, 1) }
                              else { @(1, 0) }
                    $response = @{ embeddings = @(,$vector) } | ConvertTo-Json -Depth 5 -Compress
                } else {
                    $response = '{"response":"Use environment variables [S1].","done":true}' + "`n"
                }
                $bytes = [System.Text.Encoding]::UTF8.GetBytes($response)
                $context.Response.ContentType = 'application/json'
                $context.Response.ContentLength64 = $bytes.Length
                $context.Response.OutputStream.Write($bytes, 0, $bytes.Length)
                $context.Response.Close()
                @{ Path = $path; Body = $body } | ConvertTo-Json -Depth 5 -Compress
            }
            Start-Sleep -Milliseconds 500
        } finally { $listener.Close() }
    }
    $ready = $false
    for ($attempt = 0; $attempt -lt 100; $attempt++) {
        if (@(Receive-Job $job -Keep) -contains 'READY') { $ready = $true; break }
        Start-Sleep -Milliseconds 100
    }
    Assert $ready 'Mock server did not start.'
    $output = (& dotnet $dll --docs $temp --question credentials --compare '3,1,5,1' --url $url 2>&1) -join "`n"
    Assert ($LASTEXITCODE -eq 0) $output
    $records = @(Receive-Job $job -Wait | Where-Object { $_ -ne 'READY' } | ForEach-Object { $_ | ConvertFrom-Json })
    $embeds = @($records | Where-Object Path -eq '/api/embed')
    $generations = @($records | Where-Object Path -eq '/api/generate')
    Assert ($embeds.Count -eq 4) 'Embeddings were not reused across rounds.'
    Assert ($generations.Count -eq 3) 'Expected one generation per distinct K.'
    $counts = @(3, 1, 3)
    for ($i = 0; $i -lt 3; $i++) {
        $prompt = $generations[$i].Body.prompt
        $sources = [regex]::Matches($prompt, '\[S\d+\] \[Documento: ([^\]]+)\]')
        Assert ($sources.Count -eq $counts[$i]) "Wrong source count in round $i."
        Assert ($sources[0].Groups[1].Value -eq 'b.md') 'Best match is not first.'
        if ($counts[$i] -eq 3) {
            Assert ($sources[1].Groups[1].Value -eq 'c.md') 'Second match is incorrect.'
            Assert ($sources[2].Groups[1].Value -eq 'a.md') 'Third match is incorrect.'
        }
        $sentContext = ($prompt -split "Contexto:`n", 2)[1] -split "`n`nPergunta:", 2
        Assert ($output.Contains($sentContext[0].Replace("`r`n", "`n"))) 'Printed context differs from generated context.'
        Assert ($generations[$i].Body.options.temperature -eq 0) 'Generation temperature changed.'
    }
    Assert ($output.Contains('Top-K solicitado: 5; documentos recuperados: 3')) 'K above document count not reported.'
    $invalid = @(
        @('--top-k', '0'), @('--top-k', '-1'), @('--top-k', 'abc'),
        @('--compare', '1,,3'), @('--compare', '1,0'), @('--top-k'),
        @('--top-k', '1', '--compare', '1,3'), @('--url', 'invalid'),
        @('--unknown', 'value'), @('--top-k', '1', '--top-k', '2'),
        @('--docs', (Join-Path $temp 'missing'))
    )
    foreach ($case in $invalid) {
        $result = & dotnet $dll @case 2>&1
        Assert ($LASTEXITCODE -eq 1) "Expected exit code 1 for: $case"
        Assert (($result -join "`n") -notmatch 'Unhandled exception') 'Error was not handled.'
    }
    $empty = Join-Path $temp 'empty'
    New-Item -ItemType Directory -Path $empty | Out-Null
    $null = & dotnet $dll --docs $empty 2>&1
    Assert ($LASTEXITCODE -eq 1) 'Empty folder should fail.'
    $null = & dotnet $dll --help
    Assert ($LASTEXITCODE -eq 0) 'Help should succeed.'
    Write-Output 'PASS: ranking, source IDs, printed context, embedding reuse, K limits and 12 error cases.'
} finally {
    Pop-Location
    if ($job) { Stop-Job $job; Remove-Job $job -Force }
    # Only remove the unique temporary directory created by this script.
    $resolvedTemp = [System.IO.Path]::GetFullPath($temp)
    $tempRoot = [System.IO.Path]::GetFullPath([System.IO.Path]::GetTempPath())
    if ($resolvedTemp.StartsWith($tempRoot, [System.StringComparison]::OrdinalIgnoreCase) -and
        (Split-Path $resolvedTemp -Leaf).StartsWith('kiss-top-k-')) {
        Remove-Item -LiteralPath $resolvedTemp -Recurse -Force
    }
}
