using Cuzdanim.Application.Features.Budgets.Commands.CreateBudget;
using Cuzdanim.Application.Features.Budgets.Commands.DeleteBudget;
using Cuzdanim.Application.Features.Budgets.Commands.UpdateBudget;
using Cuzdanim.Application.Features.Budgets.Queries.GetBudgetById;
using Cuzdanim.Application.Features.Budgets.Queries.GetBudgetsByUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cuzdanim.API.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class BudgetsController : ControllerBase
{
    private readonly IMediator _mediator;

    public BudgetsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Kullanıcının tüm bütçelerini getir
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetBudgets(CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirst("userId")?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized(new { message = "Geçersiz token" });
        }

        var query = new GetBudgetsByUserQuery { UserId = userId };
        var result = await _mediator.Send(query, cancellationToken);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    /// <summary>
    /// ID ile bütçe getir
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetBudgetById(Guid id, CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirst("userId")?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized(new { message = "Geçersiz token" });
        }

        var query = new GetBudgetByIdQuery { Id = id, UserId = userId };
        var result = await _mediator.Send(query, cancellationToken);

        if (!result.IsSuccess)
        {
            return NotFound(result);
        }

        return Ok(result);
    }

    /// <summary>
    /// Yeni bütçe oluştur
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateBudget([FromBody] CreateBudgetCommand command, CancellationToken cancellationToken)
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

        return CreatedAtAction(nameof(GetBudgetById), new { id = result.Data }, result);
    }

    /// <summary>
    /// Bütçe güncelle
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBudget(Guid id, [FromBody] UpdateBudgetCommand command, CancellationToken cancellationToken)
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
    /// Bütçe sil (Soft delete)
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBudget(Guid id, CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirst("userId")?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized(new { message = "Geçersiz token" });
        }

        var command = new DeleteBudgetCommand { Id = id, UserId = userId };
        var result = await _mediator.Send(command, cancellationToken);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }
}