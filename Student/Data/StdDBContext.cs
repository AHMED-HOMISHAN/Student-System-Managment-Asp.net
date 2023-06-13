using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Student.Models
{
    public partial class stdDBContext : DbContext
    {
        public stdDBContext()
        {
        }

        public stdDBContext(DbContextOptions<stdDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Departments> departments { get; set; } = null!;
        public virtual DbSet<Libraries> libraries { get; set; } = null!;
        public virtual DbSet<Students> students { get; set; } = null!;
        public virtual DbSet<Subjects> subjects { get; set; } = null!;
        public virtual DbSet<Teachers> teachers { get; set; } = null!;
        public virtual DbSet<Invoices> invoices { get; set; } = null!;
        public virtual DbSet<Courses> courses { get; set; } = null!;
        public virtual DbSet<Enrollement> Enrollement { get; set; } = null!;
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=AHMED;Database=SchoolManagement;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Departments>(entity =>
            {
                entity.ToTable("departments");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Invoices>(entity =>
            {
                entity.ToTable("invoices");

                entity.Property(e => e.Category)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Description)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.DueDate).HasColumnType("datetime");

                entity.Property(e => e.FromAddress)
                    .HasMaxLength(200)
                    .IsUnicode(false);

               
                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.StudentFk).HasColumnName("Student_FK");

                entity.Property(e => e.ToAddress)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.StudentFkNavigation)
                    .WithMany(p => p.Invoices)
                    .HasForeignKey(d => d.StudentFk)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_StudentID_Std");
            });





            modelBuilder.Entity<Courses>(entity =>
            {
                entity.ToTable("courses");

                entity.Property(e => e.Id).HasColumnName("Id");

                entity.Property(e => e.Name)
                 .HasMaxLength(255)
                 .IsUnicode(false)
                 .HasColumnName("name");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            });






            modelBuilder.Entity<Enrollement>().HasKey(sc=>new {sc.studentIdFk,sc.courseIdFk});
            modelBuilder.Entity<Enrollement>().HasOne<Students>(d => d.StudentFKNavigation).WithMany(sc=>sc.StudentCourse)
                    .HasForeignKey(d => d.studentIdFk)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_StudentID_course");

           modelBuilder.Entity<Enrollement>().HasKey(sc=>new {sc.studentIdFk,sc.courseIdFk});
            modelBuilder.Entity<Enrollement>().HasOne<Courses>(d => d.CourseFKNavigation).WithMany(sc=>sc.StudentsCourses)
                    .HasForeignKey(d => d.courseIdFk)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_CourseID_std");



            modelBuilder.Entity<Enrollement>( entity => {entity.Property(e => e.courseScore).HasColumnName("Marks"); });

            modelBuilder.Entity<Libraries>(entity =>
            {
                entity.ToTable("libraries");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DepartmentIdFk).HasColumnName("DepartmentID_FK");

                entity.Property(e => e.Language)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.Status)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Type)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.DepartmentIdFkNavigation)
                    .WithMany(p => p.Libraries)
                    .HasForeignKey(d => d.DepartmentIdFk)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_libraries_Lib");
            });

            modelBuilder.Entity<Students>(entity =>
            {
                entity.ToTable("students");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Age).HasColumnName("age");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DepartmentIdFk).HasColumnName("DepartmentID_FK");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.gender).HasColumnName("gender");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("name");


                entity.Property(e => e.Password)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("password");

                entity.Property(e => e.phoneNo).HasColumnName("phoneNo");

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.DepartmentIdFkNavigation)
                    .WithMany(p => p.Students)
                    .HasForeignKey(d => d.DepartmentIdFk)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_DepartmentID_Std");
            });

            modelBuilder.Entity<Subjects>(entity =>
            {
                entity.ToTable("subjects");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.TeacherIdFk).HasColumnName("TeacherID_FK");

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.TeacherIdFkNavigation)
                    .WithMany(p => p.Subjects)
                    .HasForeignKey(d => d.TeacherIdFk)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_TeacherID_subjects");
            });

            modelBuilder.Entity<Teachers>(entity =>
            {
                entity.ToTable("teachers");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Age).HasColumnName("age");

                entity.Property(e => e.Balance).HasColumnName("balance");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DepartmentIdFk).HasColumnName("DepartmentID_FK");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Experience)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.gender).HasColumnName("gender");

                entity.Property(e => e.JoinedDateTime).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.Password)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("password");

                entity.Property(e => e.phoneNo).HasColumnName("phoneNo");

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.DepartmentIdFkNavigation)
                    .WithMany(p => p.Teachers)
                    .HasForeignKey(d => d.DepartmentIdFk)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_DepartmentID_Teacher");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    }
}
