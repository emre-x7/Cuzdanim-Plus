using Cuzdanim.Application.Common.Models;
using Cuzdanim.Application.Features.Auth.Commands.Login;
using MediatR;

namespace Cuzdanim.Application.Features.Auth.Commands.RefreshToken;

public class RefreshTokenCommand : IRequest<Result<LoginResponse>>
{
    public string RefreshToken { get; set; } = string.Empty;
    public string IpAddress { get; set; } = string.Empty;
}