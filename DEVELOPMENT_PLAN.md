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

# Part 1 — Feature Docs

What the app does: modules, endpoints, UI pages, and roadmap.

---

## Transactions Module

### API Endpoints

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

### UI — Transactions Page

- `TransactionList` — table with columns: Date, Category, Type, Amount, Notes, Actions
- `TransactionForm` — modal or inline form for add/edit
- Filters: date range, category dropdown, type toggle
- Pagination controls
- Quick-add shortcut: clicking "+" opens form with today's date pre-filled

**UX target:** Add a transaction in under 5 seconds.

### Tasks — Backend

- [ ] Create DTOs in `Finlo.Application/DTOs/Transactions/`: `CreateTransactionDto`, `UpdateTransactionDto`, `TransactionResponseDto`
- [ ] Create `PaginationParams` and `PagedResult<T>` in `Finlo.Application/DTOs/Common/`
- [ ] Create `ITransactionRepository` interface in `Finlo.Application/Interfaces/`
- [ ] Create `TransactionRepository` in `Finlo.Infrastructure/Repositories/` (CRUD + filtered query)
- [ ] Create `TransactionService` in `Finlo.Application/Services/` (mapping + validation)
- [ ] Create endpoint classes in `Finlo.Api/Endpoints/Transaction/` (`Create`, `GetAll`, `GetById`, `Update`, `Delete` — each implements `IEndpoint`)
- [ ] Register services in DI in `Finlo.Api/Program.cs` (repositories + services)
- [ ] Test all endpoints manually (use `.http` file or Swagger)

### Tasks — Frontend

- [ ] Create `TransactionList` — table with columns: Date, Category, Type, Amount, Notes, Actions
- [ ] Create `TransactionForm` — modal or inline form for add/edit
- [ ] Implement filters: date range, category dropdown, type toggle
- [ ] Implement pagination controls
- [ ] Wire up to API via Zustand store
- [ ] Quick-add shortcut: clicking "+" opens form with today's date pre-filled

---

## Budgets Module

### API Endpoints

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

### UI — Budgets Page

- Budget cards: category, progress bar (spent/limit), remaining amount
- Color coding: green (< 75%), yellow (75–90%), red (> 90%)
- Create/edit budget form (category dropdown, limit input, month/year)
- Wired to `/api/budgets/summary` endpoint

### Tasks — Backend

- [ ] Create DTOs in `Finlo.Application/DTOs/Budgets/`: `CreateBudgetDto`, `UpdateBudgetDto`, `BudgetResponseDto`, `BudgetSummaryDto`
- [ ] Create `IBudgetRepository` interface in `Finlo.Application/Interfaces/`
- [ ] Create `BudgetRepository` in `Finlo.Infrastructure/Repositories/`
- [ ] Create `BudgetService` in `Finlo.Application/Services/` (includes summary calculation — queries Transactions via repository)
- [ ] Create endpoint classes in `Finlo.Api/Endpoints/Budget/` (`Create`, `GetAll`, `GetById`, `Update`, `Delete`, `GetSummary` — each implements `IEndpoint`)
- [ ] Register services in DI in `Finlo.Api/Program.cs`
- [ ] Test all endpoints

### Tasks — Frontend

- [ ] Display budget cards: category, progress bar (spent/limit), remaining amount
- [ ] Color coding: green (< 75%), yellow (75–90%), red (> 90%)
- [ ] Create/edit budget form (category dropdown, limit input, month/year)
- [ ] Wire up to `/api/budgets/summary` endpoint

---

## Categories Module

### API Endpoints

| Method | Route              | Description        |
|--------|--------------------|--------------------|
| GET    | `/api/categories`  | List all categories |

Simple read-only endpoint returning seeded categories. No full CRUD needed for V1.

### Tasks

- [ ] Create `ICategoryRepository` interface in `Finlo.Application/Interfaces/`
- [ ] Create `CategoryRepository` in `Finlo.Infrastructure/Repositories/`
- [ ] Create `GetAll` endpoint class in `Finlo.Api/Endpoints/Category/` (implements `IEndpoint`)
- [ ] Test endpoint

