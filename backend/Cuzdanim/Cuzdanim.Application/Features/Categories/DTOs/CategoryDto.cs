namespace Cuzdanim.Application.Features.Categories.DTOs;

public class CategoryDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string TransactionType { get; set; } = string.Empty; // ✅ YENİ: "Income" veya "Expense"
    public string Type { get; set; } = string.Empty; // "Salary", "Food", vs.
    public string? Icon { get; set; }
    public string? Color { get; set; }
    public bool IsActive { get; set; }
}