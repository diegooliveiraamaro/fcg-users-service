# ===== Fase de build =====
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia todos os arquivos e subpastas
COPY . .

# Publica o projeto
RUN dotnet publish Users.Api/Users.Api/Users.Api.csproj -c Release -o /app/publish

# ===== Fase de runtime =====
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Expõe a porta usada pelo container
EXPOSE 80

# Copia os arquivos publicados da fase de build
COPY --from=build /app/publish .

# Define o ponto de entrada
ENTRYPOINT ["dotnet", "Users.Api.dll"]

# ===== Health Check para ECS / ALB =====
HEALTHCHECK --interval=15s --timeout=5s --start-period=10s --retries=3 \
  CMD curl -f http://localhost:80/health || exit 1
