# Segurança de credenciais

Credenciais não devem ser armazenadas no código-fonte nem em arquivos versionados. Use variáveis de ambiente ou um gerenciador de segredos para fornecer tokens durante a execução.

Ao suspeitar de exposição, revogue o token afetado, emita uma nova credencial e revise os logs de acesso. Não inclua valores de segredo em mensagens de erro ou logs.
