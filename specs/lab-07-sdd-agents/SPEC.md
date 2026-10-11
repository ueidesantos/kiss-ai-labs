# SPEC — LAB-07: SDD + agents + RAG de negócio

- Estado: definida para implementação do exemplo didático solicitado.
- Coordenador/TL: agent principal; Dev Front e Dev Back: agents delegados.
- Demanda: simular o valor final de um pedido com desconto, com regras recuperadas da documentação de negócio.
- Domínio fictício: valores e regras abaixo pertencem somente a este exemplo.

## Objetivo e escopo

Uma página com valor do pedido, desconto e botão Simular; uma API que valida e calcula; um fluxo de agents que consulta documentos, prepara a SPEC e delega propostas de Front e Back. Sem login, banco, checkout, aprovação gerencial ou implantação automática.

## Fontes de negócio

- B1: `07-sdd-agents-business-rag/business/descontos.md`: pedido maior que zero, desconto de 0 a 20%, dinheiro arredondado a duas casas com HALF_UP.
- B2: `07-sdd-agents-business-rag/business/glossario.md`: valor original, percentual e valor final.
- B3: `07-sdd-agents-business-rag/business/simulacao.md`: apenas simulação; nenhuma gravação ou cobrança.

## Requisitos e critérios de aceite

| ID | Requisito | Fonte | Critério de aceite |
|---|---|---|---|
| REQ-1 | Consultar a documentação de negócio antes de especificar | B1, B2, B3 | AC-1: recuperação local por TF-IDF/cosseno retorna trechos com IDs e caminhos |
| REQ-2 | TL produz SPEC e delega tarefas rastreáveis para Front e Back | B1, B3 | AC-2: plano JSON contém exatamente uma tarefa por papel, requisitos e critérios; referências fora do contexto são recusadas |
| REQ-3 | Front coleta valor e percentual, envia à API e mostra resultado/erro | B2, B3 | AC-3: formulário rotulado, estados aguardando/sucesso/erro, resultado em reais e contrato compartilhado |
| REQ-4 | Back valida e calcula com decimal do .NET | B1 | AC-4: 100 com 10% retorna 90; limites 0/20 aceitos; >20, negativos, bool, não finitos, campos inválidos recusados |
| REQ-5 | Devs recebem SPEC, contrato e somente a tarefa atribuída | SPEC | AC-5: duas chamadas de desenvolvimento com papéis separados; entregas com refs de requisitos e fontes |
| REQ-6 | Registrar evidências e distinguir IA real de replay offline | SPEC | AC-6: execução salva fontes, SPEC, tarefas e propostas; modo offline declara que usa respostas preparadas e não chamou modelo |

## Contrato aprovado para o exemplo

- `POST /api/discount`, JSON `{ "amount": 100, "discount": 10 }`.
- HTTP 200: `{ "original": "100.00", "discount": "10", "total": "90.00" }`. Valores monetários são strings decimais.
- HTTP 400: `{ "error": "mensagem legível" }` para dados inválidos.
- `GET /` entrega a página. Nenhuma entrada executa comandos nem grava pedidos.

## Design KISS e delegação

- .NET 11, ASP.NET Core, C# e bibliotecas do framework; HTML/CSS/JS sem build frontend. Sem pacotes NuGet externos.
- RAG lexical local com TF-IDF e cosseno sobre três Markdown inteiros, sem embeddings/banco.
- Agents: chamadas HTTP explícitas para Ollama com papéis TL, Dev Front e Dev Back; chamadas sequenciais, com contrato fixo.
- TL gera SPEC estruturada e duas tarefas; devs geram propostas de código e verificações. Propostas ficam em diretório próprio e não são executadas automaticamente.
- A aplicação de referência em `app/` é implementada pelos agents delegados nesta entrega, revisada e testada separadamente. Replay usa propostas dessa aplicação; não deve ser apresentado como geração de IA.
- SPEC JSON do fluxo é um artefato candidato; a fonte de verdade deste lab continua sendo este arquivo até revisão humana.

## Plano

| Responsável | Área | Requisitos | Validação |
|---|---|---|---|
| TL | documentação, RAG, workflow e registro de evidências | REQ-1, 2, 5, 6 | testes de ranking, contrato de delegação, recusas e replay |
| Dev Front | `app/index.html`, guia de UX/contrato | REQ-3 | revisão de HTML/JS e integração de payload/resultados |
| Dev Back | `app/DiscountApp.csproj`, cálculo e endpoint C# | REQ-4 | testes de cálculo/validação/contrato sem sockets |
| Revisor | integração e evidências | todos | revisar diff, conferir testes e apontar bloqueios |

## Gates e limites

Escopo e contrato definidos antes da implementação. Uma referência válida indica origem recuperada, não comprova que a afirmação gerada seja fiel. Revisão humana continua necessária antes de incorporar propostas. A execução real de Ollama e o servidor HTTP dependem de permissões de rede; se bloqueados, registrar a limitação sem declarar esses cenários aprovados. Não bloquear a demonstração offline por falta de modelos.
