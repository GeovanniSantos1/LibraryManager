using FluentValidation;
using LibraryManager.Application.Commands.Users.DeleteUser;

namespace LibraryManager.Application.Validators.Users
{
    public class DeleteUserValidator : AbstractValidator<DeleteUserCommand>
    {
        public DeleteUserValidator()
        {
            RuleFor(u => u.Id)
                .GreaterThan(0)
                .WithMessage("O ID do usuário é inválido");
        }
    }
}
