namespace Cuzdanim.Application.Features.Dashboard.DTOs;

public class DashboardDto
{
    public decimal TotalBalance { get; set; }
    public string Currency { get; set; } = "TRY";

    // Bu ay
    public decimal CurrentMonthIncome { get; set; }
    public decimal CurrentMonthExpense { get; set; }
    public decimal CurrentMonthNet { get; set; }

    // Geçen ay
    public decimal LastMonthIncome { get; set; }
    public decimal LastMonthExpense { get; set; }

    // Değişim yüzdeleri
    public decimal IncomeChangePercentage { get; set; }
    public decimal ExpenseChangePercentage { get; set; }

    // Hesaplar
    public int TotalAccounts { get; set; }
    public int ActiveAccounts { get; set; }

    // Hedefler
    public int TotalGoals { get; set; }
    public int ActiveGoals { get; set; }
    public int CompletedGoalsThisMonth { get; set; }

    // Bütçe uyarıları
    public List<BudgetAlertDto> BudgetAlerts { get; set; } = new();

    // Son işlemler
    public List<RecentTransactionDto> RecentTransactions { get; set; } = new();
}

public class BudgetAlertDto
{
    public Guid BudgetId { get; set; }
    public string BudgetName { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
    public decimal BudgetAmount { get; set; }
    public decimal SpentAmount { get; set; }
    public decimal SpentPercentage { get; set; }
    public string AlertLevel { get; set; } = string.Empty; // Warning, Danger
}

public class RecentTransactionDto
{
    public Guid Id { get; set; }
    public string Type { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
    public string CategoryIcon { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime TransactionDate { get; set; }
}