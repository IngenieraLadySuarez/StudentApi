using Microsoft.AspNetCore.Mvc;
using StudentApi.DTO;
using StudentApi.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class ProfessorController : ControllerBase
{
    private readonly IProfessorService _professorService;

    public ProfessorController(IProfessorService professorService)
    {
        _professorService = professorService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProfessorDto>>> GetProfessors()
    {
        try
        {
            var professors = await _professorService.GetAllAsync();

            if (!professors.Any())
                return NotFound(new { message = "No se encontraron profesores." });

            return Ok(professors);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Ocurrió un error al obtener los profesores.", details = ex.Message });
        }
    }

    [HttpGet("professor-subjects")]
    public async Task<ActionResult<IEnumerable<ProfessorSubjectDto>>> GetProfessorSubjects()
    {
        try
        {
            var result = await _professorService.GetProfessorSubjectsAsync();

            if (!result.Any())
                return NotFound(new { message = "No se encontraron relaciones profesor-materia." });

            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error al obtener los datos.", details = ex.Message });
        }
    }

}
