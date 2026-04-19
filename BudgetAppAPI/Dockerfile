# 1. Download image of .NET 8.0

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS builder
WORKDIR /src

# Copy the project file and restore any dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy the rest of the code
COPY . ./

# Publish the app

RUN dotnet publish -c Release -o /app/publish

# 2. Config the runtime with asp.net image

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=builder /app/publish .

EXPOSE 80

ENTRYPOINT ["dotnet", "BudgetAppAPI.dll"]