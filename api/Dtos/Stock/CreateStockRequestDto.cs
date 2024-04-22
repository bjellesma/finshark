using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Stock
{
    public class CreateStockRequestDto
    {
        // we don't want the ID in this case because we expect the ID to be autoincremented
        public string Symbol {get; set;} = string.Empty;
        public string CompanyName{get;set;} = string.Empty;
        public decimal Purchase{get;set;}
        public decimal LastDiv{get;set;}
        public string Industry {get;set;} = string.Empty;
        public long MarketCap{get;set;}
    }
}