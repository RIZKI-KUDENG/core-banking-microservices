using Domain.Common;

namespace Domain.Events;

public record AccountCreatedEvent(
    long CustomerId,
    string AccountNumber,
    decimal InitialBalance) : DomainEvent;