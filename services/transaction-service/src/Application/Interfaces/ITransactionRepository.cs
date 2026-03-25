using Domain.Entities;

namespace Application.Interfaces;

public interface ITransactionRepository
{
    Task AddAsync(Transaction transaction);
    Task<Transaction?> GetByIdAsync(long id);
    Task<Transaction?> GetReferenceNumberAsync(string referenceNumber);
    Task SaveChangesAsync();

}