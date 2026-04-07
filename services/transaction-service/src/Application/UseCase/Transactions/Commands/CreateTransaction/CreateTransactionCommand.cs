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
        if (moneyResult.IsFailure)
        {

            throw new ValidationException(moneyResult.Error); 
        }


        var transactionResult = Transaction.Create(request.Type, request.Description);
        if (transactionResult.IsFailure)
            throw new InvalidOperationException(transactionResult.Error);

        var transaction = transactionResult.Value!;


        var debitResult = transaction.AddEntry(request.SourceAccountId, EntryType.Debit, moneyResult.Value!);
        if (debitResult.IsFailure) throw new InvalidOperationException(debitResult.Error);

        if (request.DestinationAccountId.HasValue)
        {
            var creditResult = transaction.AddEntry(request.DestinationAccountId.Value, EntryType.Credit, moneyResult.Value!);
            if (creditResult.IsFailure) throw new InvalidOperationException(creditResult.Error);
        }


        var processResult = transaction.Process();
        if (processResult.IsFailure) throw new InvalidOperationException(processResult.Error);


        await _transactionRepository.AddAsync(transaction);
        await _transactionRepository.SaveChangesAsync();


        return new CreateTransactionResponse
        {
            ReferenceNumber = transaction.ReferenceNumber,
            Status = transaction.Status,
            Type = transaction.Type,
            Amount = request.Amount, 
            CreatedAt = transaction.CreatedAt
        };
    }
}