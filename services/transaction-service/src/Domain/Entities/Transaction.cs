using Domain.Events;
using Domain.Common;
using Domain.ValueObjects;

namespace Domain.Entities;

public class Transaction : AggregateRoot
{
    public Guid ReferenceNumber {get; private set;}
    public TransactionType Type {get; private set;}
    public TransactionStatus Status {get; private set;}
    public string? Description {get; private set;}

    private readonly List<TransactionEntry> _entries = new();
    public IReadOnlyCollection<TransactionEntry> Entries => _entries.AsReadOnly();

    private Transaction() {}

    public static Result<Transaction> Create(TransactionType type, string? description)
    {
        var transaction = new Transaction
        {
            ReferenceNumber = Guid.NewGuid(),
            Type = type,
            Status = TransactionStatus.Pending,
            Description = description,
            CreatedAt = DateTime.UtcNow
        };
        return Result<Transaction>.Success(transaction);
    }
    public Result AddEntry(long accountId, EntryType type, Money amount)
    {
        if(amount.Value <= 0)
        return Result.Failure("Amount must be greater than zero.");

        var entry = new TransactionEntry(accountId, type, amount);
        _entries.Add(entry);

        return Result.Success();
    }

    public Result Process()
    {
        if(Status != TransactionStatus.Pending)
        return Result.Failure("Only pending transaction can be processed.");

        var totalDebit = _entries.Where(e => e.Type == EntryType.Debit).Sum(e => e.Amount.Value);
        var totalCredit = _entries.Where(e => e.Type == EntryType.Credit).Sum(e => e.Amount.Value);

        if(Type == TransactionType.Transfer && totalDebit != totalCredit)
        return Result.Failure("For transfer transactions, total debit must equal total credit.");

        Status = TransactionStatus.Completed;
        UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new TransactionCreatedEvent(
            ReferenceNumber,
            totalDebit,
            Type.ToString()
        ));
        return Result.Success();
    }
}

public enum TransactionType
{
    Deposit,
    Withdrawal,
    Transfer
}

public enum TransactionStatus
{
    Pending,
    Completed,
    Failed
}