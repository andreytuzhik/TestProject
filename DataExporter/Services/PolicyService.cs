using DataExporter.Dtos;
using DataExporter.Model;
using DataExporter.Repositories;

namespace DataExporter.Services
{
    public class PolicyService : IPolicyService
    {
        private readonly IPolicyRepository _policyRepository;

        public PolicyService(IPolicyRepository policyRepository)
        {
            _policyRepository = policyRepository;
        }

        public async Task<ReadPolicyDto> CreatePolicyAsync(CreatePolicyDto createPolicyDto, CancellationToken cancellationToken)
        {
            var newPolicy = new Policy
            {
                PolicyNumber = createPolicyDto.PolicyNumber,
                Premium = createPolicyDto.Premium,
                StartDate = createPolicyDto.StartDate,
                Notes = createPolicyDto.Notes.Select(n => new Note
                {
                    Text = n.Content
                }).ToList()
            };

            await _policyRepository.AddAsync(newPolicy, cancellationToken);
            await _policyRepository.SaveChangesAsync(cancellationToken);

            var policyDto = new ReadPolicyDto
            {
                Id = newPolicy.Id,
                PolicyNumber = newPolicy.PolicyNumber,
                Premium = newPolicy.Premium,
                StartDate = newPolicy.StartDate              
            };

            return policyDto;
        }


        public async Task<List<ExportDto>> ExportPoliciesAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken)
        {
            var policiesWithNotes = await _policyRepository.GetByDateRangeAsync(startDate, endDate, cancellationToken);

            var exportDtos = policiesWithNotes.Select(p => new ExportDto
            {
                PolicyNumber = p.PolicyNumber,
                Premium = p.Premium,
                StartDate = p.StartDate,
                Notes = p.Notes?.Select(n => n.Text).ToList()
            }).ToList();

            return exportDtos;
        }

        public async Task<List<ReadPolicyDto>> GetAllPoliciesAsync(CancellationToken cancellationToken)
        {
            var policies = await _policyRepository.GetAllAsync(cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();

            var policyDtos = policies.Select(policy => new ReadPolicyDto
            {
                Id = policy.Id,
                PolicyNumber = policy.PolicyNumber,
                Premium = policy.Premium,
                StartDate = policy.StartDate
            }).ToList();

            return policyDtos;
        }

        public async Task<ReadPolicyDto?> ReadPolicyAsync(int id, CancellationToken cancellationToken)
        {
            var policy = await _policyRepository.GetByIdAsync(id, cancellationToken);

            if (policy == null)
            {
                return null;
            }

            var policyDto = new ReadPolicyDto
            {
                Id = policy.Id,
                PolicyNumber = policy.PolicyNumber,
                Premium = policy.Premium,
                StartDate = policy.StartDate
            };

            return policyDto;
        }
    }
}
