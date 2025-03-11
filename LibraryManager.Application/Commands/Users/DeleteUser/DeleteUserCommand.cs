using LibraryManager.Application.Models;
using MediatR;

namespace LibraryManager.Application.Commands.Users.DeleteUser
{
    public class DeleteUserCommand : IRequest<ResultViewModel<int>>
    {
        public int Id { get; set; }

        public DeleteUserCommand(int id)
        {
            Id = id;
        }
    }
}
