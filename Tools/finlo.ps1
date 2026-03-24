#!/usr/bin/env pwsh
<#
.SYNOPSIS
Finlo Development CLI Tool

.DESCRIPTION
Streamlined CLI tool with commands: start, stop, reset, logs, seed-db, and help.
#>

$Root = Split-Path -Parent $PSScriptRoot

function StartCommand {
    param (
        [string]$subCommand
    )

    switch ($subCommand.ToLower()) {
        "api" {
            Write-Host "Starting API..." -ForegroundColor Cyan
            $commandString = "-NoExit -Command & { Set-Location '$Root'; dotnet watch run --project src/Finlo.Api --no-hot-reload }"
            Start-Process -FilePath "pwsh.exe" -ArgumentList $commandString
        }
        "ui" {
            Write-Host "Starting UI dev server..." -ForegroundColor Cyan
            $commandString = "-NoExit -Command & { Set-Location '$Root\client\Finlo.UI'; npm run dev }"
            Start-Process -FilePath "pwsh.exe" -ArgumentList $commandString
        }
        "docker" {
            Write-Host "Starting Docker stack..." -ForegroundColor Cyan
            Push-Location $Root
            docker compose up -d --build
            Pop-Location

            Write-Host ""
            Write-Host "Finlo is running:" -ForegroundColor Green
            Write-Host "  API:      http://localhost:5266" -ForegroundColor White
            Write-Host "  UI:       http://localhost:3000" -ForegroundColor White
            Write-Host "  OpenAPI:  http://localhost:5266/openapi/v1.json" -ForegroundColor White
        }
        default {
            Write-Host "Starting API and UI for local development..." -ForegroundColor Cyan
            StartCommand "api"
            StartCommand "ui"
        }
    }
}

function StopCommand {
    param (
        [string]$subCommand
    )

    switch ($subCommand.ToLower()) {
        "docker" {
            Write-Host "Stopping Docker containers..." -ForegroundColor Cyan
            Push-Location $Root
            docker compose down
            Pop-Location
        }
        default {
            Write-Host "Stopping Docker containers. Close any open API/UI terminals manually." -ForegroundColor Yellow
            Push-Location $Root
            docker compose down
            Pop-Location
        }
    }
}

function ResetCommand {
    param (
        [string]$subCommand
    )

    switch ($subCommand.ToLower()) {
        "db" {
            Write-Host "Resetting database..." -ForegroundColor Cyan
            Push-Location $Root
            Remove-Item "src\Finlo.Api\finlo.db" -ErrorAction SilentlyContinue
            Remove-Item "src\Finlo.Api\finlo.db-shm" -ErrorAction SilentlyContinue
            Remove-Item "src\Finlo.Api\finlo.db-wal" -ErrorAction SilentlyContinue
            Write-Host "Database files removed. Run migrations to recreate:" -ForegroundColor Gray
            Write-Host "  dotnet ef database update --project src/Finlo.Infrastructure --startup-project src/Finlo.Api" -ForegroundColor Gray
            Pop-Location
        }
        "docker" {
            Write-Host "Resetting Docker stack (containers + volumes)..." -ForegroundColor Cyan
            Push-Location $Root
            docker compose down -v
            Pop-Location
        }
        default {
            Write-Host "Resetting everything (Docker + local DB)..." -ForegroundColor Cyan
            ResetCommand "docker"
            ResetCommand "db"
        }
    }
}

function LogsCommand {
    param (
        [string]$serviceName
    )

    Push-Location $Root
    if ($serviceName) {
        Write-Host "Showing logs for $serviceName..." -ForegroundColor Cyan
        docker compose logs -f $serviceName
    } else {
        Write-Host "Showing all logs..." -ForegroundColor Cyan
        docker compose logs -f
    }
    Pop-Location
}

function MigrateCommand {
    param (
        [string]$migrationName
    )

    Push-Location $Root
    if ($migrationName) {
        Write-Host "Creating migration: $migrationName..." -ForegroundColor Cyan
        dotnet ef migrations add $migrationName --project src/Finlo.Infrastructure --startup-project src/Finlo.Api
    } else {
        Write-Host "Applying migrations..." -ForegroundColor Cyan
        dotnet ef database update --project src/Finlo.Infrastructure --startup-project src/Finlo.Api
    }
    Pop-Location
}

function SeedDatabase {
    Write-Host "Seeding database (applying migrations)..." -ForegroundColor Cyan
    Push-Location $Root
    dotnet ef database update --project src/Finlo.Infrastructure --startup-project src/Finlo.Api
    if ($LASTEXITCODE -eq 0) {
        Write-Host "Database seeded successfully." -ForegroundColor Green
    } else {
        Write-Host "Seed failed. Check migration errors above." -ForegroundColor Red
    }
    Pop-Location
}

function Help {
    Write-Host ""
    Write-Host "Finlo Development CLI" -ForegroundColor Cyan
    Write-Host "Usage:" -ForegroundColor White
    Write-Host "  ./finlo.ps1 <command> [sub-command]"
    Write-Host ""
    Write-Host "  Commands:" -ForegroundColor White
    Write-Host "    start [api | ui | docker]       Start services (default: api + ui)"
    Write-Host "    stop [docker]                    Stop services"
    Write-Host "    reset [db | docker]              Reset state (default: all)"
    Write-Host "    logs [api | ui]                  Follow Docker container logs"
    Write-Host "    migrate [MigrationName]          Create migration or apply all"
    Write-Host "    seed-db                          Apply migrations to seed the DB"
    Write-Host "    help                             Show this help"
    Write-Host ""
    Write-Host "  Examples:" -ForegroundColor Gray
    Write-Host "    ./finlo.ps1 start                # Start API + UI locally"
    Write-Host "    ./finlo.ps1 start docker         # Start full stack in Docker"
    Write-Host "    ./finlo.ps1 stop docker           # Stop Docker containers"
    Write-Host "    ./finlo.ps1 reset db             # Delete local SQLite DB"
    Write-Host "    ./finlo.ps1 logs api             # Follow API container logs"
    Write-Host "    ./finlo.ps1 migrate AddIndex     # Create a new migration"
    Write-Host "    ./finlo.ps1 migrate              # Apply pending migrations"
    Write-Host "    ./finlo.ps1 seed-db              # Seed DB via migrations"
    Write-Host ""
}

# Entry point
if ($args.Count -lt 1) {
    Write-Host "No command provided." -ForegroundColor Red
    Help
    exit 1
}

$Command = $args[0].ToLower()
$SubCommand = if ($args.Count -ge 2) { $args[1] } else { "" }

switch ($Command) {
    "start"    { StartCommand $SubCommand }
    "stop"     { StopCommand $SubCommand }
    "reset"    { ResetCommand $SubCommand }
    "logs"     { LogsCommand $SubCommand }
    "migrate"  { MigrateCommand $SubCommand }
    "seed-db"  { SeedDatabase }
    "help"     { Help }
    default {
        Write-Host "Unknown command: $Command" -ForegroundColor Red
        Help
    }
}