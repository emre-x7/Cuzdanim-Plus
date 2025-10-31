using Cuzdanim.Application.Common.Models;
using Cuzdanim.Application.Features.Reports.Queries.GetReport;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cuzdanim.API.Controllers.v1;

[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReportsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Rapor verilerini getir
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(Result<ReportDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result<ReportDto>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetReport(
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate,
        CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirst("userId")?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized(new { message = "Geçersiz token" });
        }

        var query = new GetReportQuery
        {
            UserId = userId,
            StartDate = startDate,
            EndDate = endDate
        };

        var result = await _mediator.Send(query, cancellationToken);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }
}
