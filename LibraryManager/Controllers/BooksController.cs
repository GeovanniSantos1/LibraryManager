using LibraryManager.Application.Commands.Books.DeleteBook;
using LibraryManager.Application.Commands.Books.InsertBook;
using LibraryManager.Application.Commands.Books.UpdateBook;
using LibraryManager.Application.Queries.Books.GetAllBooks;
using LibraryManager.Application.Queries.Books.GetBookById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;

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
        [Consumes("multipart/form-data")]
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> Post([FromForm] InsertBookCommand command)
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

        [HttpPost("upload-test")]
        [AllowAnonymous]
        public async Task<IActionResult> TestUpload([FromForm] IFormFile file, [FromServices] IStorageService storageService)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return BadRequest("Nenhum arquivo enviado");

                var fileName = $"teste-{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                var imageUrl = await storageService.UploadImageAsync(file, fileName);
                
                return Ok(new { 
                    message = "Upload realizado com sucesso!",
                    imageUrl = imageUrl 
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { 
                    error = "Erro ao fazer upload", 
                    details = ex.Message 
                });
            }
        }
    }
}