# Política de descontos — B1

O valor original do pedido deve ser um número finito maior que zero.
O desconto percentual permitido vai de 0 a 20, incluindo os limites.
Descontos negativos ou acima de 20 devem ser recusados, com mensagem legível.
O valor final é valor original multiplicado por (1 - desconto / 100).
Valores monetários são arredondados a duas casas decimais com HALF_UP.
Exemplo: 100 com desconto de 10 por cento resulta em 90.00.
Estas regras pertencem a uma loja fictícia usada neste laboratório.
