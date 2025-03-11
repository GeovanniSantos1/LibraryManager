using LibraryManager.Application.Models;
using LibraryManager.Core.Interfaces;
using MediatR;

namespace LibraryManager.Application.Queries.Users.GetByIdUsers
{
    public class GetByIdUsersHandler : IRequestHandler<GetByIdUsersQuery, ResultViewModel<UserViewModel>>
    {
        private readonly IUserRepository _userRepository;

        public GetByIdUsersHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<ResultViewModel<UserViewModel>> Handle(GetByIdUsersQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetById(request.Id);

            if (user == null)
                return ResultViewModel<UserViewModel>.Error("Usuário não encontrado.");

            var userViewModel = UserViewModel.FromEntity(user);
            return ResultViewModel<UserViewModel>.Sucess(userViewModel);
        }
    }
}
