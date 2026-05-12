namespace Finlo.Domain.Entities;

public class Expense
{
    public Guid Id { get; set; }
    public long Amount { get; set; }
    public Guid CategoryId { get; set; }
    public DateTime Date { get; set; }
    public string? Description { get; set; }
}