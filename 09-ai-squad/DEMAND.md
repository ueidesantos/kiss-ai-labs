# Uma mudança pequena, um ciclo completo

Um atendente precisa simular descontos conforme o perfil informado: **Padrão até 10%** e **VIP até 20%**. O valor da compra deve ser positivo, com limite técnico de R$ 1 bilhão. O resultado mostra o valor original, o percentual aplicado e o total, com dinheiro apresentado em duas casas decimais.

A demanda é fictícia. Selecionar VIP não autentica uma pessoa nem concede autorização comercial: este é um simulador didático, sem cobrança, cadastro ou persistência de clientes.

O desafio de engenharia é acompanhar a mudança da regra até uma aplicação candidata revisável: TL consulta documentos, Front e Back propõem alterações, testes verificam comportamento e um segundo modelo faz revisão adversarial, com evidência e severidade. Uma correção é permitida e exige nova revisão. A entrega não pode ser aprovada só porque o modelo disse que está correta.

Resultado esperado: aplicação funcional, fontes rastreáveis, SPEC candidata, propostas delimitadas, diff e evidências; um draft PR é uma etapa opcional e explícita. A [SPEC canônica](../specs/lab-09-ai-squad/SPEC.md) define o aceite.
