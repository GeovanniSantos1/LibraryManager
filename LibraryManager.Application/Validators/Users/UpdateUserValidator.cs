using FluentValidation;
using LibraryManager.Application.Commands.Users.UpdateUser;

namespace LibraryManager.Application.Validators.Users
{
    public class UpdateUserValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserValidator()
        {
            RuleFor(u => u.Id)
                .GreaterThan(0)
                .WithMessage("O ID do usuário é inválido");
            RuleFor(u => u.Name)
                .NotEmpty()
                .WithMessage("O nome do usuário é obrigatório");
            RuleFor(u => u.Email)
                .NotEmpty()
                .WithMessage("O e-mail do usuário é obrigatório")
                .EmailAddress()
                .WithMessage("O e-mail do usuário é inválido");
        }
    }
}
