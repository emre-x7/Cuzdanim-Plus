using Cuzdanim.Application.Common.Models;
using MediatR;

namespace Cuzdanim.Application.Features.Reports.Queries.GetReport;

public class GetReportQuery : IRequest<Result<ReportDto>>
{
    public Guid UserId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}