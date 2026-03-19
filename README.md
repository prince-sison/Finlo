# Finlo — Personal Finance Tracker

A simple, fast, and reliable personal finance tracker for managing transactions, budgets, and financial insights.

## Tech Stack

| Layer    | Technology                  |
|----------|-----------------------------|
| Backend  | .NET 10 Web API             |
| Frontend | React + TypeScript (Vite)   |
| Database | SQLite (EF Core)            |
| State    | Zustand                     |
| Charts   | Recharts                    |
| Styling  | Tailwind CSS                |

## Features (MVP)

- **Transactions** — Add, edit, delete income/expense entries with category, date, and notes
- **Budgets** — Set monthly budgets per category, track spent vs. limit
- **Reports** — Monthly summary, category breakdown, spending trends
- **Dashboard** — One-glance overview of balance, recent activity, and budget health

## Project Structure

```
Finlo/
├── Finlo.slnx
├── src/
│   ├── Finlo.Api/             # .NET Web API (Modular Monolith)
│   │   ├── Modules/
│   │   │   ├── Transactions/
│   │   │   ├── Budgets/
│   │   │   ├── Categories/
│   │   │   └── Reports/
│   │   ├── Database/
│   │   └── Shared/
│   └── finlo-web/             # React + TypeScript frontend
│       └── src/
│           ├── features/
│           ├── components/
│           ├── api/
│           └── store/
```

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Node.js 20+](https://nodejs.org/)

### Run the API

```bash
cd src/Finlo.Api
dotnet run
```

### Run the Frontend

```bash
cd src/finlo-web
npm install
npm run dev
```

## API Endpoints

### Transactions
| Method | Route                      | Description        |
|--------|----------------------------|--------------------|
| GET    | `/api/transactions`        | List (paginated)   |
| GET    | `/api/transactions/{id}`   | Get by ID          |
| POST   | `/api/transactions`        | Create             |
| PUT    | `/api/transactions/{id}`   | Update             |
| DELETE | `/api/transactions/{id}`   | Delete             |

### Budgets
| Method | Route                   | Description            |
|--------|-------------------------|------------------------|
| GET    | `/api/budgets`          | List                   |
| GET    | `/api/budgets/{id}`     | Get by ID              |
| POST   | `/api/budgets`          | Create                 |
| PUT    | `/api/budgets/{id}`     | Update                 |
| DELETE | `/api/budgets/{id}`     | Delete                 |
| GET    | `/api/budgets/summary`  | Budget vs Actual       |

### Reports
| Method | Route                              | Description          |
|--------|------------------------------------|----------------------|
| GET    | `/api/reports/monthly-summary`     | Income/expense totals|
| GET    | `/api/reports/category-breakdown`  | Spending by category |
| GET    | `/api/reports/trends`              | Monthly trends       |

### Categories
| Method | Route              | Description      |
|--------|--------------------|------------------|
| GET    | `/api/categories`  | List all         |

## Development

See [DEVELOPMENT_PLAN.md](DEVELOPMENT_PLAN.md) for the full phased roadmap and task checklists.

## License

MIT