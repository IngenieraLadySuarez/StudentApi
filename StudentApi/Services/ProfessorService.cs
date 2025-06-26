using Microsoft.EntityFrameworkCore;
using StudentApi.Data;
using StudentApi.DTO;
using StudentApi.Exceptions;
using StudentApi.Interfaces;
using StudentApi.Models;

namespace StudentApi.Services
{
    public class ProfessorService : IProfessorService
    {
        private readonly StudentDbContext _context;

        public ProfessorService(StudentDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProfessorDto>> GetAllAsync()
        {
            var professors = await _context.Professors.ToListAsync();

            return professors.Select(p => new ProfessorDto
            {
                Id = p.Id,
                FirstName = p.FirstName,
                LastName = p.LastName
            });
        }

        public async Task<IEnumerable<ProfessorSubjectDto>> GetProfessorSubjectsAsync()
        {
            var result = await _context.Professors
                .Join(_context.ProfessorSubjects,
                      p => p.Id,
                      ps => ps.ProfessorId,
                      (p, ps) => new { p, ps })
                .Join(_context.Subjects,
                      combined => combined.ps.SubjectId,
                      s => s.Id,
                      (combined, s) => new ProfessorSubjectDto
                      {
                          ProfessorId = combined.p.Id,
                          ProfessorName = combined.p.FirstName + " " + combined.p.LastName,
                          SubjectId = s.Id,
                          SubjectName = s.Name
                      })
                .ToListAsync();

            return result;
        }

    }
}
