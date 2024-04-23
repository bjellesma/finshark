using System.ComponentModel.DataAnnotations;
using api.Validation;

namespace api.Dtos.Stock
{
    public class UpdateStockRequestDto
    {
        public const int MinLength = 1;
        public const int MaxLength = 10;
        // we don't want the ID in this case because we expect the ID to be autoincremented
        

        [Required]
        [LengthValidation(MinLength, MaxLength)]
        public string Symbol {get; set;} = string.Empty;
        [Required]
        [LengthValidation(MinLength, MaxLength)]
        public string CompanyName{get;set;} = string.Empty;
        [Required]
        [Range(1, 100000000000)]
        public decimal Purchase{get;set;}
        [Required]
        [Range(0.001, 100)]
        public decimal LastDiv{get;set;}
        [Required]
        [LengthValidation(MinLength, MaxLength)]
        public string Industry {get;set;} = string.Empty;
        [Required]
        [Range(1, 5000000000)]
        public long MarketCap{get;set;}
    }
}