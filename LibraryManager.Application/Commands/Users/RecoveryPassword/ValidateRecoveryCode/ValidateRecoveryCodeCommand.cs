using LibraryManager.Application.Models;
using MediatR;

namespace LibraryManager.Application.Commands.Users.RecoveryPassword.ValidateRecoveryCode
{
    public class ValidateRecoveryCodeCommand : IRequest<ResultViewModel<bool>>
    {
        public string Email { get; set; }
        public string Code { get; set; }
    }
}
