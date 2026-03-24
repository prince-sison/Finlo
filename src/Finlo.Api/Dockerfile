FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /srcFROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src

# Copy csproj files and restore (layer caching)
COPY src/Finlo.Domain/Finlo.Domain.csproj Finlo.Domain/
COPY src/Finlo.Application/Finlo.Application.csproj Finlo.Application/
COPY src/Finlo.Infrastructure/Finlo.Infrastructure.csproj Finlo.Infrastructure/
COPY src/Finlo.Api/Finlo.Api.csproj Finlo.Api/
RUN dotnet restore Finlo.Api/Finlo.Api.csproj

# Copy source and publish
COPY src/ .
RUN dotnet publish Finlo.Api/Finlo.Api.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:5266
ENV ASPNETCORE_ENVIRONMENT=Development
EXPOSE 5266

ENTRYPOINT ["dotnet", "Finlo.Api.dll"]