# Habits API

Uma API RESTful completa para gerenciamento de hábitos pessoais, construída com .NET 8, arquitetura em camadas (DDD), princípios SOLID e validação robusta. Ideal para controle de hábitos, categorias e perfis de usuário, com autenticação via JWT e suporte a múltiplos idiomas para mensagens de erro.

## 📦 Estrutura do Projeto
`
  ├── src
  │   ├── Habits.Api # Camada de apresentação (Controllers, Middlewares, etc.)
  │   ├── Habits.Application # Casos de uso, validações e regras de negócio
  │   ├── Habits.Communication # DTOs para requests/responses e enums compartilhados
  │   ├── Habits.Domain # Entidades, contratos e lógica de domínio
  │   ├── Habits.Exception # Exceptions personalizadas e mensagens localizadas
  │   └── Habits.Infrastructure # Acesso a dados, repositórios, segurança e configurações
  ├── tests # Testes unitários e de integração
  ├── .github/workflows # Pipeline CI com GitHub Actions
  ├── Habits.sln # Solution principal
  └── README.md
`

## 🚀 Tecnologias Utilizadas

- [.NET 8](https://learn.microsoft.com/dotnet)
- C#
- Entity Framework Core
- PostgreSQL
- AutoMapper
- FluentValidation
- JWT (JSON Web Tokens)
- BCrypt (hash de senhas)
- GitHub Actions (CI/CD)

## 📌 Funcionalidades

### 🧑 Usuários
- Registro de novos usuários
- Login e autenticação com JWT
- Atualização e exclusão de conta
- Alteração de senha
- Consulta de perfil

### 📆 Hábitos
- Criação, leitura, atualização e exclusão de hábitos
- Marcação de hábitos por dia
- Filtro por categorias

### 🗂️ Categorias
- CRUD de categorias de hábitos
- Suporte a cores personalizadas (HexColor)

## 🌍 Suporte a Idiomas

Mensagens de erro e validação disponíveis em múltiplos idiomas via arquivos `.resx`:
- `pt-BR`
- `en-US` (default)

## 🛡️ Segurança

- Hashing de senhas com BCrypt
- Autenticação e autorização via JWT
- Middleware de cultura e controle de exceções

## 📑 Documentação da API

Você pode testar os endpoints usando ferramentas como:
- Postman
- Thunder Client (VSCode)
- Ou diretamente com os arquivos `.http` incluídos no projeto

## 🧪 Testes

Os testes estão localizados na pasta `tests/`, utilizando builders, criptografia mockada e testes de entidades e regras de negócio.

## 🛠️ Como Rodar Localmente

### Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download)
- [PostgreSQL](https://www.postgresql.org/)
- IDE como Visual Studio ou VSCode

### Passos

```bash
# Clone o repositório
git clone https://github.com/seu-usuario/habits-api.git
cd habits-api

# Crie o banco de dados PostgreSQL e configure as strings de conexão em:
# - src/Habits.Api/appsettings.Development.json

# Aplique as migrações (opcional, caso esteja configurado para gerar automaticamente)
dotnet ef database update --project src/Habits.Infrastructure

# Rode a aplicação
dotnet run --project src/Habits.Api
```

## ⚙️ Pipeline CI
O projeto está integrado ao GitHub Actions com o workflow:

ci.yml: Build, testes e validação de camadas

## 👨‍💻 Contribuindo
Pull requests são bem-vindos. Para mudanças maiores, abra uma issue antes para discutir o que você gostaria de mudar.

## 📝 Licença
Este projeto é licenciado sob a MIT License.

Desenvolvido por Matheus Teixeira 💻
