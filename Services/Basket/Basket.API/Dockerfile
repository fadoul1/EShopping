#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Services/Basket/Basket.API/Basket.API.csproj", "Services/Basket/Basket.API/"]
COPY ["Services/Basket/Basket.Application/Basket.Application.csproj", "Services/Basket/Basket.Application/"]
COPY ["Services/Basket/Basket.Core/Basket.Core.csproj", "Services/Basket/Basket.Core/"]
COPY ["Services/Basket/Basket.Infrastructure/Basket.Infrastructure.csproj", "Services/Basket/Basket.Infrastructure/"]
COPY ["Infrastructure/EventBus.Messages/EventBus.Messages.csproj", "Infrastructure/EventBus.Messages/"]

RUN dotnet restore "Services/Basket/Basket.API/Basket.API.csproj"
COPY . .
WORKDIR "/src/Services/Basket/Basket.API"
RUN dotnet build "Basket.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Basket.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Basket.API.dll"]