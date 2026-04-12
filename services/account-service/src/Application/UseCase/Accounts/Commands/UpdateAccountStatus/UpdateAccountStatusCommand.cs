using MediatR;
using Application.Interfaces;
using Domain.Entities;
using Application.DTOs.Accounts;
using System.Data.Common;

namespace Application.UseCase.Accounts.Commands.UpdateAccountStatus;

public record UpdateAccountStatusCommand(long Id, AccountStatus NewStatus) : IRequest<UpdateAccountStatusResponse>;

public class UpdateAccountStatusCommandHandler : IRequestHandler<UpdateAccountStatusCommand, UpdateAccountStatusResponse>
{
    private readonly IAccountRepository _accountRepository;

    public UpdateAccountStatusCommandHandler(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<UpdateAccountStatusResponse> Handle(UpdateAccountStatusCommand request, CancellationToken cancellationToken)
    {
        var account = await _accountRepository.GetByIdAsync(request.Id);
        if (account == null)
        {
            throw new KeyNotFoundException($"Account with ID {request.Id} not found.");
        }
        account.UpdateStatus(request.NewStatus);
        await _accountRepository.SaveChangesAsync();
        return new UpdateAccountStatusResponse
        {
            Id = account.Id,
            AccountNumber = account.AccountNumber,
            Status = account.Status,
            UpdatedAt = account.UpdatedAt
        };
    }
}