using LibraryManager.Application.Models;
using LibraryManager.Core.Interfaces;
using MediatR;

namespace LibraryManager.Application.Commands.Users.UpdateUser
{
    public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, ResultViewModel<int>>
    {
        private readonly IUserRepository _userRepository;

        public UpdateUserHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<ResultViewModel<int>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Name))
                return ResultViewModel<int>.Error("O nome do usuário é obrigatório.");

            if (string.IsNullOrEmpty(request.Email))
                return ResultViewModel<int>.Error("O email do usuário é obrigatório.");

            var existingUser = await _userRepository.GetById(request.Id);

            if (existingUser == null)
                return ResultViewModel<int>.Error("Usuário não encontrado.");

            existingUser.Name = request.Name;
            existingUser.Email = request.Email;

            await _userRepository.Update(existingUser);
            return ResultViewModel<int>.Sucess(existingUser.Id);
        }
    }
}
