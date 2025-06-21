using MediatR;
using pathly_backend.CareerTest.Application.Internal.DTOs;
using System.Collections.Generic;

namespace pathly_backend.CareerTest.Domain.Model.Queries;

public class GetAllQuestionsQuery : IRequest<List<QuestionDto>>
{
}