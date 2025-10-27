using Cuzdanim.Application.Common.Interfaces;
using Cuzdanim.Application.Common.Models;
using Cuzdanim.Application.Features.Auth.Commands.Login;
using Cuzdanim.Domain.Entities; 
using MediatR;
using Microsoft.Extensions.Options;
using RefreshTokenEntity = Cuzdanim.Domain.Entities.RefreshToken; 

namespace Cuzdanim.Application.Features.Auth.Commands.RefreshToken;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, Result<LoginResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly JwtSettings _jwtSettings;

    public RefreshTokenCommandHandler(
        IUnitOfWork unitOfWork,
        IJwtTokenService jwtTokenService,
        IOptions<JwtSettings> jwtSettings)
    {
        _unitOfWork = unitOfWork;
        _jwtTokenService = jwtTokenService;
        _jwtSettings = jwtSettings.Value;
    }

    public async Task<Result<LoginResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        // 1. Refresh token'ı bul
        var refreshToken = await _unitOfWork.RefreshTokens.GetByTokenAsync(request.RefreshToken, cancellationToken);

        if (refreshToken == null)
        {
            return Result<LoginResponse>.Failure("Geçersiz token");
        }

        // 2. Token aktif mi kontrol et
        if (!refreshToken.IsActive)
        {
            return Result<LoginResponse>.Failure("Token süresi dolmuş veya iptal edilmiş");
        }

        // 3. Kullanıcıyı al
        var user = await _unitOfWork.Users.GetByIdAsync(refreshToken.UserId, cancellationToken);

        if (user == null)
        {
            return Result<LoginResponse>.Failure("Kullanıcı bulunamadı");
        }

        // 4. Yeni access token oluştur
        var newAccessToken = _jwtTokenService.GenerateAccessToken(user);

        // 5. Yeni refresh token oluştur
        var newRefreshTokenString = _jwtTokenService.GenerateRefreshToken();
        var newRefreshTokenExpiresAt = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationInDays);

        // Alias kullanarak Entity'ye erişiyoruz
        var newRefreshToken = RefreshTokenEntity.Create(
            user.Id,
            newRefreshTokenString,
            newRefreshTokenExpiresAt,
            request.IpAddress
        );

        // 6. Eski token'ı iptal et, yenisini kaydet
        refreshToken.Revoke(request.IpAddress, newRefreshTokenString);

        await _unitOfWork.RefreshTokens.AddAsync(newRefreshToken, cancellationToken);
        _unitOfWork.RefreshTokens.Update(refreshToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // 7. Response oluştur
        var response = new LoginResponse
        {
            UserId = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            AccessToken = newAccessToken,
            RefreshToken = newRefreshTokenString,
            ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationInMinutes)
        };

        return Result<LoginResponse>.Success(response, "Token yenilendi");
    }
}