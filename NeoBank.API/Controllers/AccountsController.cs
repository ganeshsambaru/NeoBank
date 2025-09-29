using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NeoBank.Api.Services.Interfaces;
using NeoBank.API.Models.DTOs;

namespace NeoBank.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _service;

        public AccountsController(IAccountService service)
        {
            _service = service;
        }

        // View all accounts – Managers and BankStaff only
        [HttpGet]
        //[Authorize(Roles = "Manager,BankStaff")]
        public async Task<ActionResult<IEnumerable<AccountDto>>> GetAll()
        {
            var accounts = await _service.GetAllAsync();
            return Ok(accounts);
        }

        // View a single account – Manager, Staff, or the account’s own Customer
        [HttpGet("{id:int}")]
        //[Authorize(Roles = "Manager,BankStaff,Customer")]
        public async Task<ActionResult<AccountDto>> GetById(int id)
        {
            var dto = await _service.GetByIdAsync(id);
            if (dto == null)
                return NotFound();

            // optional: restrict Customer to only his/her account
            // if (User.IsInRole("Customer") && User.Identity?.Name != dto.CustomerId.ToString())
            //     return Forbid();

            return Ok(dto);
        }

        // Create account – Manager or Staff only
        [HttpPost]
        //[Authorize(Roles = "Manager,BankStaff")]
        public async Task<ActionResult<AccountDto>> Create([FromBody] AccountDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _service.AddAsync(dto);

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // Update account – Manager or Staff, or customer’s own account (with extra check)
        [HttpPut("{id:int}")]
        //[Authorize(Roles = "Manager,BankStaff,Customer")]
        public async Task<IActionResult> Update(int id, [FromBody] AccountDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existing = await _service.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            // optional: restrict Customer to only his/her account
            // if (User.IsInRole("Customer") && User.Identity?.Name != existing.CustomerId.ToString())
            //     return Forbid();

            await _service.UpdateAsync(id, dto);

            return NoContent();
        }

        // Delete account – Managers only
        [HttpDelete("{id:int}")]
        //[Authorize(Roles = "Manager")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _service.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            await _service.DeleteAsync(id);

            return NoContent();
        }
    }
}
