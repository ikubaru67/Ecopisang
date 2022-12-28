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

        public DbSet<PisangItemDTO> pisangItemDTOs { get; set; }
        public DbSet<PembeliItemDTO> pembeliItemDTOs { get; set; }
        public DbSet<PenjualItemDTO> penjualItemDTOs { get; set; }
    }
}