FROM mcr.microsoft.com/dotnet/aspnet:8.0-bookworm-slim-arm32v7 AS base
USER app
WORKDIR /app
EXPOSE 8080
RUN apt-get update && apt-get install -y \
    rpi.gpio-common \
    python3-rpi.gpio
RUN groupadd -f -r gpio && usermod -a -G gpio app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/PollerBox/PollerBox.csproj", "src/PollerBox/"]
RUN dotnet restore "./src/PollerBox/PollerBox.csproj"
COPY . .
WORKDIR "/src/src/PollerBox"
RUN dotnet build "./PollerBox.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./PollerBox.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PollerBox.dll"]