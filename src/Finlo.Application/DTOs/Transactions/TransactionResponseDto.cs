namespace Finlo.Application.DTOs.Transactions;

public class TransactionResponseDto
{
    public Guid Id { get; set; }
    public decimal Amount { get; set; }
    public string Type { get; set; }
    public string Category { get; set; }
    public DateTime Date { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}