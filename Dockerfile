# Railway-optimized Dockerfile for .NET 8.0 ASP.NET Core
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution file if it exists
COPY *.sln .

# Copy project files
COPY ["ArifTanPortfolio/ArifTanPortfolio.csproj", "ArifTanPortfolio/"]

# Restore dependencies
RUN dotnet restore "ArifTanPortfolio/ArifTanPortfolio.csproj"

# Copy everything else
COPY . .

# Build the project
WORKDIR "/src/ArifTanPortfolio"
RUN dotnet build "ArifTanPortfolio.csproj" -c Release -o /app/build

# Publish the project
FROM build AS publish
RUN dotnet publish "ArifTanPortfolio.csproj" -c Release -o /app/publish

# Final stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copy published files
COPY --from=publish /app/publish .

# Railway-specific: Configure for Railway's PORT environment variable
ENV ASPNETCORE_URLS=http://0.0.0.0:$PORT
ENV ASPNETCORE_ENVIRONMENT=Production

# Expose port (Railway will inject the actual port)
EXPOSE $PORT

ENTRYPOINT ["dotnet", "ArifTanPortfolio.dll"]