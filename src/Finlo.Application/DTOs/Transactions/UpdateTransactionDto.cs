using Finlo.Domain.Enums;

namespace Finlo.Application.DTOs.Transactions;

public class UpdateTransactionDto
{
    public decimal Amount { get; set; }
    public TransactionType Type { get; set; }
    public string Category { get; set; }
    public DateTime Date { get; set; }
    public string? Notes { get; set; }
}