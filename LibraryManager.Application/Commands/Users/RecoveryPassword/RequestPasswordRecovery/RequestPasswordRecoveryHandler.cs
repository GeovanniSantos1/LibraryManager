using LibraryManager.Application.Models;
using LibraryManager.Core.Interfaces;
using LibraryManager.Infrastructure.Notifications;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace LibraryManager.Application.Commands.Users.RecoveryPassword.RequestPasswordRecovery
{
    public class RequestPasswordRecoveryHandler : IRequestHandler<RequestPasswordRecoveryCommand, ResultViewModel<bool>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMemoryCache _memoryCache;
        private readonly IEmailService _emailService;

        public RequestPasswordRecoveryHandler(
            IUserRepository userRepository,
            IMemoryCache memoryCache,
            IEmailService emailService)
        {
            _userRepository = userRepository;
            _memoryCache = memoryCache;
            _emailService = emailService;
        }

        public async Task<ResultViewModel<bool>> Handle(RequestPasswordRecoveryCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmail(request.Email);

            if (user is null)
                return ResultViewModel<bool>.Error("Usuário não encontrado.");

            var code = new Random().Next(100000, 999999).ToString();
            var cacheKey = $"RecoveryCode:{request.Email}";

            _memoryCache.Set(cacheKey, code, TimeSpan.FromMinutes(10));

            await _emailService.SendAsync(
                user.Email,
                "Código de recuperação de senha",
                $"Seu código de recuperação é: {code}");

            return ResultViewModel<bool>.Sucess(true);
        }
    }
}
