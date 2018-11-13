FROM microsoft/dotnet:2.1-runtime AS base
WORKDIR /app

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY src/Chaldea.IdentityServer Chaldea.IdentityServer
COPY src/Chaldea.Core Chaldea.Core
WORKDIR /src/Chaldea.IdentityServer
RUN dotnet restore Chaldea.IdentityServer.csproj && dotnet publish Chaldea.IdentityServer.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=build /app .

ENTRYPOINT ["dotnet", "Chaldea.IdentityServer.dll"]
