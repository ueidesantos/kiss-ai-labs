# Lab 09 — KISS AI Squad: uma IA desenvolve, outra revisa

Demanda aprovada: clientes Padrão podem simular até 10% de desconto; VIP até 20%. Valor positivo até R$ 1 bilhão, dinheiro decimal e arredondamento AwayFromZero. Sem autenticação: perfil informado para simulação, sem cobrança ou persistência de clientes.

## Requisitos e aceite

- **REQ-1 — Dois modelos:** CLI .NET 11 executa TL, Front e Back no modelo desenvolvedor (`--model`) e revisão adversarial no revisor (`--review-model`), via Ollama. No modo real, modelos devem ter nomes e digests distintos; aliases do mesmo modelo são recusados. Offline identifica propostas e revisão preparadas, sem chamadas de IA.
- **REQ-2 — Contexto:** RAG lexical local recupera documentação de negócio com caminho, trecho e SHA256. Documentos são dados, não instruções de execução.
- **REQ-3 — Escopo controlado:** propostas JSON delimitadas alimentam templates conhecidos. Não executar texto arbitrário retornado pelo LLM. Explicar que o desenvolvedor propõe textos e limites, não código livre.
- **REQ-4 — Gate independente:** testes determinísticos protegem limites, entradas inválidas e dinheiro. Revisor não pode alterar testes ou superar reprovações. Achados `high`/`critical` bloqueiam aprovação. No máximo uma correção Back seguida de build, testes e nova revisão; nova falha encerra com reprovação.
- **REQ-5 — Evidência:** gerar aplicação ASP.NET Core funcional, plano, SPEC candidata, propostas, diff, build, testes, revisões e eventos. Registrar identidades dos modelos, duração e tokens quando disponíveis. Painel diferencia ausência de relatório, execução e resultado.
- **REQ-6 — Demo honesta:** offline prepara proposta errada Padrão=20%; testes reais detectam a violação. Uma correção preparada para 10% passa pelos mesmos gates. Revisões offline são rotuladas como preparadas; não alegar descoberta autônoma de IA.
- **REQ-7 — Entrega revisável:** script opt-in verifica gate aprovado e manifesto de integridade, cria branch e draft PR; nenhum merge automático. Publicar o laboratório em main é distinto de criar um PR com uma candidata.
- **REQ-8 — Apresentação:** lab 09 passa a ser o destaque principal da capa, preservando a cena original e lab 08 como projeto anterior. Figma é referência opcional por link; não simular captura real.

## Revisão adversarial

O revisor recebe requisitos, fontes, proposta e resultados da candidata. Procura contradições de regra, validação e experiência de uso. Cada achado contém identificação, severidade, evidência e correção sugerida. A revisão é um segundo gate: a ausência de achados graves não substitui testes. Erro de transporte, resposta inválida ou identidade de modelo não verificável falha de forma explícita; nunca aciona offline automaticamente.

“Claude desenvolve, Codex revisa” ilustra a separação desejada. Esta implementação usa **dois modelos distintos disponíveis no Ollama**, não integra as APIs comerciais Claude ou Codex. Nomes diferentes não garantem independência estatística nem ausência de vieses; a verificação de digest apenas evita usar aliases da mesma instalação.

## Arquitetura e critérios de verificação

Console runner + domínio + app de referência + testes determinísticos + painel estático. Fluxo sequencial delimitado, demanda fixa, uma correção. Propostas são dados tipados; não é um agente de programação de propósito geral.

Validar build, demo com reprovação inicial/correção/nova revisão, bloqueio com revisão grave, modelos iguais/aliases recusados, resposta inválida, integridade da publicação e simulador. Registrar separadamente integração real e testes com servidor preparado: uma resposta HTTP simulada não comprova raciocínio de modelos reais.
