using System;
using Microsoft.EntityFrameworkCore;
using pisang.Models;

namespace pisang.Data
{
    public class UserContext : DbContext
    {

        public UserContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseSerialColumns();
        }

        public DbSet<Pisangs> PisangsBase { get; set; }
        public DbSet<Cart> CartBase { get; set; }
        public DbSet<Orders> OrdersBase { get; set; }
        public DbSet<OrderItems> OrderItemsBase { get; set; }
        public DbSet<UserCred> UsersCredBase { get; set; }
        public DbSet<TblUsers> TblUsersBase { get; set; }
    }
}