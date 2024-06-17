using DataExporter.Model;

namespace DataExporter.Repositories
{
    public interface IPolicyRepository
    {
        Task<Policy?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<List<Policy>> GetAllAsync(CancellationToken cancellationToken);
        Task<List<Policy>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken);
        Task AddAsync(Policy policy, CancellationToken cancellationToken);
        Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}
