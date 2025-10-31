using Cuzdanim.Application.Common.Interfaces;
using Cuzdanim.Application.Common.Models;
using Cuzdanim.Application.Features.Reports.DTOs;
using Cuzdanim.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Cuzdanim.Application.Features.Reports.Queries.GetReport;

public class GetReportQueryHandler : IRequestHandler<GetReportQuery, Result<ReportDto>>
{
    private readonly IApplicationDbContext _context;

    public GetReportQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<ReportDto>> Handle(GetReportQuery request, CancellationToken cancellationToken)
    {
        // Default tarih aralığı: Son 30 gün
        var endDate = request.EndDate ?? DateTime.UtcNow;
        var startDate = request.StartDate ?? endDate.AddDays(-30);

        // Kullanıcının işlemleri
        var transactions = await _context.Transactions
            .Include(t => t.Category)
            .Where(t => t.UserId == request.UserId
                && t.TransactionDate >= startDate
                && t.TransactionDate <= endDate
                && !t.IsDeleted)
            .ToListAsync(cancellationToken);

        if (!transactions.Any())
        {
            return Result<ReportDto>.Success(new ReportDto(), "Veri bulunamadı");
        }

        // Varsayılan currency (ilk işlemden al)
        var currency = transactions.First().Amount.Currency.ToString();

        // 1. SUMMARY
        var totalIncome = transactions
            .Where(t => t.Type == TransactionType.Income)
            .Sum(t => t.Amount.Amount);

        var totalExpense = transactions
            .Where(t => t.Type == TransactionType.Expense)
            .Sum(t => t.Amount.Amount);

        var summary = new ReportSummaryDto
        {
            TotalIncome = totalIncome,
            TotalExpense = totalExpense,
            NetAmount = totalIncome - totalExpense,
            Currency = currency
        };

        // 2. INCOME BY CATEGORY
        var incomeByCategory = transactions
            .Where(t => t.Type == TransactionType.Income)
            .GroupBy(t => new { t.CategoryId, t.Category.Name, t.Category.Icon, t.Category.Color })
            .Select(g => new CategoryReportDto
            {
                CategoryId = g.Key.CategoryId,
                CategoryName = g.Key.Name,
                CategoryIcon = g.Key.Icon,
                CategoryColor = g.Key.Color,
                TotalAmount = g.Sum(t => t.Amount.Amount),
                TransactionCount = g.Count(),
                Percentage = totalIncome > 0 ? (g.Sum(t => t.Amount.Amount) / totalIncome * 100) : 0
            })
            .OrderByDescending(c => c.TotalAmount)
            .ToList();

        // 3. EXPENSE BY CATEGORY
        var expenseByCategory = transactions
            .Where(t => t.Type == TransactionType.Expense)
            .GroupBy(t => new { t.CategoryId, t.Category.Name, t.Category.Icon, t.Category.Color })
            .Select(g => new CategoryReportDto
            {
                CategoryId = g.Key.CategoryId,
                CategoryName = g.Key.Name,
                CategoryIcon = g.Key.Icon,
                CategoryColor = g.Key.Color,
                TotalAmount = g.Sum(t => t.Amount.Amount),
                TransactionCount = g.Count(),
                Percentage = totalExpense > 0 ? (g.Sum(t => t.Amount.Amount) / totalExpense * 100) : 0
            })
            .OrderByDescending(c => c.TotalAmount)
            .ToList();

        // 4. MONTHLY TREND
        var monthlyTrend = transactions
            .GroupBy(t => new { t.TransactionDate.Year, t.TransactionDate.Month })
            .Select(g => new MonthlyReportDto
            {
                Month = $"{g.Key.Year}-{g.Key.Month:D2}",
                Year = g.Key.Year,
                MonthName = GetMonthName(g.Key.Month, g.Key.Year),
                Income = g.Where(t => t.Type == TransactionType.Income).Sum(t => t.Amount.Amount),
                Expense = g.Where(t => t.Type == TransactionType.Expense).Sum(t => t.Amount.Amount),
                Net = g.Where(t => t.Type == TransactionType.Income).Sum(t => t.Amount.Amount) -
                      g.Where(t => t.Type == TransactionType.Expense).Sum(t => t.Amount.Amount)
            })
            .OrderBy(m => m.Month)
            .ToList();

        // 5. COMPARISON
        var comparison = new IncomeExpenseComparisonDto
        {
            Income = totalIncome,
            Expense = totalExpense
        };

        var reportDto = new ReportDto
        {
            Summary = summary,
            IncomeByCategory = incomeByCategory,
            ExpenseByCategory = expenseByCategory,
            MonthlyTrend = monthlyTrend,
            Comparison = comparison
        };

        return Result<ReportDto>.Success(reportDto, "Rapor başarıyla oluşturuldu");
    }

    private static string GetMonthName(int month, int year)
    {
        var culture = new CultureInfo("tr-TR");
        var date = new DateTime(year, month, 1);
        return date.ToString("MMMM yyyy", culture);
    }
}