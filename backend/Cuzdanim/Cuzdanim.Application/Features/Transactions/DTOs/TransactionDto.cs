namespace Cuzdanim.Application.Features.Transactions.DTOs;

public class TransactionDto
{
    public Guid Id { get; set; }
    public Guid AccountId { get; set; }
    public string AccountName { get; set; } = string.Empty;
    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = string.Empty;
    public DateTime TransactionDate { get; set; }
    public string? Description { get; set; }
    public string? Notes { get; set; }
    public string? ReceiptUrl { get; set; }
    public List<string> Tags { get; set; } = new();
    public bool IsAutoCategorized { get; set; }
    public DateTime CreatedAt { get; set; }
}