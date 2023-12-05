using BinanceBotNuGetPackage.Db.Entities;

namespace MVCApp.Models;

public class ChartModel
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public List<Deal> Deals { get; set; }
}