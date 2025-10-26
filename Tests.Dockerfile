FROM ghcr.io/shimat/opencvsharp/ubuntu24-dotnet8-opencv4.12.0:20250815 AS tests

RUN apt-get update && apt-get install dotnet-sdk-8.0 -y

ARG BUILD_CONFIGURATION=Release

WORKDIR /src
COPY ["OpenLlamaAPI/", "OpenLlamaAPI/"]
COPY ["OpenLlamaApiTests/", "OpenLlamaApiTests/"]
COPY ["OpenLlamaAPI.sln", "."]

RUN dotnet restore 

RUN dotnet build -c $BUILD_CONFIGURATION

ENTRYPOINT ["dotnet", "test", "--collect:\"XPlat Code Coverage\"", "--results-directory", "/results"]
