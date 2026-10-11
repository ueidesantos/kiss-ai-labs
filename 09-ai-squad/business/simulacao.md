# Cálculo e validação da simulação

O valor da compra deve ser um número decimal positivo e não ultrapassar o limite técnico de R$ 1 bilhão. O percentual deve ser numérico, não negativo e respeitar o limite do perfil. Não aceitar campos numéricos enviados como texto, valores ausentes ou perfis desconhecidos.

Usar aritmética decimal. Arredondar dinheiro para duas casas com MidpointRounding.AwayFromZero. O total é o valor da compra multiplicado por (1 - percentual / 100), arredondado ao final. A resposta informa o percentual aplicado, não um campo de desconto em dinheiro. A apresentação não deve perder centavos ao converter respostas monetárias.

Não há pagamento, persistência, autenticação ou integração com clientes reais. A seleção do perfil existe apenas para exercitar a regra.
