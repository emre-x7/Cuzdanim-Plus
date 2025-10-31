namespace Cuzdanim.Application.Features.Reports.DTOs;

public class ReportSummaryDto
{
    public decimal TotalIncome { get; set; }
    public decimal TotalExpense { get; set; }
    public decimal NetAmount { get; set; }
    public string Currency { get; set; } = string.Empty;
}