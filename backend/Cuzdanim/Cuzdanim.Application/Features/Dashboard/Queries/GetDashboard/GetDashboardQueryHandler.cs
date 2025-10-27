using Cuzdanim.Application.Common.Interfaces;
using Cuzdanim.Application.Common.Models;
using Cuzdanim.Application.Features.Dashboard.DTOs;
using Cuzdanim.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Cuzdanim.Application.Features.Dashboard.Queries.GetDashboard;

public class GetDashboardQueryHandler : IRequestHandler<GetDashboardQuery, Result<DashboardDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetDashboardQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<DashboardDto>> Handle(GetDashboardQuery request, CancellationToken cancellationToken)
    {
        // 1. Kullanıcı kontrolü
        var user = await _unitOfWork.Users.GetByIdAsync(request.UserId, cancellationToken);
        if (user == null)
        {
            return Result<DashboardDto>.Failure("Kullanıcı bulunamadı");
        }

        // 2. Tarih aralıkları
        var now = DateTime.UtcNow;
        var currentMonthStart = new DateTime(now.Year, now.Month, 1);
        var currentMonthEnd = currentMonthStart.AddMonths(1).AddDays(-1);
        var lastMonthStart = currentMonthStart.AddMonths(-1);
        var lastMonthEnd = currentMonthStart.AddDays(-1);

        // 3. Toplam bakiye
        var totalBalance = await _unitOfWork.Accounts.GetTotalBalanceAsync(request.UserId, cancellationToken);

        // 4. Bu ay gelir/gider
        var currentMonthIncome = await _unitOfWork.Transactions.GetTotalIncomeAsync(
            request.UserId, currentMonthStart, currentMonthEnd, cancellationToken);

        var currentMonthExpense = await _unitOfWork.Transactions.GetTotalExpenseAsync(
            request.UserId, currentMonthStart, currentMonthEnd, cancellationToken);

        // 5. Geçen ay gelir/gider
        var lastMonthIncome = await _unitOfWork.Transactions.GetTotalIncomeAsync(
            request.UserId, lastMonthStart, lastMonthEnd, cancellationToken);

        var lastMonthExpense = await _unitOfWork.Transactions.GetTotalExpenseAsync(
            request.UserId, lastMonthStart, lastMonthEnd, cancellationToken);

        // 6. Değişim yüzdeleri
        var incomeChange = lastMonthIncome > 0
            ? ((currentMonthIncome - lastMonthIncome) / lastMonthIncome) * 100
            : 0;

        var expenseChange = lastMonthExpense > 0
            ? ((currentMonthExpense - lastMonthExpense) / lastMonthExpense) * 100
            : 0;

        // 7. Hesap sayıları
        var allAccounts = await _unitOfWork.Accounts.GetByUserIdAsync(request.UserId, cancellationToken);
        var activeAccounts = allAccounts.Count(a => a.IsActive);

        // 8. Hedef sayıları
        var allGoals = await _unitOfWork.Goals.GetByUserIdAsync(request.UserId, cancellationToken);
        var activeGoals = allGoals.Count(g => g.Status == GoalStatus.Active);
        var completedGoalsThisMonth = allGoals.Count(g =>
            g.Status == GoalStatus.Completed &&
            g.UpdatedAt.HasValue &&
            g.UpdatedAt.Value >= currentMonthStart);

        // 9. Bütçe uyarıları
        var budgetAlerts = await GetBudgetAlertsAsync(request.UserId, currentMonthStart, currentMonthEnd, cancellationToken);

        // 10. Son 10 işlem
        var recentTransactions = await GetRecentTransactionsAsync(request.UserId, cancellationToken);

        // 11. DTO oluştur
        var dashboard = new DashboardDto
        {
            TotalBalance = totalBalance,
            Currency = user.PreferredCurrency.ToString(),
            CurrentMonthIncome = currentMonthIncome,
            CurrentMonthExpense = currentMonthExpense,
            CurrentMonthNet = currentMonthIncome - currentMonthExpense,
            LastMonthIncome = lastMonthIncome,
            LastMonthExpense = lastMonthExpense,
            IncomeChangePercentage = Math.Round(incomeChange, 2),
            ExpenseChangePercentage = Math.Round(expenseChange, 2),
            TotalAccounts = allAccounts.Count,
            ActiveAccounts = activeAccounts,
            TotalGoals = allGoals.Count,
            ActiveGoals = activeGoals,
            CompletedGoalsThisMonth = completedGoalsThisMonth,
            BudgetAlerts = budgetAlerts,
            RecentTransactions = recentTransactions
        };

        return Result<DashboardDto>.Success(dashboard, "Dashboard verileri başarıyla getirildi");
    }

    private async Task<List<BudgetAlertDto>> GetBudgetAlertsAsync(
        Guid userId,
        DateTime startDate,
        DateTime endDate,
        CancellationToken cancellationToken)
    {
        var budgets = await _unitOfWork.Budgets.GetActiveByUserIdAsync(userId, cancellationToken);
        var alerts = new List<BudgetAlertDto>();

        foreach (var budget in budgets)
        {
            // Bu bütçe döneminde mi?
            if (!budget.IsCurrentPeriod(DateTime.UtcNow))
                continue;

            // Bu kategorideki harcamaları hesapla
            var categoryTransactions = await _unitOfWork.Transactions.GetByCategoryIdAsync(
                budget.CategoryId, startDate, endDate, cancellationToken);

            var spentAmount = categoryTransactions
                .Where(t => t.Type == TransactionType.Expense)
                .Sum(t => t.Amount.Amount);

            var spentPercentage = (spentAmount / budget.Amount.Amount) * 100;

            string alertLevel;
            if (spentPercentage >= 100)
            {
                alertLevel = "Danger";
            }
            else if (spentPercentage >= budget.AlertThresholdPercentage)
            {
                alertLevel = "Warning";
            }
            else
            {
                alertLevel = "Normal";
            }

            if (alertLevel != "Normal")
            {
                alerts.Add(new BudgetAlertDto
                {
                    BudgetId = budget.Id,
                    BudgetName = budget.Name,
                    CategoryName = budget.Category.Name,
                    BudgetAmount = budget.Amount.Amount,
                    SpentAmount = spentAmount,
                    SpentPercentage = Math.Round(spentPercentage, 2),
                    AlertLevel = alertLevel
                });
            }
        }

        return alerts.OrderByDescending(a => a.SpentPercentage).ToList();
    }

    private async Task<List<RecentTransactionDto>> GetRecentTransactionsAsync(
        Guid userId,
        CancellationToken cancellationToken)
    {
        var transactions = await _unitOfWork.Transactions.GetByUserIdAsync(
            userId,
            startDate: DateTime.UtcNow.AddDays(-30),
            endDate: DateTime.UtcNow,
            cancellationToken);

        return transactions
            .OrderByDescending(t => t.TransactionDate)
            .Take(10)
            .Select(t => new RecentTransactionDto
            {
                Id = t.Id,
                Type = t.Type.ToString(),
                CategoryName = t.Category.Name,
                CategoryIcon = t.Category.Icon ?? "📦",
                Amount = t.Amount.Amount,
                Description = t.Description ?? "İsimsiz işlem",
                TransactionDate = t.TransactionDate
            })
            .ToList();
    }
}