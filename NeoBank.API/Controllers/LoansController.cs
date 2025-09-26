using Microsoft.AspNetCore.Http;
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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LoanDto>>> GetAll() =>
            Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<ActionResult<LoanDto>> Get(int id)
        {
            var loan = await _service.GetByIdAsync(id);
            if (loan == null) return NotFound();
            return Ok(loan);
        }

        [HttpPost]
        public async Task<ActionResult<LoanDto>> Create([FromBody] LoanDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _service.AddAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = dto.Id }, dto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] LoanDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _service.UpdateAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }

}
