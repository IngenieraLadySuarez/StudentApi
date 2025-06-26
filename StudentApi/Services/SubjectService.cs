using Microsoft.EntityFrameworkCore;
using StudentApi.Data;
using StudentApi.DTO;
using StudentApi.Interfaces;
using StudentApi.Models;

namespace StudentApi.Services
{
    public class SubjectService : ISubjectService
    {
        private readonly StudentDbContext _context;

        public SubjectService(StudentDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SubjectDto>> GetAllAsync()
        {
            var subjects = await _context.Subjects.AsNoTracking().ToListAsync();

            return subjects.Select(s => new SubjectDto
            {
                Id = s.Id,
                Name = s.Name
            });
        }

    }

}
