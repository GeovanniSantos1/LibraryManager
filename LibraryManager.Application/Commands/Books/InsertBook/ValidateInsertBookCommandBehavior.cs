using LibraryManager.Application.Models;
using MediatR;
using System.Text.RegularExpressions;

namespace LibraryManager.Application.Commands.Books.InsertBook
{
    public class ValidateInsertBookCommandBehavior : IPipelineBehavior<InsertBookCommand, ResultViewModel<int>>
    {
        public async Task<ResultViewModel<int>> Handle(InsertBookCommand request, RequestHandlerDelegate<ResultViewModel<int>> next, CancellationToken cancellationToken)
        {
            var errors = new List<string>();

            // Validação do Título
            if (string.IsNullOrEmpty(request.Title))
                errors.Add("O título do livro é obrigatório");
            else if (request.Title.Length > 200)
                errors.Add("O título não pode ter mais que 200 caracteres");

            // Validação do ISBN
            //if (string.IsNullOrEmpty(request.ISBN))
            //    errors.Add("O ISBN é obrigatório");
            //else if (!Regex.IsMatch(request.ISBN, @"^(?=(?:\D*\d){10}(?:(?:\D*\d){3})?$)[\d-]+$"))
            //    errors.Add("ISBN inválido");

            // Validação do Autor
            if (string.IsNullOrEmpty(request.Author))
                errors.Add("O autor é obrigatório");
            else if (request.Author.Length > 200)
                errors.Add("O nome do autor não pode ter mais que 200 caracteres");

            if (errors.Any())
                return ResultViewModel<int>.Error(string.Join(", ", errors));

            return await next();
        }
    }
}
