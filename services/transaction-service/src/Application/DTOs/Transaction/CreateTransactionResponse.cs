using Domain.Entities;

namespace Application.DTOs.Transaction;

    public class CreateTransactionResponse
    {
        public Guid ReferenceNumber { get; set; }
        public TransactionStatus Status { get; set; }
        public TransactionType Type { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
