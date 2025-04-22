using Microsoft.EntityFrameworkCore;
using ShiftYar.Domain.Entities.DepartmentModel;
using ShiftYar.Domain.Entities.HospitalModel;
using ShiftYar.Domain.Entities.RoleModel;
using ShiftYar.Domain.Entities.SecurityModel;
using ShiftYar.Domain.Entities.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShiftYar.Infrastructure.Persistence.AppDbContext
{
    public class ShiftYarDbContext: DbContext
    {
        public ShiftYarDbContext(DbContextOptions<ShiftYarDbContext> options) : base(options)
        {

        }

        public DbSet<Hospital> Hospitals { get; set; }
        public DbSet<HospitalPhoneNumber> HospitalPhoneNumbers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<LoginHistory> LoginHistories { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<Department> Departments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Hospital>()
                .HasMany(h => h.PhoneNumbers)
                .WithOne(p => p.Hospital)
                .HasForeignKey(p => p.HospitalId);

            modelBuilder.Entity<Hospital>()
                .HasMany(h => h.Departments)
                .WithOne(p => p.Hospital)
                .HasForeignKey(p => p.HospitalId);

            modelBuilder.Entity<User>()
                .HasMany(h => h.OtherPhoneNumbers)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId);

            modelBuilder.Entity<User>()
                .HasMany(h => h.UserRoles)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
