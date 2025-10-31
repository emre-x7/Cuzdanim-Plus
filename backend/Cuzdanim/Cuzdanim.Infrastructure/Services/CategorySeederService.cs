using Cuzdanim.Application.Common.Interfaces;
using Cuzdanim.Domain.Entities;
using Cuzdanim.Domain.Enums;
using Cuzdanim.Infrastructure.Data.Context;
using Microsoft.Extensions.Logging;

namespace Cuzdanim.Infrastructure.Services;

public class CategorySeederService : ICategorySeederService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<CategorySeederService> _logger;

    public CategorySeederService(ApplicationDbContext context, ILogger<CategorySeederService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task SeedDefaultCategoriesForUserAsync(Guid userId)
    {
        try
        {
            var defaultCategories = new List<Category>
            {
                // ============================================
                // GIDER KATEGORİLERİ (EXPENSE)
                // ============================================
                Category.Create(userId, "Yemek & İçecek", TransactionType.Expense, CategoryType.Food, "🍔", "#FF6B6B", true),
                Category.Create(userId, "Ulaşım", TransactionType.Expense, CategoryType.Transportation, "🚗", "#4ECDC4", true),
                Category.Create(userId, "Alışveriş", TransactionType.Expense, CategoryType.Shopping, "🛍️", "#95E1D3", true),
                Category.Create(userId, "Faturalar", TransactionType.Expense, CategoryType.Bills, "📄", "#F38181", true),
                Category.Create(userId, "Sağlık", TransactionType.Expense, CategoryType.Health, "💊", "#AA96DA", true),
                Category.Create(userId, "Eğitim", TransactionType.Expense, CategoryType.Education, "📚", "#FCBAD3", true),
                Category.Create(userId, "Eğlence", TransactionType.Expense, CategoryType.Entertainment, "🎮", "#FEE440", true),
                Category.Create(userId, "Seyahat", TransactionType.Expense, CategoryType.Travel, "✈️", "#00BBF9", true),
                Category.Create(userId, "Konut", TransactionType.Expense, CategoryType.Housing, "🏠", "#F28482", true),
                Category.Create(userId, "Sigorta", TransactionType.Expense, CategoryType.Insurance, "🛡️", "#84A59D", true),
                Category.Create(userId, "Birikim", TransactionType.Expense, CategoryType.Savings, "💰", "#06FFA5", true),
                Category.Create(userId, "Diğer Giderler", TransactionType.Expense, CategoryType.Other, "📦", "#A0A0A0", true),

                // ============================================
                // GELİR KATEGORİLERİ (INCOME)
                // ============================================
                Category.Create(userId, "Maaş", TransactionType.Income, CategoryType.Salary, "💵", "#06D6A0", true),
                Category.Create(userId, "İkramiye", TransactionType.Income, CategoryType.Bonus, "🎁", "#FFD23F", true),
                Category.Create(userId, "Yatırım Getirisi", TransactionType.Income, CategoryType.Investment, "📈", "#118AB2", true),
                Category.Create(userId, "Hediye", TransactionType.Income, CategoryType.Gift, "🎉", "#EF476F", true),
                Category.Create(userId, "İade", TransactionType.Income, CategoryType.Refund, "↩️", "#06FFA5", true),
            };

            await _context.Categories.AddRangeAsync(defaultCategories);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Default categories created for user {UserId}. Total: {Count} categories",
                userId, defaultCategories.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error seeding default categories for user {UserId}", userId);
            throw;
        }
    }
}