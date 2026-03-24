namespace Domain.Entities;

using Domain.Common;


public class Transaction : BaseEntity
{
    public string ReferenceNumber { get; set; } = string.Empty;
    public TransactionType Type { get; set; }
    public TransactionStatus Status { get; set; }
    public string? Description { get; set; }

    public ICollection<TransactionEntry> Entries { get; set; } = new List<TransactionEntry>();
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
