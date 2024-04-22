using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Data
{
    public class ApplicationDBContext : DbContext 
    {
        //  this syntax is called constructor chaining and base is a keyword that refers to the constructor of the base class
        // in this case, we're passing dbContextOptions to the constructor for the DbContext class
        public ApplicationDBContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
            
        }
        // DBset is a generi type provided by Entity designed to represent a collection of entitiies to query or save
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Comment> Comments{ get; set;}
    }
}