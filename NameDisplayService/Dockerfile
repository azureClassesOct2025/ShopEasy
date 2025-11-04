## Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy only the project folder
COPY . .
RUN dotnet restore NameDisplayService.csproj

RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

## Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:5010
EXPOSE 5010
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "NameDisplayService.dll"]

## FOR Docker file
WORKDIR /app
COPY . .
ENV ASPNETCORE_URLS=http://+:5010
EXPOSE 5010
ENTRYPOINT ["dotnet", "NameDisplayService.dll"]
