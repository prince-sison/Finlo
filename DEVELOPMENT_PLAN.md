# Finlo — Personal Finance Tracker: Development Plan

> **Stack:** .NET 10 Web API · React + TypeScript · SQLite  
> **Architecture:** Clean Architecture (Domain → Application → Infrastructure → Api)  
> **Approach:** API-first, then UI  

---

## Current State

- [x] Solution created (`Finlo.slnx`) with all 4 projects registered
- [x] Web API project scaffolded (`Finlo.Api` — .NET 10, Minimal APIs with OpenAPI)
- [x] Clean Architecture projects created (`Finlo.Domain`, `Finlo.Application`, `Finlo.Infrastructure`)
- [x] Project references wired up (Domain ← Application ← Infrastructure, Application + Infrastructure ← Api)
- [x] NuGet packages installed (EF Core Sqlite + Design in Infrastructure; EF Core, OpenApi, EF Core Design in Api)
- [x] Domain entities created (`Transaction`, `Budget`, `Category`, `TransactionType` enum)
- [x] Minimal API endpoint pattern established (`IEndpoint` interface + assembly-scanning registration)
- [x] EF Core `AppDbContext` created with DbSets + `DependencyInjection.cs` extension method
- [x] SQLite connection string added to `appsettings.json`, DbContext registered in `Program.cs`
- [x] Initial migration created & applied (`finlo.db` exists)
- [x] EF Core entity configurations created (`BudgetConfiguration`, `CategoryConfiguration`, `TransactionConfiguration`)
- [x] Application layer folder structure created (`Dtos/`, `Interfaces/`, `Services/`, `Repositories/` — empty)
- [x] Infrastructure layer folder structure partially created (`Data/`, `Data/Configurations/`, `Seed/` — missing `Repositories/`)
- [x] Frontend project scaffolded (`client/Finlo.UI` — Vite + React 19 + TypeScript)
- [ ] Everything below

---

## Phase 1 — Foundation (Backend API)

**Goal:** Working Transactions + Budgets CRUD with SQLite persistence.

### 1.1 Project Structure & Dependencies

The project uses Clean Architecture with four layers:

```
src/
├── Finlo.Domain/               ← Entities, enums, domain logic (no dependencies)
│   ├── Entities/
│   │   ├── Transaction.cs       ✅ done
│   │   ├── Budget.cs            ✅ done
│   │   └── Category.cs          ✅ done
│   └── Enums/
│       └── TransactionType.cs   ✅ done
│
├── Finlo.Application/          ← DTOs, interfaces, services, business logic
│   ├── DTOs/
│   │   ├── Transactions/
│   │   │   ├── CreateTransactionDto.cs
│   │   │   ├── UpdateTransactionDto.cs
│   │   │   └── TransactionResponseDto.cs
│   │   ├── Budgets/
│   │   │   ├── CreateBudgetDto.cs
│   │   │   ├── UpdateBudgetDto.cs
│   │   │   ├── BudgetResponseDto.cs
│   │   │   └── BudgetSummaryDto.cs
│   │   └── Common/
│   │       ├── PaginationParams.cs
│   │       └── PagedResult.cs
│   ├── Interfaces/
│   │   ├── ITransactionRepository.cs
│   │   ├── IBudgetRepository.cs
│   │   └── ICategoryRepository.cs
│   └── Services/
│       ├── TransactionService.cs
│       └── BudgetService.cs
│
├── Finlo.Infrastructure/       ← EF Core DbContext, repositories, migrations
│   ├── DependencyInjection.cs   ✅ done (AddInfrastructure extension)
│   ├── Data/
│   │   ├── AppDbContext.cs      ✅ done (DbSets + ApplyConfigurationsFromAssembly)
│   │   ├── Configurations/      ✅ done
│   │   │   ├── BudgetConfiguration.cs
│   │   │   ├── CategoryConfiguration.cs
│   │   │   └── TransactionConfiguration.cs
│   │   └── Migrations/          ✅ done (InitialCreate applied)
│   ├── Repositories/
│   │   ├── TransactionRepository.cs
│   │   ├── BudgetRepository.cs
│   │   └── CategoryRepository.cs
│   └── Seed/
│       └── CategorySeedData.cs  (unused — seeding done via scripts/SeedDatabase.ps1 instead)
│
└── Finlo.Api/                  ← Minimal API endpoints, middleware, DI configuration
    ├── Program.cs               ✅ done (endpoint scanning + Infrastructure DI)
    ├── Extensions/
    │   └── EndpointExtensions.cs ✅ done (assembly-scanning registration)
    ├── Endpoints/
    │   ├── IEndpoint.cs          ✅ done (endpoint contract)
    │   ├── Transaction/          (one class per endpoint action)
    │   │   ├── Create.cs          ⚠️ stub — needs real implementation
    │   │   ├── GetAll.cs
    │   │   ├── GetById.cs
    │   │   ├── Update.cs
    │   │   └── Delete.cs
    │   ├── Budget/
    │   │   ├── Create.cs
    │   │   ├── GetAll.cs
    │   │   ├── GetById.cs
    │   │   ├── Update.cs
    │   │   ├── Delete.cs
    │   │   └── GetSummary.cs
    │   └── Category/
    │       └── GetAll.cs
    └── Middleware/
        └── ExceptionHandlerMiddleware.cs
```

> **Endpoint pattern:** Each endpoint is a separate class implementing `IEndpoint`.
> Classes are auto-discovered via assembly scanning in `EndpointExtensions.cs`
> and registered/mapped in `Program.cs`.

**Project references (✅ all wired up):**

```
Finlo.Domain          → (no dependencies)
Finlo.Application     → Finlo.Domain
Finlo.Infrastructure  → Finlo.Application
Finlo.Api             → Finlo.Application, Finlo.Infrastructure
```

**Installed NuGet packages:**

