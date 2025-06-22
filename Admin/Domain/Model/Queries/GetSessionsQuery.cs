using Admin.Domain.Model.Entities;
using Admin.Infrastructure.Persistence;
using System.Collections.Generic;
using System.Linq;

namespace Admin.Domain.Model.Queries
{
    public class GetSessionsQuery
    {
        private readonly AdminDbContext _context;

        public GetSessionsQuery(AdminDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Session> Execute()
        {
            return _context.Sessions.ToList();
        }
    }
}
