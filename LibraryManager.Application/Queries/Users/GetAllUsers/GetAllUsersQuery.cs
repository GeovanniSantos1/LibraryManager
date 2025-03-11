using LibraryManager.Application.Models;
using MediatR;

namespace LibraryManager.Application.Queries.Users.GetAllUsers
{
    public class GetAllUsersQuery : IRequest<ResultViewModel<List<UserViewModel>>>
    {
    }
}
