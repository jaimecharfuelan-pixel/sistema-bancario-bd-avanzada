# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar archivos de proyecto
COPY ["BancoApi/BancoApi.csproj", "BancoApi/"]
RUN dotnet restore "BancoApi/BancoApi.csproj"

# Copiar todo el código
COPY . .
WORKDIR "/src/BancoApi"
RUN dotnet build "BancoApi.csproj" -c Release -o /app/build

# Stage 2: Publish
FROM build AS publish
RUN dotnet publish "BancoApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 3: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Instalar curl para healthcheck
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

COPY --from=publish /app/publish .

# Exponer puerto (el que uses en docker-compose)
EXPOSE 8080

# Health check (verificar que la API responda)
HEALTHCHECK --interval=30s --timeout=10s --start-period=15s --retries=3 \
    CMD curl --fail http://localhost:8080/swagger/v1/swagger.json || exit 1

# Entrypoint
ENTRYPOINT ["dotnet", "BancoApi.dll"]
