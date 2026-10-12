# Plano técnico

```mermaid
flowchart TD
  D[Demanda + documentação de negócio] --> R[RAG lexical com fontes]
  R --> TL[Modelo A · TL planeja]
  TL --> F[Modelo A · Front propõe textos]
  TL --> B[Modelo A · Back propõe política]
  F --> C[Templates geram candidata]
  B --> C
  C --> T[Build + testes determinísticos]
  T --> Q[Modelo B · revisão adversarial]
  Q --> G{Testes passam e sem high/critical?}
  G -->|Sim| E[Evidências + draft PR opt-in]
  G -->|Não · primeira tentativa| B
  G -->|Não · após correção| X[Reprovado]
```

O coordenador verifica nomes e digests distintos antes das chamadas reais. TL, Front e Back compartilham o modelo desenvolvedor; a revisão usa o segundo modelo. O runner coordena etapas sequenciais, sem afirmar autonomia geral ou paralelismo.

## Responsabilidades

Coordenador/TL fixa escopo e contratos; Front/UX cuida de textos e estados; Back mantém domínio decimal e templates; QA adversarial recebe evidências, registra achados e reavalia a única correção permitida. Testes e regras de aprovação ficam fora do controle de ambos os modelos.

## Falhas e reversão

Resposta inválida, indisponibilidade ou revisão grave impede aprovação. Artefatos ficam em diretório próprio; a SPEC canônica não é sobrescrita. O script de publicação verifica manifesto e gate, cria draft PR separado e nunca faz merge. Para descartar uma candidata publicada, fechar seu PR.

A capa dá protagonismo ao lab 09, preservando o cenário e a referência ao lab 08. Validação e limitações constam em [convergence.md](convergence.md).