---

## Reports Module

### API Endpoints

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

### UI — Reports Page

- Monthly Income vs Expense bar chart (Recharts)
- Category breakdown pie chart
- Trends line chart (last 6 months)
- Month/year selector to navigate reports

### Tasks — Backend

- [ ] Create `Finlo.Application/DTOs/Reports/` with response DTOs
- [ ] Create `Finlo.Application/Interfaces/IReportRepository.cs`
- [ ] Create `ReportService` in `Finlo.Application/Services/` (aggregate queries against Transactions)
- [ ] Create `ReportRepository` in `Finlo.Infrastructure/Repositories/`
- [ ] Create endpoint classes in `Finlo.Api/Endpoints/Report/` (`MonthlySummary`, `CategoryBreakdown`, `Trends` — each implements `IEndpoint`)
- [ ] Test all report endpoints with sample data

### Tasks — Frontend

- [ ] Monthly Income vs Expense bar chart (Recharts)
- [ ] Category breakdown pie chart
- [ ] Trends line chart (last 6 months)
- [ ] Month/year selector to navigate reports

---

## Dashboard

- Total balance card (income - expense for current month)
- Recent transactions list (last 5–10)
- Budget health summary (top 3 budgets by % used)
- Quick-add transaction button

**UX target:** One-glance overview of financial health.

### Tasks

- [ ] Total balance card (income - expense for current month)
- [ ] Recent transactions list (last 5–10)
- [ ] Budget health summary (top 3 budgets by % used)
- [ ] Quick-add transaction button

---

## Validation & Error Handling

- Add `FluentValidation` or use Data Annotations on DTOs (in `Finlo.Application`)
  - Amount > 0
  - Category required
  - Date required
  - Budget Limit > 0
  - Month 1–12
- Create global exception handler middleware in `Finlo.Api/Middleware/`
- Return consistent error response shape:

```json
{
  "status": 400,
  "message": "Validation failed",
  "errors": { "Amount": ["Amount must be greater than 0"] }
}
```

### Tasks

- [ ] Add `FluentValidation` or use Data Annotations on DTOs (in `Finlo.Application`)
- [ ] Create global exception handler middleware in `Finlo.Api/Middleware/`
- [ ] Return consistent error response shape

---

## Polish & Enhancements (Phase 4)

**Goal:** Quality-of-life improvements.

- [ ] Recurring transactions (auto-create monthly entries)
- [ ] Export transactions to CSV
- [ ] Dark mode toggle (Tailwind `dark:` classes)
- [ ] Keyboard shortcuts (Ctrl+N for new transaction)
- [ ] Toast notifications for success/error actions
- [ ] Loading skeletons for data fetches
- [ ] Confirm dialog before deleting

---

## Advanced / Future (Phase 5)

- [ ] Auto-categorization (rule-based, then ML)
- [ ] Budget alerts / notifications
- [ ] Multi-currency support
- [ ] Offline-first with sync
- [ ] Bank integration (Plaid API)
- [ ] AI insights ("You spent 30% more on food this month")
- [ ] PWA support (installable on mobile)
- [ ] Authentication (JWT) if multi-user needed

---

## Verification Checklists

### Phase 1 — Backend API

- [ ] `POST /api/transactions` — creates a transaction
- [ ] `GET /api/transactions` — returns paginated list with filters
- [ ] `PUT /api/transactions/{id}` — updates correctly
- [ ] `DELETE /api/transactions/{id}` — deletes correctly
- [ ] `POST /api/budgets` — creates a budget
- [ ] `GET /api/budgets/summary?month=3&year=2026` — returns budget vs actual
- [ ] `GET /api/categories` — returns seeded categories
- [ ] Invalid requests return proper 400 errors
- [ ] SQLite database file (`finlo.db`) is created and persists data

### Phase 2 — Reports API

