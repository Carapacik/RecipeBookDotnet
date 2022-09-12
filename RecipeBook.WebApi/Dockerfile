FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["RecipeBook.Domain/RecipeBook.Domain.csproj", "RecipeBook.Domain/"]
COPY ["RecipeBook.Infrastructure/RecipeBook.Infrastructure.csproj", "RecipeBook.Infrastructure/"]
COPY ["RecipeBook.Application/RecipeBook.Application.csproj", "RecipeBook.Application/"]
COPY ["RecipeBook.Migrations/RecipeBook.Migrations.csproj", "RecipeBook.Migrations/"]
COPY ["RecipeBook.WebApi/RecipeBook.WebApi.csproj", "RecipeBook.WebApi/"]
RUN dotnet restore "RecipeBook.WebApi/RecipeBook.WebApi.csproj"
COPY . .
WORKDIR "/src/RecipeBook.WebApi"
RUN dotnet build "RecipeBook.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RecipeBook.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RecipeBook.WebApi.dll"]
