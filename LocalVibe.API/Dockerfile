# Sử dụng môi trường .NET SDK để build code
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy file .csproj và restore các thư viện
COPY ["LocalVibe.API.csproj", "./"]
RUN dotnet restore "LocalVibe.API.csproj"

# Copy toàn bộ code và build
COPY . .
RUN dotnet publish "LocalVibe.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Sử dụng môi trường runtime siêu nhẹ để chạy app
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Expose cổng 8080 cho Backend
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "LocalVibe.API.dll"]