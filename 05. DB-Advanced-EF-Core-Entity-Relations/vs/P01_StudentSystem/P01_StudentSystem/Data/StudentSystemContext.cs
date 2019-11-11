
namespace P01_StudentSystem.Data
{
    using Microsoft.EntityFrameworkCore;
    using P01_StudentSystem.Data.Models;

    public class StudentSystemContext : DbContext
    {
        public StudentSystemContext()
        {

        }
        public StudentSystemContext(DbContextOptions options)
            : base(options)
        {

        }
        public DbSet<Student> Students { get; set; }
        public DbSet<HomeworkSubmission> HomeworkSubmissions { get; set; }
        public DbSet<StudentCourse> StudentCourses { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Resource> Resources { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            if (!builder.IsConfigured)
            {
                builder
                    .UseSqlServer(DataSettings.ConnectionString);
            }
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                 .Entity<Resource>(resourse =>
                 {
                     resourse
                      .HasKey(r => r.ResourceId);

                     resourse
                     .HasOne(c => c.Course)
                     .WithMany(r => r.Resources)
                     .HasForeignKey(c => c.CourseId);

                 });

            builder
                .Entity<Student>(student =>
                {
                    student
                    .HasKey(s => s.StudentId);

                    student
                    .Property(p => p.Name)
                    .HasMaxLength(DataValidations.Student.MaxNameLength);

                    student
                    .Property(p => p.PhoneNumber)
                    .HasMaxLength(DataValidations.Student.PhoneNumberLength)
                    .IsFixedLength()
                   .IsUnicode(false)
                   .IsRequired(false);
                });

            builder
                .Entity<Course>(course =>
                {
                    course
                    .HasKey(k => k.CourseId);

                    course
                    .Property(p => p.Name)
                    .HasMaxLength(DataValidations.Course.MaxNameLength);


                });

            builder
                .Entity<HomeworkSubmission>(hs =>
                {
                    hs
                    .HasKey(k => k.HomeworkId);

                    hs
                    .HasOne(s => s.Student)
                    .WithMany(h => h.HomeworkSubmissions)
                    .HasForeignKey(s => s.StudentId);

                    hs
                    .HasOne(c => c.Course)
                    .WithMany(h => h.HomeworkSubmissions)
                    .HasForeignKey(c => c.CourseId);
                        
                    });

            builder
                .Entity<StudentCourse>(sc =>
                {
                    sc.HasKey(e => new { e.StudentId, e.CourseId });

                    sc
                    .HasOne(s => s.Student)
                    .WithMany(c => c.CourseEnrollments)
                    .HasForeignKey(s => s.StudentId);

                    sc.HasOne(c => c.Course)
                    .WithMany(h => h.CourseEnrollments)
                    .HasForeignKey(c => c.CourseId);
                });
        }
    }
}
