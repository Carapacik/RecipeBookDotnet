# RecipeBookDotnet

## Run with Docker

In **docker-compose.yml** change default value of `FILE_STORAGE` for file strorage and `CONNECTION_STRING` for connection to db.

Run `docker-compose up -d`

## Run with IDE

In **RecipeBook.WebApi/appsettings.json** change `BasePath` for file strorage.

In **RecipeBook.WebApi/appsettings.json** and on **RecipeBook.Migrations/appsettings.json** change `ConnetctionString` for **PostgtreSQL**.

Run `dotnet ef database update` from terminal in **RecipeBook.Migrations**.

Run project.
