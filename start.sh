#!/bin/bash

# Instala a ferramenta do EF Core
dotnet tool install --global dotnet-ef

# Adiciona a ferramenta ao PATH
export PATH="$PATH:/root/.dotnet/tools"

# Aplica as migrations do Entity Framework
dotnet ef database update --project src/Habits.Infrastructure

# Inicia a aplicação
dotnet Habits.Api.dll