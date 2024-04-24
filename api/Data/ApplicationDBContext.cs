using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace api.Data
{
    public class ApplicationDBContext : IdentityDbContext<AppUser>
    {
        //  this syntax is called constructor chaining and base is a keyword that refers to the constructor of the base class
        // in this case, we're passing dbContextOptions to the constructor for the DbContext class
        public ApplicationDBContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
            
        }
        // DBset is a generi type provided by Entity designed to represent a collection of entitiies to query or save
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Comment> Comments{ get; set;}
        public DbSet<Portfolio> Portfolios{ get; set;}
        // protected access modifiers are only accessible by instances of this class
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Portfolio>(x => x.HasKey(p => new {p.AppUserId, p.StockId}));

            // set the relationships on the joining table
            builder.Entity<Portfolio>().HasOne(user => user.AppUser).WithMany(user => user.Portfolios).HasForeignKey(user => user.AppUserId);
            builder.Entity<Portfolio>().HasOne(user => user.Stock).WithMany(stock => stock.Portfolios).HasForeignKey(user => user.StockId);

            List<IdentityRole> roles = new List<IdentityRole>
            {
                // admins will just have slightly evalvated priveledges
                new IdentityRole
                {
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole
                {
                    Name = "User",
                    NormalizedName = "USER"
                },
            };
            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}