- [ ] Monthly summary returns correct totals
- [ ] Category breakdown percentages add up to 100%
- [ ] Trends returns correct number of months
- [ ] Empty months return zero values (not missing)

### Phase 3 — Frontend

- [ ] Can create, edit, delete transactions from UI
- [ ] Transactions list filters and paginates correctly
- [ ] Budget progress bars reflect actual spending
- [ ] Dashboard shows accurate summary
- [ ] Charts render with real data
- [ ] App is responsive (works on mobile widths)

---

# Part 2 — Technical Docs

How to build, run, seed, and deploy the app.

---

## Dev Workflow

```bash
# Run API (from repo root)
dotnet run --project src/Finlo.Api

# Run API with watch (from repo root)
dotnet watch run --project src/Finlo.Api

# Run Frontend (from client/Finlo.UI)
npm run dev
```

---

## EF Core Commands Reference

```bash
# Add migration
dotnet ef migrations add <Name> --project src/Finlo.Infrastructure --startup-project src/Finlo.Api

# Apply migrations
dotnet ef database update --startup-project src/Finlo.Api

# Remove last migration (if not applied)
dotnet ef migrations remove --project src/Finlo.Infrastructure --startup-project src/Finlo.Api
```

---

## Database Seeding

**Goal:** Seed the SQLite database with default categories using EF Core's `HasData` in entity configurations.
Seed data is defined in C# and applied automatically via EF Core migrations — no external tools required.

### How it works

```
src/Finlo.Infrastructure/
├── Seed/
│   └── CategorySeedData.cs       ← static list of 8 default categories
└── Data/
    └── Configurations/
        └── CategoryConfiguration.cs  ← calls builder.HasData(CategorySeedData.GetCategories())
```

1. `CategorySeedData.GetCategories()` returns the 8 default `Category` entities with fixed GUIDs
2. `CategoryConfiguration.Configure()` calls `builder.HasData(...)` to register them with EF Core
3. EF Core generates `InsertData` statements in the migration, applied on `dotnet ef database update`

### Step 1 — Create `CategorySeedData.cs`

Create `src/Finlo.Infrastructure/Seed/CategorySeedData.cs`:

```csharp
public static class CategorySeedData
{
    public static Category[] GetCategories() =>
    [
        new() { Id = Guid.Parse("a1b2c3d4-0001-0000-0000-000000000001"), Name = "Food", Type = TransactionType.Expense },
        new() { Id = Guid.Parse("a1b2c3d4-0002-0000-0000-000000000002"), Name = "Transport", Type = TransactionType.Expense },
        new() { Id = Guid.Parse("a1b2c3d4-0003-0000-0000-000000000003"), Name = "Utilities", Type = TransactionType.Expense },
        new() { Id = Guid.Parse("a1b2c3d4-0004-0000-0000-000000000004"), Name = "Entertainment", Type = TransactionType.Expense },
        new() { Id = Guid.Parse("a1b2c3d4-0005-0000-0000-000000000005"), Name = "Health", Type = TransactionType.Expense },
        new() { Id = Guid.Parse("a1b2c3d4-0006-0000-0000-000000000006"), Name = "Shopping", Type = TransactionType.Expense },
        new() { Id = Guid.Parse("a1b2c3d4-0007-0000-0000-000000000007"), Name = "Salary", Type = TransactionType.Income },
        new() { Id = Guid.Parse("a1b2c3d4-0008-0000-0000-000000000008"), Name = "Other", Type = TransactionType.Expense },
    ];
}
```

> **Why fixed GUIDs?** EF Core `HasData` requires stable primary keys so migrations can detect
> additions/changes. Random GUIDs would create duplicate insert migrations every time.

### Step 2 — Wire up in `CategoryConfiguration.cs`

Add `builder.HasData(CategorySeedData.GetCategories())` at the end of `Configure()`:

```csharp
public void Configure(EntityTypeBuilder<Category> builder)
{
    // ... existing config ...
    builder.HasData(CategorySeedData.GetCategories());
}
```

### Step 3 — Create and apply migration

