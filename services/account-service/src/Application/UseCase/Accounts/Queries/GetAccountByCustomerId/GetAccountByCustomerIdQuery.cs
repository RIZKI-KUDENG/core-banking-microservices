using MediatR;
using Application.Interfaces;
using Domain.Entities;
using Application.DTOs.Accounts;

namespace Application.UseCase.Accounts.Queries.GetAccountByCustomerId;

public record GetAccountByCustomerIdQuery(long CustomerId) : IRequest<GetAccountByCustomerIdResponse>;

public class GetAccountByCustomerIdQueryHandler : IRequestHandler<GetAccountByCustomerIdQuery, GetAccountByCustomerIdResponse>
{
    private readonly IAccountRepository _accountRepository;

    public GetAccountByCustomerIdQueryHandler(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<GetAccountByCustomerIdResponse> Handle(GetAccountByCustomerIdQuery requset, CancellationToken cancellationToken)
    {
        var account = await _accountRepository.GetByCustomerIdAsync(requset.CustomerId);
        if (account == null)
        {
            throw new KeyNotFoundException($"Account for Customer ID {requset.CustomerId} not found.");
        }
        return new GetAccountByCustomerIdResponse
        {
            Id = account.Id,
            AccountNumber = account.AccountNumber,
            Balance = account.Balance,
            Status = account.Status,
            CreatedAt = account.CreatedAt
        };
    }
}