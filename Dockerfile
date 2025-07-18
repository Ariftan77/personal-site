# Use the official .NET 8 SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy project files
COPY ArifTanPortfolio/*.csproj ./ArifTanPortfolio/
COPY ArifTanPortfolio.Tests/*.csproj ./ArifTanPortfolio.Tests/

# Restore dependencies
RUN dotnet restore ArifTanPortfolio/ArifTanPortfolio.csproj

# Copy source code
COPY . .

# Build the application
WORKDIR /app/ArifTanPortfolio
RUN dotnet build -c Release -o /app/build

# Publish the application
RUN dotnet publish -c Release -o /app/publish --no-restore

# Use the official ASP.NET Core runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Install Entity Framework Core tools for migrations
RUN dotnet tool install --global dotnet-ef
ENV PATH="${PATH}:/root/.dotnet/tools"

# Copy published app from build stage
COPY --from=build /app/publish .

# Create directory for SQLite database
RUN mkdir -p /app/data

# Expose port (Railway uses PORT environment variable)
EXPOSE 8080

# Set environment variables
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:8080

# Health check
HEALTHCHECK --interval=30s --timeout=10s --start-period=5s --retries=3 \
  CMD curl -f http://localhost:8080/health || exit 1

# Run database migrations and start the application
ENTRYPOINT ["sh", "-c", "dotnet ef database update --no-build && dotnet ArifTanPortfolio.dll"]