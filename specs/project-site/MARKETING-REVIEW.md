# Revisão adversarial — Marketing e Vendas

Data: 2026-10-11 (America/Sao_Paulo). Papéis: UX, comunicação e implementação frontend. Escopo: página pública KISS AI Labs, sem alterar laboratórios ou suas evidências. Objetivo de conversão: ajudar o visitante a escolher e abrir um exemplo relevante; ao final, conhecer a trajetória e contato do autor. Não foram inferidas ofertas de consultoria, clientes, depoimentos ou resultados comerciais.

## Achados e correções executadas

| Prioridade | Achado adversarial | Remediação | Requisito |
| --- | --- | --- | --- |
| Alta | Sobre mim interrompia a exploração dos labs e não estava na posição pedida. | Último item do menu, depois de GitHub; última seção de main, depois do histórico. | REQ-6 |
| Alta | A abertura listava tecnologias sem explicitar para quem serviam e qual problema resolviam. | Público .NET/Python e exemplos concretos: revisar diff, localizar regras, delegar demanda. | REQ-7 |
| Alta | Nove escolhas equivalentes aumentavam a dificuldade de iniciar; faltava uma jornada para quem já conhece IA. | Três entradas por problema, com links para 00, 03 e 07; avançar para 05/08 fica explícito na trilha. | REQ-8 |
| Alta | A linguagem do destaque podia sugerir integração Figma já validada. O card 07 omitia limite de execução real. | Destaque como proposta, evidências acessíveis, pendências Figma/Ollama/SQLite junto dos cards. | REQ-9 |
| Média | Siglas e dependências eram uma barreira para o visitante decidir. | Resumo expansível de RAG, MCP, SDD, Spec Kit, TL, SDK/Python/Ollama e demonstrações offline. | REQ-9 |
| Média | CTAs genéricos não explicavam o que abriria depois do clique. | “Escolher minha trilha”, “Ver código e especificação”, “Ver design no Figma”; catálogo avisa que abre GitHub e README. | REQ-10 |
| Média | A transição de conteúdo técnico para conhecer o autor tinha pouco contexto. | Sobre mim conecta os projetos às experiências profissionais; CTA para currículo e contato. | REQ-6, REQ-7 |
| Média | Compartilhamento sem metadados sociais não controlava título, resumo e capa. | Canonical, Open Graph, imagem/alt e cartão social; sem afirmações não verificadas. | REQ-10 |
| Média | O menu precisava tolerar larguras intermediárias ao mudar ordem e texto. | Quebra de linha no menu, grid de trilhas responsivo, ajuste de fluxo em telas estreitas. | REQ-4 |

Todos os achados acima foram corrigidos no escopo da página. Aumentos de conversão não foram medidos nem prometidos. Capa, cores, nove laboratórios e links originais foram preservados.

## Validação executada

- Playwright com Edge headless, arquivo local, viewports 320, 390, 768 e 1440 px: sem overflow horizontal; nove cards; três entradas; Sobre mim último link e última seção; todas as âncoras resolvem; imagem carregada.
- Primeiro foco de teclado: Pular para o conteúdo. Enter no resumo abre requisitos. Movimento reduzido: scroll-behavior auto. Nenhum erro de página.
- Screenshots completos em 390 e 1440 px inspecionados: hierarquia e espaçamento mantidos; os caminhos ficam empilhados em telas pequenas.
- Todos os links GitHub para arquivos/diretórios também foram conferidos contra o checkout local: nenhum destino faltante.
- Checagem HTTP com Node e certificados do sistema: 18 URLs responderam 200, incluindo site, currículo, código e documentação. Figma Community respondeu 403 à requisição automatizada; mantido o link fornecido e aprovado pelo usuário. Isso não valida nem invalida acesso humano ao design, e não é evidência de integração MCP real.
- A primeira checagem HTTP pelo request do Playwright encontrou erro da cadeia de certificados; repetida com `node --use-system-ca`, sem desabilitar a validação TLS.
- Deploy desta revisão: responsabilidade do coordenador; esta evidência local não declara nova versão publicada.
