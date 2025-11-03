## Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore as distinct layers
COPY NameInputService/NameInputService.csproj NameInputService/
RUN dotnet restore NameInputService/NameInputService.csproj

# Copy everything and build
COPY NameInputService/ NameInputService/
WORKDIR /src/NameInputService
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

## Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "NameInputService.dll"]


