using LibraryManager.Application.Models;

namespace LibraryManager.Application.Services
{
    public interface IBookService
    {
        ResultViewModel<List<BookItemViewModel>> GetAll(string searc = "");
        ResultViewModel<BookItemViewModel> GetById(int id);
        ResultViewModel<int> Insert(CreateBookInputModel model);
        ResultViewModel Update(UpdateBookInputModel model);
        ResultViewModel Delete(int id);
    }
}
