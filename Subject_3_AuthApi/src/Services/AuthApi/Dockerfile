FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build

WORKDIR /code
EXPOSE 80
EXPOSE 443

COPY . .

RUN dotnet restore

RUN dotnet publish -o /out/ -c Release


RUN dotnet dev-certs https --clean
RUN dotnet dev-certs https -ep ./AuthApi.pfx  -p crypticpassword

RUN mv ./AuthApi.pfx /out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base

COPY --from=build /out /app/

WORKDIR /app

ENTRYPOINT ["dotnet", "AuthApi.dll"]