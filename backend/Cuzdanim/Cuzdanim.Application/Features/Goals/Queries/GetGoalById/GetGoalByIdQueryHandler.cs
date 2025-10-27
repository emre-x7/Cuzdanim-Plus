using AutoMapper;
using Cuzdanim.Application.Common.Interfaces;
using Cuzdanim.Application.Common.Models;
using Cuzdanim.Application.Features.Goals.DTOs;
using MediatR;

namespace Cuzdanim.Application.Features.Goals.Queries.GetGoalById;

public class GetGoalByIdQueryHandler : IRequestHandler<GetGoalByIdQuery, Result<GoalDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetGoalByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<GoalDto>> Handle(GetGoalByIdQuery request, CancellationToken cancellationToken)
    {
        var goal = await _unitOfWork.Goals.GetByIdAsync(request.Id, cancellationToken);

        if (goal == null)
        {
            return Result<GoalDto>.Failure("Hedef bulunamadı");
        }

        // Yetki kontrolü
        if (goal.UserId != request.UserId)
        {
            return Result<GoalDto>.Failure("Bu hedefe erişim yetkiniz yok");
        }

        var goalDto = _mapper.Map<GoalDto>(goal);

        return Result<GoalDto>.Success(goalDto);
    }
}