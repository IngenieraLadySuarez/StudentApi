using Microsoft.EntityFrameworkCore;
using StudentApi.Data;
using StudentApi.DTO;
using StudentApi.Interfaces;
using StudentApi.Models;

namespace StudentApi.Services
{
    public class StudentService : IStudentService
    {
        private readonly StudentDbContext _context;

        public StudentService(StudentDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<StudentDto>> GetAllAsync()
        {
            var students = await _context.Students
                .AsNoTracking()
                .ToListAsync();

            return students.Select(s => new StudentDto
            {
                Id = s.Id,
                FirstName = s.FirstName,
                LastName = s.LastName,
                Email = s.Email,
                Course = s.Course,
                CreatedAt = s.CreatedAt,
                UpdatedAt = s.UpdatedAt
            });
        }

        public async Task<Student?> GetByIdAsync(int id)
            => await _context.Students.FindAsync(id);

        public async Task<StudentResponseDto> CreateAsync(StudentCreateDto dto)
        {
            var normalizedInput = dto.Course?.Trim().ToLowerInvariant();

            if (string.IsNullOrWhiteSpace(normalizedInput))
                throw new ArgumentException("La materia no puede estar vacía.");

            var count = await _context.Students.CountAsync(s => s.Id == dto.Id);
            if (count >= 3)
                throw new ArgumentException($"Ya existen {count} materias para este estudiante. No se pueden registrar más.");

            var courseExistsForStudent = await _context.Students
                .AnyAsync(s => s.Id == dto.Id &&
                    EF.Functions.Collate(s.Course, "Latin1_General_CI_AI") == normalizedInput);

            if (courseExistsForStudent)
                throw new ArgumentException($"El estudiante con ID {dto.Id} ya está registrado en la materia '{dto.Course}'.");

            var subjectExists = await _context.Subjects
                .AnyAsync(s => EF.Functions.Collate(s.Name, "Latin1_General_CI_AI") == normalizedInput);

            if (!subjectExists)
                throw new ArgumentException($"La materia '{dto.Course}' no existe.");

            var now = DateTime.UtcNow;

            var student = new Student
            {
                Id = dto.Id,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Course = dto.Course,
                CreatedAt = now,
                UpdatedAt = now
            };

            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            return new StudentResponseDto
            {
                StudentRecordId = student.StudentRecordId,
                Id = student.Id,
                FirstName = student.FirstName,
                LastName = student.LastName,
                Email = student.Email,
                CourseName = dto.Course
            };
        }


        public async Task UpdateAsync(int id, string course, StudentUpdateDto dto)
        {
            var student = await _context.Students
                    .FirstOrDefaultAsync(s =>
                        s.Id == id &&
                        EF.Functions.Collate(s.Course, "Latin1_General_CI_AI") == course.Trim());



            if (student == null)
                throw new KeyNotFoundException($"Estudiante con ID {id} no encontrado.");

            var emailExists = await _context.Students
    .           AnyAsync(s => s.Email == dto.Email && s.Id != s.Id);


            if (emailExists)
                throw new ArgumentException($"Ya existe un estudiante con el correo {dto.Email}.");

            var normalizedCourse = dto.Course.ToLowerInvariant();
            var subjectExists = await _context.Subjects
                .AnyAsync(s => EF.Functions.Collate(s.Name, "Latin1_General_CI_AI") == normalizedCourse);

            if (!subjectExists)
                throw new ArgumentException($"La materia '{dto.Course}' no existe.");

            student.FirstName = dto.FirstName;
            student.LastName = dto.LastName;
            student.Email = dto.Email;
            student.Course = dto.Course;
            student.UpdatedAt = DateTime.UtcNow;
            student.UpdatedBy = "SystemUdate";

            _context.Students.Update(student);
            await _context.SaveChangesAsync();
        }


        public async Task<bool> DeleteAsync(int id, string course)
        {
            var student = await _context.Students
                .FirstOrDefaultAsync(s => s.Id == id && s.Course.ToLower() == course.ToLower());

            if (student == null)
                return false;

            var relatedSubjects = await _context.StudentSubjects
                .Where(ss => ss.StudentId == id)
                .ToListAsync();

            _context.StudentSubjects.RemoveRange(relatedSubjects);

            _context.Students.Remove(student);

            await _context.SaveChangesAsync();
            return true;
        }

    }
}

    
