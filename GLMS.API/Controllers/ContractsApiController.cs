using GLMS.Data;
using GLMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GLMS.API.Controllers
{
    [Route("api/contracts")]
    [ApiController]
    public class ContractsApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ContractsApiController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /api/contracts
        [HttpGet]
        public async Task<IActionResult> GetContracts(string? status, DateTime? startDate, DateTime? endDate)
        {
            var contracts = _context.Contracts
                .Include(c => c.Client)
                .AsQueryable();

            if (!string.IsNullOrEmpty(status))
            {
                contracts = contracts.Where(c => c.Status == status);
            }

            if (startDate.HasValue)
            {
                contracts = contracts.Where(c => c.StartDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                contracts = contracts.Where(c => c.EndDate <= endDate.Value);
            }

            var result = await contracts.ToListAsync();

            return Ok(result);
        }

        // POST: /api/contracts
        [HttpPost]
        public async Task<IActionResult> CreateContract([FromBody] Contract contract)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Contracts.Add(contract);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetContracts), new { id = contract.Id }, contract);
        }

        // PATCH: /api/contracts/5/status
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateContractStatus(int id, [FromBody] string status)
        {
            var contract = await _context.Contracts.FindAsync(id);

            if (contract == null)
            {
                return NotFound();
            }

            contract.Status = status;
            await _context.SaveChangesAsync();

            return Ok(contract);
        }
    }
}