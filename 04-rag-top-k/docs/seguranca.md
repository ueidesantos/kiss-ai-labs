# Credential Security

Credentials must not be stored in source code or versioned files. Use environment variables or a secrets manager to provide tokens at runtime.

If you suspect a credential was exposed, revoke the affected token, issue a new credential, and review access logs. Do not include secret values in error messages or logs.
