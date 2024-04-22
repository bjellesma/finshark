using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class Stocks
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
    }
}