## Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy only the project folder
COPY . .
RUN dotnet restore NameInputService.csproj

RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

## Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:5255
EXPOSE 5255
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "NameInputService.dll"]

## FOR Docker file
WORKDIR /app
COPY . .
ENV ASPNETCORE_URLS=http://+:5255
EXPOSE 5255

RUN mkdir -p /app/bin/Debug/net8.0/backupLog

ENTRYPOINT ["dotnet", "NameInputService.dll"]
