# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Api/RetailHub.Api/RetailHub.Api.csproj", "Api/RetailHub.Api/"]
COPY ["SharedKernel/Application/SharedKernel.Application/SharedKernel.Application.csproj", "SharedKernel/Application/SharedKernel.Application/"]
COPY ["SharedKernel/Domain/SharedKernel.Domain/SharedKernel.Domain.csproj", "SharedKernel/Domain/SharedKernel.Domain/"]
COPY ["Modules/Cart/Cart.Application/Cart.Application.csproj", "Modules/Cart/Cart.Application/"]
COPY ["Modules/Catalog/Catalog.Application/Catalog.Application.csproj", "Modules/Catalog/Catalog.Application/"]
COPY ["Modules/Catalog/Catalog.Domain/Catalog.Domain.csproj", "Modules/Catalog/Catalog.Domain/"]
COPY ["Modules/Cart/Cart.Domain/Cart.Domain.csproj", "Modules/Cart/Cart.Domain/"]
COPY ["Modules/Cart/Cart.Infrastructure/Cart.Infrastructure.csproj", "Modules/Cart/Cart.Infrastructure/"]
COPY ["SharedKernel/Infrastructure/SharedKernel.Infrastructure/SharedKernel.Infrastructure.csproj", "SharedKernel/Infrastructure/SharedKernel.Infrastructure/"]
COPY ["Modules/Catalog/Catalog.Infrastructure/Catalog.Infrastructure.csproj", "Modules/Catalog/Catalog.Infrastructure/"]
COPY ["Modules/Identity/Identity.Application/Identity.Application.csproj", "Modules/Identity/Identity.Application/"]
COPY ["Modules/Identity/Identity.Infrastructure/Identity.Infrastructure.csproj", "Modules/Identity/Identity.Infrastructure/"]
COPY ["Modules/Orders/Orders.Application/Orders.Application.csproj", "Modules/Orders/Orders.Application/"]
COPY ["Modules/Orders/Orders.Domain/Orders.Domain.csproj", "Modules/Orders/Orders.Domain/"]
COPY ["Modules/Orders/Orders.Infrastructure/Orders.Infrastructure.csproj", "Modules/Orders/Orders.Infrastructure/"]
RUN dotnet restore "./Api/RetailHub.Api/RetailHub.Api.csproj"
COPY . .
WORKDIR "/src/Api/RetailHub.Api"
RUN dotnet build "./RetailHub.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./RetailHub.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RetailHub.Api.dll"]