using Domain.Entities;
namespace Application.DTOs.Transaction;

public class CreateTransactionRequest
{
    public long SourceAccountId {get; set;}
    public long? DestinationAccountId {get; set;}
    public decimal Amount {get; set;}
    public TransactionType Type {get; set;}
    public string? Description {get; set;}
}