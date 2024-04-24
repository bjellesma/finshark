using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    [Table("Comments")]
    public class Comment
    {
        public int Id {get;set;}
        public string Title {get;set;} = string.Empty;
        public string Content {get;set;} = string.Empty;
        public DateTime CreatedOn {get;set;} = DateTime.Now;
        public int? StockId {get;set;}
        // stock will be a navigation property that we'll use to access the stock objects
        public Stock? Stock {get;set;}
    }
}