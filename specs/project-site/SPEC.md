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
