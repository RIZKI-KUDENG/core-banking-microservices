using Domain.Common;
namespace Domain.Entities;

public class Account : AggregateRoot
{
    public long Id { get; private set; }
    public long CustomerId { get; private set; }
    public string AccountNumber { get; private set; }

    public decimal Balance { get; private set; }
    public AccountType Type { get; private set; }
    public Status AccountStatus { get; private set; }
    public byte[] RowVersion { get; private set; }

    private Account() {}
}
