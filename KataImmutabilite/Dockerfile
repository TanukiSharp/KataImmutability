FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["KataImmutabilite/KataImmutabilite.csproj", "KataImmutabilite/"]
RUN dotnet restore "KataImmutabilite/KataImmutabilite.csproj"
COPY . .
WORKDIR "/src/KataImmutabilite"
RUN dotnet build "KataImmutabilite.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "KataImmutabilite.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "KataImmutabilite.dll"]
