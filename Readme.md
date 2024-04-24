# Intro

Finshark is an API for delivering stock data. This is based on a project from [Teddy Smith](https://www.youtube.com/playlist?list=PL82C6-O4XrHfrGOCPmKmwTO7M0avXyQKc).

## Environment Setup

You will need:

1. Download SQL Server. Download from [Microsoft](https://www.microsoft.com/en-us/sql-server/sql-server-downloads). It's recommended to download the Express version
2. Edit `appsettings.sample.json` and rename it to `appsettings.sample.json` when you're ready for production use 

## Development commands

`dotnet run watch` will run your project in a hot reload environment
`dotnet ef migrations add {comment}` will run the migration script so that your migration script will include any changes that your app wants to make to the database. Comment can be any string that uniquely identifies the current migration that we want to perform.
`dotnet ef database update` will update the database will any recent migration script

## Quickstart

Before starting the app, update your database by running `dotnet ef migrations add {comment}` and `dotnet ef database update`. `dotnet ef database update` will run the actual SQL to update your database.

Once the database is updated, run `dotnet watch run` which will open Swagger (an api doc generation tool). You can then use the `api/account/login` endpoint to login or use `api/account/register` to register and retrieve a token. The response once you login or register will look like the following:

![Swagger login or register](image.png)

```json
{
  "userName": "user",
  "email": "user@example.com",
  "token": "someLongString"
}
```

Once you receive a token, click the authorize button in the top corner and use this to authenticate and paste your token

![alt text](image-1.png)

Now the Authorize button will show a locked symbol indicating that Swagger is now holding a token and will send that as an HTTP header with every request. In this way, you can use authorized routes like the portfolio routes.

![alt text](image-2.png)

![alt text](image-3.png)