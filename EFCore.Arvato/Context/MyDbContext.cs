using EFCore.Arvato.Core.Auth;
using EFCore.Arvato.Core.Orders;
using EFCore.Arvato.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EFCore.Arvato.Context
{
    public class MyDbContext : IdentityDbContext<ViewUser,IdentityRole<Guid>,Guid> 
    {

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderComment> OrderComments { get; set; }
        public DbSet <User> AppUsers { get; set; }
        public MyDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions) { 
        
         

        }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{

          
        //    base.OnModelCreating(modelBuilder);

            
        //}


    }
}
