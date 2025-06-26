using Microsoft.EntityFrameworkCore;
using StudentApi.Data;
using StudentApi.DTO;
using StudentApi.Exceptions;
using StudentApi.Interfaces;
using StudentApi.Models;

namespace StudentApi.Services
{
    public class StudentSubjectService : IStudentSubjectService
    {
        private readonly StudentDbContext _context;

        public StudentSubjectService(StudentDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<StudentSubject>> GetAllAsync()
        {
            return await _context.StudentSubjects
                .Include(ss => ss.Student)
                .Include(ss => ss.Subject)
                .ThenInclude(s => s.ProfessorSubjects)
                .ToListAsync();
        }

        public async Task<AssignSubjectResponseDto> AssignSubjectToStudentAsync(int studentId, int subjectId)
        {
            var student = await _context.Students.FirstOrDefaultAsync(s => s.Id == studentId);

            if (student == null)
                throw new ArgumentException($"El estudiante con id {studentId} no existe.");

            var subject = await _context.Subjects
                .Include(s => s.ProfessorSubjects)
                    .ThenInclude(ps => ps.Professor)
                .FirstOrDefaultAsync(s => s.Id == subjectId);
            if (subject == null)
                throw new ArgumentException($"La materia con id {subjectId} no existe.");

            var asignacionesCount = await _context.StudentSubjects.CountAsync(ss => ss.StudentId == studentId);
            if (asignacionesCount >= 3)
                throw new ArgumentException("El estudiante ya tiene asignadas 3 materias. No se pueden asignar más.");

            var studentSubjects = await _context.StudentSubjects
                .Where(ss => ss.StudentId == studentId)
                .Include(ss => ss.Subject)
                    .ThenInclude(s => s.ProfessorSubjects)
                        .ThenInclude(ps => ps.Professor)
                .ToListAsync();

            var existingProfessorIds = studentSubjects
                .SelectMany(ss => ss.Subject.ProfessorSubjects)
                .Select(ps => ps.ProfessorId)
                .ToHashSet();

            var newSubjectProfessorIds = subject.ProfessorSubjects
                .Select(ps => ps.ProfessorId)
                .ToHashSet();

            var conflict = existingProfessorIds.Intersect(newSubjectProfessorIds).Any();
            if (conflict)
                throw new ArgumentException("Este estudiante ya tiene clases con uno de los profesores de esta materia.");

            var studentSubject = new StudentSubject
            {
                StudentId = studentId,
                SubjectId = subjectId
            };

            _context.StudentSubjects.Add(studentSubject);
            await _context.SaveChangesAsync();

            var response = new AssignSubjectResponseDto
            {
                StudentId = studentId,
                Student = $"{student.FirstName} {student.LastName}",
                SubjectId = subjectId,
                Subject = subject.Name,
                Professor = string.Join(", ", subject.ProfessorSubjects.Select(ps => $"{ps.Professor.FirstName} {ps.Professor.LastName}"))
            };

            return response;
        }

        public async Task<IEnumerable<ClassmatesBySubjectDto>> GetClassmatesGroupedBySubjectAsync(int studentId)
        {
            var studentSubjects = await _context.StudentSubjects
                .Where(ss => ss.StudentId == studentId)
                .Select(ss => ss.SubjectId)
                .Distinct()
                .ToListAsync();

            var result = await _context.Subjects
                .Where(subject => studentSubjects.Contains(subject.Id))
                .Select(subject => new ClassmatesBySubjectDto
                {
                    SubjectId = subject.Id,
                    SubjectName = subject.Name,
                    Classmates = subject.StudentSubjects
                        .Where(ss => ss.StudentId != studentId)
                        .Select(ss => new ClassmateDto
                        {
                            FirstName = ss.Student.FirstName,
                            LastName = ss.Student.LastName
                        })
                        .ToList()
                })
                .ToListAsync();

            return result;
        }
        public async Task<IEnumerable<ClassmateGroupedDto>> GetClassmatesAsync(int studentId)
        {
            var subjectIds = await _context.StudentSubjects
                .Where(ss => ss.StudentId == studentId)
                .Select(ss => ss.SubjectId)
                .ToListAsync();

            var classmatesRaw = await _context.StudentSubjects
                .Where(ss => subjectIds.Contains(ss.SubjectId) && ss.StudentId != studentId)
                .ToListAsync();

            var studentIds = classmatesRaw.Select(c => c.StudentId).Distinct().ToList();
            var subjectIdsUsed = classmatesRaw.Select(c => c.SubjectId).Distinct().ToList();

            var students = await _context.Students
                .Where(s => studentIds.Contains(s.Id))
                .GroupBy(s => s.Id)
                .Select(g => g.First())
                .ToDictionaryAsync(s => s.Id, s => s.FirstName + " " + s.LastName);

            var subjects = await _context.Subjects
                .Where(s => subjectIdsUsed.Contains(s.Id))
                .ToDictionaryAsync(s => s.Id, s => s.Name);

            var grouped = classmatesRaw
                .GroupBy(c => c.StudentId)
                .Select(g => new ClassmateGroupedDto
                {
                    StudentId = g.Key,
                    StudentName = students.ContainsKey(g.Key) ? students[g.Key] : "Desconocido",
                    SharedSubjects = g.Select(c => subjects.ContainsKey(c.SubjectId) ? subjects[c.SubjectId] : "Materia desconocida")
                                      .Distinct()
                                      .ToList()
                });

            return grouped;
        }



    }
}
