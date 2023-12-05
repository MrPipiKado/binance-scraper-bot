using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
namespace BinanceBotNuGetPackage.Db.Entities;

public class PrimaryCryptoInfo
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ID { get; set; }
    public string? CryptoName { get; set; }
    public string ShortCryptoName { get; set; }
    
}