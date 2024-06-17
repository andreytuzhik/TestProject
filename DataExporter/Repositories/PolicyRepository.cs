using DataExporter.Model;
using Microsoft.EntityFrameworkCore;

namespace DataExporter.Repositories
{
    public class PolicyRepository : IPolicyRepository
    {
        private readonly ExporterDbContext _dbContext;

        public PolicyRepository(ExporterDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Policy?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _dbContext.Policies.Include(p => p.Notes).SingleOrDefaultAsync(p => p.Id == id, cancellationToken);
        }

        public async Task<List<Policy>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _dbContext.Policies.Include(p => p.Notes).ToListAsync(cancellationToken);
        }

        public async Task<List<Policy>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken)
        {
            return await _dbContext.Policies
                .Include(p => p.Notes)
                .Where(p => p.StartDate >= startDate && p.StartDate <= endDate)
                .ToListAsync(cancellationToken);
        }

        public async Task AddAsync(Policy policy, CancellationToken cancellationToken)
        {
            await _dbContext.Policies.AddAsync(policy, cancellationToken);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
