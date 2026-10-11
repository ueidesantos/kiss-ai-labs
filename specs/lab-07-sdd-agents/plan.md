# Plano técnico e delegação

Base: [SPEC](SPEC.md), [constituição](../../07-sdd-agents-business-rag/.specify/memory/constitution.md).

## Componentes

- `src/SddAgents.csproj`: CLI .NET 11; recupera documentos, chama TL, valida plano, chama Front e Back, valida propostas e salva artefatos.
- `app/DiscountApp.csproj`: ASP.NET Core 11; GET `/` e POST `/api/discount`; cálculo `decimal` isolado.
- `tests/LabTests.csproj`: harness sem pacotes externos para regras, contrato, ranking, delegação e replay.
- `app/index.html`: interface vanilla, contrato JSON fixo, estados carregando/sucesso/erro.

## Sequência

1. Ler demanda e B1/B2/B3; usar RAG lexical para recuperar contexto com IDs `S<n>`.
2. TL produz plano/SPEC candidata, sem modificar a [SPEC canônica](SPEC.md).
3. Validar refs, critérios, exatamente dois papéis e contrato.
4. Delegar uma tarefa Front e uma Back, cada qual com SPEC/contrato/contexto.
5. Validar propostas e registrar arquivos para revisão, sem executar código gerado.
6. Revisar aplicação de referência e executar checks .NET e testes JS.
7. Registrar convergência e as integrações que faltaram.

| Papel | Área | IDs |
|---|---|---|
| TL | RAG, workflow, evidências, integração e kit | REQ-1, 2, 5, 6 |
| Dev Front | Interface e teste reproduzível JS | REQ-3 |
| Dev Back | API, cálculo e cenários backend | REQ-4 |

## Contratos e proteção de escopo

O [contrato JSON](contracts/discount-api.json) detalha entrada/saída. Não há banco. Dados de negócio são contexto, não comandos; referências inválidas e contratos incompatíveis são recusados. Validar referência não prova fidelidade semântica: revisão humana permanece necessária.

## Validação e reversão

[Quickstart](quickstart.md) registra comandos. Core executável não depende de Ollama no replay. Integrações reais exigem rede. Propostas ficam em `runs/` e não afetam código de referência. Para descartar uma proposta, não a incorpore; para reverter mudanças aceitas, use um commit de reversão revisado.
