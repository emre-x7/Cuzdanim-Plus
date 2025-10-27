using AutoMapper;
using Cuzdanim.Application.Common.Interfaces;
using Cuzdanim.Application.Common.Models;
using Cuzdanim.Application.Features.Transactions.DTOs;
using MediatR;

namespace Cuzdanim.Application.Features.Transactions.Queries.GetTransactionById;

public class GetTransactionByIdQueryHandler : IRequestHandler<GetTransactionByIdQuery, Result<TransactionDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetTransactionByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<TransactionDto>> Handle(GetTransactionByIdQuery request, CancellationToken cancellationToken)
    {
        var transaction = await _unitOfWork.Transactions.GetByIdAsync(request.Id, cancellationToken);

        if (transaction == null)
        {
            return Result<TransactionDto>.Failure("İşlem bulunamadı");
        }

        // Yetki kontrolü
        if (transaction.UserId != request.UserId)
        {
            return Result<TransactionDto>.Failure("Bu işleme erişim yetkiniz yok");
        }

        var transactionDto = _mapper.Map<TransactionDto>(transaction);

        return Result<TransactionDto>.Success(transactionDto);
    }
}