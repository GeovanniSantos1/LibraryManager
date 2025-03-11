using FluentValidation;
using LibraryManager.Application.Commands.Users.InsertUser;

namespace LibraryManager.Application.Validators.Users
{
    public class InsertUserValidator : AbstractValidator<InsertUserCommand>
    {
        public InsertUserValidator()
        {
            RuleFor(u => u.Name)
                .NotEmpty().WithMessage("O nome do usuário é obrigatório")
                .MaximumLength(100).WithMessage("O nome do usuário não pode ter mais de 100 caracteres");
            RuleFor(u => u.Email)
                .NotEmpty().WithMessage("O e-mail do usuário é obrigatório")
                .MaximumLength(100).WithMessage("O e-mail do usuário não pode ter mais de 100 caracteres")
                .EmailAddress().WithMessage("O e-mail do usuário é inválido");
        }
    }
}
