FROM mcr.microsoft.com/dotnet/aspnet:10.0.0-preview.7 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/aspnet:10.0.0-preview.7 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Worker/Worker.csproj", "Worker/"]
COPY ["../Shared/Shared.csproj", "../Shared/"]
RUN dotnet restore "Worker/Worker.csproj"
COPY . .
WORKDIR "/src/Worker"
RUN dotnet build "./Worker.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Worker.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Worker.dll"]
