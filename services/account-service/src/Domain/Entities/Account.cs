using Domain.Common;
using Domain.Events;
namespace Domain.Entities;

public class Account : AggregateRoot
{
    public long CustomerId { get; private set; }
    public string AccountNumber { get; private set; } = null!;
    public decimal Balance { get; private set; }
    public AccountType Type { get; private set; }
    public AccountStatus Status { get; private set; }
    public byte[] RowVersion { get; private set; } = null!;

    private Account() {}

    public static Result<Account> Create(long customerId, decimal Balance, AccountType type)
    {
        var account = new Account
        {
            CustomerId = customerId,
            AccountNumber = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 12).ToUpper(),
            Type = type,
            Status = AccountStatus.Active,
            Balance = Balance,
            CreatedAt = DateTime.UtcNow
        };
        account.AddDomainEvent(new AccountCreatedEvent(
            customerId,
            account.AccountNumber,
            account.Balance
        ));
        return Result<Account>.Success(account);
    }
    public Result Deposit(decimal amount)
    {
        if(amount <= 0)
        return Result.Failure("Deposit amount must be greater than zero");

        if(Status != AccountStatus.Active)
        return Result.Failure("Only active accounts can accept deposits.");

        Balance += amount;
        UpdatedAt = DateTime.UtcNow;
        return Result.Success();
    }
    public Result Withdraw(decimal amount)
    {
        if(amount <= 0)
        return Result.Failure("Withdrawal amount must be greater than zero");

        if(Status != AccountStatus.Active)
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

public enum AccountStatus
{
    Active,
    Inactive,
    Blocked
}
