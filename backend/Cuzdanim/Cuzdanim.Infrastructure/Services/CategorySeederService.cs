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
                // Gider Kategorileri
                Category.Create(userId, "Yemek & İçecek", CategoryType.Food, "🍔", "#FF6B6B", true),
                Category.Create(userId, "Ulaşım", CategoryType.Transportation, "🚗", "#4ECDC4", true),
                Category.Create(userId, "Alışveriş", CategoryType.Shopping, "🛍️", "#95E1D3", true),
                Category.Create(userId, "Faturalar", CategoryType.Bills, "📄", "#F38181", true),
                Category.Create(userId, "Sağlık", CategoryType.Health, "💊", "#AA96DA", true),
                Category.Create(userId, "Eğitim", CategoryType.Education, "📚", "#FCBAD3", true),
                Category.Create(userId, "Eğlence", CategoryType.Entertainment, "🎮", "#FEE440", true),
                Category.Create(userId, "Seyahat", CategoryType.Travel, "✈️", "#00BBF9", true),
                Category.Create(userId, "Konut", CategoryType.Housing, "🏠", "#F28482", true),
                Category.Create(userId, "Sigorta", CategoryType.Insurance, "🛡️", "#84A59D", true),
                Category.Create(userId, "Birikim", CategoryType.Savings, "💰", "#06FFA5", true),
                Category.Create(userId, "Diğer Giderler", CategoryType.Other, "📦", "#A0A0A0", true),

                // Gelir Kategorileri
                Category.Create(userId, "Maaş", CategoryType.Salary, "💵", "#06D6A0", true),
                Category.Create(userId, "İkramiye", CategoryType.Bonus, "🎁", "#FFD23F", true),
                Category.Create(userId, "Yatırım Getirisi", CategoryType.Investment, "📈", "#118AB2", true),
                Category.Create(userId, "Hediye", CategoryType.Gift, "🎉", "#EF476F", true),
                Category.Create(userId, "İade", CategoryType.Refund, "↩️", "#06FFA5", true),
            };

            await _context.Categories.AddRangeAsync(defaultCategories);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Default categories created for user {UserId}", userId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error seeding default categories for user {UserId}", userId);
            throw;
        }
    }
}