FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
EXPOSE 80

COPY bin/Release/net8.0/publish .
ENTRYPOINT ["dotnet", "Users.Api.dll"]