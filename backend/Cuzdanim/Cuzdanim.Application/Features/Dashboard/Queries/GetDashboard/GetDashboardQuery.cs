using Cuzdanim.Application.Common.Models;
using Cuzdanim.Application.Features.Dashboard.DTOs;
using MediatR;

namespace Cuzdanim.Application.Features.Dashboard.Queries.GetDashboard;

public class GetDashboardQuery : IRequest<Result<DashboardDto>>
{
    public Guid UserId { get; set; }
}