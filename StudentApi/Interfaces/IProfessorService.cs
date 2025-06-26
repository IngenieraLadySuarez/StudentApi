using StudentApi.DTO;
using StudentApi.Models;
namespace StudentApi.Interfaces
{
    public interface IProfessorService
    {
        Task<IEnumerable<ProfessorDto>> GetAllAsync();
        Task<IEnumerable<ProfessorSubjectDto>> GetProfessorSubjectsAsync();

    }

}
