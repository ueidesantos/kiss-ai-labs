# Esclarecimentos da demanda

[SPEC canônica](SPEC.md) registra o resultado aprovado para implementação.

| Tema | Decisão registrada |
|---|---|
| Usuário e necessidade | Atendente quer simular desconto antes de apresentar proposta |
| Stack | .NET 11, ASP.NET Core, C#; HTML/CSS/JS sem dependências |
| Domínio | Fictício; documentação deste lab não é política de uma empresa real |
| Limite | Pedido > 0; desconto de 0 a 20%, inclusive |
| Arredondamento | HALF_UP para dinheiro positivo, em duas casas |
| Responsabilidade | API aplica regra; Front coleta e apresenta; TL coordena e verifica |
| Agents runtime | Chamadas Ollama com papéis distintos; devs recebem só sua tarefa |
| Fonte de verdade | SPEC desta pasta; saída de modelo continua candidata |
| Ação comercial | Nenhuma cobrança, gravação de pedidos ou aprovação gerencial |
| Publicação | Coordenador tenta commit/push; resultado deve informar bloqueio real |

Não há ambiguidade de produto impeditiva do incremento. Há dependências operacionais pendentes: HTTP real, Ollama real e acesso ao GitHub. Elas não alteram critérios de aceite para fazer o trabalho parecer aprovado.
