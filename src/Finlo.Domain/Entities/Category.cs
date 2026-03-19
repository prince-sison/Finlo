namespace Finlo.Domain.Entities;

public class Category
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public TransactionType Type { get; set; }    
}