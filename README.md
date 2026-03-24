# Finlo — Personal Finance Tracker

A simple, fast, and reliable personal finance tracker for managing transactions, budgets, and financial insights.

---

## Tech Stack

| Layer    | Technology                  |
|----------|-----------------------------|
| Backend  | .NET 10 Minimal API         |
| Frontend | React 19 + TypeScript (Vite)|
| Database | SQLite (EF Core)            |
| State    | Zustand                     |
| Charts   | Recharts                    |
| Styling  | Tailwind CSS 4              |
| HTTP     | Axios                       |
| Routing  | React Router 7              |

**Architecture:** Clean Architecture (Domain → Application → Infrastructure → Api) · API-first

---

## Features (Planned MVP)

- **Transactions** — Add, edit, delete income/expense entries with category, date, and notes
- **Budgets** — Set monthly budgets per category, track spent vs. limit
- **Reports** — Monthly summary, category breakdown, spending trends
- **Dashboard** — One-glance overview of balance, recent activity, and budget health

---

## Project Structure

```
Finlo/
├── Finlo.slnx                     # Solution file
├── docker-compose.yml             # Docker Compose (API + UI)
├── DEVELOPMENT_PLAN.md            # Detailed development roadmap
├── Tools/
│   └── finlo.ps1                  # Development CLI helper
├── src/
│   ├── Finlo.Api/                 # .NET 10 Minimal API (entry point)
│   ├── Finlo.Application/        # Application logic / use cases
│   ├── Finlo.Domain/             # Domain entities & enums
│   └── Finlo.Infrastructure/     # EF Core, data access, seeding
└── client/
    └── Finlo.UI/                  # React + TypeScript frontend (Vite)
```

---

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Node.js 20+](https://nodejs.org/)
- [Docker](https://www.docker.com/) (optional, for containerized setup)

### Clone the Repository

```bash
git clone https://github.com/<your-username>/Finlo.git
cd Finlo
```

### Option 1 — Local Development

**Backend (API):**

```bash
cd src/Finlo.Api
dotnet restore
dotnet run
```

The API will be available at **http://localhost:5266** (or https://localhost:7178).

**Frontend (UI):**

```bash
cd client/Finlo.UI
npm install
npm run dev
```

The dev server will start at **http://localhost:5173**.

### Option 2 — Docker

```bash
docker compose up -d --build
```

| Service | URL |
|---|---|
| API | http://localhost:5266 |
| UI | http://localhost:3000 |
| OpenAPI | http://localhost:5266/openapi/v1.json |

---

## CLI Tool

A PowerShell helper script is available at `Tools/finlo.ps1`:

```powershell
./finlo.ps1 <command> [sub-command]
```

| Command | Description |
|---|---|
| `start [api \| ui \| docker]` | Start services (default: api + ui locally) |
| `stop [docker]` | Stop services |
| `reset [db \| docker]` | Reset state (DB, Docker, or both) |
| `logs [api \| ui]` | Follow Docker container logs |
| `migrate [MigrationName]` | Create a migration or apply all pending |
| `seed-db` | Apply migrations to seed the database |
| `help` | Show all available commands |

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

When running in Development mode, the OpenAPI spec is available at:

```
http://localhost:5266/openapi/v1.json
```

---

## Development Status

This project is under active development. The backend scaffolding and domain model are in place; endpoint implementation is in progress. See [DEVELOPMENT_PLAN.md](DEVELOPMENT_PLAN.md) for the full roadmap and task checklists.

---

## License

This project is for personal use.
