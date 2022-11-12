# RecipeBookDotnet

## Run with Docker

In **docker-compose.yml** change the environment value of `FILE_STORAGE` for strorage of static files and `CONNECTION_STRING` for connection to db.
You can also change the output ports of recipe-api and recipe-pg.

Run `docker-compose up`

## Run with IDE

In **RecipeBook.WebApi/appsettings.json** change `BasePath` for file strorage.

In **RecipeBook.WebApi/appsettings.json** and on *your **PostgtreSQL**.

Run `dotnet ef database update` from terminal in **RecipeBook.Migrations**.

Run **RecipeBook.WebApi** project.
