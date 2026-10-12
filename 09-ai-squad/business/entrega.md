# Evidências da mudança

A entrega deve permitir entender por que o desconto foi aceito ou recusado. Interface e backend devem comunicar os limites de Padrão e VIP de forma coerente.

Cada execução registra as fontes de negócio consultadas e sua integridade. A SPEC produzida pelos agentes é candidata; não substitui a especificação aprovada. Testes determinísticos são o gate, e a opinião do agente QA não pode torná-los aprovados.

No máximo uma correção de backend por execução. Uma nova falha encerra o fluxo com estado explícito. Publicar um draft PR exige uma ação separada. Não fazer merge automaticamente.

Um modelo distinto revisa adversarialmente cada candidata. Achados high/critical bloqueiam a aprovação, mesmo com testes aprovados. Após a única correção permitida, executar os testes e pedir nova revisão; não transportar aprovação antiga para a candidata alterada.
