using MediatR;
using Domain.Entities;
using Application.Interfaces;
using Application.DTOs.Transaction;
using Domain.ValueObjects;
using FluentValidation;

namespace Application.UseCase.Transactions.Commands.CreateTransaction;

public record CreateTransactionCommand : IRequest<CreateTransactionResponse>
{
    public long SourceAccountId {get; set;}
    public long? DestinationAccountId {get; set;}
    public decimal Amount {get; set;}
    public TransactionType Type {get; set;}
    public string? Description {get; set;}
}

public class CreateTransactionCommandHandler : IRequestHandler<CreateTransactionCommand, CreateTransactionResponse>
{
    private readonly ITransactionRepository _transactionRepository;

    public CreateTransactionCommandHandler(ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }
    public async Task<CreateTransactionResponse> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
    {
      var moneyResult = Money.Create(request.Amount);
      if(moneyResult.IsFailure) throw new ValidationException(moneyResult.Error);
    }
}