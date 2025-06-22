using Admin.Infrastructure.Persistence;
using Admin.Domain.Model.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Admin.Domain.Model.Queries
{
    public class GetFeedbackQuery
    {
        private readonly AdminDbContext _context;

        public GetFeedbackQuery(AdminDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Feedback> Execute()
        {
            return _context.Feedbacks.ToList();
        }
    }
}