```
# Infrastructure (✅ installed)
Microsoft.EntityFrameworkCore.Sqlite      10.0.5
Microsoft.EntityFrameworkCore.Design      10.0.5

# Api (✅ installed)
Microsoft.AspNetCore.OpenApi              10.0.4
Microsoft.EntityFrameworkCore             10.0.5
Microsoft.EntityFrameworkCore.Design      10.0.5
```

**Tasks:**

- [X] Register all projects in `Finlo.slnx`
- [X] Add project references (Domain ← Application ← Infrastructure, Application + Infrastructure ← Api)
- [X] Install NuGet packages (EF Core Sqlite, EF Core Design in Infrastructure; EF Core, OpenApi in Api)
- [X] Create `IEndpoint` interface and `EndpointExtensions` (assembly-scanning endpoint registration)
- [X] Configure `Program.cs` with endpoint scanning, OpenAPI, HTTPS redirection, Infrastructure DI
- [X] Create folder structure in Application project (`Dtos/`, `Interfaces/`, `Services/`, `Repositories/` — empty)
- [X] Create folder structure in Infrastructure project (`Data/`, `Data/Migrations/`, `Seed/`)
- [ ] Create `Repositories/` folder in Infrastructure project

---

### 1.2 Domain Entities

All domain entities are implemented in `src/Finlo.Domain/`.

**`TransactionType` enum** (`src/Finlo.Domain/Enums/TransactionType.cs`):

```csharp
public enum TransactionType
{
    Expense = 0,
    Income = 1
}
```

**`Transaction` entity** (`src/Finlo.Domain/Entities/Transaction.cs`):

| Property    | Type              | Notes                       |
|-------------|-------------------|-----------------------------|
| Id          | `Guid`            | PK, auto-generated          |
| Amount      | `decimal`         | Always positive              |
| Type        | `TransactionType` | Income or Expense            |
| Category    | `string`          | FK to Category.Name or free text |
| Date        | `DateTime`        | When the transaction occurred |
| Notes       | `string?`         | Optional                     |
| CreatedAt   | `DateTime`        | Auto-set on creation         |
| UpdatedAt   | `DateTime?`       | Auto-set on update           |

**`Budget` entity** (`src/Finlo.Domain/Entities/Budget.cs`):

| Property  | Type      | Notes                          |
|-----------|-----------|--------------------------------|
| Id        | `Guid`    | PK                             |
| Category  | `string`  | Budget category                |
| Limit     | `decimal` | Monthly budget limit           |
| Month     | `int`     | 1–12                           |
| Year      | `int`     | e.g. 2026                      |
| CreatedAt | `DateTime`| Auto-set                       |

**`Category` entity** (`src/Finlo.Domain/Entities/Category.cs`):

| Property | Type              | Notes       |
|----------|-------------------|-------------|
| Id       | `Guid`            | PK          |
| Name     | `string`          | Unique      |
| Type     | `TransactionType` | Income/Expense |

**Tasks:**

- [x] Create `TransactionType.cs` enum
- [x] Create `Transaction.cs` entity
- [x] Create `Budget.cs` entity
- [x] Create `Category.cs` entity

---

### 1.3 Database (EF Core + SQLite)

**`AppDbContext`** (in `src/Finlo.Infrastructure/Data/AppDbContext.cs`):

- DbSet\<Transaction\>, DbSet\<Budget\>, DbSet\<Category\>
- Configure indexes: `Date`, `Category`, composite `(Month, Year)` on Budgets
- Seed default categories (Food, Transport, Utilities, Salary, Entertainment, Health, Shopping, Other)

**Connection string** (`src/Finlo.Api/appsettings.json`):

```json
"ConnectionStrings": {
  "DefaultConnection": "Data Source=finlo.db"
}
```

**Tasks:**

