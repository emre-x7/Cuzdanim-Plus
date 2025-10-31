namespace Cuzdanim.Application.Features.Reports.DTOs;

public class MonthlyReportDto
{
    public string Month { get; set; } = string.Empty; // "2025-01"
    public int Year { get; set; }
    public string MonthName { get; set; } = string.Empty; // "Ocak 2025"
    public decimal Income { get; set; }
    public decimal Expense { get; set; }
    public decimal Net { get; set; }
}