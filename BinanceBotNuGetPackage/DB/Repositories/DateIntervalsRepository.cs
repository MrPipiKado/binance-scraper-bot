using BinanceBotNuGetPackage.DB.DbContext;
using BinanceBotNuGetPackage.Db.Entities;

using Microsoft.EntityFrameworkCore;

namespace BinanceBotNuGetPackage.DB.Repositories;

public class DateIntervalsRepository
{
    private readonly BinanceBotDbContext _context;

    public DateIntervalsRepository(BinanceBotDbContext context)
    {
        _context = context;
    }

    public void Delete(DateInterval entity) { 
        _context.Remove(entity);
        _context.SaveChanges();
    }
    
    public Task DeleteByIdAsync(int id) {
        return Task.Run(() =>
        {
            _context.DateIntervals.Remove(_context.DateIntervals.First(x => x.Id == id));
            _context.SaveChanges();
        });
    }
    
    public void Update(DateInterval entity) {
        _context.Entry(entity).State = entity.Id == 0 ? EntityState.Added : EntityState.Modified;
        _context.SaveChanges();
    }

    public async Task AddAsync(DateInterval entity) {
        await _context.DateIntervals.AddAsync(entity);
        await _context.SaveChangesAsync();
    }
    
    public IQueryable<DateInterval> RetrieveAll() {
        return _context.DateIntervals.AsQueryable();
    }

    public async Task<DateInterval> RetrieveByIdAsync(int id) {
        return await _context.DateIntervals.FindAsync(id);
    }
    
    public async Task<DateInterval> RetrieveByNameAsync(string periodName)
    {
        return await _context.DateIntervals.Where(x => x.Period == periodName).FirstOrDefaultAsync();
    }
    
}