using pathly_backend.Psychologist.Application.Internal.DTOs;
using pathly_backend.Psychologist.Domain.Model.Commands;
using pathly_backend.Psychologist.Domain.Model.Entities;
using Microsoft.EntityFrameworkCore;
using pathly_backend.IAM.Infrastructure.Persistence.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;
using pathly_backend.Psychologist.Domain.Model.Queries;
namespace pathly_backend.Psychologist.Domain.Services;

public class SectionService : IRequestHandler<CreateSectionCommand,Guid>
{  //crear seccion
    private readonly ApplicationDbContext _context;
      public SectionService(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<Guid> Handle(CreateSectionCommand request, CancellationToken cancellationToken)
    {  
        if (request.StudentId == Guid.Empty)
        {
            throw new ArgumentException("Student ID cannot be empty");
        }

        // Verificar que el estudiante existe
        var student = await _context.Students
            .FirstOrDefaultAsync(s => s.Id == request.StudentId, cancellationToken);

        if (student == null)
        {
            throw new ArgumentException($"Student with ID {request.StudentId} not found");
        }

            
        var section = new Section
        { 
            StudentId = request.StudentId,
            StudentName = student.Name,
            Title = request.Title,
            Description = request.Description,
            Date = request.Date,
            Status = request.Status,
            Mode = request.Mode,
          
        };
            
        _context.Sections.Add(section);
        await _context.SaveChangesAsync(cancellationToken);
            
        return section.Id;
    }
}
public class UpdateSectionHandler : IRequestHandler<UpdateSectionCommand, bool>
{
    private readonly ApplicationDbContext _context;
        
    public UpdateSectionHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UpdateSectionCommand request, CancellationToken cancellationToken)
    {
        var section = await _context.Sections
            .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);
                
        if (section == null)
            return false;
                
        // Verificar que el estudiante existe
        var studentExists = await _context.Students
            .AnyAsync(s => s.Id == request.StudentId, cancellationToken);
                
        if (!studentExists)
            throw new ArgumentException($"Student with ID {request.StudentId} not found");
            
        section.StudentId = request.StudentId;
        section.Title = request.Title;
        section.Description = request.Description;
        section.Date = request.Date;
        section.Status = request.Status;
        section.Mode = request.Mode;
       
            
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}

     public class GetAllSectionsHandler : IRequestHandler<GetAllSectionQuery,SectionResponseDto>
    {
        private readonly ApplicationDbContext _context;
        
        public GetAllSectionsHandler(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<SectionResponseDto> Handle(GetAllSectionQuery request, CancellationToken cancellationToken)
        {
            var students = await _context.Students
                .Select(s => new StudentDTo 
                { 
                    Id=s.Id,
                    Name = s.Name 
                })
                .ToListAsync(cancellationToken);
                
            var sections = await _context.Sections
                .Select(s => new SectionDTo
                {
                      Id = s.Id,
                    StudentName =s.StudentName,
                    Title = s.Title,
                    Description = s.Description,
                    Date = s.Date,
                    Status = s.Status,
                    Mode = s.Mode
                })
                .ToListAsync(cancellationToken);
                
            return new SectionResponseDto
            {
                Students = students,
                Sections = sections
            };
        }
    }

     public class GetSectionByIdHandler : IRequestHandler<GetSectionByIdQuery, Section>
     {
         private readonly ApplicationDbContext _context;
        
         public GetSectionByIdHandler(ApplicationDbContext context)
         {
             _context = context;
         }
        
         public async Task<Section> Handle(GetSectionByIdQuery request, CancellationToken cancellationToken)
         {
             return await _context.Sections
                 .Include(s => s.Student)
                 .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);
         }
     }
     public class DeleteSectionHandler : IRequestHandler<DeleteSectionCommand, bool>
     {
         private readonly ApplicationDbContext _context;
        
         public DeleteSectionHandler(ApplicationDbContext context)
         {
             _context = context;
         }
        
         public async Task<bool> Handle(DeleteSectionCommand request, CancellationToken cancellationToken)
         {
             var section = await _context.Sections
                 .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);
                
             if (section == null)
                 return false;
            
             _context.Sections.Remove(section);
             await _context.SaveChangesAsync(cancellationToken);
             return true;
         }
     }
     public class GetSectionsByStudentHandler : IRequestHandler<GetSectionsByStudentQuery, List<Section>>
     {
         private readonly ApplicationDbContext _context;
        
         public GetSectionsByStudentHandler(ApplicationDbContext context)
         {
             _context = context;
         }
        
         public async Task<List<Section>> Handle(GetSectionsByStudentQuery request, CancellationToken cancellationToken)
         {
             return await _context.Sections
                 .Where(s => s.StudentId == request.StudentId)
                 .Include(s => s.Student)
                 .OrderBy(s => s.Date)
                 .ToListAsync(cancellationToken);
         }
     }
