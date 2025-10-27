using Cuzdanim.Application.Common.Models;
using Cuzdanim.Application.Features.Users.DTOs;
using MediatR;

namespace Cuzdanim.Application.Features.Users.Queries.GetUserById;

public class GetUserByIdQuery : IRequest<Result<UserDto>>
{
    public Guid UserId { get; set; }
}