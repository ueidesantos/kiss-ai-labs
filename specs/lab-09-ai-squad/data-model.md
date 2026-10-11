# Modelo conceitual

| Dado | Conteúdo | Restrição |
|---|---|---|
| Demanda | Mudança fixa de limites por perfil | Não é prompt livre para executar comandos |
| Fonte | Caminho, trecho recuperado, SHA256 | Origem verificável; texto tratado como dado |
| Plano TL | Passos e distribuição do trabalho | Deve permanecer no escopo aprovado |
| Proposta Front | Textos da interface | Sem HTML ou script arbitrário executável |
| Proposta Back | Limites numéricos por perfil | Dados tipados; comportamento submetido aos testes |
| Revisão QA | Comentários sobre resultados e riscos | Não altera gate determinístico |
| Evento | Etapa, estado, instante, duração e modo | Offline explicitamente identificado |
| Uso de modelo | Nome e tokens quando disponíveis | Ausência não é contagem zero medida |
| Candidata | Aplicação materializada por templates | Separada da fonte canônica |
| Evidência | Build, testes, diff e estado final | Não presumir sucesso de etapas não executadas |

Dinheiro usa decimal com arredondamento AwayFromZero. Os perfis são categorias didáticas. Não há cadastro, banco de dados, armazenamento de clientes ou pagamento.

Este documento descreve os conceitos; o código e os relatórios gerados definem os nomes concretos dos campos de execução. O [contrato de negócio](contracts/business-rules.json) fixa os limites que os testes devem proteger.
