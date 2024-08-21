using Microsoft.EntityFrameworkCore;
using BurgerMania.Models;

namespace BurgerMania.Data
{
    public class BurgerManiaContext: DbContext
    {
        public DbSet<Orders> Orders { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Burger> Burgers { get; set; }
        public DbSet<OrderItem> Items { get; set; }


        public BurgerManiaContext(DbContextOptions<BurgerManiaContext> Options) : base(Options)
        {
            
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    base.OnConfiguring(optionsBuilder);

        //    if(!optionsBuilder.IsConfigured)
        //    {
        //        optionsBuilder.UseInMemoryDatabase("BurgerManiaFullFledged");
        //    }
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<User>()
                .HasMany(o => o.Orders)
                .WithOne(u => u.User)
                .HasForeignKey(u => u.UserID)
                .IsRequired();


            modelBuilder.Entity<Orders>()
                .HasMany(i => i.OrderItems)
                .WithOne(o => o.Orders)
                .HasForeignKey(o => o.OrderID)
                .IsRequired();


            modelBuilder.Entity<Burger>()
                .HasMany(i => i.OrderItems)
                .WithOne(b => b.Burger)
                .HasForeignKey(b => b.BurgerID)
                .IsRequired();

        }
    }
}
