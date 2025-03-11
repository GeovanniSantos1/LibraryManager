using LibraryManager.Application.Models;
using LibraryManager.Core.Entities;
using LibraryManager.Core.Interfaces;
using LibraryManager.Infrastructure.Auth;
using MediatR;

namespace LibraryManager.Application.Commands.Users.InsertUser
{
    public class InsertUserHandler : IRequestHandler<InsertUserCommand, ResultViewModel<int>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthService _authService;

        public InsertUserHandler(IUserRepository userRepository, IAuthService authService)
        {
            _userRepository = userRepository;
            _authService = authService;
        }

        public async Task<ResultViewModel<int>> Handle(InsertUserCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Name))
                return ResultViewModel<int>.Error("O nome do usuário é obrigatório.");

            if (string.IsNullOrEmpty(request.Email))
                return ResultViewModel<int>.Error("O email do usuário é obrigatório.");

            var passwordHash = _authService.ComputeHash(request.Password);

            var user = new User(request.Name, request.Email, passwordHash, request.Role);
            var createdUser = await _userRepository.Add(user);

            return ResultViewModel<int>.Sucess(createdUser.Id);
        }
    }
}
