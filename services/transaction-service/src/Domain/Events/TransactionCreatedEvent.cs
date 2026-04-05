using Domain.Common;
namespace Domain.Events;

public record TransactionCreatedEvent(
    Guid ReferenceNumber,
    decimal Amount,
    string TransactionType) : DomainEvent;


