# Simplified Dockerfile without EF tools (recommended approach)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY ["ArifTanPortfolio.csproj", "./"]
RUN dotnet restore "ArifTanPortfolio.csproj"

# Copy everything else and build
COPY . .
RUN dotnet build "ArifTanPortfolio.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "ArifTanPortfolio.csproj" -c Release -o /app/publish

# Final runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Copy published app
COPY --from=publish /app/publish .

# Set environment variables
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:80

ENTRYPOINT ["dotnet", "ArifTanPortfolio.dll"]