using FluentValidation;
using LibraryManager.Application.Commands.Books.UpdateBook;

namespace LibraryManager.Application.Validators.Books
{
    public class UpdateBookValidator : AbstractValidator<UpdateBookCommand>
    {
        public UpdateBookValidator()
        {
            RuleFor(b => b.IdBook)
                .GreaterThan(0).WithMessage("O ID do livro é inválido");

            RuleFor(b => b.Title)
                .NotEmpty().WithMessage("O título do livro é obrigatório")
                .MaximumLength(100).WithMessage("O título do livro deve ter no máximo 100 caracteres");

            RuleFor(b => b.Author)
                .NotEmpty().WithMessage("O autor do livro é obrigatório")
                .MaximumLength(100).WithMessage("O autor do livro deve ter no máximo 100 caracteres");

            RuleFor(b => b.ISBN)
                .NotEmpty().WithMessage("O ISBN do livro é obrigatório")
                .MaximumLength(13).WithMessage("O ISBN do livro deve ter no máximo 13 caracteres")
                .Matches(@"^\d{10}(\d{3})?$").WithMessage("O ISBN deve conter 10 ou 13 dígitos numéricos");

            RuleFor(b => b.PublicationYear)
                .NotEmpty().WithMessage("O ano de publicação é obrigatório")
                .GreaterThan(0).WithMessage("O ano de publicação deve ser maior que zero")
                .LessThanOrEqualTo(DateTime.Now.Year).WithMessage("O ano de publicação não pode ser maior que o ano atual");
        }
    }
}
