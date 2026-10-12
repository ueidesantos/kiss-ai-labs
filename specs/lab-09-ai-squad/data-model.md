# Modelo de dados

| Artefato | Conteúdo | Restrição |
|---|---|---|
| Fontes no relatório | Caminho, trecho, score e SHA256 | Integridade não garante veracidade |
| Identidades | Modelo desenvolvedor/revisor e digests | Real exige duas identidades distintas |
| Propostas | Papel e conteúdo JSON | Sem execução de código livre |
| `policy.json` | Limites Padrão/VIP | Testes protegem 10/20 |
| `front.json` | `title`, `help`, `standardLabel`, `vipLabel` | Texto apresentado sem interpretar HTML |
| Revisão | `model`, `summary`, `findings` | Cada achado tem `id`, `severity`, `evidence`, `fix` |
| Gate | Testes + revisão final | High/critical ou teste falho bloqueia |
| `report.json` | Estado, política, revisões, testes, histórico, propostas, eventos, fontes e uso de modelos | Offline explicitamente rotulado |
| `manifest.json` | Integridade dos artefatos da execução | Publicação recusa divergência |
| `candidate` | App e core materializados | Separados da fonte canônica |

O histórico permite comparar a proposta inicial e a única correção. Registros de tokens são uso retornado pelo provedor quando disponível, não estimativa inventada. Dinheiro usa decimal e arredondamento AwayFromZero; não há cadastro ou pagamentos.
