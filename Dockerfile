FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

COPY src/Habits.Api/*.csproj ./Habits.Api/
COPY src/Habits.Application/*.csproj ./Habits.Application/
COPY src/Habits.Communication/*.csproj ./Habits.Communication/
COPY src/Habits.Domain/*.csproj ./Habits.Domain/
COPY src/Habits.Exception/*.csproj ./Habits.Exception/
COPY src/Habits.Infrastructure/*.csproj ./Habits.Infrastructure/

WORKDIR /app/Habits.Api
RUN dotnet restore

WORKDIR /app
COPY src/ .

WORKDIR /app/Habits.Api
RUN dotnet publish -c Release -o /app/out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=build-env /app/out .

ENTRYPOINT ["dotnet", "Habits.Api.dll"]
