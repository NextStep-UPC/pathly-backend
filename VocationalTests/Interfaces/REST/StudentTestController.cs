using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pathly_backend.VocationalTests.Application.Dtos;
using pathly_backend.VocationalTests.Application.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace pathly_backend.VocationalTests.Interfaces.REST
{
    [ApiController]
    public class StudentTestController : ControllerBase
    {
        private readonly IStudentTestService _svc;
        public StudentTestController(IStudentTestService svc) => _svc = svc;

        [Authorize]
        [HttpPost("api/sessions/{sessionId:guid}/tests/{testId:guid}/start")]
        [SwaggerOperation(Summary = "Iniciar test en sesión", Description = "Crea un StudentTest vinculado a la sesión.")]
        [SwaggerResponse(201, "StudentTest creado", typeof(StudentTestStartDto))]
        public async Task<ActionResult<StudentTestStartDto>> Start(Guid sessionId, Guid testId)
        {
            var studentId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var dto = await _svc.StartAsync(sessionId, testId, studentId);
            return CreatedAtAction(nameof(GetResults), new { studentTestId = dto.StudentTestId }, dto);
        }

        [Authorize]
        [HttpPost("api/student-tests/{studentTestId:guid}/answers")]
        [SwaggerOperation(Summary = "Enviar respuesta", Description = "Guarda o actualiza una respuesta de pregunta.")]
        [SwaggerResponse(204, "Respuesta registrada")]
        public async Task<IActionResult> SubmitAnswer(Guid studentTestId, [FromBody] SubmitAnswerDto dto)
        {
            await _svc.SubmitAnswerAsync(studentTestId, dto);
            return NoContent();
        }

        [Authorize]
        [HttpPost("api/student-tests/{studentTestId:guid}/complete")]
        [SwaggerOperation(Summary = "Completar test", Description = "Marca el test como finalizado.")]
        [SwaggerResponse(204, "Test completado")]
        public async Task<IActionResult> Complete(Guid studentTestId)
        {
            await _svc.CompleteAsync(studentTestId);
            return NoContent();
        }

        [Authorize]
        [HttpGet("api/sessions/{sessionId:guid}/tests/{studentTestId:guid}/results")]
        [SwaggerOperation(Summary = "Resultados del test en sesión", Description = "Devuelve respuestas y respuestas correctas.")]
        [SwaggerResponse(200, "Resultados", typeof(StudentTestResultDto))]
        public async Task<ActionResult<StudentTestResultDto>> GetResults(Guid sessionId, Guid studentTestId)
        {
            var res = await _svc.GetResultsAsync(studentTestId);
            if (res == null) return NotFound();
            return Ok(res);
        }
    }
}