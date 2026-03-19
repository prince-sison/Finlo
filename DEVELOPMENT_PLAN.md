# Finlo — Personal Finance Tracker: Development Plan

> **Stack:** .NET 10 Web API · React + TypeScript · SQLite  
> **Architecture:** Modular Monolith (Feature-based)  
> **Approach:** API-first, then UI  

---

## Current State

- [x] Solution created (`Finlo.slnx`)
- [x] Web API project scaffolded (`Finlo.Api` — .NET 10, default template)
- [ ] Everything below

---

## Phase 1 — Foundation (Backend API)

**Goal:** Working Transactions + Budgets CRUD with SQLite persistence.

### 1.1 Project Structure & Dependencies

Set up the modular monolith folder structure inside `Finlo.Api`:

```
src/Finlo.Api/
├── Program.cs
├── Database/
│   └── AppDbContext.cs
├── Modules/
│   ├── Transactions/
│   │   ├── TransactionController.cs
│   │   ├── TransactionService.cs
│   │   ├── TransactionRepository.cs
│   │   ├── Transaction.cs              (entity)
│   │   └── Dtos/
│   │       ├── CreateTransactionDto.cs
│   │       ├── UpdateTransactionDto.cs
│   │       └── TransactionResponseDto.cs
│   ├── Budgets/
│   │   ├── BudgetController.cs
│   │   ├── BudgetService.cs
│   │   ├── BudgetRepository.cs
│   │   ├── Budget.cs                   (entity)
│   │   └── Dtos/
│   │       ├── CreateBudgetDto.cs
│   │       ├── UpdateBudgetDto.cs
│   │       └── BudgetResponseDto.cs
│   └── Categories/
│       ├── Category.cs                 (entity)
│       └── CategorySeedData.cs
├── Shared/
│   ├── Enums/
│   │   └── TransactionType.cs
│   └── Pagination/
│       ├── PaginationParams.cs
│       └── PagedResult.cs
└── Migrations/
```

**Install NuGet packages:**

```bash
dotnet add package Microsoft.EntityFrameworkCore.Sqlite
dotnet add package Microsoft.EntityFrameworkCore.Design
```

**Tasks:**

- [ ] Remove weather forecast boilerplate from `Program.cs`
- [ ] Create folder structure above
- [ ] Add NuGet packages
- [ ] Add `AddControllers()` to `Program.cs` (replace minimal API setup)

---

### 1.2 Domain Entities

**`TransactionType` enum:**

```csharp
public enum TransactionType
{
    Income = 0,
    Expense = 1
}
```

**`Transaction` entity:**

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

**`Budget` entity:**

| Property  | Type      | Notes                          |
|-----------|-----------|--------------------------------|
| Id        | `Guid`    | PK                             |
| Category  | `string`  | Budget category                |
| Limit     | `decimal` | Monthly budget limit           |
| Month     | `int`     | 1–12                           |
| Year      | `int`     | e.g. 2026                      |
| CreatedAt | `DateTime`| Auto-set                       |

**`Category` entity (optional, seed data):**

| Property | Type              | Notes       |
|----------|-------------------|-------------|
| Id       | `Guid`            | PK          |
| Name     | `string`          | Unique      |
| Type     | `TransactionType` | Income/Expense |

**Tasks:**

- [ ] Create `TransactionType.cs` enum
- [ ] Create `Transaction.cs` entity
- [ ] Create `Budget.cs` entity
- [ ] Create `Category.cs` entity

---

### 1.3 Database (EF Core + SQLite)

**`AppDbContext`:**

- DbSet\<Transaction\>, DbSet\<Budget\>, DbSet\<Category\>
- Configure indexes: `Date`, `Category`, composite `(Month, Year)` on Budgets
- Seed default categories (Food, Transport, Utilities, Salary, Entertainment, Health, Shopping, Other)

**Connection string** (`appsettings.json`):

```json
"ConnectionStrings": {
  "DefaultConnection": "Data Source=finlo.db"
}
```

