FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Testavimo buildas, jei bent vienas testas nepavyksta, buildas sustoja
FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS testBuild
WORKDIR /src
COPY ["ServerXUnitTests/ServerXUnitTests.csproj", "ServerXUnitTests/"]
RUN dotnet restore "ServerXUnitTests/ServerXUnitTests.csproj"
COPY . .
WORKDIR "/src/ServerXUnitTests"
RUN dotnet test

# Buildinimo buildas
FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Server/Server.csproj", "Server/"]
RUN dotnet restore "Server/Server.csproj"
COPY . .
WORKDIR "/src/Server"
RUN dotnet build "Server.csproj" -c Release -o /app/build

# Subuildiname pati projekta
FROM build AS publish
RUN dotnet publish "Server.csproj" -c Release -o /app/publish

# Galutinis image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Server.dll"]