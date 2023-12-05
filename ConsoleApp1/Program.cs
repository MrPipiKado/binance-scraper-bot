using BinanceBotNuGetPackage.BotExecutor;
using BinanceBotNuGetPackage.DB.DbContext;
using BinanceBotNuGetPackage.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MVCApp.Models;


var gg = @"Data Source=C:\Users\Viktor_Romashko\Desktop\diplomaproj\BinanceBotWebApplication\MVCApp\BinanceBotDb.db";

var optionsBuilder = new DbContextOptionsBuilder<BinanceBotDbContext>();
optionsBuilder.UseSqlite(gg);
using (var dbContext = new BinanceBotDbContext(optionsBuilder.Options))
{
    foreach (var VARIABLE in dbContext.DateIntervals)
    {
        Console.WriteLine(VARIABLE.Id + " " + VARIABLE.Period);

    }
}
var dataForCart = FinderHelper.ReturnDataByPeriod(
    DateTime.Parse("2022-10-10T07:07"),
    DateTime.Parse("2022-10-18T07:07"),
    "1D",
    "Bitcoin",
    gg
);
FinderHelper.RemoveDuplicates(gg);

var uiPathExecutor = new AutomationExecutor("MVCApp");
uiPathExecutor.ExecuteProcessWithNotParsedOutput(
    "BinanceBot.1.0.32.nupkg",
    "Bitcoin",
    "2023-03-01T07:07",
    "2023-05-02T07:07",
    "1W",
    FinderHelper.ReplaceSpacesAndBackslashes("Data Source=C:\\Users\\Viktor_Romashko\\Desktop\\diplomaproj\\BinanceBotWebApplication\\MVCApp\\BinanceBotDb.db")
);


var data = new List<string>
{
    "O1216.16 H1228.22 L1181.05 C1200.431 200.43−15.74 (−1.29%)Vol 531.537K",
    "O1316.16 H1228.22 L1181.05 C1200.431 200.43−15.74 (−1.29%)Vol 532.537K",
    "O1416.16 H1228.22 L1181.05 C1200.431 200.43−15.74 (−1.29%)Vol 533.537K",
    "O1516.16 H1228.22 L1181.05 C1200.431 200.43−15.74 (−1.29%)Vol 534.537K",
    "O1616.16 H1228.22 L1181.05 C1200.431 200.43−15.74 (−1.29%)Vol 535.537K",
};

FinderHelper.ParsePrices("O1216.16 H1228.22 L1181.05 C1200.431 200.43−15.74 (−1.29%)Vol 530.537K");

FinderHelper.InsertFullCryptoIntoDb(
    data,
    "Bitcoin",
    "BTC/USDT",
    "15m",
    DateTime.Parse("2023-03-01T07:07"),
    DateTime.Parse("2023-05-02T07:07"),
    gg
);
