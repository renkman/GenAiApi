FROM ghcr.io/shimat/opencvsharp/ubuntu24-dotnet8-opencv4.12.0:20250815 AS base

WORKDIR /app
EXPOSE 8080
EXPOSE 8081
USER $APP_UID

FROM mcr.microsoft.com/dotnet/sdk:8.0-noble AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["OpenLlamaAPI/OpenLlamaAPI.csproj", "OpenLlamaAPI/"]
RUN dotnet restore "OpenLlamaAPI/OpenLlamaAPI.csproj"
COPY . .
WORKDIR "/src/OpenLlamaAPI"
RUN dotnet build "./OpenLlamaAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./OpenLlamaAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final

WORKDIR /build

WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "OpenLlamaAPI.dll"]
