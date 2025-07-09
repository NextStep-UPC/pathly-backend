using System.Threading.Tasks;

namespace pathly_backend.VocationalTests.Application.Interfaces
{
    public interface IVocationalTestsUnitOfWork
    {
        Task SaveChangesAsync();
    }
}