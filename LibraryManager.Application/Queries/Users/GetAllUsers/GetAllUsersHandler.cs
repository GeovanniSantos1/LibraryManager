using LibraryManager.Application.Models;
using LibraryManager.Core.Interfaces;
using MediatR;

namespace LibraryManager.Application.Queries.Users.GetAllUsers
{
    public class GetAllUsersHandler : IRequestHandler<GetAllUsersQuery, ResultViewModel<List<UserViewModel>>>
    {
        private readonly IUserRepository _userRepository;

        public GetAllUsersHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<ResultViewModel<List<UserViewModel>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _userRepository.GetAll();

            var userViewModel = users.Select(UserViewModel.FromEntity).ToList();

            return ResultViewModel<List<UserViewModel>>.Sucess(userViewModel);
        }
    }
}
