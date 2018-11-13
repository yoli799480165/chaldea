FROM microsoft/dotnet:2.1-runtime AS base
WORKDIR /app

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /lib
COPY lib/aliyun aliyun
WORKDIR /src
COPY src/Chaldea Chaldea
COPY src/Chaldea.Core Chaldea.Core
COPY src/Chaldea.Infrastructure.Sms.Aliyun Chaldea.Infrastructure.Sms.Aliyun
WORKDIR /src/Chaldea
RUN dotnet restore Chaldea.csproj && dotnet publish Chaldea.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=build /app .

ENTRYPOINT ["dotnet", "Chaldea.dll"]