```bash
# Create migration
dotnet ef migrations add SeedDefaultCategories --project src/Finlo.Infrastructure --startup-project src/Finlo.Api

# Apply to database
dotnet ef database update --project src/Finlo.Infrastructure --startup-project src/Finlo.Api
```

### Adding more seed data later

To seed a new table (e.g., default Budgets):

1. Create `src/Finlo.Infrastructure/Seed/BudgetSeedData.cs` with a static method returning entities
2. Call `builder.HasData(BudgetSeedData.GetBudgets())` in `BudgetConfiguration.cs`
3. Create a new migration: `dotnet ef migrations add SeedDefaultBudgets ...`
4. Apply: `dotnet ef database update ...`

### Tasks

- [x] Implement `CategorySeedData.cs` with 8 default category entities
- [x] Add `builder.HasData(CategorySeedData.GetCategories())` to `CategoryConfiguration.cs`
- [x] Create migration: `dotnet ef migrations add SeedDefaultCategories --project src/Finlo.Infrastructure --startup-project src/Finlo.Api`
- [x] Apply migration: `dotnet ef database update --project src/Finlo.Infrastructure --startup-project src/Finlo.Api`
- [x] Verify 8 rows in Categories table

---

## Docker Setup

**Goal:** Run the entire stack (API + UI) in Docker containers locally with a single command.

### Step 1 — Create `.dockerignore` (repo root)

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

### Step 2 — Create API Dockerfile

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

> **Key concepts:**
> - Multi-stage build: SDK image for building, smaller ASP.NET runtime image for running
> - `COPY *.csproj` + `RUN dotnet restore` first = Docker layer caching (restores are cached if csproj unchanged)
> - `ASPNETCORE_URLS` tells Kestrel which port to listen on inside the container

### Step 3 — Create UI Dockerfile + nginx config

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

### Step 4 — Create `docker-compose.yml` (repo root)

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

> **Key concepts:**
> - `ConnectionStrings__DefaultConnection` overrides `appsettings.json` via env var (double underscore = nested JSON key)
> - `volumes: api-data` persists the SQLite database across container restarts
> - `healthcheck` → UI container waits for API to be ready before starting
> - `depends_on` with `condition: service_healthy` → orderly startup

### Step 5 — Create `Tools/finlo.ps1` CLI

Single CLI tool at `Tools/finlo.ps1` replacing separate `scripts/run.ps1` and `scripts/seed.ps1`.
Run from the `Tools/` directory.

**Commands:**

| Command | Description |
|---|---|
| `./finlo.ps1 start` | Start API + UI locally (each in a new terminal) |
| `./finlo.ps1 start api` | Start just the API with `dotnet watch` |
| `./finlo.ps1 start ui` | Start just the Vite dev server |
| `./finlo.ps1 start docker` | Build & start the full Docker stack |
| `./finlo.ps1 stop docker` | Stop Docker containers |
| `./finlo.ps1 reset db` | Delete local SQLite DB files |
| `./finlo.ps1 reset docker` | Remove containers + volumes |
| `./finlo.ps1 logs [api\|ui]` | Follow Docker container logs |
| `./finlo.ps1 migrate [Name]` | Create migration (with name) or apply all (no name) |
| `./finlo.ps1 seed-db` | Seed DB by applying migrations |
| `./finlo.ps1 help` | Show help |

> **Key concepts:**
> - Single entry-point CLI inspired by the Credentialing Requirements `ruledev.ps1` pattern
> - `$Root` resolves to repo root via `Split-Path -Parent $PSScriptRoot`
> - `start api` / `start ui` each open a new `pwsh` terminal with `-NoExit`
> - `start docker` runs `docker compose up -d --build` and prints service URLs
> - `migrate` wraps EF Core commands with correct `--project` / `--startup-project` paths

### Step 6 — Test it

