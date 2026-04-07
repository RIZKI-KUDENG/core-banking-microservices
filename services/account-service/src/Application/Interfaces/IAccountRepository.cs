using Domain.Entities;
namespace Application.Interfaces;

public interface IAccountRepository
{
    Task<Account?> GetByIdAsync(long id);
    Task<Account?> GetByAccountNumberAsync(string accountNumber);
    Task AddAsync(Account account);
    Task SaveChangesAsync();
}