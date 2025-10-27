using Cuzdanim.Application.Common.Interfaces;
using Cuzdanim.Application.Common.Models;
using Cuzdanim.Domain.Entities; 
using MediatR;
using Microsoft.Extensions.Options;
using RefreshTokenEntity = Cuzdanim.Domain.Entities.RefreshToken; 

namespace Cuzdanim.Application.Features.Auth.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<LoginResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly JwtSettings _jwtSettings;

    public LoginCommandHandler(
        IUnitOfWork unitOfWork,
        IJwtTokenService jwtTokenService,
        IOptions<JwtSettings> jwtSettings)
    {
        _unitOfWork = unitOfWork;
        _jwtTokenService = jwtTokenService;
        _jwtSettings = jwtSettings.Value;
    }

    public async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        // 1. Kullanıcıyı email ile bul
        var user = await _unitOfWork.Users.GetByEmailAsync(request.Email, cancellationToken);

        if (user == null)
        {
            return Result<LoginResponse>.Failure("Email veya şifre hatalı");
        }

        // 2. Şifre kontrolü
        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            return Result<LoginResponse>.Failure("Email veya şifre hatalı");
        }

        // 3. Email doğrulanmış mı kontrolünü geçici olarak devre dışı bırak (test için)
        // if (!user.IsEmailVerified)
        // {
        //     return Result<LoginResponse>.Failure("Email adresinizi doğrulamalısınız");
        // }

        // 4. Access token oluştur
        var accessToken = _jwtTokenService.GenerateAccessToken(user);

        // 5. Refresh token oluştur ve kaydet
        var refreshTokenString = _jwtTokenService.GenerateRefreshToken();
        var refreshTokenExpiresAt = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationInDays);

        //  Alias kullanarak Entity'ye erişiyoruz
        var refreshToken = RefreshTokenEntity.Create(
            user.Id,
            refreshTokenString,
            refreshTokenExpiresAt,
            request.IpAddress
        );

        // 6. Veritabanına kaydet
        await _unitOfWork.RefreshTokens.AddAsync(refreshToken, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // 7. Response oluştur
        var response = new LoginResponse
        {
            UserId = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            AccessToken = accessToken,
            RefreshToken = refreshTokenString,
            ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationInMinutes)
        };

        return Result<LoginResponse>.Success(response, "Giriş başarılı");
    }
}