```powershell
# Start full Docker stack
.\finlo.ps1 start docker

# Verify:
#   - http://localhost:5266/openapi/v1.json → API docs (JSON)
#   - http://localhost:3000 → UI (Vite React app)

# Check logs
.\finlo.ps1 logs

# Stop everything
.\finlo.ps1 stop docker

# Local dev (API + UI in separate terminals)
.\finlo.ps1 start

# Seed database
.\finlo.ps1 seed-db

# Create a new migration
.\finlo.ps1 migrate AddIndexes

# Apply pending migrations
.\finlo.ps1 migrate

# Reset local database
.\finlo.ps1 reset db
```

**Docker URLs when running:**

| Service | URL                                  |
|---------|--------------------------------------|
| UI      | http://localhost:3000                 |
| API     | http://localhost:5266                 |
| OpenAPI | http://localhost:5266/openapi/v1.json |

### Tasks

- [x] Create `.dockerignore` at repo root
- [x] Create `src/Finlo.Api/Dockerfile` (multi-stage: SDK build → ASP.NET runtime)
- [x] Create `client/Finlo.UI/nginx.conf` (SPA routing + `/api/` reverse proxy)
- [x] Create `client/Finlo.UI/Dockerfile` (multi-stage: Node build → nginx)
- [x] Create `docker-compose.yml` at repo root (API + UI services, volume, healthcheck)
- [x] Create `Tools/finlo.ps1` CLI (start/stop/reset/logs/migrate/seed-db commands)
- [ ] Run `.\finlo.ps1 start docker` and verify both containers start
- [ ] Open http://localhost:5266/openapi/v1.json — verify API responds
- [ ] Open http://localhost:3000 — verify UI loads
- [ ] Run `.\finlo.ps1 stop docker` to clean up

---

## Frontend Setup

The React + TypeScript project is already scaffolded at `client/Finlo.UI/` (Vite + React 19 + TypeScript).

**Install key dependencies:**

```bash
cd client/Finlo.UI
npm install axios react-router-dom zustand recharts
npm install -D tailwindcss @tailwindcss/vite
```

### Tasks

- [x] Scaffold Vite + React + TS project (`client/Finlo.UI/`)
- [ ] Install dependencies (axios, router, zustand, recharts, tailwind)
- [ ] Set up Tailwind CSS
- [ ] Create folder structure
- [ ] Create axios client with base URL pointing to API
- [ ] Configure CORS on .NET API for local dev
- [ ] Set up React Router with routes for all pages
- [ ] Create `Layout` + `Sidebar` components

---

## API Response Conventions

| Scenario          | Status | Body                        |
|-------------------|--------|-----------------------------|
| Success (list)    | 200    | `{ data: [...], pagination }` |
| Success (single)  | 200    | `{ data: {...} }`            |
| Created           | 201    | `{ data: {...} }`            |
| No content        | 204    | (empty)                      |
| Validation error  | 400    | `{ status, message, errors }` |
| Not found         | 404    | `{ status, message }`        |
| Server error      | 500    | `{ status, message }`        |

---

## Git Branch Strategy

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

# Part 3 — Architecture Docs

System design, project structure, domain model, and database schema.

---

## Clean Architecture Overview

The project uses Clean Architecture with four layers:

**Project references (all wired up):**

```
Finlo.Domain          → (no dependencies)
Finlo.Application     → Finlo.Domain
Finlo.Infrastructure  → Finlo.Application
Finlo.Api             → Finlo.Application, Finlo.Infrastructure
```

**Layers (Clean Architecture):**

1. **Endpoints** (`Finlo.Api/Endpoints/`) — Minimal API route groups, returns DTOs
2. **Service** (`Finlo.Application/Services/`) — Business logic, mapping entity ↔ DTO
3. **Repository Interface** (`Finlo.Application/Interfaces/`) — Contracts for data access
4. **Repository** (`Finlo.Infrastructure/Repositories/`) — EF Core queries

---

## Project Structure

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
│       └── CategorySeedData.cs  ← EF Core HasData seed (8 default categories)
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

### Frontend Structure

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

---

## NuGet Packages

