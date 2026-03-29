using Domain.Entities;

namespace Application.DTOs.Transaction;

public class GetTransactionIdResponse
{
    public decimal Amount { get; set; }
    public TransactionStatus Status { get; set; }
    public TransactionType Type { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
}