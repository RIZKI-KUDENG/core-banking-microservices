using Domain.Entities;

namespace Application.DTOs.Accounts;

public class CreateAccountRequest
{
    public long CustomerId {get; set;}
    public AccountType Type {get; set;}
    public decimal InitialDeposit {get; set;}
}