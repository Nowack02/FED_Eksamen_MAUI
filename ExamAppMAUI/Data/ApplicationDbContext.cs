using Microsoft.EntityFrameworkCore;
using ExamAppMAUI.Models;

namespace ExamAppMAUI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Exam> Exams { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<StudentExam> StudentExams { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<StudentExam>()
                .HasOne(se => se.Exam)
                .WithMany(e => e.StudentExams)
                .HasForeignKey(se => se.ExamId);

            modelBuilder.Entity<StudentExam>()
                .HasOne(se => se.Student)
                .WithMany(s => s.StudentExams)
                .HasForeignKey(se => se.StudentId);

            modelBuilder.Entity<Student>()
                .HasIndex(s => s.StudentNumber)
                .IsUnique();
            
            // SQLite does not support TimeSpan directly, so we convert it to ticks
            modelBuilder.Entity<Exam>()
                .Property(e => e.ExaminationDuration)
                .HasConversion(
                    v => v.Ticks,
                    v => TimeSpan.FromTicks(v));

            modelBuilder.Entity<StudentExam>()
                .Property(se => se.ActualExaminationDuration)
                .HasConversion(
                    v => v.HasValue ? v.Value.Ticks : (long?)null,
                    v => v.HasValue ? TimeSpan.FromTicks(v.Value) : (TimeSpan?)null);

            // Convert Grade to string for SQLite compatibility
            modelBuilder.Entity<StudentExam>()
                .Property(se => se.Grade)
                .HasConversion<string>();
        }
    }
}