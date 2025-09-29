using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NeoBankApi.DTOs;
using NeoBankApi.Services;

namespace NeoBankApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _service;

        public TransactionsController(ITransactionService service)
        {
            _service = service;
        }

        // View all transactions – Manager, BankStaff
        [HttpGet]
        [Authorize(Roles = "Manager,BankStaff")]
        public async Task<ActionResult<IEnumerable<TransactionDto>>> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        // View a single transaction – Manager, BankStaff, Customer (own only)
        [HttpGet("{id}")]
        [Authorize(Roles = "Manager,BankStaff,Customer")]
        public async Task<ActionResult<TransactionDto>> Get(int id)
        {
            var tx = await _service.GetByIdAsync(id);
            if (tx == null) return NotFound();

            // optional: restrict customers to only their own transaction
            // if (User.IsInRole("Customer") && User.Identity?.Name != tx.CustomerId.ToString())
            //     return Forbid();

            return Ok(tx);
        }

        // Create transaction – Manager, BankStaff
        [HttpPost]
        [Authorize(Roles = "Manager,BankStaff")]
        public async Task<ActionResult<TransactionDto>> Create([FromBody] CreateTransactionDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        // Update transaction – Manager, BankStaff
        [HttpPut("{id}")]
        [Authorize(Roles = "Manager,BankStaff")]
        public async Task<ActionResult<TransactionDto>> Update(int id, [FromBody] UpdateTransactionDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var updated = await _service.UpdateAsync(id, dto);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        // Delete transaction – Manager only
        [HttpDelete("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
