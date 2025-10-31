namespace Cuzdanim.Application.Features.Budgets.DTOs;

public class BudgetDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string? CategoryIcon { get; set; } 
    public string? CategoryColor { get; set; } 
    public decimal Amount { get; set; }
    public string Currency { get; set; } = string.Empty;
    public DateTime PeriodStartDate { get; set; }
    public DateTime PeriodEndDate { get; set; }
    public decimal AlertThresholdPercentage { get; set; }
    public bool AlertWhenExceeded { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }

    public decimal Spent { get; set; } 
    public decimal Remaining { get; set; } 
    public decimal PercentageUsed { get; set; } 
    public string Status { get; set; } = "Normal"; 
}