using AutoMapper;
using Cuzdanim.Application.Common.Interfaces;
using Cuzdanim.Application.Common.Models;
using Cuzdanim.Application.Features.Budgets.DTOs;
using Cuzdanim.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Cuzdanim.Application.Features.Budgets.Queries.GetBudgetsByUser;

public class GetBudgetsByUserQueryHandler : IRequestHandler<GetBudgetsByUserQuery, Result<List<BudgetDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IApplicationDbContext _context;

    public GetBudgetsByUserQueryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IApplicationDbContext context)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _context = context;
    }

    public async Task<Result<List<BudgetDto>>> Handle(GetBudgetsByUserQuery request, CancellationToken cancellationToken)
    {
        // Bütçeleri getir (Category ile birlikte)
        var budgets = await _unitOfWork.Budgets.GetActiveByUserIdAsync(request.UserId, cancellationToken);

        // DTO'ya map et
        var budgetDtos = _mapper.Map<List<BudgetDto>>(budgets);

        // Her bütçe için spent, remaining, percentageUsed, status hesapla
        foreach (var budgetDto in budgetDtos)
        {
            var budget = budgets.First(b => b.Id == budgetDto.Id);

            // Category bilgilerini ekle (mapper'da eksik olabilir)
            budgetDto.CategoryIcon = budget.Category?.Icon;
            budgetDto.CategoryColor = budget.Category?.Color;

            // Bu periyotta bu kategoriye yapılan harcamaları hesapla
            var spent = await _context.Transactions
                .Where(t => t.UserId == request.UserId
                    && t.CategoryId == budget.CategoryId
                    && t.Type == TransactionType.Expense
                    && t.TransactionDate >= budget.Period.StartDate
                    && t.TransactionDate <= budget.Period.EndDate
                    && !t.IsDeleted)
                .SumAsync(t => t.Amount.Amount, cancellationToken);

            // Kalan ve yüzde hesapla
            var remaining = budget.Amount.Amount - spent;
            var percentageUsed = budget.Amount.Amount > 0
                ? (spent / budget.Amount.Amount) * 100
                : 0;

            // Status belirle
            string status = "Normal";
            if (percentageUsed >= 100)
                status = "Exceeded";
            else if (percentageUsed >= budget.AlertThresholdPercentage)
                status = "Warning";

            // DTO'ya ata
            budgetDto.Spent = spent;
            budgetDto.Remaining = remaining;
            budgetDto.PercentageUsed = percentageUsed;
            budgetDto.Status = status;
        }

        return Result<List<BudgetDto>>.Success(budgetDtos, $"{budgetDtos.Count} bütçe bulundu");
    }
}