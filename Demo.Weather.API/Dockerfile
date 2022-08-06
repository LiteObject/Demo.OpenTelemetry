#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Demo.Weather.API/Demo.Weather.API.csproj", "Demo.Weather.API/"]
RUN dotnet restore "Demo.Weather.API/Demo.Weather.API.csproj"
COPY . .
WORKDIR "/src/Demo.Weather.API"
RUN dotnet build "Demo.Weather.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Demo.Weather.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Demo.Weather.API.dll"]