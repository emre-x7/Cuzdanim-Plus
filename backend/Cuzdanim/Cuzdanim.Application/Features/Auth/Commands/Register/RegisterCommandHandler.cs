using Cuzdanim.Application.Common.Interfaces;
using Cuzdanim.Application.Common.Models;
using Cuzdanim.Domain.Entities;
using MediatR;

namespace Cuzdanim.Application.Features.Auth.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<RegisterResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICategorySeederService _categorySeeder;

    public RegisterCommandHandler(IUnitOfWork unitOfWork, ICategorySeederService categorySeeder)
    {
        _unitOfWork = unitOfWork;
        _categorySeeder = categorySeeder;
    }

    public async Task<Result<RegisterResponse>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        // 1. Email zaten var mı kontrol et
        if (await _unitOfWork.Users.IsEmailExistsAsync(request.Email, cancellationToken))
        {
            return Result<RegisterResponse>.Failure("Bu email adresi zaten kayıtlı");
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
            var dateOfBirthUtc = request.DateOfBirth.HasValue
                ? DateTime.SpecifyKind(request.DateOfBirth.Value, DateTimeKind.Utc)
                : (DateTime?)null;

            user.UpdateProfile(
                request.FirstName,
                request.LastName,
                request.PhoneNumber ?? string.Empty,
                dateOfBirthUtc
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

            // 6. Response oluştur
            var response = new RegisterResponse
            {
                UserId = user.Id,
                Email = user.Email,
                Message = "Kayıt başarılı! Giriş yapabilirsiniz." // Email doğrulama kapalı olduğu için direkt login olabilir
            };

            return Result<RegisterResponse>.Success(response, "Kullanıcı başarıyla oluşturuldu");
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }
}