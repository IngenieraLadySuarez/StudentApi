    using StudentApi.DTO;
using StudentApi.Models;

namespace StudentApi.Interfaces
{
    public interface IStudentSubjectService
    {
        Task<IEnumerable<StudentSubject>> GetAllAsync();

        Task<AssignSubjectResponseDto> AssignSubjectToStudentAsync(int studentId, int subjectId);
        Task<IEnumerable<ClassmatesBySubjectDto>> GetClassmatesGroupedBySubjectAsync(int studentId);

        Task<IEnumerable<ClassmateGroupedDto>> GetClassmatesAsync(int studentId);
    }
}
