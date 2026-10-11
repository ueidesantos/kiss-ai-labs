# Handoff — Dev Front

Papel: Dev Front / implementação de REQ-3 e AC-3, conforme `specs/lab-07-sdd-agents/SPEC.md`.

- `index.html`: HTML/CSS/JS vanilla, sem dependências ou build.
- Backend contratado: ASP.NET Core / .NET 11; o frontend usa somente HTTP/JSON.
- Campos rotulados, pedido positivo, desconto de 0 a 20%, mensagem com limite da política.
- POST `/api/discount` com `amount` e `discount` numéricos; valores retornados em strings são formatados em BRL. A API calcula e valida as regras, a página somente apresenta o resultado.
- Estados de cálculo, sucesso e erro; botão desabilitado durante a chamada, regiões de status e alerta, inserção de mensagens via `textContent`.
- Resultado anterior ocultado ao simular novamente; falhas de transporte/JSON recebem mensagem legível.

## Evidências

Validação executada com Node.js bundled em 2026-10-11:

1. Extração do `<script>` e compilação com `node:vm.Script`: `JavaScript syntax OK`.
2. Execução do script com DOM e `fetch` simulados: POST/payload numérico, formatação `90,00`, erro de API, rejeição de 21% sem chamada HTTP e restauração do botão passaram.
3. Teste reproduzível: `node --test tests/frontend.test.cjs`, executado na pasta do lab. Nove testes cobrem sintaxe/labels, payload/BRL, carregamento e proteção contra duplicação, erro da API, entradas inválidas, falhas de transporte/contrato e precisão monetária.
4. Correção de QA: `100000000000000.01` é rejeitado antes do fetch se a conversão para número JSON perde dígitos. A identidade decimal normalizada aceita notação científica e zeros finais equivalentes, sem teto monetário arbitrário. Respostas monetárias são formatadas diretamente das strings, preservando centavos de valores grandes. Campo amount permite qualquer incremento positivo; a API continua responsável pela regra e arredondamento.

Esta validação não executou um navegador nem o servidor real. Integração HTTP e revisão visual ficam com o coordenador/revisor; o Dev Front não aprovou o próprio trabalho.
