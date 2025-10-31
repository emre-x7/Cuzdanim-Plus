using Cuzdanim.Application.Features.Reports.DTOs;

namespace Cuzdanim.Application.Features.Reports.Queries.GetReport;

public class ReportDto
{
    public ReportSummaryDto Summary { get; set; } = new();
    public List<CategoryReportDto> IncomeByCategory { get; set; } = new();
    public List<CategoryReportDto> ExpenseByCategory { get; set; } = new();
    public List<MonthlyReportDto> MonthlyTrend { get; set; } = new();
    public IncomeExpenseComparisonDto Comparison { get; set; } = new();
}