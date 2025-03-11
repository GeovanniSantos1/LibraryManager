using LibraryManager.Application.Models;
using LibraryManager.Core.Interfaces;
using LibraryManager.Infrastructure.Auth;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace LibraryManager.Application.Commands.Users.RecoveryPassword.ChangePassword
{
    public class ChangePasswordHandler : IRequestHandler<ChangePasswordCommand, ResultViewModel<bool>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMemoryCache _memoryCache;
        private readonly IAuthService _authService;

        public ChangePasswordHandler(
            IUserRepository userRepository,
            IMemoryCache memoryCache,
            IAuthService authService)
        {
            _userRepository = userRepository;
            _memoryCache = memoryCache;
            _authService = authService;
        }

        public async Task<ResultViewModel<bool>> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var cacheKey = $"RecoveryCode:{request.Email}";

            if (!_memoryCache.TryGetValue(cacheKey, out string? code) || code != request.Code)
                return ResultViewModel<bool>.Error("Código inválido ou expirado.");

            _memoryCache.Remove(cacheKey);

            var user = await _userRepository.GetByEmail(request.Email);

            if (user is null)
                return ResultViewModel<bool>.Error("Usuário não encontrado.");

            var hash = _authService.ComputeHash(request.NewPassword);
            user.UpdatePassword(hash);

            await _userRepository.Update(user);

            return ResultViewModel<bool>.Sucess(true);
        }
    }
}
