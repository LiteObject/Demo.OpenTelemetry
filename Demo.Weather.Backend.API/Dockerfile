#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Demo.Weather.Backend.API/Demo.Weather.Backend.API.csproj", "Demo.Weather.Backend.API/"]
RUN dotnet restore "Demo.Weather.Backend.API/Demo.Weather.Backend.API.csproj"
COPY . .
WORKDIR "/src/Demo.Weather.Backend.API"
RUN dotnet build "Demo.Weather.Backend.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Demo.Weather.Backend.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Demo.Weather.Backend.API.dll"]