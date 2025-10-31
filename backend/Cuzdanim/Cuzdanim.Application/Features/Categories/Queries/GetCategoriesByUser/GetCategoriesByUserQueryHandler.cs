using AutoMapper;
using Cuzdanim.Application.Common.Interfaces;
using Cuzdanim.Application.Common.Models;
using Cuzdanim.Application.Features.Categories.DTOs;
using MediatR;

namespace Cuzdanim.Application.Features.Categories.Queries.GetCategoriesByUser;

public class GetCategoriesByUserQueryHandler : IRequestHandler<GetCategoriesByUserQuery, Result<List<CategoryDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetCategoriesByUserQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<List<CategoryDto>>> Handle(GetCategoriesByUserQuery request, CancellationToken cancellationToken)
    {
        var categories = await _unitOfWork.Categories.GetByUserIdAsync(request.UserId, cancellationToken);

        var categoryDtos = _mapper.Map<List<CategoryDto>>(categories);

        return Result<List<CategoryDto>>.Success(categoryDtos, $"{categoryDtos.Count} kategori bulundu");
    }
}