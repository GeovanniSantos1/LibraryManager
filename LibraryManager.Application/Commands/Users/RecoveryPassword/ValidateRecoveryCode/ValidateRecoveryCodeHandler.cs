using LibraryManager.Application.Models;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace LibraryManager.Application.Commands.Users.RecoveryPassword.ValidateRecoveryCode
{
    public class ValidateRecoveryCodeHandler : IRequestHandler<ValidateRecoveryCodeCommand, ResultViewModel<bool>>
    {
        private readonly IMemoryCache _memoryCache;

        public ValidateRecoveryCodeHandler(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public Task<ResultViewModel<bool>> Handle(ValidateRecoveryCodeCommand request, CancellationToken cancellationToken)
        {
            var cacheKey = $"RecoveryCode:{request.Email}";

            if (!_memoryCache.TryGetValue(cacheKey, out string? code) || code != request.Code)
                return Task.FromResult(ResultViewModel<bool>.Error("Código inválido ou expirado."));

            return Task.FromResult(ResultViewModel<bool>.Sucess(true));
        }
    }
}