```
# Infrastructure (✅ installed)
Microsoft.EntityFrameworkCore.Sqlite      10.0.5
Microsoft.EntityFrameworkCore.Design      10.0.5

# Api (✅ installed)
Microsoft.AspNetCore.OpenApi              10.0.4
Microsoft.EntityFrameworkCore             10.0.5
Microsoft.EntityFrameworkCore.Design      10.0.5
```

---

## Domain Entities

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

### Tasks

- [x] Create `TransactionType.cs` enum
- [x] Create `Transaction.cs` entity
- [x] Create `Budget.cs` entity
- [x] Create `Category.cs` entity

---

## Database (EF Core + SQLite)

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

### Tasks

- [X] Create `AppDbContext.cs` in `Finlo.Infrastructure/Data/` with DbSets + `ApplyConfigurationsFromAssembly`
- [X] Create `DependencyInjection.cs` in `Finlo.Infrastructure/` (`AddInfrastructure` extension registering DbContext with SQLite)
- [X] Add connection string to `appsettings.json` (`"Data Source=finlo.db"`)
- [X] Register DbContext in `Finlo.Api/Program.cs` via `builder.Services.AddInfrastructure(builder.Configuration)`
- [X] Create initial migration: `InitialCreate` (tables: Budgets, Categories, Transactions)
- [X] Apply migration (`finlo.db` database file created)
- [X] Create entity configurations (`BudgetConfiguration`, `CategoryConfiguration`, `TransactionConfiguration` in `Data/Configurations/`)
- [ ] Add indexes to configurations: `Date` + `Category` on Transactions, composite `(Month, Year)` on Budgets
- [x] Implement seed data via EF Core `HasData` in `CategoryConfiguration` — see [Database Seeding](#database-seeding)
- [ ] Create new migration to apply configuration changes (indexes, seed data, max lengths, column types) to database

---

## Setup Tasks

- [X] Register all projects in `Finlo.slnx`
- [X] Add project references (Domain ← Application ← Infrastructure, Application + Infrastructure ← Api)
- [X] Install NuGet packages (EF Core Sqlite, EF Core Design in Infrastructure; EF Core, OpenApi in Api)
- [X] Create `IEndpoint` interface and `EndpointExtensions` (assembly-scanning endpoint registration)
- [X] Configure `Program.cs` with endpoint scanning, OpenAPI, HTTPS redirection, Infrastructure DI
- [X] Create folder structure in Application project (`Dtos/`, `Interfaces/`, `Services/`, `Repositories/` — empty)
- [X] Create folder structure in Infrastructure project (`Data/`, `Data/Migrations/`, `Seed/`)
- [ ] Create `Repositories/` folder in Infrastructure project

---

## Build Order (Recommended Sequence)

This is the exact order to build, task by task:

```
 1. [Setup]          Solution registration + project references + NuGet packages + endpoint pattern  ✅ DONE
 2. [Architecture]   Domain entities + enum                    ✅ DONE
 3. [Architecture]   AppDbContext + SQLite + migration + configs  ✅ DONE (indexes remaining)
 4. [Technical]      Database seeding (CategorySeedData + HasData + migration)
 5. [Technical]      Docker setup (Dockerfiles, compose, scripts)
 6. [Feature]        Transactions CRUD (interfaces/DTOs → repo → service → endpoints)
 7. [Feature]        Budgets CRUD + summary endpoint
 8. [Feature]        Categories endpoint
 9. [Feature]        Validation + error handling
10. [Verification]   ✅ Verify all API endpoints
11. [Feature]        Reports endpoints
12. [Verification]   ✅ Verify reports
13. [Technical]      Frontend deps + routing + layout           (scaffold ✅ DONE)
14. [Feature]        Transactions UI
15. [Feature]        Budgets UI
16. [Feature]        Dashboard
17. [Feature]        Reports + charts
18. [Verification]   ✅ Full integration test
19. [Feature]        Polish & enhancements
20. [Feature]        Advanced features
```

**Next up: Step 4 — Database seeding (CategorySeedData + HasData + migration).**
