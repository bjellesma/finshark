using System.ComponentModel.DataAnnotations.Schema;
namespace api.Models
{
    /// <summary>
    /// create table for stocks
    /// </summary>
    [Table("Stocks")]
    public class Stock
    {
        public int Id {get;set;}
        public string Symbol {get; set;} = string.Empty;
        public string CompanyName{get;set;} = string.Empty;
        // tells sql that it'll be 18 digits and 2 decimal places
        [Column(TypeName = "decimal(18,2)")]
        public decimal Purchase{get;set;}
        [Column(TypeName = "decimal(18,2)")]
        public decimal LastDiv{get;set;}
        public string Industry {get;set;} = string.Empty;
        public long MarketCap{get;set;}
        // stock can have many comments but a comment can only be linked to one stock
        public List<Comment>Comments{get;set;} = new List<Comment>();
        public List<Portfolio> Portfolios {get;set;} = new List<Portfolio>();
    }
}