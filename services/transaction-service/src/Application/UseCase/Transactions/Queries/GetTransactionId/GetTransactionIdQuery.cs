using MediatR;
using Application.Interfaces;
using Application.DTOs.Transaction;
using Domain.Entities;
using Application.Common.Exceptions;

namespace Application.UseCase.Transactions.Queries.GetTransactionId;

public record GetTransactionIdQuery(long Id) : IRequest<GetTransactionIdResponse>;

public class GetTransactionIdQueryHandler : IRequestHandler<GetTransactionIdQuery, GetTransactionIdResponse>
{
    private readonly ITransactionRepository _transactionRepository;

    public GetTransactionIdQueryHandler(ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }

    public async Task<GetTransactionIdResponse> Handle(GetTransactionIdQuery request, CancellationToken cancellationToken)
    {
        var transaction = await _transactionRepository.GetByIdAsync(request.Id);
        if(transaction == null)
        {
            throw new NotFoundException($"Transaction id with {request.Id} not found");
        }
        return new GetTransactionIdResponse
        {
            Amount = transaction?.Entries.FirstOrDefault()?.Amount ?? 0,
            Status = transaction!.Status,
            Type = transaction.Type,
            Description = transaction.Description,
            CreatedAt = transaction.CreatedAt
        };
    }
}