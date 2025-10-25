# Base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy csproj and restore as distinct layers
COPY ["EVChargingStation.CARC.WebAPI.HoaHTT/EVChargingStation.CARC.WebAPI.HoaHTT.csproj", "EVChargingStation.CARC.WebAPI.HoaHTT/"]
COPY ["EVChargingStation.CARC.Application.HoaHTT/EVChargingStation.CARC.Application.HoaHTT.csproj", "EVChargingStation.CARC.Application.HoaHTT/"]
COPY ["EVChargingStation.CARC.Infrastructure.HoaHTT/EVChargingStation.CARC.Infrastructure.HoaHTT.csproj", "EVChargingStation.CARC.Infrastructure.HoaHTT/"]
COPY ["EVChargingStation.CARC.Domain.HoaHTT/EVChargingStation.CARC.Domain.HoaHTT.csproj", "EVChargingStation.CARC.Domain.HoaHTT/"]
RUN dotnet restore "./EVChargingStation.CARC.WebAPI.HoaHTT/EVChargingStation.CARC.WebAPI.HoaHTT.csproj"
COPY . .
WORKDIR "/src/EVChargingStation.CARC.WebAPI.HoaHTT"
RUN dotnet build "./EVChargingStation.CARC.WebAPI.HoaHTT.csproj" -c $BUILD_CONFIGURATION -o /app/build

# PUBLISH stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./EVChargingStation.CARC.WebAPI.HoaHTT.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# FINAL image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EVChargingStation.CARC.WebAPI.HoaHTT.dll"]