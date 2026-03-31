using MediatR;
using Domain.Entities;
using Application.Interfaces;
using Application.DTOs.Transaction;

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
        var transaction = new Transaction
        {
            ReferenceNumber = Guid.NewGuid(),
            Type = request.Type,
            Status = TransactionStatus.Pending,
            Description = request.Description,
            CreatedAt = DateTime.UtcNow,
        };
        transaction.Entries.Add(new TransactionEntry
        {
            AccountId = request.SourceAccountId,
            Type = EntryType.Debit,
            Amount = request.Amount,
            CreatedAt = DateTime.UtcNow,
        });
        if (request.DestinationAccountId.HasValue)
        {
            transaction.Entries.Add(new TransactionEntry
            {
                AccountId = request.DestinationAccountId.Value,
                Type = EntryType.Credit,
                Amount = request.Amount,
                CreatedAt = DateTime.UtcNow,
            });
        }
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