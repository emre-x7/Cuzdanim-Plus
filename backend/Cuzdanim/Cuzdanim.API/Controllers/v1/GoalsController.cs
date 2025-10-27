using Cuzdanim.Application.Features.Goals.Commands.AddContribution;
using Cuzdanim.Application.Features.Goals.Commands.CreateGoal;
using Cuzdanim.Application.Features.Goals.Commands.DeleteGoal;
using Cuzdanim.Application.Features.Goals.Commands.UpdateGoal;
using Cuzdanim.Application.Features.Goals.Queries.GetGoalById;
using Cuzdanim.Application.Features.Goals.Queries.GetGoalsByUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cuzdanim.API.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class GoalsController : ControllerBase
{
    private readonly IMediator _mediator;

    public GoalsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Kullanıcının tüm hedeflerini getir
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetGoals(CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirst("userId")?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized(new { message = "Geçersiz token" });
        }

        var query = new GetGoalsByUserQuery { UserId = userId };
        var result = await _mediator.Send(query, cancellationToken);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    /// <summary>
    /// ID ile hedef getir
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetGoalById(Guid id, CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirst("userId")?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized(new { message = "Geçersiz token" });
        }

        var query = new GetGoalByIdQuery { Id = id, UserId = userId };
        var result = await _mediator.Send(query, cancellationToken);

        if (!result.IsSuccess)
        {
            return NotFound(result);
        }

        return Ok(result);
    }

    /// <summary>
    /// Yeni hedef oluştur
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateGoal([FromBody] CreateGoalCommand command, CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirst("userId")?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized(new { message = "Geçersiz token" });
        }

        command.UserId = userId;

        var result = await _mediator.Send(command, cancellationToken);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return CreatedAtAction(nameof(GetGoalById), new { id = result.Data }, result);
    }

    /// <summary>
    /// Hedef güncelle
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateGoal(Guid id, [FromBody] UpdateGoalCommand command, CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirst("userId")?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized(new { message = "Geçersiz token" });
        }

        command.Id = id;
        command.UserId = userId;

        var result = await _mediator.Send(command, cancellationToken);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    /// <summary>
    /// Hedefe katkı ekle
    /// </summary>
    [HttpPost("{id}/contribute")]
    public async Task<IActionResult> AddContribution(Guid id, [FromBody] AddContributionCommand command, CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirst("userId")?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized(new { message = "Geçersiz token" });
        }

        command.GoalId = id;
        command.UserId = userId;

        var result = await _mediator.Send(command, cancellationToken);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    /// <summary>
    /// Hedef sil (Soft delete)
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGoal(Guid id, CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirst("userId")?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized(new { message = "Geçersiz token" });
        }

        var command = new DeleteGoalCommand { Id = id, UserId = userId };
        var result = await _mediator.Send(command, cancellationToken);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }
}