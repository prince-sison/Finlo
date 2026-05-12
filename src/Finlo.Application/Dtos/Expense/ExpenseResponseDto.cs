namespace Finlo.Application.Dtos.Expense;

public class ExpenseResponseDto
{
    public Guid Id { get; set; }
    public string? Description { get; set; }
    public long Amount { get; set; }
    public DateTime Date { get; set; }
    public Guid CategoryId { get; set; }
}
