using Microsoft.AspNetCore.Mvc;
using NeoBank.API.Models.DTOs;
using NeoBank.Api.Models.Entities;
using NeoBank.Api.Services.Interfaces;

namespace NeoBank.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _service;

        public AccountsController(IAccountService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var account = await _service.GetByIdAsync(id);
            if (account == null) return NotFound();
            return Ok(account);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AccountDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var account = new Account
            {
                AccountNumber = dto.AccountNumber,
                AccountType = dto.AccountType,
                Balance = dto.Balance,
                CustomerId = dto.CustomerId
                // DO NOT set Customer navigation
            };

            await _service.AddAsync(account);
            // Return dto with new Id
            dto.Id = account.Id;

            return CreatedAtAction(nameof(GetById), new { id = account.Id }, dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] AccountDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var account = new Account
            {
                Id = id,
                AccountNumber = dto.AccountNumber,
                AccountType = dto.AccountType,
                Balance = dto.Balance,
                CustomerId = dto.CustomerId
            };

            await _service.UpdateAsync(id, account);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
