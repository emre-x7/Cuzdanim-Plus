using AutoMapper;
using Cuzdanim.Application.Common.Interfaces;
using Cuzdanim.Application.Common.Models;
using Cuzdanim.Application.Features.Goals.DTOs;
using MediatR;

namespace Cuzdanim.Application.Features.Goals.Queries.GetGoalsByUser;

public class GetGoalsByUserQueryHandler : IRequestHandler<GetGoalsByUserQuery, Result<List<GoalDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetGoalsByUserQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<List<GoalDto>>> Handle(GetGoalsByUserQuery request, CancellationToken cancellationToken)
    {
        var goals = await _unitOfWork.Goals.GetByUserIdAsync(request.UserId, cancellationToken);

        var goalDtos = _mapper.Map<List<GoalDto>>(goals);

        return Result<List<GoalDto>>.Success(goalDtos, $"{goalDtos.Count} hedef bulundu");
    }
}