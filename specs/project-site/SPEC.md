# SPEC — Página pública KISS AI Labs

Papel: coordenador, UX e implementação frontend. Demanda autorizada: apresentar o projeto primeiro e colocar o histórico abaixo, em GitHub Pages.

- REQ-1: página estática em português com capa original, destaque do lab 08 e links para documentação e Figma.
- REQ-2: catálogo dos nove labs, 00–08, com descrições e limitações reais.
- REQ-3: histórico de evolução depois do catálogo, com link para os commits completos.
- REQ-4: layout responsivo, navegação por teclado, contraste e nenhuma dependência de JavaScript para ler o conteúdo.
- REQ-5: publicar em main e habilitar Pages em /docs; verificar URL pública antes de declarar publicação concluída.

Desenho: HTML/CSS estáticos, fundo escuro e acentos âmbar inspirados na capa aprovada. Imagem original copiada sem alteração para docs/assets. Sem framework, dependências ou coleta de dados. A aba Code do GitHub mantém sua própria estrutura.

Aceite: links internos e assets resolvem; nove cards; histórico após catálogo; viewport desktop/mobile sem rolagem horizontal; deploy público retorna a página. Integração Figma real continua não validada.

## Verificação

Revisão independente de QA: data inicial corrigida para 09/10 conforme commits. Navegador Edge headless em 1440 e 390 px: nove cards, imagem carregada, âncoras válidas, histórico abaixo do catálogo e ausência de overflow horizontal. Primeiro foco de teclado é o link para pular ao conteúdo. Screenshot mobile inspecionado. Publicação será confirmada pela resposta HTTP pública.

Publicação confirmada: GitHub Pages em main /docs, build 8239f6e concluído com sucesso; URL pública respondeu HTTP 200 com conteúdo e ordem esperados. Link adicionado ao campo About do repositório.

## Sobre mim

REQ-6: seção Sobre mim como última seção após o histórico e último item do menu, depois de GitHub, com nome Ueide Santos, botão para https://ueidesantos.github.io/ueide-santos-cv-web/ e âncora na navegação. Sem biografia presumida.

## Revisão adversarial de Marketing e Vendas — 2026-10-11

Papéis: UX e implementação frontend. Escopo autorizado: melhorar clareza, navegação e conversão para leitura dos labs e currículo, preservando identidade e evidências reais.

- REQ-7: comunicar público desenvolvedor .NET/Python, problemas de aplicação e conteúdo entregue, sem métricas ou provas sociais inventadas.
- REQ-8: oferecer três pontos de entrada: modelo local (00), busca em documentos/código (03–05) e especificação/delegação (07–08).
- REQ-9: explicar acrônimos e dependências; distinguir experimentos de soluções de produção; expor limitações de 06, 07 e 08 junto dos cards.
- REQ-10: manter nove cards, capa intacta, CTA principal para escolher trilha, secundário para lab 00, links de evidência e metadados coerentes.

Aceite: Sobre mim é último link do menu e última seção de main; três rotas de entrada; nove cards; âncoras e assets válidos; sem overflow em 320/390/768/1440 px; teclado e movimento reduzido preservados. Evidências em MARKETING-REVIEW.md. Deploy verificado pelo coordenador.
