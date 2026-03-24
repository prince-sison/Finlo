using Finlo.Domain.Entities;
using Finlo.Domain.Enums;

namespace Finlo.Infrastructure.Seed;

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