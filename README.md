# StageBuilder

This API is for fetching and posting stage data for 2D games

The API and the database are containerized using Docker.
The framework is netcoreapp3.1 and the database is SQL Server.

I used EntityFrameworkCore as the ORM with small SQL queries for search.

I enabled Swagger documentation for an easy UI over the API.
Open a browser to 'http://localhost:8080' while the API is running with Docker.

## Running the API

You will need the Docker CLI and Docker-Compose CLI to run this API out of the box.

`NOTE:` If you do not want to use Docker, then you will need to change the server in the appsettings
connectionString to 'localhost' from 'db' and run SQL Server locally.

With Docker-compose, you should be able to simply run

```bash
> docker-compose build
> docker-compose up
```

Here are the images to pull from Docker if the compose file is getting hung from long downloads

* .NET SDK: mcr.microsoft.com/dotnet/core/sdk:3.1
* ASP.NET: mcr.microsoft.com/dotnet/core/aspnet:3.1
* SQL Server: mcr.microsoft.com/mssql/server:2019-latest

if the `build` or  `up` commands fail, simply terminate the process and rerun them

## API Design

* GET    /stages         => fetch all stages
* GET    /stages/:id     => fetch specific stage
* POST   /stages         => post stage
* PUT    /stages/:id     => update stage
* DELETE /stages/:id     => remove stage
