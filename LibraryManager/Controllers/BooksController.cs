using LibraryManager.Application.Commands.Books.DeleteBook;
using LibraryManager.Application.Commands.Books.InsertBook;
using LibraryManager.Application.Commands.Books.UpdateBook;
using LibraryManager.Application.Queries.Books.GetAllBooks;
using LibraryManager.Application.Queries.Books.GetBookById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManager.API.Controllers
{
    [Route("api/books")]
    [ApiController]
    [Authorize]
    public class BooksController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BooksController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize(Roles = "Adm, Client")]
        public async Task<IActionResult> Get(string seach = "", int page = 0, int size = 3)
        {
            var query = new GetAllBooksQuery();
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetBookByIdQuery(id));

            if (!result.IsSuccess)
                return NotFound("Livro não encontrado.");

            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> Post(InsertBookCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(result.Message);
            }

            return CreatedAtAction(nameof(GetById), new { id = result.Data }, command);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, UpdateBookCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return NotFound("Livro não encontrado.");

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteBookCommand(id));

            if (!result.IsSuccess)
                return NotFound("Livro não encontrado.");

            return NoContent();
        }
    }
}