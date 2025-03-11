using LibraryManager.Application.Models;
using LibraryManager.Core.Interfaces;
using LibraryManager.Infrastructure.Auth;
using MediatR;

namespace LibraryManager.Application.Commands.Users.Login
{
    public class LoginHandler : IRequestHandler<LoginCommand, ResultViewModel<LoginViewModel>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthService _authService;

        public LoginHandler(IUserRepository userRepository, IAuthService authService)
        {
            _userRepository = userRepository;
            _authService = authService;
        }

        public async Task<ResultViewModel<LoginViewModel>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var hash = _authService.ComputeHash(request.Password);

            var user = await _userRepository.GetByEmailAndPassword(request.Email, hash);

            if (user is null)
                return ResultViewModel<LoginViewModel>.Error("Erro de login");

            var token = _authService.GenerateToken(user.Email, user.Role);

            var viewModel = new LoginViewModel(token);

            return ResultViewModel<LoginViewModel>.Sucess(viewModel);
        }
    }
}
