using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SchoolManagement.Core.Entities;
using System;

namespace SchoolManagement.Core.Database
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, string, IdentityUserClaim<string>, UserRole, IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public virtual DbSet<Course> Courses { get; set; } = null!;
        public virtual DbSet<Department> Departments { get; set; } = null!;


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var converter = new BoolToStringConverter("Female", "Male");

            builder.Entity<Student>().ToTable("Students");
            builder.Entity<Teacher>().ToTable("Teachers");

            builder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.IdCard).IsUnique();

                entity.HasOne(d => d.Department)
                    .WithMany(u => u!.Users)
                    .HasForeignKey(d => d!.DepartmentId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.Gender)
                    .HasConversion(converter);
            });

            builder.Entity<UserRole>(entity =>
            {
                entity.HasOne(ur => ur.Role).WithMany(r => r!.UserRoles).HasForeignKey(ur => ur.RoleId).IsRequired().OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(ur => ur.User).WithMany(u => u!.UserRoles).HasForeignKey(ur => ur.UserId).IsRequired().OnDelete(DeleteBehavior.Cascade);
            });

            //builder.Entity<StudentCourse>(entity =>
            //{
            //    entity.HasKey(sc => new { sc.CourseId });
            //    entity.HasOne(sc => sc.Course).WithMany(s => s!.StudentCourses).HasForeignKey(sc => sc.CourseId).OnDelete(DeleteBehavior.Cascade);
            //    entity.HasOne(sc => sc.Student).WithMany(c => c!.EnrolledCourses).HasForeignKey(sc => sc.StudentId).OnDelete(DeleteBehavior.Cascade);
            //});


            builder.Entity<Class>(entity =>
            {
                entity.HasIndex(e => e.ClassCode).IsUnique();

                entity.HasOne(d => d.Course)
                    .WithMany(c => c!.Classes)
                    .HasForeignKey(d => d!.CourseId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(t => t.Teacher)
                    .WithMany(c => c!.Classes)
                    .HasForeignKey(t => t!.TeacherId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.Property(e => e.Day)
                    .HasConversion(
                        v => v.ToString(),
                        v => (DayOfWeek)Enum.Parse(typeof(DayOfWeek), v));
            });

            builder.Entity<Course>(entity =>
            {
                entity.HasIndex(e => e.CourseCode).IsUnique();

                entity.HasOne(c => c.Department)
                    .WithMany(d => d.Courses)
                    .HasForeignKey(c => c.DepartmentId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Department>(entity =>
            {
                entity.HasIndex(e => e.ShortName).IsUnique();
            });
        }
    }
}
