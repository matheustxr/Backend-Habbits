# --- Estágio 1: Build ---
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copia TODOS os arquivos do projeto
COPY . ./

# Restaura e Publica o projeto da API em modo Release
RUN dotnet publish src/Habits.Api/Habits.Api.csproj -c Release -o /app/out

# --- Estágio 2: Imagem Final ---
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copia apenas os artefatos publicados do estágio de build
COPY --from=build-env /app/out .

# Copia e torna o script de inicialização executável
COPY --from=build-env /app/start.sh .
RUN chmod +x start.sh

# Define o ponto de entrada para rodar as migrations e iniciar a API
ENTRYPOINT ["/app/start.sh"]