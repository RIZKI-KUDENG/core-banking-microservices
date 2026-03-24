namespace Domain.Entities;

using Domain.Common;

public class TransactionEntry : BaseEntity
{
    public long TransactionId { get; set; }
    public long AccountId { get; set; }
    public EntryType Type { get; set; }
    public decimal Amount { get; set; }
}

public enum EntryType
{
    Debit,
    Credit
}