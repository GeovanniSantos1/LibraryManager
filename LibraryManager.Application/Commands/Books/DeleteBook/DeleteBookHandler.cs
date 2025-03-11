using LibraryManager.Application.Models;
using LibraryManager.Core.Interfaces;
using MediatR;

namespace LibraryManager.Application.Commands.Books.DeleteBook
{
    public class DeleteBookHandler : IRequestHandler<DeleteBookCommand, ResultViewModel>
    {
        private readonly IBookRepository _repository;

        public DeleteBookHandler(IBookRepository repository)
        {
            _repository = repository;
        }
        public async Task<ResultViewModel> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
        {
            var book = await _repository.GetById(request.Id);

            if (book == null)
                return ResultViewModel.Error("Livro não encontrado.");

            book.SetAsDeletedAsync();

            var updated = await _repository.Update(book);

            if (!updated)
                return ResultViewModel.Error("Não foi possível deletar o livro.");

            return ResultViewModel.Sucess();
        }
    }
}
