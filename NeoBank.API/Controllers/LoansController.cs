using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NeoBank.API.Services.Interfaces;

namespace NeoBank.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoansController : ControllerBase
    {
        private readonly ILoanService _service;
        public LoansController(ILoanService service) => _service = service;

        // Get all loans – Manager, BankStaff
        [HttpGet]
        [Authorize(Roles = "Manager,BankStaff")]
        public async Task<ActionResult<IEnumerable<LoanDto>>> GetAll() =>
            Ok(await _service.GetAllAsync());

        // Get a single loan – Manager, BankStaff, Customer (own only)
        [HttpGet("{id}")]
        [Authorize(Roles = "Manager,BankStaff,Customer")]
        public async Task<ActionResult<LoanDto>> Get(int id)
        {
            var loan = await _service.GetByIdAsync(id);
            if (loan == null) return NotFound();

            // optional: restrict customers to only their own loan
            // if (User.IsInRole("Customer") && User.Identity?.Name != loan.CustomerId.ToString())
            //     return Forbid();

            return Ok(loan);
        }

        // Create a loan – Manager, BankStaff
        [HttpPost]
        [Authorize(Roles = "Manager,BankStaff")]
        public async Task<ActionResult<LoanDto>> Create([FromBody] LoanDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _service.AddAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = dto.Id }, dto);
        }

        // Update a loan – Manager, BankStaff
        [HttpPut("{id}")]
        [Authorize(Roles = "Manager,BankStaff")]
        public async Task<ActionResult> Update(int id, [FromBody] LoanDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _service.UpdateAsync(id, dto);
            return NoContent();
        }

        // Delete a loan – Manager only
        [HttpDelete("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
