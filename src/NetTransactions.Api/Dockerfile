#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/NetTransactions.Api/NetTransactions.Api.csproj", "src/NetTransactions.Api/"]
RUN dotnet restore "src/NetTransactions.Api/NetTransactions.Api.csproj"
COPY . .
WORKDIR "/src/src/NetTransactions.Api"
RUN dotnet build "NetTransactions.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NetTransactions.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NetTransactions.Api.dll"]