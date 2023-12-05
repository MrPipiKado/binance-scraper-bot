using BinanceBotNuGetPackage.DB.DbContext;
using BinanceBotNuGetPackage.DB.Repositories;

namespace BinanceBotNuGetPackage.DB;

public class MainRepository
{
    private readonly BinanceBotDbContext _dbContext;
    private DealsRepository _dealsRepository;
    private DateIntervalsRepository _dateIntervalsRepository;
    private PrimaryCryptoInfoRepository _primaryCryptoInfoRepository;
    
    public MainRepository(BinanceBotDbContext db) {
        _dbContext = db;
    }

    public DealsRepository DealsRepository {
        get {
            if (this._dealsRepository == null) {
                this._dealsRepository = new DealsRepository(_dbContext);
            }

            return _dealsRepository;
        }
    }
    public DateIntervalsRepository DateIntervalsRepository {
        get {
            if (_dateIntervalsRepository == null) {
                _dateIntervalsRepository = new DateIntervalsRepository(_dbContext);
            }

            return _dateIntervalsRepository;
        }
    }
    public PrimaryCryptoInfoRepository PrimaryCryptoInfoRepository {
        get {
            if (_primaryCryptoInfoRepository == null) {
                _primaryCryptoInfoRepository = new PrimaryCryptoInfoRepository(_dbContext);
            }

            return _primaryCryptoInfoRepository;
        }
            
    }

}