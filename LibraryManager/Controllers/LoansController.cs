using LibraryManager.Application.Commands.Loans.DeleteLoan;
using LibraryManager.Application.Commands.Loans.InsertLoan;
using LibraryManager.Application.Commands.Loans.LoanReturn;
using LibraryManager.Application.Queries.Loans.GetAllLoans;
using LibraryManager.Application.Queries.Loans.GetByIdLoans;
using LibraryManager.Application.Queries.Loans.GetLoanStatus;
using LibraryManager.Core.Entities;
using LibraryManager.Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManager.API.Controllers
{
    [Route("api/loans")]
    [ApiController]
    public class LoansController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LoansController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/loans
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetAllLoansQuery();

            var result = await _mediator.Send(query);

            return Ok(result);
        }

        // GET: api/loans/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var query = new GetByIdLoansQuery(id);
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                return NotFound("Empréstimo não encontrado.");
            }

            return Ok(result.Data);
        }

        // POST: api/loans
        [HttpPost]
        public async Task<IActionResult> Create(InsertLoanCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(result.Message);
            }

            return CreatedAtAction(nameof(GetById), new { id = result.Data }, command);
        }

        // PUT: api/loans/5/return
        [HttpPut("{id}/return")]
        public async Task<IActionResult> ReturnBook(int id)
        {
            var command = new LoanReturnCommand(id);
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return NotFound("Empréstimo não encontrado.");
            }

            return Ok(result.Data);
        }

        // GET: api/loans/5/status
        [HttpGet("{id}/status")]
        public async Task<IActionResult> GetLoanStatus(int id)
        {
            var query = new GetLoanStatusQuery(id);
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                return NotFound("Empréstimo não encontrado.");
            }

            return Ok(result.Data);
        }

        // DELETE: api/loans/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var command = new DeleteLoanCommand(id);
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return NotFound("Empréstimo não encontrado.");
            }

            return NoContent();
        }
    }
}