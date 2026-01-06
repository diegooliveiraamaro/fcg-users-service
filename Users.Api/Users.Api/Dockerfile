# ===== Fase de build =====
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia apenas o projeto primeiro (cache mais eficiente)
COPY Users.Api/Users.Api/Users.Api.csproj ./Users.Api.csproj

# Restaura pacotes
RUN dotnet restore ./Users.Api.csproj

# Copia todo o restante do código
COPY Users.Api/. ./Users.Api

# Publica a aplicação
RUN dotnet publish ./Users.Api.csproj -c Release -o /app/publish

# ===== Fase de runtime =====
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
EXPOSE 80

# Copia a aplicação publicada
COPY --from=build /app/publish .

# Healthcheck para ALB / ECS
HEALTHCHECK --interval=15s --timeout=5s --start-period=10s --retries=3 \
  CMD curl -f http://localhost:80/health || exit 1

# Entrada da aplicação
ENTRYPOINT ["dotnet", "Users.Api.dll"]
