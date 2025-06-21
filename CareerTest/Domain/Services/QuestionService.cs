using pathly_backend.CareerTest.Domain.Model.Entities;
using pathly_backend.CareerTest.Domain.Model.Commands;
using pathly_backend.CareerTest.Domain.Model.Queries;
using pathly_backend.CareerTest.Application.Internal.DTOs;
using pathly_backend.CareerTest.Infrastructure.Persistence.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace pathly_backend.CareerTest.Domain.Services;

public class CreateQuestionHandler : IRequestHandler<CreateQuestionCommand, int>
{
    private readonly CareerTestDbContext _context;

    public CreateQuestionHandler(CareerTestDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateQuestionCommand request, CancellationToken cancellationToken)
    {
        var question = new Question
        {
            Text = request.Text,
            Options = request.Options.Select(o => new QuestionOption { Text = o.Text }).ToList()
        };

        _context.Questions.Add(question);
        await _context.SaveChangesAsync(cancellationToken);
        return question.QuestionId;
    }
}

public class UpdateQuestionHandler : IRequestHandler<UpdateQuestionCommand, bool>
{
    private readonly CareerTestDbContext _context;

    public UpdateQuestionHandler(CareerTestDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UpdateQuestionCommand request, CancellationToken cancellationToken)
    {
        var question = await _context.Questions
            .Include(q => q.Options)
            .FirstOrDefaultAsync(q => q.QuestionId == request.Id, cancellationToken);

        if (question == null)
            return false;

        question.Text = request.Text;
        question.Options.Clear();
        foreach (var option in request.Options)
        {
            question.Options.Add(new QuestionOption { Text = option.Text });
        }

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}

public class DeleteQuestionHandler : IRequestHandler<DeleteQuestionCommand, bool>
{
    private readonly CareerTestDbContext _context;

    public DeleteQuestionHandler(CareerTestDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(DeleteQuestionCommand request, CancellationToken cancellationToken)
    {
        var question = await _context.Questions
            .FirstOrDefaultAsync(q => q.QuestionId == request.Id, cancellationToken);

        if (question == null)
            return false;

        _context.Questions.Remove(question);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}

public class GetAllQuestionsHandler : IRequestHandler<GetAllQuestionsQuery, List<QuestionDto>>
{
    private readonly CareerTestDbContext _context;

    public GetAllQuestionsHandler(CareerTestDbContext context)
    {
        _context = context;
    }

    public async Task<List<QuestionDto>> Handle(GetAllQuestionsQuery request, CancellationToken cancellationToken)
    {
        return await _context.Questions
            .Include(q => q.Options)
            .Select(q => new QuestionDto
            {
                Id = q.QuestionId,
                Text = q.Text,
                Options = q.Options.Select(o => new QuestionOptionDto
                {
                    Id = o.Id,
                    Text = o.Text
                }).ToList()
            })
            .ToListAsync(cancellationToken);
    }
}

public class GetQuestionByIdHandler : IRequestHandler<GetQuestionByIdQuery, QuestionDto?>
{
    private readonly CareerTestDbContext _context;

    public GetQuestionByIdHandler(CareerTestDbContext context)
    {
        _context = context;
    }

    public async Task<QuestionDto?> Handle(GetQuestionByIdQuery request, CancellationToken cancellationToken)
    {
        var question = await _context.Questions
            .Include(q => q.Options)
            .FirstOrDefaultAsync(q => q.QuestionId == request.Id, cancellationToken);

        if (question == null)
            return null;

        return new QuestionDto
        {
            Id = question.QuestionId,
            Text = question.Text,
            Options = question.Options.Select(o => new QuestionOptionDto
            {
                Id = o.Id,
                Text = o.Text
            }).ToList()
        };
    }
}