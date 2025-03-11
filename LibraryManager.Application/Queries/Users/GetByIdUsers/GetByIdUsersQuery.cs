using LibraryManager.Application.Models;
using MediatR;

namespace LibraryManager.Application.Queries.Users.GetByIdUsers
{
    public class GetByIdUsersQuery : IRequest<ResultViewModel<UserViewModel>>
    {
        public GetByIdUsersQuery(int id)
        {
            Id = id;
        }

        public int Id { get; set; }
    }
}
