FROM microsoft/dotnet:2.1-runtime AS base
WORKDIR /app

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY src/Chaldea Chaldea
WORKDIR /src/Chaldea
RUN dotnet restore Chaldea.csproj && dotnet publish Chaldea.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=build /app .

ENTRYPOINT ["dotnet", "Chaldea.dll"]
