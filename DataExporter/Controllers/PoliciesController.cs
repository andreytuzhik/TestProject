using DataExporter.Dtos;
using DataExporter.Services;
using Microsoft.AspNetCore.Mvc;

namespace DataExporter.Controllers
{
    /// <summary>
    /// Controller to manage policies.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class PoliciesController : ControllerBase
    {
        private readonly IPolicyService _policyService;
        private readonly ILogger<PoliciesController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="PoliciesController"/> class.
        /// </summary>
        /// <param name="policyService">The policy service.</param>
        /// <param name="logger">The logger.</param>
        public PoliciesController(IPolicyService policyService, ILogger<PoliciesController> logger)
        {
            _policyService = policyService ?? throw new ArgumentNullException(nameof(policyService)); ;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger)); ;
        }

        /// <summary>
        /// Creates a new policy.
        /// </summary>
        /// <param name="createPolicyDto">The policy data.</param>
        /// <returns>Returns the created policy.</returns>
        [HttpPost]
        public async Task<IActionResult> CreatePolicyAsync([FromBody] CreatePolicyDto createPolicyDto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var createdPolicy = await _policyService.CreatePolicyAsync(createPolicyDto, cancellationToken);
                _logger.LogInformation("Policy created successfully with ID {PolicyId}.", createdPolicy.Id);

                return CreatedAtAction(nameof(GetPolicy), new { policyId = createdPolicy.Id }, createdPolicy);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the policy.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

        /// <summary>
        /// Retrieves all policies.
        /// </summary>
        /// <returns>Returns a list of policies.</returns>
        [HttpGet]
        public async Task<IActionResult> GetPolicies(CancellationToken cancellationToken)
        {
            try
            {
                var policies = await _policyService.GetAllPoliciesAsync(cancellationToken);
                return Ok(policies);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving policies.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

        /// <summary>
        /// Retrieves a policy by ID.
        /// </summary>
        /// <param name="policyId">The policy ID.</param>
        /// <returns>Returns the policy with the specified ID.</returns>
        [HttpGet("{policyId}")]
        public async Task<IActionResult> GetPolicy(int policyId, CancellationToken cancellationToken)
        {
            try
            {
                var policy = await _policyService.ReadPolicyAsync(policyId, cancellationToken);

                if (policy is null)
                {
                    _logger.LogWarning("Policy with ID {PolicyId} not found.", policyId);
                    return NotFound($"Policy with ID {policyId} not found.");
                }

                _logger.LogInformation("Policy with ID {PolicyId} retrieved successfully.", policyId);
                return Ok(policy);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving policy with ID {PolicyId}.", policyId);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

        /// <summary>
        /// Exports policies within the specified date range.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns>Returns the exported policies.</returns>
        [HttpGet("export")]
        public async Task<IActionResult> Export([FromQuery] DateTime startDate, [FromQuery] DateTime endDate, CancellationToken cancellationToken)
        {
            try
            {
                var exportData = await _policyService.ExportPoliciesAsync(startDate, endDate, cancellationToken);
                return Ok(exportData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while exporting policies for startDate {startDate}, endDate {endDate}.", startDate, endDate);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }
    }
}
