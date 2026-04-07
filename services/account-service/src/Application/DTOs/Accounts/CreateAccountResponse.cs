using Domain.Entities;

public class CreateAccountResponse
{
    public long Id {get; set;}
    public string AccountNumber {get; set;} = null!;
    public decimal Balance {get; set;}
    public AccountStatus Status {get; set;}
    public DateTime CreatedAt {get; set;}
}