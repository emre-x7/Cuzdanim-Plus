using Cuzdanim.Application.Common.Interfaces;
using Cuzdanim.Application.Common.Models;
using Cuzdanim.Domain.Entities;
using MediatR;

namespace Cuzdanim.Application.Features.Users.Commands.CreateUser;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<Guid>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICategorySeederService _categorySeeder;

    public CreateUserCommandHandler(IUnitOfWork unitOfWork, ICategorySeederService categorySeeder)
    {
        _unitOfWork = unitOfWork;
        _categorySeeder = categorySeeder;
    }

    public async Task<Result<Guid>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        // 1. Email zaten var mı kontrol et
        if (await _unitOfWork.Users.IsEmailExistsAsync(request.Email, cancellationToken))
        {
            return Result<Guid>.Failure("Bu email adresi zaten kayıtlı");
        }

        // 2. Şifreyi hash'le
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        // 3. User entity oluştur
        var user = User.Create(
            request.Email,
            passwordHash,
            request.FirstName,
            request.LastName
        );

        // 4. Opsiyonel alanları set et
        if (request.PhoneNumber != null || request.DateOfBirth.HasValue)
        {
            user.UpdateProfile(
                request.FirstName,
                request.LastName,
                request.PhoneNumber ?? string.Empty,
                request.DateOfBirth
            );
        }

        // 5. Transaction başlat (User + Categories birlikte kaydedilmeli)
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            // Kullanıcıyı kaydet
            await _unitOfWork.Users.AddAsync(user, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Default kategorileri oluştur
            await _categorySeeder.SeedDefaultCategoriesForUserAsync(user.Id);

            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            return Result<Guid>.Success(user.Id, "Kullanıcı başarıyla oluşturuldu");
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }
}