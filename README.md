# IDP Server & Client Example (Authorization Code Flow)

Esta aplicação é um exemplo didático que demonstra como construir um servidor de Identidade (IDP) usando OpenIddict e um client que consome e valida tokens JWT emitidos pelo IDP.

## Visão Geral

A arquitetura é composta por três componentes principais:

- **IDP Server**  
  Responsável por autenticar clientes e emitir tokens JWT assinados. O servidor:
    - Processa requisições para o endpoint `/connect/token` (usando o fluxo *client credentials*).
    - Expõe um endpoint JWKS (`/.well-known/jwks`) que disponibiliza a chave pública, conforme as especificações da RFC 7517.
    - Registra um client (por exemplo, "service-worker") através de um *Worker* que é executado na inicialização.

- **Worker**  
  Um serviço hospedado que, ao iniciar o servidor, verifica se o client "service-worker" já está registrado no banco de dados. Se não estiver, ele o cadastra automaticamente. Essa abordagem simplifica os testes, evitando o cadastro manual do client. Em ambientes reais, o registro dos clientes pode ser feito via endpoint administrativo ou outro mecanismo de gerenciamento.

- **Client**  
  Uma aplicação (neste exemplo, uma aplicação console) que:
    - Realiza uma chamada HTTP para o endpoint `/connect/token` do IDP para obter um token.
    - Consulta o endpoint de discovery (`.well-known/openid-configuration`) para obter a chave pública (via JWKS) e valida localmente o token recebido usando as bibliotecas do .NET (como `System.IdentityModel.Tokens.Jwt`).

## Fluxo de Funcionamento

1. **Emissão de Token:**
    - A aplicação cliente envia uma requisição POST para `https://localhost:7019/connect/token` com os parâmetros:
        - `grant_type=client_credentials`
        - `client_id=service-worker`
        - `client_secret=388D45FA-B36B-4988-BA59-B187D329C207`
    - O IDP valida as credenciais e gera um token JWT assinado com o algoritmo RS256.
    - **Nota:** Para fins didáticos, a encriptação dos tokens foi desabilitada com `options.DisableAccessTokenEncryption()`, de modo que o token é apenas assinado.

2. **Exposição da Chave Pública (JWKS):**
    - O IDP disponibiliza um endpoint JWKS em `https://localhost:7019/.well-known/jwks`, onde a chave pública é apresentada no formato JSON Web Key Set, permitindo que outros serviços validem a assinatura do token sem precisar de chamadas adicionais ao IDP.

3. **Validação do Token pelo Client:**
    - O client obtém o token e, por meio do endpoint de discovery (ou diretamente via JWKS), recupera as chaves públicas.
    - Com a biblioteca `System.IdentityModel.Tokens.Jwt` e `Microsoft.IdentityModel.Protocols.OpenIdConnect`, o client valida o token verificando a assinatura, o emissor (issuer), a audiência (audience) e a validade temporal.

## Pré-requisitos do Banco de Dados

Antes de executar a aplicação, é necessário criar e atualizar o banco de dados com as tabelas necessárias pelo OpenIddict. **Caso o arquivo `openidict-test.db` já esteja presente na raiz do projeto, a execução dos comandos abaixo não é necessária.**
```bash
dotnet ef migrations add InitialMigration
dotnet ef database update
```

## Documentação do IDP Server

Acesse a documentação completa do IDP Server em:
https://localhost:7019/scalar/idp-server/

### Exemplos de Uso com Curl

- Chamada para o endpoint /connect/token:
```bash
curl --location 'https://localhost:7019/connect/token' \
     --header 'Content-Type: application/x-www-form-urlencoded' \
     --data-urlencode 'grant_type=client_credentials' \
     --data-urlencode 'client_id=service-worker' \
     --data-urlencode 'client_secret=388D45FA-B36B-4988-BA59-B187D329C207'
```

- Chamada para o endpoint /jwks:
```bash
  curl --location 'https://localhost:7019/.well-known/jwks'
```

## Como Funciona a Validação do Token no Client

O client não armazena as configurações de JWT localmente. Em vez disso, ele consulta o endpoint de discovery para obter a configuração OpenID Connect, que inclui as chaves públicas necessárias para validar o token.

## Considerações Importantes

- Finalidades Didáticas:
Este exemplo é para fins didáticos. Para facilitar os testes, a encriptação dos tokens está desabilitada com options.DisableAccessTokenEncryption(), fazendo com que os tokens sejam apenas assinados.
- Segurança em Produção:
Em um ambiente real, recomenda-se que:
  - Tokens sejam, idealmente, criptografados ou que se utilize um mecanismo seguro para gerenciar chaves (como um Key Vault).
  - As configurações de JWT (chaves, issuer, audience) sejam gerenciadas de forma segura, sem expor informações sensíveis em arquivos de configuração.

## Conclusão

Esta aplicação demonstra como:

- Um IDP Server (Authorization Code Flow) centraliza a autenticação e a emissão de tokens JWT.
- Um Worker registra automaticamente o client necessário para os testes.
- O Client consome o token emitido e valida-o utilizando o endpoint de discovery para obter a chave pública (JWKS).

Siga as instruções de migração do Entity Framework para configurar o banco de dados e utilize os exemplos de chamada com curl para testar os endpoints.