- [X] Create `AppDbContext.cs` in `Finlo.Infrastructure/Data/` with DbSets + `ApplyConfigurationsFromAssembly`
- [X] Create `DependencyInjection.cs` in `Finlo.Infrastructure/` (`AddInfrastructure` extension registering DbContext with SQLite)
- [X] Add connection string to `appsettings.json` (`"Data Source=finlo.db"`)
- [X] Register DbContext in `Finlo.Api/Program.cs` via `builder.Services.AddInfrastructure(builder.Configuration)`
- [X] Create initial migration: `InitialCreate` (tables: Budgets, Categories, Transactions)
- [X] Apply migration (`finlo.db` database file created)
- [X] Create entity configurations (`BudgetConfiguration`, `CategoryConfiguration`, `TransactionConfiguration` in `Data/Configurations/`)
- [ ] Add indexes to configurations: `Date` + `Category` on Transactions, composite `(Month, Year)` on Budgets
- [ ] Implement seed data SQL files + PowerShell seed script — see [Phase 1.3a](#13a-database-seeding) below
- [ ] Create new migration to apply configuration changes (indexes, max lengths, column types) to database

---

### 1.3a Database Seeding

**Goal:** Seed the SQLite database with default data using SQL files and a PowerShell script.
Instead of seeding in C# code on startup, we keep seed data as plain `.sql` files (easy to read, edit, version-control)
and run them via a `SeedDatabase.ps1` script whenever needed.

#### Folder structure

```
scripts/
├── SeedDatabase.ps1              ← Main script (import/clear seed data)
└── seed_data/
    ├── Categories.sql            ← INSERT statements for default categories
    └── (future: Budgets.sql, Transactions.sql, etc.)
```

#### Step 1 — Create seed SQL files

Create `scripts/seed_data/Categories.sql`:

```sql
-- Default categories for Finlo
-- This file is idempotent: uses INSERT OR IGNORE so re-running won't duplicate rows.

INSERT OR IGNORE INTO Categories (Id, Name, Type) VALUES ('a1b2c3d4-0001-0000-0000-000000000001', 'Food', 0);
INSERT OR IGNORE INTO Categories (Id, Name, Type) VALUES ('a1b2c3d4-0002-0000-0000-000000000002', 'Transport', 0);
INSERT OR IGNORE INTO Categories (Id, Name, Type) VALUES ('a1b2c3d4-0003-0000-0000-000000000003', 'Utilities', 0);
INSERT OR IGNORE INTO Categories (Id, Name, Type) VALUES ('a1b2c3d4-0004-0000-0000-000000000004', 'Entertainment', 0);
INSERT OR IGNORE INTO Categories (Id, Name, Type) VALUES ('a1b2c3d4-0005-0000-0000-000000000005', 'Health', 0);
INSERT OR IGNORE INTO Categories (Id, Name, Type) VALUES ('a1b2c3d4-0006-0000-0000-000000000006', 'Shopping', 0);
INSERT OR IGNORE INTO Categories (Id, Name, Type) VALUES ('a1b2c3d4-0007-0000-0000-000000000007', 'Salary', 1);
INSERT OR IGNORE INTO Categories (Id, Name, Type) VALUES ('a1b2c3d4-0008-0000-0000-000000000008', 'Other', 0);
```

> **Why `INSERT OR IGNORE`?** SQLite skips the insert if the primary key already exists.
> This makes the script safe to run multiple times without duplicating data.
>
> **Why `Type = 0` / `Type = 1`?** The `TransactionType` enum stores as integer in SQLite:
> `Expense = 0`, `Income = 1`.

#### Step 2 — Create `SeedDatabase.ps1`

Create `scripts/SeedDatabase.ps1`:

```powershell
<#
.SYNOPSIS
    Seeds the Finlo SQLite database with initial data from SQL files.
.DESCRIPTION
    Reads .sql files from the seed_data/ folder and executes them against the SQLite database.
    Supports importing specific tables, clearing tables before import, or listing available seed files.
.EXAMPLE
    .\scripts\SeedDatabase.ps1                          # Seed all tables (interactive)
    .\scripts\SeedDatabase.ps1 -Tables Categories       # Seed only Categories
    .\scripts\SeedDatabase.ps1 -Tables "*"              # Seed all tables (no prompt)
    .\scripts\SeedDatabase.ps1 -Mode clear -Tables "*"  # Clear all seeded tables
    .\scripts\SeedDatabase.ps1 -Mode list               # List available seed files
#>
param(
    # What to do: 'import' seeds data, 'clear' deletes data, 'list' shows available tables.
    [ValidateSet("import", "clear", "list")]
    [string]$Mode = "import",

    # Comma-separated table names to seed (must match filenames in seed_data/ without .sql extension).
    # Use "*" to select all. If omitted, you'll be prompted interactively.
    [string]$Tables = $null,

    # Path to the SQLite database file.
    [string]$Database = $null
)

$ErrorActionPreference = "Stop"
$ScriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$SeedDataDir = Join-Path $ScriptDir "seed_data"

##############################
# Auto-detect database path
##############################

if (-not $Database) {
    # Default: look for finlo.db relative to repo root
    $RepoRoot = Split-Path -Parent $ScriptDir
    $Database = Join-Path $RepoRoot "src/Finlo.Api/finlo.db"
}

if (-not (Test-Path $Database)) {
    Write-Host "Database not found at: $Database" -ForegroundColor Red
    Write-Host "Run the API first to create the database: dotnet run --project src/Finlo.Api" -ForegroundColor Yellow
    exit 1
}

##############################
# Discover available seed files
##############################

$availableSeedFiles = Get-ChildItem -Path $SeedDataDir -Filter "*.sql" -ErrorAction SilentlyContinue |
    Sort-Object Name |
    ForEach-Object { $_.BaseName }

if ($availableSeedFiles.Count -eq 0) {
    Write-Host "No .sql seed files found in: $SeedDataDir" -ForegroundColor Red
    exit 1
}

##############################
# List mode
##############################

if ($Mode -eq "list") {
    Write-Host "Available seed files in $SeedDataDir :" -ForegroundColor Cyan
    $i = 1
    $availableSeedFiles | ForEach-Object {
        Write-Host "  $i) $_" -ForegroundColor White
        $i++
    }
    exit 0
}

##############################
# Resolve which tables to use
##############################

function Get-TablesToUse {
    if ($Tables) {
        if ($Tables -eq "*") {
            return $availableSeedFiles
        }
        $requested = $Tables -split ',\s*'
        foreach ($t in $requested) {
            if (-not ($availableSeedFiles -contains $t)) {
                Write-Host "Seed file not found: '$t'. Available:" -ForegroundColor Red
                $availableSeedFiles | ForEach-Object { Write-Host "  - $_" -ForegroundColor Yellow }
                exit 1
            }
        }
        return $requested
    }
    else {
        # Interactive prompt
        Write-Host "Select tables to $Mode :" -ForegroundColor Cyan
        $i = 1
        $availableSeedFiles | ForEach-Object {
            Write-Host "  $i) $_" -ForegroundColor White
            $i++
        }
        Write-Host "Enter numbers separated by commas (e.g. 1,2) or '*' for all: " -ForegroundColor Yellow -NoNewline
        $response = Read-Host
        if ($response.Trim() -eq "") {
            Write-Host "No input. Exiting." -ForegroundColor Red
            exit 1
        }
        if ($response.Trim() -eq "*") {
            return $availableSeedFiles
        }
        $indexes = $response.Trim() -split ',\s*' -as [int[]]
        return $indexes | ForEach-Object { $availableSeedFiles[$_ - 1] }
    }
}

$tablesToUse = Get-TablesToUse

##############################
# Summary
##############################

Write-Host ""
Write-Host "///////////////// SeedDatabase.ps1 /////////////////" -ForegroundColor Cyan
Write-Host "// Mode: $Mode" -ForegroundColor Cyan
Write-Host "// Database: $Database" -ForegroundColor Cyan
Write-Host "// Tables:" -ForegroundColor Cyan
$tablesToUse | ForEach-Object { Write-Host "//   - $_" -ForegroundColor Cyan }
Write-Host "////////////////////////////////////////////////////" -ForegroundColor Cyan
Write-Host ""

##############################
# Confirm destructive operations
##############################

if ($Mode -eq "clear") {
    Write-Host "WARNING: This will DELETE all data from the selected tables." -ForegroundColor Red
    Write-Host "Continue? (y/N) " -ForegroundColor Yellow -NoNewline
    $response = Read-Host
    if ($response -ne "Y" -and $response -ne "y") {
        Write-Host "Cancelled." -ForegroundColor Red
        exit 0
    }
}

##############################
# Execute
##############################

foreach ($table in $tablesToUse) {
    if ($Mode -eq "import") {
        $sqlFile = Join-Path $SeedDataDir "$table.sql"
        Write-Host "Seeding $table ..." -ForegroundColor Gray
        sqlite3 $Database ".read $sqlFile"
        if ($LASTEXITCODE -ne 0) {
            Write-Host "  FAILED to seed $table" -ForegroundColor Red
            exit 1
        }
        Write-Host "  $table seeded." -ForegroundColor Green
    }
    elseif ($Mode -eq "clear") {
        Write-Host "Clearing $table ..." -ForegroundColor Gray
        sqlite3 $Database "DELETE FROM $table;"
        if ($LASTEXITCODE -ne 0) {
            Write-Host "  FAILED to clear $table" -ForegroundColor Red
            exit 1
        }
        Write-Host "  $table cleared." -ForegroundColor Green
    }
}

Write-Host ""
Write-Host "Seed $Mode completed successfully." -ForegroundColor Green
```

> **Prerequisite:** This script requires `sqlite3` CLI. Install it:
> ```powershell
> winget install SQLite.SQLite
> ```
> After installation, restart your terminal so `sqlite3` is on your PATH.

#### Step 3 — Apply migrations first

Before seeding, the database must have the tables. Either:
- Run the API once (`dotnet run --project src/Finlo.Api`) — if you've wired auto-migration in `Program.cs`, or
- Run EF Core migrations manually:

```bash
dotnet ef database update --project src/Finlo.Infrastructure --startup-project src/Finlo.Api
```

#### Step 4 — Run the seed script

```powershell
# See what seed files are available
.\scripts\SeedDatabase.ps1 -Mode list

# Seed all tables (no prompt)
.\scripts\SeedDatabase.ps1 -Tables "*"

# Seed just Categories
.\scripts\SeedDatabase.ps1 -Tables Categories

# Interactive mode (prompts you to pick)
.\scripts\SeedDatabase.ps1

# Clear all seeded data
.\scripts\SeedDatabase.ps1 -Mode clear -Tables "*"
```

#### Step 5 — Verify

```powershell
# Check the data was inserted
sqlite3 src/Finlo.Api/finlo.db "SELECT * FROM Categories;"
```

You should see 8 rows (Food, Transport, Utilities, Entertainment, Health, Shopping, Salary, Other).

#### Adding more seed data later

To seed a new table (e.g., sample Transactions for development):

1. Create `scripts/seed_data/Transactions.sql` with `INSERT OR IGNORE` statements
2. Run `.\scripts\SeedDatabase.ps1 -Tables Transactions`

The script auto-discovers any `.sql` file in `seed_data/`.

**Tasks:**

- [ ] Install `sqlite3` CLI: `winget install SQLite.SQLite` (restart terminal after)
- [ ] Create `scripts/seed_data/` folder
- [ ] Create `scripts/seed_data/Categories.sql` with 8 default category INSERT statements
- [ ] Create `scripts/SeedDatabase.ps1` with import/clear/list modes
- [ ] Run `dotnet ef database update --project src/Finlo.Infrastructure --startup-project src/Finlo.Api` to ensure tables exist
- [ ] Run `.\scripts\SeedDatabase.ps1 -Tables "*"` to seed
- [ ] Run `sqlite3 src/Finlo.Api/finlo.db "SELECT * FROM Categories;"` to verify 8 rows

---

### 1.3b Docker Setup

**Goal:** Run the entire stack (API + UI) in Docker containers locally with a single command.

#### Step 1 — Create `.dockerignore` (repo root)

Create `Finlo/.dockerignore` to keep build context small:

```
**/bin/
**/obj/
**/node_modules/
**/dist/
**/.git
**/.vs
**/.vscode
*.db
*.user
*.suo
```

#### Step 2 — Create API Dockerfile

Create `src/Finlo.Api/Dockerfile` — multi-stage build:

```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src

# Copy csproj files and restore (layer caching)
COPY src/Finlo.Domain/Finlo.Domain.csproj Finlo.Domain/
COPY src/Finlo.Application/Finlo.Application.csproj Finlo.Application/
COPY src/Finlo.Infrastructure/Finlo.Infrastructure.csproj Finlo.Infrastructure/
COPY src/Finlo.Api/Finlo.Api.csproj Finlo.Api/
RUN dotnet restore Finlo.Api/Finlo.Api.csproj

# Copy source and publish
COPY src/ .
RUN dotnet publish Finlo.Api/Finlo.Api.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:5266
ENV ASPNETCORE_ENVIRONMENT=Development
EXPOSE 5266

ENTRYPOINT ["dotnet", "Finlo.Api.dll"]
```

> **Key concepts to understand:**
> - Multi-stage build: SDK image for building, smaller ASP.NET runtime image for running
> - `COPY *.csproj` + `RUN dotnet restore` first = Docker layer caching (restores are cached if csproj unchanged)
> - `ASPNETCORE_URLS` tells Kestrel which port to listen on inside the container

#### Step 3 — Create UI Dockerfile + nginx config

Create `client/Finlo.UI/nginx.conf` — SPA routing + API reverse proxy:

```nginx
server {
    listen 80;
    server_name localhost;
    root /usr/share/nginx/html;
    index index.html;

    location / {
        try_files $uri $uri/ /index.html;
    }

    location /api/ {
        proxy_pass http://api:5266/api/;
        proxy_set_header Host $host;
        proxy_set_header X-Real-IP $remote_addr;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;
    }
}
```

> **What this does:**
> - `try_files` → serves `index.html` for any route (SPA client-side routing)
> - `/api/` → proxies API calls to the `api` container (Docker Compose service name)

Create `client/Finlo.UI/Dockerfile`:

```dockerfile
FROM node:22-alpine AS build
WORKDIR /app

COPY client/Finlo.UI/package.json client/Finlo.UI/package-lock.json* ./
RUN npm install

COPY client/Finlo.UI/ .
RUN npm run build

FROM nginx:alpine AS runtime
COPY --from=build /app/dist /usr/share/nginx/html
COPY client/Finlo.UI/nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80
```

#### Step 4 — Create `docker-compose.yml` (repo root)

```yaml
services:
  api:
    build:
      context: .
      dockerfile: src/Finlo.Api/Dockerfile
    ports:
      - "5266:5266"
    environment:
      - ASPNETCORE_ENVIRONMENT=Development
      - ConnectionStrings__DefaultConnection=Data Source=/app/data/finlo.db
    volumes:
      - api-data:/app/data
    healthcheck:
      test: ["CMD-SHELL", "curl -f http://localhost:5266/openapi/v1.json || exit 1"]
      interval: 10s
      timeout: 5s
      retries: 5

  ui:
    build:
      context: .
      dockerfile: client/Finlo.UI/Dockerfile
    ports:
      - "3000:80"
    depends_on:
      api:
        condition: service_healthy

volumes:
  api-data:
```

> **Key concepts to understand:**
> - `ConnectionStrings__DefaultConnection` overrides `appsettings.json` via env var (double underscore = nested JSON key)
> - `volumes: api-data` persists the SQLite database across container restarts
> - `healthcheck` → UI container waits for API to be ready before starting
> - `depends_on` with `condition: service_healthy` → orderly startup

#### Step 5 — Create PowerShell scripts

Create `scripts/run.ps1` — orchestrates Docker:

```powershell
<#
.SYNOPSIS
    Runs the full Finlo stack via Docker Compose.
.PARAMETER Build
    Force rebuild of Docker images before starting.
.PARAMETER Stop
    Stop all running containers.
.PARAMETER Logs
    Follow container logs after starting.
#>
param(
    [switch]$Build,
    [switch]$Stop,
    [switch]$Logs
)

$ErrorActionPreference = "Stop"
$Root = Split-Path -Parent $PSScriptRoot

Push-Location $Root

if ($Stop) {
    Write-Host "Stopping Finlo containers..." -ForegroundColor Cyan
    docker compose down
    Pop-Location
    exit 0
}

Write-Host "Starting Finlo stack..." -ForegroundColor Cyan

$composeArgs = @("compose", "up", "-d")
if ($Build) {
    $composeArgs += "--build"
}

docker @composeArgs

if ($LASTEXITCODE -ne 0) {
    Pop-Location
    Write-Host "Failed to start containers." -ForegroundColor Red
    exit 1
}

Write-Host ""
Write-Host "Finlo is running:" -ForegroundColor Green
Write-Host "  API: http://localhost:5266" -ForegroundColor White
Write-Host "  UI:  http://localhost:3000" -ForegroundColor White
Write-Host "  API Docs: http://localhost:5266/openapi/v1.json" -ForegroundColor White
Write-Host ""
Write-Host "Stop with: .\scripts\run.ps1 -Stop" -ForegroundColor Gray

if ($Logs) {
    docker compose logs -f
}

Pop-Location
```

Create `scripts/seed.ps1` — seeds the database:

```powershell
<#
.SYNOPSIS
    Seeds the Finlo database with default categories.
.PARAMETER Docker
    If set, seeds the database inside the running Docker container.
#>
param(
    [switch]$Docker
)

$ErrorActionPreference = "Stop"
$Root = Split-Path -Parent $PSScriptRoot

if ($Docker) {
    Write-Host "Seeding database inside Docker container..." -ForegroundColor Cyan

    $containerId = docker compose ps -q api 2>$null
    if (-not $containerId) {
        Write-Host "API container is not running. Start it first with: .\scripts\run.ps1" -ForegroundColor Red
        exit 1
    }

    # The API auto-seeds on startup, so just verify
    $response = docker exec $containerId curl -sf http://localhost:5266/api/categories 2>$null
    if ($LASTEXITCODE -eq 0) {
        Write-Host "Database is seeded (API is running and responding)." -ForegroundColor Green
    }
    else {
        Write-Host "API is starting up — seed runs automatically on startup." -ForegroundColor Yellow
    }
}
else {
    Write-Host "Seeding database locally..." -ForegroundColor Cyan

    Push-Location $Root

    # Apply migrations
    Write-Host "Applying migrations..." -ForegroundColor Gray
    dotnet ef database update --project src/Finlo.Infrastructure --startup-project src/Finlo.Api
    if ($LASTEXITCODE -ne 0) {
        Pop-Location
        Write-Host "Migration failed." -ForegroundColor Red
        exit 1
    }

    # Run the API briefly — it seeds on startup, then stop it
    Write-Host "Starting API to trigger seed..." -ForegroundColor Gray
    $job = Start-Job -ScriptBlock {
        param($root)
        Set-Location $root
        dotnet run --project src/Finlo.Api --no-launch-profile 2>&1
    } -ArgumentList $Root

    # Wait for the API to start and seed
    $ready = $false
    for ($i = 0; $i -lt 30; $i++) {
        Start-Sleep -Seconds 1
        try {
            $null = Invoke-RestMethod -Uri "http://localhost:5266/openapi/v1.json" -TimeoutSec 2 -ErrorAction SilentlyContinue
            $ready = $true
            break
        }
        catch { }
    }

    Stop-Job $job -ErrorAction SilentlyContinue
    Remove-Job $job -Force -ErrorAction SilentlyContinue

    Pop-Location

    if ($ready) {
        Write-Host "Database seeded successfully." -ForegroundColor Green
    }
    else {
        Write-Host "Could not confirm seed completed. Check API logs." -ForegroundColor Yellow
    }
}
```

#### Step 6 — Test it

```powershell
# First run (builds images + starts containers + auto-seeds)
.\scripts\run.ps1 -Build

# Verify:
#   - http://localhost:5266/openapi/v1.json → API docs (JSON)
#   - http://localhost:3000 → UI (Vite React app)

# Check logs
.\scripts\run.ps1 -Logs

# Stop everything
.\scripts\run.ps1 -Stop

# Seed verification (Docker)
.\scripts\run.ps1 -Build
.\scripts\seed.ps1 -Docker

# Seed verification (local, without Docker)
.\scripts\seed.ps1
```

**Docker URLs when running:**

| Service | URL                                |
|---------|------------------------------------|N| UI      | http://localhost:3000              |
| API     | http://localhost:5266              |
| OpenAPI | http://localhost:5266/openapi/v1.json |

**Tasks:**

- [ ] Create `.dockerignore` at repo root
- [ ] Create `src/Finlo.Api/Dockerfile` (multi-stage: SDK build → ASP.NET runtime)
- [ ] Create `client/Finlo.UI/nginx.conf` (SPA routing + `/api/` reverse proxy)
- [ ] Create `client/Finlo.UI/Dockerfile` (multi-stage: Node build → nginx)
- [ ] Create `docker-compose.yml` at repo root (API + UI services, volume, healthcheck)
- [ ] Create `scripts/run.ps1` (start/stop/rebuild Docker stack)
- [ ] Create `scripts/seed.ps1` (seed database locally or in Docker)
- [ ] Run `scripts/run.ps1 -Build` and verify both containers start
- [ ] Open http://localhost:5266/openapi/v1.json — verify API responds
- [ ] Open http://localhost:3000 — verify UI loads
- [ ] Run `scripts/run.ps1 -Stop` to clean up

---

### 1.4 Transactions Module — CRUD

**API Endpoints:**

| Method | Route                      | Description                     |
|--------|----------------------------|---------------------------------|
| GET    | `/api/transactions`        | List (paginated, filterable)    |
| GET    | `/api/transactions/{id}`   | Get single                      |
| POST   | `/api/transactions`        | Create                          |
| PUT    | `/api/transactions/{id}`   | Update                          |
| DELETE | `/api/transactions/{id}`   | Delete                          |

**Query parameters for GET list:**

- `page` (default: 1)
- `pageSize` (default: 20, max: 100)
- `type` (Income/Expense, optional)
- `category` (optional)
- `startDate` / `endDate` (optional)
- `search` (searches Notes field, optional)

**Layers (Clean Architecture):**

1. **Endpoints** (`Finlo.Api/Endpoints/`) — Minimal API route groups, returns DTOs
2. **Service** (`Finlo.Application/Services/`) — Business logic, mapping entity ↔ DTO
3. **Repository Interface** (`Finlo.Application/Interfaces/`) — Contracts for data access
4. **Repository** (`Finlo.Infrastructure/Repositories/`) — EF Core queries

**Tasks:**

- [ ] Create DTOs in `Finlo.Application/DTOs/Transactions/`: `CreateTransactionDto`, `UpdateTransactionDto`, `TransactionResponseDto`
- [ ] Create `PaginationParams` and `PagedResult<T>` in `Finlo.Application/DTOs/Common/`
- [ ] Create `ITransactionRepository` interface in `Finlo.Application/Interfaces/`
- [ ] Create `TransactionRepository` in `Finlo.Infrastructure/Repositories/` (CRUD + filtered query)
- [ ] Create `TransactionService` in `Finlo.Application/Services/` (mapping + validation)
- [ ] Create endpoint classes in `Finlo.Api/Endpoints/Transaction/` (`Create`, `GetAll`, `GetById`, `Update`, `Delete` — each implements `IEndpoint`)
- [ ] Register services in DI in `Finlo.Api/Program.cs` (repositories + services)
- [ ] Test all endpoints manually (use `.http` file or Swagger)

---

### 1.5 Budgets Module — CRUD

**API Endpoints:**

| Method | Route                   | Description                      |
|--------|-------------------------|----------------------------------|
| GET    | `/api/budgets`          | List (filter by month/year)      |
| GET    | `/api/budgets/{id}`     | Get single                       |
| POST   | `/api/budgets`          | Create                           |
| PUT    | `/api/budgets/{id}`     | Update                           |
| DELETE | `/api/budgets/{id}`     | Delete                           |
| GET    | `/api/budgets/summary`  | Budget vs Actual for month/year  |

**Budget Summary response shape:**

```json
{
  "month": 3,
  "year": 2026,
  "budgets": [
    {
      "category": "Food",
      "limit": 500.00,
      "spent": 320.50,
      "remaining": 179.50,
      "percentUsed": 64.1
    }
  ],
  "totalBudget": 2000.00,
  "totalSpent": 1450.00
}
```

**Tasks:**

- [ ] Create DTOs in `Finlo.Application/DTOs/Budgets/`: `CreateBudgetDto`, `UpdateBudgetDto`, `BudgetResponseDto`, `BudgetSummaryDto`
- [ ] Create `IBudgetRepository` interface in `Finlo.Application/Interfaces/`
- [ ] Create `BudgetRepository` in `Finlo.Infrastructure/Repositories/`
- [ ] Create `BudgetService` in `Finlo.Application/Services/` (includes summary calculation — queries Transactions via repository)
- [ ] Create endpoint classes in `Finlo.Api/Endpoints/Budget/` (`Create`, `GetAll`, `GetById`, `Update`, `Delete`, `GetSummary` — each implements `IEndpoint`)
- [ ] Register services in DI in `Finlo.Api/Program.cs`
- [ ] Test all endpoints

---

### 1.6 Categories Endpoint

| Method | Route              | Description        |
|--------|--------------------|--------------------|
| GET    | `/api/categories`  | List all categories |

Simple read-only endpoint returning seeded categories. No full CRUD needed for V1.

**Tasks:**

- [ ] Create `ICategoryRepository` interface in `Finlo.Application/Interfaces/`
- [ ] Create `CategoryRepository` in `Finlo.Infrastructure/Repositories/`
- [ ] Create `GetAll` endpoint class in `Finlo.Api/Endpoints/Category/` (implements `IEndpoint`)
- [ ] Test endpoint

---

### 1.7 Validation & Error Handling

- [ ] Add `FluentValidation` or use Data Annotations on DTOs (in `Finlo.Application`)
  - Amount > 0
  - Category required
  - Date required
  - Budget Limit > 0
  - Month 1–12
- [ ] Create global exception handler middleware in `Finlo.Api/Middleware/`
- [ ] Return consistent error response shape:

```json
{
  "status": 400,
  "message": "Validation failed",
  "errors": { "Amount": ["Amount must be greater than 0"] }
}
```

---

### 1.8 Phase 1 Verification Checklist

- [ ] `POST /api/transactions` — creates a transaction
- [ ] `GET /api/transactions` — returns paginated list with filters
- [ ] `PUT /api/transactions/{id}` — updates correctly
- [ ] `DELETE /api/transactions/{id}` — deletes correctly
- [ ] `POST /api/budgets` — creates a budget
- [ ] `GET /api/budgets/summary?month=3&year=2026` — returns budget vs actual
- [ ] `GET /api/categories` — returns seeded categories
- [ ] Invalid requests return proper 400 errors
- [ ] SQLite database file (`finlo.db`) is created and persists data

---

## Phase 2 — Reports API

**Goal:** Aggregation endpoints for charts and insights.

### 2.1 Reports Endpoints

| Method | Route                              | Description                         |
|--------|------------------------------------|-------------------------------------|
| GET    | `/api/reports/monthly-summary`     | Total income/expense for a month    |
| GET    | `/api/reports/category-breakdown`  | Spending per category for a month   |
| GET    | `/api/reports/trends`              | Monthly totals over last N months   |

**Monthly Summary response:**

```json
{
  "month": 3,
  "year": 2026,
  "totalIncome": 5000.00,
  "totalExpense": 3200.00,
  "netSavings": 1800.00
}
```

**Category Breakdown response:**

```json
{
  "month": 3,
  "year": 2026,
  "breakdown": [
    { "category": "Food", "amount": 450.00, "percentage": 14.06 },
    { "category": "Transport", "amount": 200.00, "percentage": 6.25 }
  ]
}
```

**Trends response:**

```json
{
  "months": 6,
  "data": [
    { "month": 10, "year": 2025, "income": 4500, "expense": 3000 },
    { "month": 11, "year": 2025, "income": 4800, "expense": 3200 }
  ]
}
```

**Tasks:**

- [ ] Create `Finlo.Application/DTOs/Reports/` with response DTOs
- [ ] Create `Finlo.Application/Interfaces/IReportRepository.cs`
- [ ] Create `ReportService` in `Finlo.Application/Services/` (aggregate queries against Transactions)
- [ ] Create `ReportRepository` in `Finlo.Infrastructure/Repositories/`
- [ ] Create endpoint classes in `Finlo.Api/Endpoints/Report/` (`MonthlySummary`, `CategoryBreakdown`, `Trends` — each implements `IEndpoint`)
- [ ] Test all report endpoints with sample data

---

### 2.2 Phase 2 Verification Checklist

- [ ] Monthly summary returns correct totals
- [ ] Category breakdown percentages add up to 100%
- [ ] Trends returns correct number of months
- [ ] Empty months return zero values (not missing)

---

## Phase 3 — Frontend (React + TypeScript)

**Goal:** Functional UI with Dashboard, Transactions, Budgets, Reports pages.

### 3.1 Project Setup

The React + TypeScript project is already scaffolded at `client/Finlo.UI/` (Vite + React 19 + TypeScript).

**Install key dependencies:**

```bash
cd client/Finlo.UI
npm install axios react-router-dom zustand recharts
npm install -D tailwindcss @tailwindcss/vite
```

**Folder structure:**

```
client/Finlo.UI/
├── src/
│   ├── App.tsx
│   ├── main.tsx
│   ├── api/
│   │   ├── client.ts          (axios instance, base URL config)
│   │   ├── transactions.ts    (API call functions)
│   │   ├── budgets.ts
│   │   └── reports.ts
│   ├── features/
│   │   ├── dashboard/
│   │   │   └── DashboardPage.tsx
│   │   ├── transactions/
│   │   │   ├── TransactionsPage.tsx
│   │   │   ├── TransactionForm.tsx
│   │   │   └── TransactionList.tsx
│   │   ├── budgets/
│   │   │   ├── BudgetsPage.tsx
│   │   │   └── BudgetCard.tsx
│   │   └── reports/
│   │       ├── ReportsPage.tsx
│   │       ├── MonthlyChart.tsx
│   │       └── CategoryPieChart.tsx
│   ├── components/
│   │   ├── Layout.tsx
│   │   ├── Sidebar.tsx
│   │   └── ui/               (reusable UI primitives)
│   ├── store/
│   │   ├── transactionStore.ts
│   │   └── budgetStore.ts
│   ├── hooks/
│   │   └── useDebounce.ts
│   └── types/
│       └── index.ts          (shared TS types matching API DTOs)
```

**Tasks:**

- [x] Scaffold Vite + React + TS project (`client/Finlo.UI/`)
- [ ] Install dependencies (axios, router, zustand, recharts, tailwind)
- [ ] Set up Tailwind CSS
- [ ] Create folder structure
- [ ] Create axios client with base URL pointing to API
- [ ] Configure CORS on .NET API for local dev
- [ ] Set up React Router with routes for all pages
- [ ] Create `Layout` + `Sidebar` components

---

### 3.2 Transactions Page

- [ ] Create `TransactionList` — table with columns: Date, Category, Type, Amount, Notes, Actions
- [ ] Create `TransactionForm` — modal or inline form for add/edit
- [ ] Implement filters: date range, category dropdown, type toggle
- [ ] Implement pagination controls
- [ ] Wire up to API via Zustand store
- [ ] Quick-add shortcut: clicking "+" opens form with today's date pre-filled

**UX target:** Add a transaction in under 5 seconds.

---

### 3.3 Budgets Page

- [ ] Display budget cards: category, progress bar (spent/limit), remaining amount
- [ ] Color coding: green (< 75%), yellow (75–90%), red (> 90%)
- [ ] Create/edit budget form (category dropdown, limit input, month/year)
- [ ] Wire up to `/api/budgets/summary` endpoint

---

### 3.4 Dashboard

- [ ] Total balance card (income - expense for current month)
- [ ] Recent transactions list (last 5–10)
- [ ] Budget health summary (top 3 budgets by % used)
- [ ] Quick-add transaction button

**UX target:** One-glance overview of financial health.

---

### 3.5 Reports Page

- [ ] Monthly Income vs Expense bar chart (Recharts)
- [ ] Category breakdown pie chart
- [ ] Trends line chart (last 6 months)
- [ ] Month/year selector to navigate reports

---

### 3.6 Phase 3 Verification Checklist

- [ ] Can create, edit, delete transactions from UI
- [ ] Transactions list filters and paginates correctly
- [ ] Budget progress bars reflect actual spending
- [ ] Dashboard shows accurate summary
- [ ] Charts render with real data
- [ ] App is responsive (works on mobile widths)

---

## Phase 4 — Polish & Enhancements

**Goal:** Quality-of-life improvements.

- [ ] Recurring transactions (auto-create monthly entries)
- [ ] Export transactions to CSV
- [ ] Dark mode toggle (Tailwind `dark:` classes)
- [ ] Keyboard shortcuts (Ctrl+N for new transaction)
- [ ] Toast notifications for success/error actions
- [ ] Loading skeletons for data fetches
- [ ] Confirm dialog before deleting

---

## Phase 5 — Advanced (Future)

- [ ] Auto-categorization (rule-based, then ML)
- [ ] Budget alerts / notifications
- [ ] Multi-currency support
- [ ] Offline-first with sync
- [ ] Bank integration (Plaid API)
- [ ] AI insights ("You spent 30% more on food this month")
- [ ] PWA support (installable on mobile)
- [ ] Authentication (JWT) if multi-user needed

---

## Technical Reference

### API Response Conventions

| Scenario          | Status | Body                        |
|-------------------|--------|-----------------------------|
| Success (list)    | 200    | `{ data: [...], pagination }` |
| Success (single)  | 200    | `{ data: {...} }`            |
| Created           | 201    | `{ data: {...} }`            |
| No content        | 204    | (empty)                      |
| Validation error  | 400    | `{ status, message, errors }` |
| Not found         | 404    | `{ status, message }`        |
| Server error      | 500    | `{ status, message }`        |

### EF Core Commands Reference

```bash
# Add migration
dotnet ef migrations add <Name> --project src/Finlo.Infrastructure --startup-project src/Finlo.Api

# Apply migrations
dotnet ef database update --startup-project src/Finlo.Api

# Remove last migration (if not applied)
dotnet ef migrations remove --project src/Finlo.Infrastructure --startup-project src/Finlo.Api
```

### Dev Workflow

```bash
# Run API (from repo root)
dotnet run --project src/Finlo.Api

# Run API with watch (from repo root)
dotnet watch run --project src/Finlo.Api

# Run Frontend (from client/Finlo.UI)
npm run dev
```

### Git Branch Strategy

```
main          ← stable releases
└── develop   ← integration branch
    ├── feature/transactions-crud
    ├── feature/budgets-crud
    ├── feature/reports-api
    ├── feature/frontend-shell
    ├── feature/transactions-ui
    ├── feature/budgets-ui
    └── feature/reports-ui
```

---

## Build Order (Recommended Sequence)

This is the exact order to build, task by task:

```
 1. [Phase 1.1] Solution registration + project references + NuGet packages + endpoint pattern  ✅ DONE
 2. [Phase 1.2] Domain entities + enum                    ✅ DONE
 3. [Phase 1.3] AppDbContext + SQLite + migration + configs  ✅ DONE (indexes remaining)
 3a.[Phase 1.3a] SQL seed files + SeedDatabase.ps1 script
 3b.[Phase 1.3b] Docker setup (Dockerfiles, compose, scripts)
 4. [Phase 1.4] Transactions CRUD (interfaces/DTOs → repo → service → endpoints)
 5. [Phase 1.5] Budgets CRUD + summary endpoint
 6. [Phase 1.6] Categories endpoint
 7. [Phase 1.7] Validation + error handling
 8. [Phase 1.8] ✅ Verify all API endpoints
 9. [Phase 2.1] Reports endpoints
10. [Phase 2.2] ✅ Verify reports
11. [Phase 3.1] Frontend deps + routing + layout           (scaffold ✅ DONE)
12. [Phase 3.2] Transactions UI
13. [Phase 3.3] Budgets UI
14. [Phase 3.4] Dashboard
15. [Phase 3.5] Reports + charts
16. [Phase 3.6] ✅ Full integration test
17. [Phase 4]   Polish & enhancements
18. [Phase 5]   Advanced features
```

**Next up: Step 3a — Create seed SQL files + SeedDatabase.ps1 script.**
