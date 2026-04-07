using MediatR;
using Domain.Entities;
using Application.Interfaces;
using Application.DTOs.Accounts;
using FluentValidation;

namespace Application.UseCase.Accounts.Commands.CreateAccount;

public record CreateAccountCommand : IRequest<CreateAccountResponse>
{
    public long CustomerId { get; set; }
    public AccountType Type { get; set; }
    public decimal InitialDeposit { get; set; }
}

public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, CreateAccountResponse>
{
    private readonly IAccountRepository _accountRepository;

    public CreateAccountCommandHandler(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<CreateAccountResponse> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        var result = Account.Create(request.CustomerId, request.InitialDeposit, request.Type);
        if (result.IsFailure)
        {
            throw new ValidationException(result.Error);
        }
        var account = result.Value!;
        await _accountRepository.AddAsync(account);
        await _accountRepository.SaveChangesAsync();

        return new CreateAccountResponse
        {
            Id = account.Id,
            AccountNumber = account.AccountNumber,
            Balance = account.Balance,
            Status = account.Status,
            CreatedAt = account.CreatedAt
        };
    }
}