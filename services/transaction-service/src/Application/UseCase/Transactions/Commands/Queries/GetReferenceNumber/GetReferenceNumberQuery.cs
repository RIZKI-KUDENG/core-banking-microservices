using MediatR;
using Application.DTOs.Transaction;
using Application.Interfaces;


namespace Application.UseCase.Transactions.Commands.Queries.GetReferenceNumber;

public record GetReferenceNumberQuery(string ReferenceNumber) : IRequest<GetReferenceNumberResponse>;

public class GetReferenceNumberQueryHandler : IRequestHandler<GetReferenceNumberQuery, GetReferenceNumberResponse>
{
    private readonly ITransactionRepository _transactionRepository;

    public GetReferenceNumberQueryHandler(ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }

    public async Task<GetReferenceNumberResponse> Handle(GetReferenceNumberQuery request, CancellationToken cancellationToken)
    {
        var transaction = await _transactionRepository.GetReferenceNumberAsync(request.ReferenceNumber);

        if(transaction == null) 
        {
            throw new Exception("Transaction not found");
        }
        return new GetReferenceNumberResponse
        {
            ReferenceNumber = transaction?.ReferenceNumber ?? Guid.Empty,
            Amount = transaction?.Entries.FirstOrDefault()?.Amount ?? 0,
            Status = transaction?.Status ?? Domain.Entities.TransactionStatus.Failed, 
            Type = transaction?.Type ?? Domain.Entities.TransactionType.Deposit,
            Description = transaction?.Description,
            CreatedAt = transaction?.CreatedAt ?? DateTime.MinValue
        };
    }
}