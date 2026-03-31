using MediatR;
using Application.UseCase.Transactions.Commands.CreateTransaction;
using Application.UseCase.Transactions.Queries.GetTransactionId;
using Application.UseCase.Transactions.Commands.Queries.GetReferenceNumber;
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
            var result = await _mediator.Send(command);
            return Ok(result);
    }
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetTransactionId(long id)
    {
            var result = await _mediator.Send(new GetTransactionIdQuery(id));
            return Ok(result);
    }
    [HttpGet("reference/{referenceNumber}")]
    public async Task<IActionResult> GetReferenceNumber(string referenceNumber)
    {
            var result = await _mediator.Send(new GetReferenceNumberQuery(referenceNumber));
            return Ok(result);
    }
}