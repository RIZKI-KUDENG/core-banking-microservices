using System.Formats.Tar;
using Domain.Common;
using Domain.ValueObjects;

namespace Domain.Entities;

public class TransactionEntry : BaseEntity
{
    public long TransactionId {get; private set;}
    public long AccountId {get; private set;}
    public EntryType Type {get; private set;}
    public Money Amount {get; private set;} = null!;

    private TransactionEntry() {}

    internal TransactionEntry( long accountId, EntryType type, Money amount)
    {
        AccountId = accountId;
        Type = type;
        Amount = amount;
        CreatedAt = DateTime.UtcNow;
    }
}
public enum EntryType
{
    Credit,
    Debit
}