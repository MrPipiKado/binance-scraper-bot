using BinanceBotNuGetPackage.DB.DbContext;
using BinanceBotNuGetPackage.Db.Entities;

using Microsoft.EntityFrameworkCore;

namespace BinanceBotNuGetPackage.DB.Repositories;

public class DealsRepository
{
    private readonly BinanceBotDbContext _context;

    public DealsRepository(BinanceBotDbContext context)
    {
        _context = context;
    }

    public void Delete(Deal entity) { 
        _context.Remove(entity);
        _context.SaveChanges();
    }
    
    public Task DeleteByIdAsync(int id) {
        return Task.Run(() =>
        {
            _context.Deals.Remove(_context.Deals.First(x => x.ID == id));
            _context.SaveChanges();
        });
    }
    
    public void Update(Deal entity) {
        _context.Entry(entity).State = entity.ID == 0 ? EntityState.Added : EntityState.Modified;
        _context.SaveChanges();
    }

    public async Task AddAsync(Deal entity) {
        await _context.Deals.AddAsync(entity);
        await _context.SaveChangesAsync();
    }
    
    public IQueryable<Deal> RetrieveAll() {
        return _context.Deals.AsQueryable();
    }

    public async Task<Deal> RetrieveByIdAsync(int id) {
        return await _context.Deals.FindAsync(id);
    }
    
}