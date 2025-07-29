# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0-preview AS build
WORKDIR /app

COPY ./*.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o out

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

# Khai báo PORT cho Render biết cần expose cổng nào
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

COPY --from=build /app/out .

ENTRYPOINT ["dotnet", "WebhookReceiver.dll"]