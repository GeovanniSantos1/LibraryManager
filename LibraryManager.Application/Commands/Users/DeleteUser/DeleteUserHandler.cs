using LibraryManager.Application.Models;
using LibraryManager.Core.Interfaces;
using MediatR;

namespace LibraryManager.Application.Commands.Users.DeleteUser
{
    public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, ResultViewModel<int>>
    {
        private readonly IUserRepository _userRepository;

        public DeleteUserHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<ResultViewModel<int>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetById(request.Id);
            if (user == null)
            {
                return ResultViewModel<int>.Error("Usuário não encontrado.");
            }

            var hasActiveLoans = await _userRepository.HasActiveLoans(user.Id);

            if (hasActiveLoans)
            {
                return ResultViewModel<int>.Error("Não é possível excluir um usuário com empréstimos ativos.");
            }

            user.SetAsDeletedAsync();

            var updated = await _userRepository.Update(user);

            if (updated == null)
                return ResultViewModel<int>.Error("Não foi possível deletar o usuário.");

            return ResultViewModel<int>.Sucess(user.Id); 
        }
    }
}
