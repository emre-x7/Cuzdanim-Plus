using AutoMapper;
using Cuzdanim.Application.Common.Interfaces;
using Cuzdanim.Application.Common.Models;
using Cuzdanim.Application.Features.Transactions.DTOs;
using MediatR;

namespace Cuzdanim.Application.Features.Transactions.Queries.GetTransactionsByDateRange;

public class GetTransactionsByDateRangeQueryHandler
    : IRequestHandler<GetTransactionsByDateRangeQuery, Result<List<TransactionDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetTransactionsByDateRangeQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<List<TransactionDto>>> Handle(
        GetTransactionsByDateRangeQuery request,
        CancellationToken cancellationToken)
    {
        // 1. Kullanıcı kontrolü
        var user = await _unitOfWork.Users.GetByIdAsync(request.UserId, cancellationToken);
        if (user == null)
        {
            return Result<List<TransactionDto>>.Failure("Kullanıcı bulunamadı");
        }

        // 2. İşlemleri getir
        var transactions = await _unitOfWork.Transactions.GetByUserIdAsync(
            request.UserId,
            request.StartDate,
            request.EndDate,
            cancellationToken
        );

        // 3. Hesap filtresi varsa uygula
        if (request.AccountId.HasValue)
        {
            transactions = transactions
                .Where(t => t.AccountId == request.AccountId.Value)
                .ToList();
        }

        // 4. Kategori filtresi varsa uygula
        if (request.CategoryId.HasValue)
        {
            transactions = transactions
                .Where(t => t.CategoryId == request.CategoryId.Value)
                .ToList();
        }

        // 5. DTO'ya dönüştür
        var transactionDtos = _mapper.Map<List<TransactionDto>>(transactions);

        return Result<List<TransactionDto>>.Success(
            transactionDtos,
            $"{transactionDtos.Count} işlem bulundu"
        );
    }
}