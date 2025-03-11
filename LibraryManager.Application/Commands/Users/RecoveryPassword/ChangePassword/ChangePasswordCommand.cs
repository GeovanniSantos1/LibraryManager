using LibraryManager.Application.Models;
using MediatR;

namespace LibraryManager.Application.Commands.Users.RecoveryPassword.ChangePassword
{
    public class ChangePasswordCommand : IRequest<ResultViewModel<bool>>
    {
        public string Email { get; set; }
        public string Code { get; set; }
        public string NewPassword { get; set; }
    }
}
