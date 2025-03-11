using FluentValidation;
using LibraryManager.Application.Commands.Books.DeleteBook;

namespace LibraryManager.Application.Validators.Books
{
    public class DeleteBookValidator : AbstractValidator<DeleteBookCommand>
    {
        public DeleteBookValidator()
        {
            RuleFor(b => b.Id)
                .GreaterThan(0).WithMessage("O ID do livro é inválido");
        }
    }
}
