using Microsoft.EntityFrameworkCore;
using StudentApi.Models;

namespace StudentApi.Data
{
    public class StudentDbContext : DbContext
    {
        public StudentDbContext(DbContextOptions<StudentDbContext> options) : base(options) { }

        public DbSet<Student> Students { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Professor> Professors { get; set; }
        public DbSet<ProfessorSubject> ProfessorSubjects { get; set; }
        public DbSet<StudentSubject> StudentSubjects { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProfessorSubject>()
                .HasKey(ps => new { ps.ProfessorId, ps.SubjectId });

            modelBuilder.Entity<Professor>()
                .HasMany(p => p.ProfessorSubjects)
                .WithOne(ps => ps.Professor)
                .HasForeignKey(ps => ps.ProfessorId);

            modelBuilder.Entity<Subject>()
                .HasMany(s => s.ProfessorSubjects)
                .WithOne(ps => ps.Subject)
                .HasForeignKey(ps => ps.SubjectId);

            modelBuilder.Entity<StudentSubject>(entity =>
            {
                entity.HasKey(ss => ss.Id); 
                entity.Property(ss => ss.Id)
                      .ValueGeneratedOnAdd(); 

                entity.HasOne(ss => ss.Student)
                      .WithMany(s => s.StudentSubjects)
                      .HasForeignKey(ss => ss.StudentId);

                entity.HasOne(ss => ss.Subject)
                      .WithMany(s => s.StudentSubjects)
                      .HasForeignKey(ss => ss.SubjectId);
            });



        }

    }

}
