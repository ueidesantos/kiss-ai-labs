# Esclarecimentos

| Questão | Decisão |
|---|---|
| Demanda arbitrária? | Não; simulador fixo Padrão10/VIP20. |
| O que o desenvolvedor gera? | Propostas JSON de plano/textos/política, materializadas por templates. |
| Claude e Codex integrados? | Não; exemplo conceitual. Implementação usa dois modelos Ollama distintos. |
| Nomes diferentes bastam? | Não; digests também devem diferir para rejeitar aliases. |
| Dois modelos garantem independência? | Não. Podem compartilhar vieses; testes continuam obrigatórios. |
| Como a revisão bloqueia? | Achado high/critical ou teste reprovado fecha gate. |
| Quantas correções? | Uma Back, seguida de build/testes e nova revisão. Persistência encerra reprovado. |
| Offline usa IA? | Não. Propostas e revisões preparadas; testes executados de verdade. |
| RAG usa embeddings? | Não; TF-IDF e cosseno lexical reutilizados do lab 07. |
| Figma obrigatório? | Não; somente referência complementar, sem alegar captura. |
| Entrega em main? | Lab em main; candidata em draft PR opt-in separado. |
| Qual capa? | Pedido mais recente: lab 09 principal, lab 08 secundário; preservar cenário. |
