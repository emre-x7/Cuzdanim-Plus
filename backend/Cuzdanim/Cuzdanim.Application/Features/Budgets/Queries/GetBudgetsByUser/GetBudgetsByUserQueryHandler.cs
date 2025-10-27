using AutoMapper;
using Cuzdanim.Application.Common.Interfaces;
using Cuzdanim.Application.Common.Models;
using Cuzdanim.Application.Features.Budgets.DTOs;
using MediatR;

namespace Cuzdanim.Application.Features.Budgets.Queries.GetBudgetsByUser;

public class GetBudgetsByUserQueryHandler : IRequestHandler<GetBudgetsByUserQuery, Result<List<BudgetDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetBudgetsByUserQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<List<BudgetDto>>> Handle(GetBudgetsByUserQuery request, CancellationToken cancellationToken)
    {
        var budgets = await _unitOfWork.Budgets.GetActiveByUserIdAsync(request.UserId, cancellationToken);

        var budgetDtos = _mapper.Map<List<BudgetDto>>(budgets);

        return Result<List<BudgetDto>>.Success(budgetDtos, $"{budgetDtos.Count} bütçe bulundu");
    }
}