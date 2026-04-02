# RetailHub

## Prerequisites

Install the following on your machine before you run anything.

| Requirement | Notes |
|-------------|--------|
| **.NET 8 SDK** | Required to build and run the API. [Download .NET 8](https://dotnet.microsoft.com/download/dotnet/8.0). |
| **SQL Server 2025** | The database project targets **SQL Server 2025** (`Sql170`). Install **SQL Server 2025** (e.g. **Developer** edition) and use the **default instance** so the server is reachable as **`localhost`**. |
| **Visual Studio 2022 or later** | Recommended: open the solution, publish the SQL project, and run the API. Install the **SQL Server Data Tools** (or **Data storage and processing**) workload so the `.sqlproj` loads and **Publish** works. |
| **SQL Server Management Studio** (optional) | Useful for running queries and checking data; not required to run the API. Use a recent SSMS build for best compatibility with SQL Server 2025. |

### Why `localhost`?

The API and the checked-in publish profile use **`Server=localhost`** / **`Data Source=localhost`**, which means the **default** SQL Server instance on your PC (the Windows service **SQL Server (MSSQLSERVER)**). If you only install a **named** instance (for example `MSSQLSERVER01`), you must change the connection string to match (see below).

---

## 1. Clone the repository

```bash
git clone https://github.com/peshovskim/retail-hub.git
cd retail-hub
```

---

## 2. Create the database (publish)

The schema and test seed data live in the SQL Server Database project.

1. Open **`src/services/RetailHub.sln`** in Visual Studio.
2. Open the **`RetailHub.Database`** project (`src/services/Database/RetailHub.Database/`).
3. Right‑click the project → **Publish**.
4. Use the profile **`RetailHub.Database.publish.xml`**, which deploys to **`localhost`** and database **`RetailHub`** (and creates the database if needed). Post‑deployment scripts apply seed data (for example categories).

Ensure SQL Server 2025 is running and that you can connect to **`localhost`** with **Windows Authentication** before publishing.

**Command line (optional):** From the database project folder you can build a DACPAC with `dotnet build`, then publish with **`sqlpackage.exe`** using the same target server and database name. The publish profile in the repo is the source of truth for the intended connection string.

---

## 3. Configure the API connection string (if needed)

Default configuration is in **`src/services/Api/RetailHub.Api/appsettings.json`**:

- **`ConnectionStrings:RetailHubDatabase`** → `Server=localhost;Database=RetailHub;...` (Windows authentication).

If your instance is **not** the default (for example **`localhost\SQLEXPRESS`** or **`localhost\MSSQLSERVER01`**), do **not** commit machine-specific values. Use **User Secrets** (Visual Studio: right‑click the API project → **Manage User Secrets**) or environment variables, for example:

```json
{
  "ConnectionStrings": {
    "RetailHubDatabase": "Server=localhost\\YOURINSTANCE;Database=RetailHub;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=true"
  }
}
```

In JSON, backslashes in the server name must be escaped (`\\`).

---

## 4. Run the API

**Visual Studio:** Set **`RetailHub.Api`** as the startup project and press **F5** (or **Ctrl+F5**).

**Command line:**

```bash
cd src/services/Api/RetailHub.Api
dotnet run
```

Check **`Properties/launchSettings.json`** for the URLs (for example `https://localhost:7296` and `http://localhost:5175`).

In **Development**, Swagger UI is enabled. Open **`/swagger`** on the running site to try endpoints (for example **`GET /api/catalog/categories`** and **`GET /api/catalog/categories/menu`**).

---

## 5. Frontend (optional)

An Angular client lives under **`src/clients/angular`**. Running it is separate from the API; follow the usual **`npm install`** / **`ng serve`** workflow when you work on the storefront. Point the Angular environment **`apiBaseUrl`** at your running API URL.

---

## Troubleshooting

| Issue | What to check |
|--------|----------------|
| Publish fails with a **version / target platform** error | The server must be **SQL Server 2025** (or another engine that matches **`Sql170`**). Older instances (for example SQL Server 2019) will not match the project target. |
| **`Cannot connect to localhost`** | Confirm the **SQL Server (MSSQLSERVER)** service exists and is **Running** in **`services.msc`**. If you only have a **named** instance, update the connection string (see above). |
| **`dotnet run` / build fails** with **MSB3027** or **“file is locked by RetailHub.Api”** | Another API instance is still running (terminal, **dotnet run**, or Visual Studio debug). Stop it first: end **`RetailHub.Api`** in Task Manager, or press **Shift+F5** in Visual Studio, or close the terminal that is hosting the API. Then build or run again. |
| API returns **empty** category lists | Confirm you published to the **same** server and database as **`RetailHubDatabase`**, and that seed scripts ran. In SSMS, query **`catalog.Category`** on database **`RetailHub`**. |
| **Database diagrams** in SSMS fail on SQL 2025 | Update SSMS to the latest version, or manage schema through the **RetailHub.Database** project instead of the diagram designer. |

---

## Repository layout (services)

| Path | Description |
|------|-------------|
| **`src/services/RetailHub.sln`** | Main Visual Studio solution |
| **`src/services/Api/RetailHub.Api/`** | ASP.NET Core Web API |
| **`src/services/Database/RetailHub.Database/`** | SQL Server Database project (schema + publish profile) |
| **`src/services/Modules/Catalog/`** | Catalog domain, application, and infrastructure |
