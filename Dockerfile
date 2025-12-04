# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiamos csproj y restauramos dependencias (caché)
COPY ["KeepiProg.csproj", "./"]
RUN dotnet restore "./KeepiProg.csproj"

# Copiamos todo y publicamos
COPY . .
RUN dotnet publish "KeepiProg.csproj" -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
EXPOSE 80
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "KeepiProg.dll"]
