# Decisões de implementação

## Agentes com saída delimitada

REQ-3 usa JSON para representar plano, textos e política. Templates conhecidos materializam a aplicação. Isso torna o diff pequeno e auditável e mantém os testes fora da influência do modelo. A contrapartida é explícita: não se trata de programação autônoma de propósito geral.

## RAG reutilizado

REQ-2 reutiliza a recuperação lexical do lab 07 sobre poucos Markdown. Não precisa de vector database, embeddings ou infraestrutura adicional. Caminho, trecho e SHA256 permitem relacionar o contexto usado à versão dos documentos. SHA256 comprova integridade, não veracidade da fonte.

## Testes antes da opinião do QA

REQ-4 separa avaliação determinística de comentário do modelo. Os testes verificam entradas, dinheiro e limites definidos na SPEC. O QA recebe evidências para explicar riscos; sua resposta não muda o resultado do gate.

## Uma correção e parada

Limitar a uma rodada torna custo e diagnóstico previsíveis. Nova falha encerra a execução. O limite não garante sucesso; impede que uma demonstração esconda tentativas ilimitadas.

## Demo e modelo real

REQ-6 torna o primeiro contato reproduzível sem Ollama. A proposta Padrão=20 é um erro preparado, identificado como tal. No modo real não se injeta esse erro nem se substitui indisponibilidade de modelo por resposta offline.

## PR separado

REQ-7 usa publicação opt-in em branch própria e draft PR. A evidência local não equivale a revisão humana e não autoriza merge automático.
