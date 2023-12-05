using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BinanceBotNuGetPackage.Db.Entities;

public class Deal
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ID { get; set; }
    public int FirstInTradingPair { get; set; }
    public int SecondInTradingPair { get; set; }
    public double CurrentPrice { get; set; }
    public double HighestPrice { get; set; }
    public double LowestPrice { get; set; }
    
    public double DealsVolume { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int Period { get; set; }
    public double ChangePercentage { get; set; }

    [ForeignKey("Period")]
    public DateInterval DateInterval { get; set; }
    
    [ForeignKey("FirstInTradingPair")]
    public PrimaryCryptoInfo FirstCrypto { get; set; }
    
    [ForeignKey("SecondInTradingPair")]
    public PrimaryCryptoInfo SecondCrypto { get; set; }
}