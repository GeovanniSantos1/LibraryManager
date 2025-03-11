using LibraryManager.Application.Models;
using MediatR;

namespace LibraryManager.Application.Commands.Users.InsertUser
{
    public class InsertUserCommand : IRequest<ResultViewModel<int>>
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
