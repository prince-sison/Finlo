# Finlo

A simple, lightweight expense tracker for managing your day-to-day finances. Finlo lets you record income and expenses, organize them into categories, and get a clear picture of where your money is going.

## Overview

Finlo is built as a personal finance companion focused on simplicity. The core idea is to make it effortless to:

- Log expenses and income as they happen
- Group transactions under custom categories (e.g. Food, Rent, Salary, Freelance)
- Review spending patterns over time

The project is structured following **Clean Architecture** principles, keeping domain logic independent from frameworks and infrastructure concerns.

## Tech Stack

- **.NET 10** with C# (nullable reference types + implicit usings)
- **ASP.NET Core Web API** — REST API surface (`Finlo.Api`)
- **OpenAPI / Swagger** via `Microsoft.AspNetCore.OpenApi`
- **Clean Architecture** layering:
  - `Finlo.Domain` — entities (`Expense`, `Income`, `Category`) and enums
  - `Finlo.Application` — business logic and use cases
  - `Finlo.Infrastructure` — data access and external integrations
  - `Finlo.Api` — HTTP entry point and controllers

## Project Structure

```
src/
├── Finlo.Api/             # ASP.NET Core Web API (presentation layer)
├── Finlo.Application/     # Use cases and application services
├── Finlo.Domain/          # Core entities and business rules
└── Finlo.Infrastructure/  # Persistence and external services
tests/                     # Unit and integration tests
tools/                     # Supporting tooling
```

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)

### Run the API

```pwsh
dotnet run --project src/Finlo.Api
```

### Build the solution

```pwsh
dotnet build Finlo.slnx
```

## Status

Finlo is in early development. Expect the domain model and API surface to evolve.
