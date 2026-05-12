using System.Reflection;
using Finlo.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Finlo.Infrastructure.Persistence.Sql;

public class FinloDbContext : DbContext
{
    public FinloDbContext(DbContextOptions<FinloDbContext> options) : base(options)
    {
    }

    public DbSet<Expense> Expenses { get; set; }
    public DbSet<Income> Incomes { get; set; }
    public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}