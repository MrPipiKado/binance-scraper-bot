using System.Data;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;
using BinanceBotNuGetPackage.DB.DbContext;
using BinanceBotNuGetPackage.Db.Entities;
using Microsoft.EntityFrameworkCore;

namespace BinanceBotNuGetPackage.Helpers;

public static class FinderHelper
{
    public static bool IsCryptoNameFound(string cryptoName, DataRow dataRow)
    {
        var str = "";
        str = dataRow.ItemArray[0]!.ToString();
        var splited_str = str.Split("\n");
        if (splited_str.Contains(cryptoName))
        {
            return true;
        }
        return false;
        
    }

    public static string? ReturnDate(string date)
    {
        var date_of = date.Split("T").FirstOrDefault();
        return date_of;
    }
    
    public static string? ReturnTime(string time)
    {
        var time_of = time.Split("T").LastOrDefault();
        return time_of;
    }

    public static List<int> GetListOfCoordinates(
        int width,
        int startPointX,
        DateTime startDate,
        DateTime endDate,
        string periodSelector)
    {
        List<int> cordinates = new List<int>();
        int periods = 0;
        var dateDifference = endDate - startDate;
        
        if(periodSelector == "15m"){
            periods = dateDifference.Hours * 4;
        }
        else if(periodSelector == "1H"){
            periods = dateDifference.Hours;
        }
        else if(periodSelector == "4H"){
            periods = dateDifference.Hours / 4;
        }
        else if(periodSelector == "1D"){
            periods = dateDifference.Days;
        }
        else if(periodSelector == "1W"){
            periods = dateDifference.Days / 7;
        }
        
        int step = width / periods;
        for (int i = 0; i < periods; i++)
        {
            cordinates.Add(startPointX);
            startPointX += step;
        }

        return cordinates;
    }

    public static List<double> ParsePrices(string data)
    {
        // O1216.16 H1228.22 L1181.05 C1200.431 200.43−15.74 (−1.29%)Vol 530.537K
        
        List<double> numbers = new List<double>();
        Regex regex = new Regex(@"[-+]?\d+(\.\d+)?");

        foreach (Match match in regex.Matches(data)) {
            double number;
            if (double.TryParse(match.Value, out number)) {
                numbers.Add(number);
            }
        }

        return numbers;
    }
    
    public static void CheckIfPeriodsExist(string connectionStr)
    {
        var optionsBuilder = new DbContextOptionsBuilder<BinanceBotDbContext>();
        optionsBuilder.UseSqlite(connectionStr);
        using (var dbContext = new BinanceBotDbContext(optionsBuilder.Options))
        {
            if (dbContext.DateIntervals.FirstOrDefault(
                    x =>
                        x.Period == "15m" ||
                        x.Period == "1H" ||
                        x.Period == "4H" ||
                        x.Period == "1D" ||
                        x.Period == "1W") == null)
            {
                dbContext.DateIntervals.Add(new DateInterval{Period = "15m"});
                dbContext.DateIntervals.Add(new DateInterval{Period = "1H"});
                dbContext.DateIntervals.Add(new DateInterval{Period = "4H"});
                dbContext.DateIntervals.Add(new DateInterval{Period = "1D"});
                dbContext.DateIntervals.Add(new DateInterval{Period = "1W"});
                dbContext.SaveChanges();
            }
        }
    }

