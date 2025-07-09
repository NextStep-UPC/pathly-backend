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
    [Route("api/tests")]
    public class TestController : ControllerBase
    {
        private readonly ITestService _svc;
        public TestController(ITestService svc) => _svc = svc;

        [Authorize]
        [HttpGet]
        [SwaggerOperation(Summary = "Listar tests disponibles", Description = "Devuelve todos los tests vocacionales.")]
        [SwaggerResponse(200, "Listado de tests", typeof(IEnumerable<TestDto>))]
        public async Task<ActionResult<IEnumerable<TestDto>>> List()
            => Ok(await _svc.ListAllAsync());

        [Authorize]
        [HttpGet("{testId:guid}")]
        [SwaggerOperation(Summary = "Obtener un test", Description = "Devuelve preguntas y opciones de un test.")]
        [SwaggerResponse(200, "Test encontrado", typeof(TestDto))]
        public async Task<ActionResult<TestDto>> Get(Guid testId)
        {
            var test = await _svc.GetByIdAsync(testId);
            if (test == null) return NotFound();
            return Ok(test);
        }
    }
}