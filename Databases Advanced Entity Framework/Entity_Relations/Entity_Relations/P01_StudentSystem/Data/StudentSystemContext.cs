using Microsoft.EntityFrameworkCore;
using P01_StudentSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace P01_StudentSystem.Data
{
    public class StudentSystemContext : DbContext
    {
        public StudentSystemContext()
        {
        }

        public StudentSystemContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Config.connectionString);
            }           
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<Homework> HomeworkSubmissions { get; set; }
        public DbSet<StudentCourse> StudentCourses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            CreateStudentModel(modelBuilder);
            CreateCourseModel(modelBuilder);
            CreateResourceModel(modelBuilder);
            CreateHomeworkModel(modelBuilder);
            CreateStudentCourseModel(modelBuilder);
        }

        private void CreateStudentCourseModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<StudentCourse>()
                .HasKey(x => new { x.CourseId, x.StudentId });

            modelBuilder
                .Entity<StudentCourse>()
                .HasOne(x => x.Student)
                .WithMany(x => x.CourseEnrollments)
                .HasForeignKey(x => x.StudentId);

            modelBuilder
                .Entity<StudentCourse>()
                .HasOne(x => x.Course)
                .WithMany(x => x.StudentsEnrolled)
                .HasForeignKey(x => x.CourseId);
        }

        private void CreateHomeworkModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Homework>()
                .HasKey(x => x.HomeworkId);

            modelBuilder
                .Entity<Homework>()
                .Property(p => p.Content)
                .IsRequired();

            modelBuilder
                .Entity<Homework>()
                .Property(p => p.ContentType)
                .IsRequired();

            modelBuilder
                .Entity<Homework>()
                .Property(p => p.SubmissionTime)
                .IsRequired();

            modelBuilder
                .Entity<Homework>()
                .HasOne(x => x.Student)
                .WithMany(x => x.HomeworkSubmissions);

            modelBuilder
                .Entity<Homework>()
                .HasOne(x => x.Course)
                .WithMany(x => x.HomeworkSubmissions);
        }

        private void CreateResourceModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Resource>()
                .HasKey(x => x.ResourceId);

            modelBuilder
                .Entity<Resource>()
                .Property(p => p.Name)
                .HasMaxLength(50)
                .IsUnicode()
                .IsRequired();

            modelBuilder
                .Entity<Resource>()
                .Property(p => p.Url)
                .IsRequired();

            modelBuilder
                .Entity<Resource>()
                .Property(p => p.ResourceType)
                .IsRequired();

            modelBuilder
                .Entity<Resource>()
                .HasOne(x => x.Course)
                .WithMany(x => x.Resources);

        }

        private void CreateCourseModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Course>()
                .HasKey(x => x.CourseId);

            modelBuilder
                .Entity<Course>()
                .Property(p => p.Name)
                .HasMaxLength(80)
                .IsUnicode()
                .IsRequired();

            modelBuilder
                .Entity<Course>()
                .Property(p => p.Description)
                .IsUnicode()
                .IsRequired(false);

            modelBuilder
                .Entity<Course>()
                .Property(p => p.StartDate)
                .IsRequired();

            modelBuilder
                .Entity<Course>()
                .Property(p => p.EndDate)
                .IsRequired();

            modelBuilder
                .Entity<Course>()
                .Property(p => p.Price)
                .IsRequired();

        }

        private void CreateStudentModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Student>()
                .HasKey(x => x.StudentId);

            modelBuilder
                .Entity<Student>()
                .Property(p => p.Name)
                .HasMaxLength(100)
                .IsUnicode()
                .IsRequired();

            modelBuilder
                .Entity<Student>()
                .Property(p => p.PhoneNumber)
                .HasColumnType("CHAR(10)")
                .IsRequired(false);

            modelBuilder
                .Entity<Student>()
                .Property(p => p.RegisteredOn)
                .HasDefaultValueSql("GETDATE()")
                .IsRequired();

            modelBuilder
                .Entity<Student>()
                .Property(p => p.Birthday)
                .IsRequired(false);
        }

    }
}
