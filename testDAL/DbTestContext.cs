using DTOs.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
        public DbSet<Permission> Permissions { get; set; }

        public DbTestContext() : base() { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(constr);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new KeyEfConfig());
            modelBuilder.ApplyConfiguration(new BookingActonEfConfig());
            modelBuilder.ApplyConfiguration(new UserEfConfig());
        }
        //private const string constr = "Data Source = 84.38.189.95,31892; Database = KrokUser; Persist Security Info = false; User ID = 'sa'; Password = 'Ghbdtn010102'; MultipleActiveResultSets = True; Trusted_Connection = False;";
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
    public class UserEfConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasMany(x => x.Permissions).WithMany(x=>x.UsersWithPermissions);
        }
    }
    public class KeyEfConfig : IEntityTypeConfiguration<KeyObject>
    {
        public void Configure(EntityTypeBuilder<KeyObject> builder)
        {
            builder.HasKey(x => x.Id);
            
        }
    }
}
