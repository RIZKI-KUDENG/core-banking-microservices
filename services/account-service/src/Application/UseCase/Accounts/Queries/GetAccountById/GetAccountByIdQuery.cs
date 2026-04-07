using MediatR;
using Domain.Entities;
using Application.Interfaces;
using Application.Common.Exceptions;

namespace Application.UseCase.Accounts.Queries.GetAccountById;

public record GetAccountByIdQuery(long Id) : IRequest<GetAccountByIdResponse>;

public class GetAccountByIdQueryHandler : IRequestHandler<GetAccountByIdQuery, GetAccountByIdResponse>
{
    private readonly IAccountRepository _accountRepository;

    public GetAccountByIdQueryHandler(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<GetAccountByIdResponse> Handle(GetAccountByIdQuery request, CancellationToken cancellationToken)
    {
        var account = await _accountRepository.GetByIdAsync(request.Id);
        if (account == null)
        {
            throw new NotFoundException($"Account with ID {request.Id} not found.");
        }
        return new GetAccountByIdResponse
        {
            Id = account.Id,
            AccountNumber = account.AccountNumber,
            Balance = account.Balance,
            Status = account.Status,
            CreatedAt = account.CreatedAt
        };
    }

}