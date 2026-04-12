using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly AppDbContext _context;

    public AccountRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Account?>GetByIdAsync(long id)
    {
        return await _context.Accounts.FindAsync(id);
    }
    public async Task<Account?> GetByAccountNumberAsync(string accountNumber)
    {
        return await _context.Accounts.FirstOrDefaultAsync(a => a.AccountNumber == accountNumber);
    }
    public async Task AddAsync(Account account)
    {
        await _context.Accounts.AddAsync(account);
    }
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}