    public static TimeSpan GetTimeBuffer(string dateInterval)
    {
        if(dateInterval == "15m"){  
            return new TimeSpan(0, 0, 15, 0, 0);
        }
        else if(dateInterval == "1H"){     
            return new TimeSpan(0, 1, 0, 0, 0);
        }
        else if(dateInterval == "4H"){     
            return new TimeSpan(0, 4, 0, 0, 0);
        }
        else if(dateInterval == "1D"){    
            return new TimeSpan(1, 0, 0, 0, 0);
        }
        else if(dateInterval == "1W"){    
            return new TimeSpan(7, 0, 0, 0, 0);
        }
        

        return new TimeSpan();
    }
    public static string ReplaceSpacesAndBackslashes(string input)
    {
        var res1 = input.Replace(" ", "*");
        var res2 = res1.Replace("\\", "SLASH");
        return res2;
    }
    public static string RestoreOriginalString(string input)
    {
        string result = input.Replace("*", " ").Replace("SLASH", "\\");
        return result;
    }
    public static void InsertFullCryptoIntoDb(
        IList<string> data,
        string cryptoName,
        string tradingPair,
        string period,
        DateTime start,
        DateTime end,
        string connectionStr)
    {
        var firstCr = tradingPair.Split(@"/")[0];
        var secondCr = tradingPair.Split(@"/")[1];
        var cryptoInserted = false;
        var cryptoList = new List<Deal>();
        var periodId = 0;
        var firstCrId = 0;
        var secondCrId = 0;
        var timeBuffer = GetTimeBuffer(period);
        var localStart = start;
        var optionsBuilder = new DbContextOptionsBuilder<BinanceBotDbContext>();
        optionsBuilder.UseSqlite(connectionStr);

        using (var dbContext = new BinanceBotDbContext(optionsBuilder.Options))
        {
            if (dbContext.PrimaryCryptoInfo
                    .FirstOrDefault(x => x.ShortCryptoName == firstCr) == null)
            {
                dbContext.PrimaryCryptoInfo.Add(
                    new PrimaryCryptoInfo { CryptoName = cryptoName, ShortCryptoName = firstCr });
            }

            if (dbContext.PrimaryCryptoInfo
                    .FirstOrDefault(x => x.ShortCryptoName == secondCr) == null)
            {
                dbContext.PrimaryCryptoInfo.Add(
                    new PrimaryCryptoInfo { ShortCryptoName = secondCr });
            }

            dbContext.SaveChanges();
        }
        using (var dbContext = new BinanceBotDbContext(optionsBuilder.Options)){

            periodId = dbContext.DateIntervals
                .FirstOrDefault(x=>x.Period == period)!.Id;
            firstCrId = dbContext.PrimaryCryptoInfo
                .FirstOrDefault(x=>x.ShortCryptoName == firstCr)!.ID;
            secondCrId = dbContext.PrimaryCryptoInfo
                .FirstOrDefault(x=>x.ShortCryptoName == secondCr)!.ID;
            
            dbContext.SaveChanges();
        }
        foreach (var priceData in data)
        {
            var parsedPrice = ParsePrices(priceData);
            var cryptoCur = new Deal()
            {
                FirstInTradingPair = firstCrId,
                SecondInTradingPair = secondCrId,
                CurrentPrice = parsedPrice[3],
                LowestPrice = parsedPrice[2],
                HighestPrice = parsedPrice[1],
                ChangePercentage = parsedPrice[6],
                DealsVolume = parsedPrice[7],
                Period = periodId,
                StartDate = localStart,
                EndDate = localStart + timeBuffer
            };
            localStart += timeBuffer;
            cryptoList.Add(cryptoCur);
        }
        using (var dbContext = new BinanceBotDbContext(optionsBuilder.Options))
        {
            foreach (var deal in cryptoList)
            {
                dbContext.Deals.Add(deal);
            }
            dbContext.SaveChanges();
        }
    }

    public static List<string> ReadFromFileAndParse(string filePath)
    {
        List<string> parsedData = new List<string>();
        string[] lines = File.ReadAllLines(filePath);

        return lines.ToList();
    }

    public static List<Deal> ReturnDataByPeriod(DateTime start, DateTime end, string period,string name, string connectionStr)
    {
        var resultList = new List<Deal>();
        var optionsBuilder = new DbContextOptionsBuilder<BinanceBotDbContext>();
        optionsBuilder.UseSqlite(connectionStr);
        using (var dbContext = new BinanceBotDbContext(optionsBuilder.Options))
        {
            var cryptoId = dbContext.PrimaryCryptoInfo
                .FirstOrDefault(x => x.CryptoName == name)!
                .ID;
            var periodId = dbContext.DateIntervals
                .FirstOrDefault(x=>x.Period == period)!.Id;
            resultList = dbContext.Deals.Where(
                    d => d.Period == periodId &&
                         d.FirstCrypto.ID == cryptoId &&
                         d.StartDate >= start &&
                         d.EndDate <= end
            ).ToList();
        }

        return resultList;
    }
    public static void RemoveDuplicates(string connectionStr)
    {
        var optionsBuilder = new DbContextOptionsBuilder<BinanceBotDbContext>();
        optionsBuilder.UseSqlite(connectionStr);
        using (var dbContext = new BinanceBotDbContext(optionsBuilder.Options))
        {
            var duplicates = dbContext.Deals
                .ToList()
                .GroupBy(x => new { x.StartDate, x.EndDate, x.Period })
                .Where(g => g.Count() > 1)
                .SelectMany(g => g.Skip(1))
                .ToList();
            
            dbContext.Deals.RemoveRange(duplicates);
            dbContext.SaveChanges();
        }
        
    }
    

}