using AutoMapper;
using Cuzdanim.Application.Common.Interfaces;
using Cuzdanim.Application.Common.Models;
using Cuzdanim.Application.Features.Accounts.DTOs;
using MediatR;

namespace Cuzdanim.Application.Features.Accounts.Queries.GetAccountsByUser;

public class GetAccountsByUserQueryHandler : IRequestHandler<GetAccountsByUserQuery, Result<List<AccountDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAccountsByUserQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<List<AccountDto>>> Handle(GetAccountsByUserQuery request, CancellationToken cancellationToken)
    {
        var accounts = await _unitOfWork.Accounts.GetByUserIdAsync(request.UserId, cancellationToken);

        var accountDtos = _mapper.Map<List<AccountDto>>(accounts);

        return Result<List<AccountDto>>.Success(accountDtos, $"{accountDtos.Count} hesap bulundu");
    }
}