using LibraryManager.Application.Models;
using LibraryManager.Core.Interfaces;
using MediatR;

namespace LibraryManager.Application.Commands.Books.UpdateBook
{
    public class UpdateBookHandler : IRequestHandler<UpdateBookCommand, ResultViewModel>
    {
        private readonly IBookRepository _repository;

        public UpdateBookHandler(IBookRepository repository)
        {
            _repository = repository;
        }
        public async Task<ResultViewModel> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
        {
            var book = await _repository.GetById(request.IdBook);
            if (book == null)
            {
                return (ResultViewModel<int>)ResultViewModel.Error("Livro não encontrado.");
            }

            request.UpdateEntity(book);

            var updated = await _repository.Update(book);

            if (!updated)
                return ResultViewModel.Error("Não foi possível atualizar o livro.");

            return ResultViewModel.Sucess();
        }
    }
}
