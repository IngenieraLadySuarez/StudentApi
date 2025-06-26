using Microsoft.AspNetCore.Mvc;
using StudentApi.DTO;
using StudentApi.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class SubjectController : ControllerBase
{
    private readonly ISubjectService _subjectService;

    public SubjectController(ISubjectService subjectService)
    {
        _subjectService = subjectService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SubjectDto>>> GetSubjects()
    {
        try
        {
            var subjects = await _subjectService.GetAllAsync();

            if (!subjects.Any())
                return NotFound(new { message = "No se encontraron materias." });

            return Ok(subjects);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Ocurrió un error al obtener las materias.", details = ex.Message });
        }
    }
}
