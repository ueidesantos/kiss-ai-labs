# Requires PowerShell 7 and .NET 11. No Ollama or downloaded models needed.
$ErrorActionPreference = 'Stop'
$repo = Split-Path (Split-Path $PSScriptRoot -Parent) -Parent
$project = Join-Path $repo '05-rag-source-code/src/RagSourceCode.csproj'
$dll = Join-Path $repo '05-rag-source-code/src/bin/Debug/net11.0/RagSourceCode.dll'
function Assert($condition, $message) { if (-not $condition) { throw $message } }
& dotnet build $project --nologo
Assert ($LASTEXITCODE -eq 0) 'Build failed.'

$portProbe = [System.Net.Sockets.TcpListener]::new([System.Net.IPAddress]::Loopback, 0)
$portProbe.Start()
$port = $portProbe.LocalEndpoint.Port
$portProbe.Stop()
$url = "http://127.0.0.1:$port"
$temp = Join-Path ([System.IO.Path]::GetTempPath()) ('kiss-source-' + [guid]::NewGuid())
New-Item -ItemType Directory -Path $temp | Out-Null
$job = $null
Push-Location $repo
try {
    New-Item -ItemType Directory -Path (Join-Path $temp 'nested'), (Join-Path $temp 'bin'), (Join-Path $temp 'obj'), (Join-Path $temp '.git') | Out-Null
    foreach ($excluded in @('bin','obj','.git')) { Set-Content (Join-Path $temp "$excluded/Generated.cs") 'credentials' }
    Set-Content (Join-Path $temp 'notes.md') 'credentials'
    Set-Content (Join-Path $temp 'a.cs') 'irrelevant'
    Set-Content (Join-Path $temp 'b.cs') 'credentials'
    Set-Content (Join-Path $temp 'nested/c.CS') 'related'
    $job = Start-Job -ArgumentList $url -ScriptBlock {
        param($url)
        $listener = [System.Net.HttpListener]::new()
        $listener.Prefixes.Add("$url/")
        $listener.Start()
        'READY'
        try {
            for ($i = 0; $i -lt 5; $i++) {
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
    $output = (& dotnet $dll --code $temp --question credentials --top-k 5 --cpu --url $url 2>&1) -join "`n"
    Assert ($LASTEXITCODE -eq 0) $output
    $records = @(Receive-Job $job -Wait | Where-Object { $_ -ne 'READY' } | ForEach-Object { $_ | ConvertFrom-Json })
    $embeds = @($records | Where-Object Path -eq '/api/embed')
    $generations = @($records | Where-Object Path -eq '/api/generate')
    Assert ($embeds.Count -eq 4) 'Embeddings were not reused across rounds.'
    Assert ($generations.Count -eq 1) 'Expected one generation.'
    foreach ($record in $records) { Assert ($record.Body.options.num_gpu -eq 0) 'CPU mode not sent to Ollama.' }
    $counts = @(3)
    for ($i = 0; $i -lt 1; $i++) {
        $prompt = $generations[$i].Body.prompt
        $sources = [regex]::Matches($prompt, '\[S\d+\] \[Arquivo: ([^\]]+):1-1\]')
        Assert ($sources.Count -eq $counts[$i]) "Wrong source count in round $i."
        Assert ($sources[0].Groups[1].Value -eq 'b.cs') 'Best match is not first.'
        if ($counts[$i] -eq 3) {
            Assert ($sources[1].Groups[1].Value -eq (Join-Path 'nested' 'c.CS')) 'Second match is incorrect.'
            Assert ($sources[2].Groups[1].Value -eq 'a.cs') 'Third match is incorrect.'
        }
        $sentContext = ($prompt -split "Contexto:`n", 2)[1] -split "`n`nPergunta:", 2
        Assert ($output.Contains($sentContext[0].Replace("`r`n", "`n"))) 'Printed context differs from generated context.'
        Assert ($generations[$i].Body.options.temperature -eq 0) 'Generation temperature changed.'
    }
    Assert ($output.Contains('Top-K solicitado: 5; documentos recuperados: 3')) 'K above document count not reported.'
    $invalid = @(
        @('--top-k', '0'), @('--top-k', '-1'), @('--top-k', 'abc'),
        @('--question', ''), @('--model'), @('--top-k'),
        @('--compare', '1,3'), @('--url', 'invalid'),
        @('--unknown', 'value'), @('--top-k', '1', '--top-k', '2'),
        @('--code', (Join-Path $temp 'missing'))
    )
    foreach ($case in $invalid) {
        $result = & dotnet $dll @case 2>&1
        Assert ($LASTEXITCODE -eq 1) "Expected exit code 1 for: $case"
        Assert (($result -join "`n") -notmatch 'Unhandled exception') 'Error was not handled.'
    }
    $empty = Join-Path $temp 'empty'
    New-Item -ItemType Directory -Path $empty | Out-Null
    $null = & dotnet $dll --code $empty 2>&1
    Assert ($LASTEXITCODE -eq 1) 'Empty folder should fail.'
    $null = & dotnet $dll --help
    Assert ($LASTEXITCODE -eq 0) 'Help should succeed.'
    Write-Output 'PASS: recursive .cs discovery, generated-folder exclusions, ranking, line references, context, K limits and 12 error cases.'
} finally {
    Pop-Location
    if ($job) { Stop-Job $job; Remove-Job $job -Force }
    # Only remove the unique temporary directory created by this script.
    $resolvedTemp = [System.IO.Path]::GetFullPath($temp)
    $tempRoot = [System.IO.Path]::GetFullPath([System.IO.Path]::GetTempPath())
    if ($resolvedTemp.StartsWith($tempRoot, [System.StringComparison]::OrdinalIgnoreCase) -and
        (Split-Path $resolvedTemp -Leaf).StartsWith('kiss-source-')) {
        Remove-Item -LiteralPath $resolvedTemp -Recurse -Force
    }
}
