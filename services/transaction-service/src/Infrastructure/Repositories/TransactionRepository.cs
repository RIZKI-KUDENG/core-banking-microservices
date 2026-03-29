using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly AppDbContext _context;

    public TransactionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Transaction transaction)
    {
        await _context.Transactions.AddAsync(transaction);
    }

    public async Task<Transaction?> GetByIdAsync(long id)
    {
        return await _context.Transactions.Include(t => t.Entries).FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<Transaction?> GetReferenceNumberAsync(string referenceNumber)
    {
        if (Guid.TryParse(referenceNumber, out Guid guidRef))
        {
            return await _context.Transactions
                .FirstOrDefaultAsync(t => t.ReferenceNumber == guidRef);
        }
        
        return null;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}