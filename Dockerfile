# -------- BUILD STAGE --------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia o arquivo da solution
COPY VianaHub.Global.Gerit.sln ./

# Copia todos os projetos
COPY src ./src

# Restaura dependências
RUN dotnet restore src/VianaHub.Global.Gerit.Api/VianaHub.Global.Gerit.Api.csproj

# Publica aplicação
RUN dotnet publish src/VianaHub.Global.Gerit.Api/VianaHub.Global.Gerit.Api.csproj \
    -c Release -o /app/publish

# -------- RUNTIME STAGE --------
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "VianaHub.Global.Gerit.Api.dll"]