FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build

WORKDIR /code
EXPOSE 80

COPY . .

RUN dotnet restore

RUN dotnet publish -o /out/ -c Release

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base

COPY --from=build /out /app/

WORKDIR /app

ENTRYPOINT ["dotnet", "CatalogApi.dll"]