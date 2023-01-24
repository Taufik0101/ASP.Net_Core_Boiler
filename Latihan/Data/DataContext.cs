using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Latihan.Models;

namespace Latihan.Data
{
    public class DataContext : DbContext
    {
        public DataContext (DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().Property(u => u.Role).HasConversion<string>().HasMaxLength(50);
        }
        
        public DbSet<Latihan.Models.User> User { get; set; } = default!;
    }
}
