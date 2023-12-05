using BinanceBotNuGetPackage.DB.DbContext;
using BinanceBotNuGetPackage.Db.Entities;
using Microsoft.EntityFrameworkCore;

namespace BinanceBotNuGetPackage.DB.Repositories;

public class PrimaryCryptoInfoRepository
{
    private readonly BinanceBotDbContext _context;

    public PrimaryCryptoInfoRepository(BinanceBotDbContext context)
    {
        _context = context;
    }

    public void Delete(PrimaryCryptoInfo entity) { 
        _context.Remove(entity);
        _context.SaveChanges();
    }
    
    public Task DeleteByIdAsync(int id) {
        return Task.Run(() =>
        {
            _context.PrimaryCryptoInfo.Remove(_context.PrimaryCryptoInfo.First(x => x.ID == id));
            _context.SaveChanges();
        });
    }
    
    public void Update(PrimaryCryptoInfo entity) {
        _context.Entry(entity).State = entity.ID == 0 ? EntityState.Added : EntityState.Modified;
        _context.SaveChanges();
    }

    public async Task AddAsync(PrimaryCryptoInfo entity) {
        await _context.PrimaryCryptoInfo.AddAsync(entity);
        await _context.SaveChangesAsync();
    }
    
    public IQueryable<PrimaryCryptoInfo> RetrieveAll() {
        return _context.PrimaryCryptoInfo.AsQueryable();
    }

    public async Task<PrimaryCryptoInfo> RetrieveByIdAsync(int id) {
        return await _context.PrimaryCryptoInfo.FindAsync(id);
    }
    public async Task<PrimaryCryptoInfo> RetrieveByNameAsync(string shortName) {
        return await _context.PrimaryCryptoInfo.Where(x=>x.ShortCryptoName == shortName).FirstOrDefaultAsync();
    }
    
}