**Tasks:**

- [ ] Create `AppDbContext.cs` with entity configurations
- [ ] Add connection string to `appsettings.json`
- [ ] Register DbContext in `Program.cs`
- [ ] Create initial migration: `dotnet ef migrations add InitialCreate`
- [ ] Apply migration: `dotnet ef database update`
- [ ] Seed default categories

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

**Layers:**

1. **Controller** — Route handling, validation, returns DTOs
2. **Service** — Business logic, mapping entity ↔ DTO
3. **Repository** — EF Core queries

**Tasks:**

- [ ] Create DTOs: `CreateTransactionDto`, `UpdateTransactionDto`, `TransactionResponseDto`
- [ ] Create `PaginationParams` and `PagedResult<T>`
- [ ] Create `TransactionRepository` (CRUD + filtered query)
- [ ] Create `TransactionService` (mapping + validation)
- [ ] Create `TransactionController` (endpoints)
- [ ] Register services in DI (`Program.cs`)
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

- [ ] Create DTOs: `CreateBudgetDto`, `UpdateBudgetDto`, `BudgetResponseDto`, `BudgetSummaryDto`
- [ ] Create `BudgetRepository`
- [ ] Create `BudgetService` (includes summary calculation — queries Transactions table)
- [ ] Create `BudgetController`
- [ ] Register services in DI
- [ ] Test all endpoints

---

### 1.6 Categories Endpoint

| Method | Route              | Description        |
|--------|--------------------|--------------------|
| GET    | `/api/categories`  | List all categories |

Simple read-only endpoint returning seeded categories. No full CRUD needed for V1.

**Tasks:**

- [ ] Create `CategoriesController` (single GET endpoint)
- [ ] Test endpoint

---

### 1.7 Validation & Error Handling

- [ ] Add `FluentValidation` or use Data Annotations on DTOs
  - Amount > 0
  - Category required
  - Date required
  - Budget Limit > 0
  - Month 1–12
- [ ] Create global exception handler middleware
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

- [ ] Create `ReportsModule/` folder structure
- [ ] Create `ReportService` (aggregate queries against Transactions)
- [ ] Create `ReportsController`
- [ ] Create response DTOs
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

```bash
# From repo root
npx create-vite@latest finlo-web --template react-ts
cd finlo-web
npm install
```

**Install key dependencies:**

```bash
npm install axios react-router-dom zustand recharts
npm install -D tailwindcss @tailwindcss/vite
```

**Folder structure:**

```
src/finlo-web/
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

- [ ] Scaffold Vite + React + TS project
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
dotnet ef migrations add <Name> --project src/Finlo.Api

# Apply migrations
dotnet ef database update --project src/Finlo.Api

# Remove last migration (if not applied)
dotnet ef migrations remove --project src/Finlo.Api
```

### Dev Workflow

```bash
# Run API (from src/Finlo.Api)
dotnet watch run

# Run Frontend (from src/finlo-web)
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
 1. [Phase 1.1] Project structure + packages
 2. [Phase 1.2] Domain entities + enum
 3. [Phase 1.3] DbContext + SQLite + migration + seed
 4. [Phase 1.4] Transactions CRUD (repo → service → controller)
 5. [Phase 1.5] Budgets CRUD + summary endpoint
 6. [Phase 1.6] Categories endpoint
 7. [Phase 1.7] Validation + error handling
 8. [Phase 1.8] ✅ Verify all API endpoints
 9. [Phase 2.1] Reports endpoints
10. [Phase 2.2] ✅ Verify reports
11. [Phase 3.1] Frontend scaffold + routing + layout
12. [Phase 3.2] Transactions UI
13. [Phase 3.3] Budgets UI
14. [Phase 3.4] Dashboard
15. [Phase 3.5] Reports + charts
16. [Phase 3.6] ✅ Full integration test
17. [Phase 4]   Polish & enhancements
18. [Phase 5]   Advanced features
```

---

*Start with step 1. Build one thing at a time. Test before moving on.*
