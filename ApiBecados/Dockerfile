# Imagen base oficial de .NET SDK
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# Copiar los archivos de proyecto
COPY ["ApiBecados.csproj", "./"]
RUN dotnet restore "ApiBecados.csproj"

# Copiar el resto del código y compilar
COPY . .
RUN dotnet publish "ApiBecados.csproj" -c Release -o /app/publish

# Imagen final
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Puerto por defecto para ASP.NET
ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

ENTRYPOINT ["dotnet", "ApiBecados.dll"]
