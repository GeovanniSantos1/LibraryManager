using LibraryManager.Application.Commands.Reviews.CreateReview;
using LibraryManager.Application.Queries.Reviews.GetBookReviews;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManager.API.Controllers
{
    [Route("api/reviews")]
    [ApiController]
    [Authorize]
    public class BookReviewsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BookReviewsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> CreateReview([FromBody] CreateReviewCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(result);

            return CreatedAtAction(nameof(GetByBookId), new { bookId = command.BookId }, result.Data);
        }

        [HttpGet("book/{bookId}")]
        public async Task<IActionResult> GetByBookId(string bookId)
        {
            var query = new GetBookReviewsQuery(bookId);
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
                return NotFound(result);

            return Ok(result.Data);
        }
    }
} 