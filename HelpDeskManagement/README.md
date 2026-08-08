# HelpDesk Management

Help Desk Ticket Management System built using ASP.NET Core Web API, ASP.NET Core MVC, Entity Framework Core, SQL Server, xUnit, Moq and GitHub.

## Solution Structure

| Project | Type | Purpose |
|---|---|---|
| `HelpDesk.Api` | ASP.NET Core Web API | Implements REST APIs, Entity Framework Core, SQL Server, and Repository Pattern |
| `HelpDesk.Mvc` | ASP.NET Core MVC | Consumes the Web API through a Service Layer (HttpClient) |
| `HelpDesk.Tests` | xUnit Test Project | Contains unit tests using xUnit and Moq |

## Features

- Raise new tickets
- View all tickets
- View ticket details
- Update ticket information
- Delete tickets
- Filter tickets by status
- Dashboard showing total / open / closed ticket counts

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- SQL Server or SQL Server LocalDB (Windows) — on Mac/Linux, use a Docker SQL Server container, or swap the provider to SQLite/PostgreSQL
- [Visual Studio Code](https://code.visualstudio.com/) with the **C# Dev Kit** extension (or **C#** extension), or Visual Studio 2022
- Git

## Running the Project Locally

### 1. Clone the repository

```bash
git clone https://github.com/<your-username>/HelpDeskManagement.git
cd HelpDeskManagement
```

### 2. Open in VS Code and restore packages

Open the folder in VS Code:

```bash
code .
```

Accept the prompt to add build/debug assets if VS Code shows it, then restore all project dependencies from the solution root:

```bash
dotnet restore
```

### 3. Configure the database connection

Open `HelpDesk.Api/appsettings.json` and update the `DefaultConnection` string to match your environment. The default targets SQL Server LocalDB:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=HelpDeskDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
}
```

If you're on Mac/Linux or using a full SQL Server instance / Docker container, replace it with something like:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost,1433;Database=HelpDeskDb;User Id=sa;Password=YourPassword123;TrustServerCertificate=True"
}
```

### 4. Create the database

Install the EF Core CLI tool once (if not already installed):

```bash
dotnet tool install --global dotnet-ef
```

Then, from the `HelpDesk.Api` folder, create and apply the migration:

```bash
cd HelpDesk.Api
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### 5. Run the Web API

From the `HelpDesk.Api` folder:

```bash
dotnet run
```

The API starts at `https://localhost:7001` (Swagger UI at `/swagger`). Note the actual port shown in the console output.

### 6. Point the MVC app at the API

Open `HelpDesk.Mvc/appsettings.json` and set `ApiSettings:BaseUrl` to match the API's URL from the previous step:

```json
"ApiSettings": {
  "BaseUrl": "https://localhost:7001/"
}
```

### 7. Run the MVC application

In a separate terminal, from the `HelpDesk.Mvc` folder:

```bash
dotnet run
```

Open the URL shown in the console (default `https://localhost:7101`) in your browser.

### 8. Run the unit tests

From the `HelpDesk.Tests` folder:

```bash
dotnet test
```

These tests mock the repository layer with Moq, so they run without needing SQL Server.

## API Endpoints

| HTTP Method | Endpoint | Description |
|---|---|---|
| GET | `/api/Ticket/All` | Get all tickets |
| GET | `/api/Ticket/{id}` | Get ticket by Id |
| POST | `/api/Ticket` | Create a new ticket |
| PUT | `/api/Ticket/{id}` | Update an existing ticket |
| DELETE | `/api/Ticket/{id}` | Delete a ticket |
| GET | `/api/Ticket/Status/{status}` | Get all tickets by status |

## Valid Values

**Priority:** Low, Medium, High

**Status:** Open, In Progress, Closed

## Pushing This Project to GitHub

If you're setting this up as a brand-new repository (skip to step 3 if you already have a local Git repo with a commit):

### 1. Initialize Git in the project folder

```bash
cd HelpDeskManagement
git init
```

### 2. Stage and commit all files

```bash
git add .
git commit -m "Initial Commit"
```

### 3. Create a new repository on GitHub

Go to [github.com/new](https://github.com/new), name it `HelpDeskManagement`, leave it empty (no README/.gitignore/license — you already have these locally), and click **Create repository**.

### 4. Connect your local repository to GitHub

Copy the repository URL GitHub gives you, then run:

```bash
git remote add origin https://github.com/<your-username>/HelpDeskManagement.git
```

### 5. Push your code

```bash
git branch -M main
git push -u origin main
```

(Use `git push -u origin master` instead if your default branch is `master` and you want to keep that name.)

### 6. Verify

Refresh the GitHub repository page — you should see `HelpDesk.Api`, `HelpDesk.Mvc`, `HelpDesk.Tests`, `HelpDeskManagement.sln`, `README.md`, and `.gitignore`.

### Making future changes

```bash
git add .
git commit -m "Describe your change here"
git push
```
