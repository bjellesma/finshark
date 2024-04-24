using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    /// <summary>
    /// This model is meant to be a join table with a many to many relationship between users and stocks
    /// </summary>
    [Table("Portfolios")]
    public class Portfolio
    {
        public string AppUserId {get;set;}
        public int StockId {get;set;}
        // appUser and Stock are just navigation props to help the developer recognize keys
        public AppUser AppUser {get;set;}
        public Stock Stock {get;set;}
    }
}