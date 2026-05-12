namespace Finlo.Domain.Entities;

public class Expense
{
    public Guid Id { get; set; }
    public long Amount { get; set; }
    public string? Note { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime Date { get; set; }
}