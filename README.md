# Finlo — Personal Finance Tracker

A simple, fast, and reliable personal finance tracker for managing transactions, budgets, and financial insights.

---

## Tech Stack

| Layer    | Technology                  |
|----------|-----------------------------|
| Backend  | .NET 10 Web API             |
| Frontend | React 19 + TypeScript (Vite)|
| Database | SQLite (EF Core)            |
| State    | Zustand                     |
| Charts   | Recharts                    |
| Styling  | Tailwind CSS                |

**Architecture:** Modular Monolith (feature-based) · API-first

---

## Features (MVP)

- **Transactions** — Add, edit, delete income/expense entries with category, date, and notes
- **Budgets** — Set monthly budgets per category, track spent vs. limit
- **Reports** — Monthly summary, category breakdown, spending trends
- **Dashboard** — One-glance overview of balance, recent activity, and budget health

---

## Project Structure

```
Finlo/
├── Finlo.slnx                     # Solution file
├── DEVELOPMENT_PLAN.md            # Detailed development roadmap
├── src/
│   ├── Finlo.Api/                 # .NET 10 Web API (entry point)
│   ├── Finlo.Application/        # Application logic / use cases
│   ├── Finlo.Domain/             # Domain entities & interfaces
│   └── Finlo.Infrastructure/     # Data access, external services
└── client/
    └── Finlo.UI/                  # React + TypeScript frontend (Vite)
```

---

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Node.js 20+](https://nodejs.org/)

### Clone the Repository

```bash
git clone https://github.com/<your-username>/Finlo.git
cd Finlo
```

### Backend (API)

```bash
cd src/Finlo.Api
dotnet restore
dotnet run
```

The API will be available at **http://localhost:5266** (or https://localhost:7178).

### Frontend (UI)

```bash
cd client/Finlo.UI
npm install
npm run dev
```

The dev server will start at **http://localhost:5173** (default Vite port).

---

## Available Scripts

### Backend

| Command | Description |
|---|---|
| `dotnet run` | Start the API server |
| `dotnet build` | Build the solution |
| `dotnet test` | Run tests |

### Frontend

| Command | Description |
|---|---|
| `npm run dev` | Start Vite dev server with HMR |
| `npm run build` | Type-check and build for production |
| `npm run lint` | Run ESLint |
| `npm run preview` | Preview production build locally |

---

## API Documentation

When running in Development mode, OpenAPI docs are available at:

```
http://localhost:5266/openapi/v1.json
```

---

## Development Status

This project is under active development. See [DEVELOPMENT_PLAN.md](DEVELOPMENT_PLAN.md) for the full roadmap and current progress.

---

## License

This project is for personal use.

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