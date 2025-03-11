using LibraryManager.Application.Models;
using MediatR;

namespace LibraryManager.Application.Commands.Users.UpdateUser
{
    public class UpdateUserCommand : IRequest<ResultViewModel<int>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public UpdateUserCommand(int id, string name, string email)
        {
            Id = id;
            Name = name;
            Email = email;
        }
    }
}
