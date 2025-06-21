using Microsoft.AspNetCore.Mvc;
using MediatR;
using pathly_backend.CareerTest.Domain.Model.Commands;
using pathly_backend.CareerTest.Domain.Model.Queries;
using pathly_backend.CareerTest.Application.Internal.DTOs;

namespace pathly_backend.CareerTest.Interfaces.Rest;

[ApiController]
[Route("api/[controller]")]
public class QuestionController : ControllerBase
{
    private readonly IMediator _mediator;

    public QuestionController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<QuestionDto>>> GetAll()
    {
        var result = await _mediator.Send(new GetAllQuestionsQuery());
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<QuestionDto>> GetById(int id)
    {
        var result = await _mediator.Send(new GetQuestionByIdQuery(id));
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<int>> Create([FromBody] CreateQuestionCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, [FromBody] UpdateQuestionCommand command)
    {
        if (id != command.Id) return BadRequest();
        var success = await _mediator.Send(command);
        if (!success) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var success = await _mediator.Send(new DeleteQuestionCommand { Id = id });
        if (!success) return NotFound();
        return NoContent();
    }
}