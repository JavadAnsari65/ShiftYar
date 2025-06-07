using Microsoft.EntityFrameworkCore;
using ShiftYar.Domain.Entities.AddressModel;
using ShiftYar.Domain.Entities.DepartmentModel;
using ShiftYar.Domain.Entities.HospitalModel;
using ShiftYar.Domain.Entities.PermissionModel;
using ShiftYar.Domain.Entities.RoleModel;
using ShiftYar.Domain.Entities.RolePermissionModel;
using ShiftYar.Domain.Entities.SecurityModel;
using ShiftYar.Domain.Entities.ShiftDateModel;
using ShiftYar.Domain.Entities.ShiftModel;
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

        public DbSet<Province> Provinces { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Hospital> Hospitals { get; set; }
        public DbSet<HospitalPhoneNumber> HospitalPhoneNumbers { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<LoginHistory> LoginHistories { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }

        //جدول تخصص های لازم در هر شیفت
        public DbSet<Specialty> Specialties { get; set; }
        public DbSet<Shift> Shifts { get; set; }
        public DbSet<ShiftRequiredSpecialty> ShiftRequiredSpecialties { get; set; }
        public DbSet<ShiftAssignment> ShiftAssignments { get; set; }

        public DbSet<ShiftDate> ShiftDates { get; set; }

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

            modelBuilder.Entity<Department>()
                .HasMany(d => d.DepartmentUsers)
                .WithOne(u => u.Department)
                .HasForeignKey(u => u.DepartmentId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
