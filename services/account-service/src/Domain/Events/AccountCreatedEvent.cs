using Domain.Common;

namespace Domain.Events;

public record AccountCreatedEvent(
    Guid AccountId,
    string AccountNumber,
    decimal InitialBalance) : DomainEvent;