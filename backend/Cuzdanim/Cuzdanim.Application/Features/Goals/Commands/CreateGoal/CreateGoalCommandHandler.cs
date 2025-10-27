using Cuzdanim.Application.Common.Interfaces;
using Cuzdanim.Application.Common.Models;
using Cuzdanim.Domain.Entities;
using Cuzdanim.Domain.ValueObjects;
using MediatR;

namespace Cuzdanim.Application.Features.Goals.Commands.CreateGoal;

public class CreateGoalCommandHandler : IRequestHandler<CreateGoalCommand, Result<Guid>>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateGoalCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateGoalCommand request, CancellationToken cancellationToken)
    {
        // 1. Kullanıcı kontrolü
        var user = await _unitOfWork.Users.GetByIdAsync(request.UserId, cancellationToken);
        if (user == null)
        {
            return Result<Guid>.Failure("Kullanıcı bulunamadı");
        }

        // 2. Money Value Object oluştur
        var targetAmount = new Money(request.TargetAmount, request.Currency);

        // 3. Goal entity oluştur
        var goal = Goal.Create(
            request.UserId,
            request.Name,
            targetAmount,
            request.TargetDate,
            request.Description
        );

        // 4. Opsiyonel alanlar
        if (!string.IsNullOrEmpty(request.ImageUrl))
        {
            var imageProperty = typeof(Goal).GetProperty("ImageUrl");
            imageProperty?.SetValue(goal, request.ImageUrl);
        }

        if (!string.IsNullOrEmpty(request.Icon))
        {
            var iconProperty = typeof(Goal).GetProperty("Icon");
            iconProperty?.SetValue(goal, request.Icon);
        }

        // 5. Veritabanına kaydet
        await _unitOfWork.Goals.AddAsync(goal, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(goal.Id, "Hedef başarıyla oluşturuldu");
    }
}