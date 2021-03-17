using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Core.Entities;
using System;

namespace SchoolManagement.Core.Database
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, string, IdentityUserClaim<string>, UserRole, IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public virtual DbSet<Course> Courses { get; set; } = null!;
        public virtual DbSet<Department> Departments { get; set; } = null!;
        public virtual DbSet<Student> Students { get; set; } = null!;
        public virtual DbSet<Teacher> Teachers { get; set; } = null!;


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Student>().ToTable("Students");
            builder.Entity<Teacher>().ToTable("Teachers");

            builder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.IdCard).IsUnique();
                entity.HasOne(d => d.Department)
                    .WithMany(u => u!.Users)
                    .HasForeignKey(d => d!.DepartmentId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<UserRole>(entity =>
            {
                entity.HasOne(ur => ur.Role).WithMany(r => r!.UserRoles).HasForeignKey(ur => ur.RoleId).IsRequired().OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(ur => ur.User).WithMany(u => u!.UserRoles).HasForeignKey(ur => ur.UserId).IsRequired().OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<StudentCourse>(entity =>
            {
                entity.HasKey(sc => new { sc.CourseId });
                entity.HasOne(sc => sc.Course).WithMany(s => s!.StudentCourses).HasForeignKey(sc => sc.CourseId).OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(sc => sc.Student).WithMany(c => c!.EnrolledCourses).HasForeignKey(sc => sc.StudentId).OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Course>(entity =>
            {
                entity.HasOne(d => d.Department)
                    .WithMany(c => c!.Courses)
                    .HasForeignKey(d => d!.DepartmentId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(t => t.Teacher)
                    .WithMany(c => c!.Courses)
                    .HasForeignKey(t => t!.TeacherId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.Day)
                    .HasConversion(
                        v => v.ToString(),
                        v => (DayOfWeek)Enum.Parse(typeof(DayOfWeek), v));
            });
        }
    }
}
