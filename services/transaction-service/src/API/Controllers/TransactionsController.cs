using MediatR;
using Application.UseCase.Transactions.Commands.CreateTransaction;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;

namespace API.Controllers;

[ApiController]
[Route("api/controller")]
public class TransactionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public TransactionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateTransaction([FromBody] CreateTransactionCommand command)
    {
        try
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        catch(ValidationException ex)
        {
            var errors = ex.Errors.Select(e => new {e.PropertyName, e.ErrorMessage});
            return BadRequest(new { Message = "Validasi gagal", Errors = errors });
        }
    }
}