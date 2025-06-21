
using pathly_backend.Psychologist.Application.Internal.DTOs;
using pathly_backend.Psychologist.Domain.Model.Commands;
using pathly_backend.Psychologist.Domain.Model.Entities;
using pathly_backend.IAM.Infrastructure.Persistence.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;
using pathly_backend.Psychologist.Domain.Model.Queries;
using Microsoft.AspNetCore.Mvc;

using Swashbuckle.AspNetCore.Annotations;

namespace pathly_backend.Psychologist.Interfaces.Rest
{
    [ApiController]
    [Route("api/[controller]")]
    public class SectionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SectionsController(IMediator mediator)
        {
            _mediator = mediator;
        }
   
        [HttpGet]
        [SwaggerOperation(
            Summary = "Obtener todas las secciones",
            Description = "Retorna la lista de todas las secciones junto con sus estudiantes asociados."
        )]
        public async Task<ActionResult<SectionDTo>> GetAllSections()
        {
            try
            {
                var result = await _mediator.Send(new GetAllSectionQuery());
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", details = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Obtener sección por ID",
            Description = "Devuelve una sección específica por su ID."
        )]
        public async Task<ActionResult<Section>> GetSectionById(Guid id)
        {
            try
            {
                var section = await _mediator.Send(new GetSectionByIdQuery(id));
                if (section == null)
                    return NotFound(new { message = $"Sección con ID {id} no encontrada" });

                return Ok(section);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", details = ex.Message });
            }
        }

        
        [HttpGet("student/{studentId}")]
        
        [SwaggerOperation(
            Summary = "Obtener todas las seccciones de un estudiante por ID",
            Description = "Devuelve todas las secciones de un estudiante por ID."
        )]
        public async Task<ActionResult<List<Section>>> GetSectionsByStudent(Guid studentId)
        {
            try
            {
                var sections = await _mediator.Send(new GetSectionsByStudentQuery(studentId));
                return Ok(sections);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", details = ex.Message });
            }
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Crear nueva sección",
            Description = "Crea una nueva sección en el sistema y devuelve el ID generado."
        )]
        public async Task<ActionResult<int>> CreateSection([FromBody] CreateSectionCommand command)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var sectionId = await _mediator.Send(command);
                return CreatedAtAction(nameof(GetSectionById), new { id = sectionId },
                    new { id = sectionId, message = "Sección creada exitosamente" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", details = ex.Message });
            }
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteSection(Guid id)
        {
            try
            {
                var result = await _mediator.Send(new DeleteSectionCommand { Id = id });
                
                if (!result)
                    return NotFound(new { message = $"Sección con ID {id} no encontrada" });
                
                return Ok(new { message = "Sección eliminada exitosamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", details = ex.Message });
            }
        }
        [HttpPut("{id}")]
        [SwaggerOperation(
            Summary = "Actualizar sección",
            Description = "Actualiza los datos de una sección existente según el ID."
        )]
        public async Task<ActionResult> UpdateSection(Guid id, [FromBody] UpdateSectionCommand command)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                
                if (id != command.Id)
                    return BadRequest(new
                        { message = "El ID de la URL no coincide con el ID del cuerpo de la petición" });

                var result = await _mediator.Send(command);
                if (!result)
                    return NotFound(new { message = $"Sección con ID {id} no encontrada" });

                return Ok(new { message = "Sección actualizada exitosamente" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", details = ex.Message });
            }
        }
        

    }

}
