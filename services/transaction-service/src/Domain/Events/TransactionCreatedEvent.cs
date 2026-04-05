using Domain.Common;
namespace Domain.Events;

public record TransactionCreatedEvent(
    Guid TransactionId,
    Guid ReferenceNumber,
    decimal Amount,
    string TransactionType) : DomainEvent;


