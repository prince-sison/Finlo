using Finlo.Domain.Enums;

namespace Finlo.Domain.Entities;

public class Income
{
    public Guid Id { get; set; }
    public long Amount { get; set; }
    public IncomeType Source { get; set; }
    public DateTime Date { get; set; }
    public DateTime CreatedAt { get; set; }
}