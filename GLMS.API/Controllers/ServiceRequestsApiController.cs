using GLMS.Data;
using GLMS.Models;
using GLMS.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GLMS.API.Controllers
{
    [Route("api/servicerequests")]
    [ApiController]
    public class ServiceRequestsApiController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ContractValidationService _contractValidationService;

        public ServiceRequestsApiController(
            AppDbContext context,
            ContractValidationService contractValidationService)
        {
            _context = context;
            _contractValidationService = contractValidationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetServiceRequests()
        {
            var requests = await _context.ServiceRequests
                .Include(s => s.Contract)
                .ThenInclude(c => c.Client)
                .ToListAsync();

            return Ok(requests);
        }

        [HttpPost]
        public async Task<IActionResult> CreateServiceRequest([FromBody] ServiceRequest serviceRequest)
        {
            var contract = await _context.Contracts.FindAsync(serviceRequest.ContractId);

            if (contract == null)
            {
                return NotFound("Contract not found.");
            }

            if (!_contractValidationService.CanCreateServiceRequest(contract))
            {
                return BadRequest("Service request cannot be created for Expired or On Hold contracts.");
            }

            _context.ServiceRequests.Add(serviceRequest);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetServiceRequests), new { id = serviceRequest.Id }, serviceRequest);
        }
    }
}