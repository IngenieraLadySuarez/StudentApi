using Microsoft.AspNetCore.Mvc;
using StudentApi.Interfaces;
using System;

namespace StudentApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentSubjectsController : ControllerBase
    {
        private readonly IStudentSubjectService _service;

        public StudentSubjectsController(IStudentSubjectService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var data = await _service.GetAllAsync();

                if (data == null || !data.Any())
                {
                    return NotFound(new { message = "No se encontraron asignaciones de estudiantes a materias." });
                }

                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ocurrió un error al obtener las asignaciones de estudiantes.", details = ex.Message });
            }
        }

        [HttpPost("{studentId}/assign/{subjectId}")]
        public async Task<IActionResult> Assign(int studentId, int subjectId)
        {
            try
            {
                if (studentId <= 0 || subjectId <= 0)
                {
                    return BadRequest(new { message = "Los IDs de estudiante y materia deben ser mayores a 0." });
                }

                var result = await _service.AssignSubjectToStudentAsync(studentId, subjectId);

                if (result == null)
                {
                    return NotFound(new { message = "Estudiante o materia no encontrados para asignar." });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ocurrió un error al asignar la materia al estudiante.", details = ex.Message });
            }
        }

        [HttpGet("{studentId}/classmates")]
        public async Task<IActionResult> GetClassmates(int studentId)
        {
            try
            {
                if (studentId <= 0)
                    return BadRequest(new { message = "El ID del estudiante debe ser mayor a 0." });

                var classmates = await _service.GetClassmatesAsync(studentId);

                if (classmates == null || !classmates.Any())
                    return NotFound(new { message = "No se encontraron compañeros para este estudiante." });

                return Ok(classmates);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al obtener compañeros.", details = ex.Message });
            }
        }


    }
}
