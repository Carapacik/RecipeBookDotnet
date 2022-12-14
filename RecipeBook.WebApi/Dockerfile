FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS build
WORKDIR /src
COPY ["RecipeBookDotnet/RecipeBook.Domain/RecipeBook.Domain.csproj", "RecipeBook.Domain/"]
COPY ["RecipeBookDotnet/RecipeBook.Infrastructure/RecipeBook.Infrastructure.csproj", "RecipeBook.Infrastructure/"]
COPY ["RecipeBookDotnet/RecipeBook.Application/RecipeBook.Application.csproj", "RecipeBook.Application/"]
COPY ["RecipeBookDotnet/RecipeBook.Migrations/RecipeBook.Migrations.csproj", "RecipeBook.Migrations/"]
COPY ["RecipeBookDotnet/RecipeBook.WebApi/RecipeBook.WebApi.csproj", "RecipeBook.WebApi/"]
RUN dotnet restore "RecipeBook.WebApi/RecipeBook.WebApi.csproj"
COPY . .
WORKDIR "/src/RecipeBookDotnet/RecipeBook.WebApi"
RUN dotnet build "RecipeBook.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RecipeBook.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RecipeBook.WebApi.dll"]