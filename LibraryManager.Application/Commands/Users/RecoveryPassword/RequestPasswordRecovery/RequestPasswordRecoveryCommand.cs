using LibraryManager.Application.Models;
using MediatR;

namespace LibraryManager.Application.Commands.Users.RecoveryPassword.RequestPasswordRecovery
{
    public class RequestPasswordRecoveryCommand : IRequest<ResultViewModel<bool>>
    {
        public string Email { get; set; }
    }
}
