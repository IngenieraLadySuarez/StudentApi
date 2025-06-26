using StudentApi.DTO;
using StudentApi.Models;

namespace StudentApi.Interfaces
{
    public interface ISubjectService
    {
        Task<IEnumerable<SubjectDto>> GetAllAsync();

    }

}
