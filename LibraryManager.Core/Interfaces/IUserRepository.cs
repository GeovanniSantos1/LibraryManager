using LibraryManager.Core.Entities;

namespace LibraryManager.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAll();
        Task<User> GetById(int id);
        Task<User> Add(User user);
        Task Delete(int id);
        Task<bool> Exists(int id);
        Task<User> Update(User user);
        Task<bool> HasActiveLoans(int userId);
        Task<User> GetByEmailAndPassword(string email, string passwordHash);
        Task<User> GetByEmail(string email);
    }
}
