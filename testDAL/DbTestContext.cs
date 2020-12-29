using DTOs.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testDAL
{
    public class DbTestContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<KeyObject> KeyObjects { get; set; }
        public DbSet<BookingAction> BookingActions { get; set; }

        public DbTestContext() : base() { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(constr);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().HasKey(x => x.Id);
            modelBuilder.Entity<KeyObject>().HasKey(x => x.Id);
            modelBuilder.ApplyConfiguration(new BookingActonEfConfig());
        }
        private const string constr = "Data Source = (localdb)\\MSSQLLocalDB; Database = KrokUser; Persist Security Info = false; User ID = 'sa'; Password = 'Ghbdtn010102'; MultipleActiveResultSets = True; Trusted_Connection = False;";
    }

    public class BookingActonEfConfig : IEntityTypeConfiguration<BookingAction>
    {
        public void Configure(EntityTypeBuilder<BookingAction> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.User);
            builder.HasOne(x => x.KeyObject);
        }
    }
}
