using LibraryManager.Application.Commands.Users.DeleteUser;
using LibraryManager.Application.Commands.Users.InsertUser;
using LibraryManager.Application.Commands.Users.Login;
using LibraryManager.Application.Commands.Users.RecoveryPassword.ChangePassword;
using LibraryManager.Application.Commands.Users.RecoveryPassword.RequestPasswordRecovery;
using LibraryManager.Application.Commands.Users.RecoveryPassword.ValidateRecoveryCode;
using LibraryManager.Application.Commands.Users.UpdateUser;
using LibraryManager.Application.Queries.Users.GetAllUsers;
using LibraryManager.Application.Queries.Users.GetByIdUsers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManager.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/users
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetAllUsersQuery();
            var result = await _mediator.Send(query);

            return Ok(result.Data);
        }

        // GET: api/users/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var query = new GetByIdUsersQuery(id);
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
                return NotFound(result.Message);

            return Ok(result.Data);
        }

        // POST: api/users
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Create(InsertUserCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return CreatedAtAction(nameof(GetById), new { id = result.Data }, result);
        }

        // PUT: api/users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateUserCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest("ID inválido.");
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return NoContent();
        }

        // DELETE: api/users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var command = new DeleteUserCommand(id);
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return NoContent();
        }

        [HttpPut("login")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAsync(LoginCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("password-recovery/request")]
        [AllowAnonymous]
        public async Task<IActionResult> RequestPasswordRecovery(RequestPasswordRecoveryCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return NoContent();
        }

        [HttpPost("password-recovery/validate")]
        [AllowAnonymous]
        public async Task<IActionResult> ValidateRecoveryCode(ValidateRecoveryCodeCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return NoContent();
        }

        [HttpPost("password-recovery/change")]
        [AllowAnonymous]
        public async Task<IActionResult> ChangePassword(ChangePasswordCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return NoContent();
        }
    }
}