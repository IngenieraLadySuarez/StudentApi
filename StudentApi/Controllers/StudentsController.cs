using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentApi.DTO;
using StudentApi.Interfaces;
using StudentApi.Models;
using System;

namespace StudentApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentsController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentDto>>> GetStudents()

        {
            try
            {
                var students = await _studentService.GetAllAsync();

                if (!students.Any())
                    return NotFound(new { message = "No se encontraron estudiantes." });

                return Ok(students);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ocurrió un error al obtener los estudiantes.", details = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudent(int id)
        {
            try
            {
                var student = await _studentService.GetByIdAsync(id);

                if (student == null)
                {
                    return NotFound(new { message = $"Estudiante con id {id} no encontrado." });
                }

                return Ok(student);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ocurrió un error al obtener el estudiante.", details = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<StudentResponseDto>> CreateStudent([FromBody] StudentCreateDto studentDto)
        {
            try
            {
                if (studentDto == null)
                    return BadRequest(new { message = "El objeto estudiante no puede ser nulo." });

                var created = await _studentService.CreateAsync(studentDto);
                return CreatedAtAction(nameof(GetStudent), new { id = created.Id }, created);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error inesperado.",
                    details = ex.InnerException?.Message ?? ex.Message
                });
            }

        }



        [HttpPut("{id}/{course}")]
        public async Task<IActionResult> UpdateStudent(int id, string course, StudentUpdateDto dto)
        {
            try
            {
                await _studentService.UpdateAsync(id, course, dto);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = $"Estudiante con id {id} no encontrado para actualizar." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new { message = "Ocurrió un error al actualizar el estudiante en la base de datos.", details = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ocurrió un error inesperado.", details = ex.Message });
            }
        }


        [HttpDelete("{id}/{course}")]
        public async Task<IActionResult> DeleteStudent(int id, string course)
        {
            try
            {
                var deleted = await _studentService.DeleteAsync(id, course);

                if (!deleted)
                    return NotFound(new { message = $"No se encontró un estudiante con id {id} y curso '{course}' para eliminar." });

                return NoContent();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new { message = "Error al eliminar el estudiante de la base de datos.", details = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ocurrió un error inesperado.", details = ex.Message });
            }
        }


    }
}
