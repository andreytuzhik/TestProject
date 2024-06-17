using DataExporter.Dtos;

namespace DataExporter.Services
{
    public interface IPolicyService
    {
        Task<List<ReadPolicyDto>> GetAllPoliciesAsync(CancellationToken cancellationToken);
        Task<ReadPolicyDto?> ReadPolicyAsync(int id, CancellationToken cancellationToken);
        Task<ReadPolicyDto> CreatePolicyAsync(CreatePolicyDto createPolicyDto, CancellationToken cancellationToken);
        Task<List<ExportDto>> ExportPoliciesAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken);
    }
}
