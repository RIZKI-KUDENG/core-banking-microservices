using MediatR;
using Application.UseCase.Transactions.Commands.CreateTransaction;
using Application.UseCase.Transactions.Queries.GetTransactionId;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
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
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetTransactionId(long id)
    {
        try
        {
            var result = await _mediator.Send(new GetTransactionIdQuery(id));
            return Ok(result);
        }
        catch
        (ValidationException ex)
        {
            var errors = ex.Errors.Select(e => new { e.PropertyName, e.ErrorMessage });
            return BadRequest(new { Message = "Validasi gagal", Errors = errors });
        }catch (Exception ex) when (ex.Message == "Transaction not found")
        {
            return NotFound(new { Message = ex.Message });
        }
    }
}