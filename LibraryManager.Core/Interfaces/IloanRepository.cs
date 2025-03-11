using LibraryManager.Core.Entities;

namespace LibraryManager.Core.Interfaces
{
    public interface IloanRepository
    {
        Task<IEnumerable<Loan>> GetAll();
        Task<Loan> GetById(int id);
        Task<Loan> Add(Loan loan);
        Task Delete(int id);
        Task<bool> Exists(int id);
        Task<Loan> ReturnBook(int id);
        Task<Loan> Update(Loan loan);
    }
}
