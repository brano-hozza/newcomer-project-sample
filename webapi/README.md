# Setup

## Setting up app settings

1. First you need to create `appsettings.json` file in root directory
2. Now copy this text to the file:

    ```json
    {
    "Logging": {
        "LogLevel": {
        "Default": "Information",
        "Microsoft.AspNetCore": "Warning"
        }
    },
    "AllowedHosts": "*",
    "ConnectionStrings": {
        "WebApiDatabase": "Data Source={db_address}; Initial Catalog={db_name}; User Id={username}; Password={password}"
    }
    }
    ```

3. Change `{db_address}`, `{db_name}`, `{username}` and `{password}` with credentials to your DB

## Setting up database

- To create all required tables you need to run `dotnet ef database update`

## Running the project

- To start a API you need to run `dotnet run`
- Your API should start at http://localhost:5000
