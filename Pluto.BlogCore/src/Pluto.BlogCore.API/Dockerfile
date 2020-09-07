#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/Pluto.BlogCore.API/Pluto.BlogCore.API.csproj", "src/Pluto.BlogCore.API/"]
COPY ["src/Pluto.BlogCore.Infrastructure/Pluto.BlogCore.Infrastructure.csproj", "src/Pluto.BlogCore.Infrastructure/"]
COPY ["src/Pluto.BlogCore.Domain/Pluto.BlogCore.Domain.csproj", "src/Pluto.BlogCore.Domain/"]
COPY ["src/Pluto.BlogCore.Application/Pluto.BlogCore.Application.csproj", "src/Pluto.BlogCore.Application/"]
RUN dotnet restore "src/Pluto.BlogCore.API/Pluto.BlogCore.API.csproj"
COPY . .
WORKDIR "/src/src/Pluto.BlogCore.API"
RUN dotnet build "Pluto.BlogCore.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Pluto.BlogCore.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Pluto.BlogCore.API.dll"]
