# Bookworm API

A **RESTful API** built with **ASP.NET Core 8** for managing users,
authentication, and reading resources.\
It provides user registration/login, and book management for reading enthusiasts.
------------------------------------------------------------------------

##  Installation

### Requirements

-   .NET 8 SDK\
-    MySQL

### Clone the repository

``` bash
git clone https://github.com/LeoLoureiro-code/bookworm-api.git
cd bookworm-api
```

### Configure the database

-   Update your `appsettings.json` with the proper connection string.\
-   Run database migrations:\

``` bash
dotnet ef database update
```

### Run the server

``` bash
dotnet run
```

By default, the API runs at:\
 `https://localhost:7046/`

------------------------------------------------------------------------

##  Main Endpoints

### Authentication

**Login**\
`/Bookworm/Auth/login`

#### Request

``` json
{
  "email": "user@mail.com",
  "password": "123456"
}
```

#### Response

``` json
{
  "accessToken": "eyJhbGciOiJIUzI1...",
  "refreshToken": "eyJhbGciOiJIUzI1...",
  "userId": "123"
}
```

------------------------------------------------------------------------

**Register**\
`POST /Bookworm/Auth/register`

#### Request

``` json
{
  "email": "user@mail.com",
  "password": "123456"
}
```

#### Response

``` json
{
  "id": "123",
  "username": "user",
  "email": "user@mail.com"
}
```

------------------------------------------------------------------------

## Project Structure

    Bookworm-API/
    ├── Controllers/       # Controllers (Auth, Books, Users)
    └── Program.cs         # Main configuration

    bookwormAPI.EF.DataAccess/
    ├── Models/            # Data models
    ├── DTOs/              # Data transfer objects
    ├── Services/          # Business logic
    ├── Context/              # Database context
------------------------------------------------------------------------

## Swagger Documentation

Once the server is running, open in your browser:\
 <https://localhost:7046/swagger/index.html>

You'll be able to test all endpoints interactively.

------------------------------------------------------------------------

## License

Apache License

------------------------------------------------------------------------

## Author

**Leonardo Loureiro**\
- [LinkedIn](https://www.linkedin.com/in/leonardo-loureiro-/)\
- [GitHub](https://github.com/LeoLoureiro-code)
