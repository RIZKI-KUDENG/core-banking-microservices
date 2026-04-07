using Domain.Common;
using Domain.Events;
namespace Domain.Entities;

public class Account : AggregateRoot
{
    public long CustomerId { get; private set; }
    public string AccountNumber { get; private set; } = null!;
    public decimal Balance { get; private set; }
    public AccountType Type { get; private set; }
    public Status AccountStatus { get; private set; }
    public byte[] RowVersion { get; private set; } = null!;

    private Account() {}

    public static Result<Account> Create(long customerId, string accountNumber, AccountType type)
    {
        var account = new Account
        {
            CustomerId = customerId,
            AccountNumber = accountNumber,
            Type = type,
            AccountStatus = Status.Active,
            Balance = 0m,
            CreatedAt = DateTime.UtcNow
        };
        account.AddDomainEvent(new AccountCreatedEvent(
            customerId,
            accountNumber,
            account.Balance
        ));
        return Result<Account>.Success(account);
    }
    public Result Deposit(decimal amount)
    {
        if(amount <= 0)
        return Result.Failure("Deposit amount must be greater than zero");

        if(AccountStatus != Status.Active)
        return Result.Failure("Only active accounts can accept deposits.");

        Balance += amount;
        UpdatedAt = DateTime.UtcNow;
        return Result.Success();
    }
    public Result Withdraw(decimal amount)
    {
        if(amount <= 0)
        return Result.Failure("Withdrawal amount must be greater than zero");

        if(AccountStatus != Status.Active)
        return Result.Failure("Only active accounts can accept withdrawals.");

        if(Balance < amount)
        return Result.Failure("Insufficient funds.");

        Balance -= amount;
        UpdatedAt = DateTime.UtcNow;
        return Result.Success();
    }


}

public enum AccountType
{
    Savings,
    Checking
}

public enum Status
{
    Active,
    Inactive,
    Blocked
}
