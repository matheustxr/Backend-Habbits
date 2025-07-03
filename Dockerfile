# Estágio de Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copia os arquivos .csproj e restaura as dependências primeiro para aproveitar o cache
COPY src/Habits.Api/*.csproj ./src/Habits.Api/
COPY src/Habits.Application/*.csproj ./src/Habits.Application/
COPY src/Habits.Communication/*.csproj ./src/Habits.Communication/
COPY src/Habits.Domain/*.csproj ./src/Habits.Domain/
COPY src/Habits.Exception/*.csproj ./src/Habits.Exception/
COPY src/Habits.Infrastructure/*.csproj ./src/Habits.Infrastructure/

# Restaura as dependências de todos os projetos
COPY Habits.sln .
RUN dotnet restore Habits.sln

# Copia o restante do código-fonte
COPY src/ ./src/

# Publica a aplicação
RUN dotnet publish src/Habits.Api/Habits.Api.csproj -c Release -o /app/out

# Estágio Final
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .

# Copia e dá permissão de execução para o script de inicialização
COPY start.sh .
RUN chmod +x start.sh

# Define o script como ponto de entrada
ENTRYPOINT ["/app/start.sh"]