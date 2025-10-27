using AutoMapper;
using Cuzdanim.Application.Common.Interfaces;
using Cuzdanim.Application.Common.Models;
using Cuzdanim.Application.Features.Budgets.DTOs;
using MediatR;

namespace Cuzdanim.Application.Features.Budgets.Queries.GetBudgetById;

public class GetBudgetByIdQueryHandler : IRequestHandler<GetBudgetByIdQuery, Result<BudgetDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetBudgetByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<BudgetDto>> Handle(GetBudgetByIdQuery request, CancellationToken cancellationToken)
    {
        var budget = await _unitOfWork.Budgets.GetByIdAsync(request.Id, cancellationToken);

        if (budget == null)
        {
            return Result<BudgetDto>.Failure("Bütçe bulunamadı");
        }

        // Yetki kontrolü
        if (budget.UserId != request.UserId)
        {
            return Result<BudgetDto>.Failure("Bu bütçeye erişim yetkiniz yok");
        }

        var budgetDto = _mapper.Map<BudgetDto>(budget);

        return Result<BudgetDto>.Success(budgetDto);
    }
}