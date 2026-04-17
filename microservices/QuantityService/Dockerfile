FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["QuantityService.csproj", "./"]
RUN dotnet restore "./QuantityService.csproj"
COPY . .
RUN dotnet build "QuantityService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "QuantityService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "QuantityService.dll"]
