using StudentApi.DTO;
using StudentApi.Models;

namespace StudentApi.Interfaces
{
    public interface IStudentService
    {
        Task<bool> DeleteAsync(int id, string course);
        Task<Student?> GetByIdAsync(int id);
        Task<StudentResponseDto> CreateAsync(StudentCreateDto studentDto);
        Task UpdateAsync(int id, string course, StudentUpdateDto dto);
        Task<IEnumerable<StudentDto>> GetAllAsync();

    }

}
