using LibraryManager.Core.Entities;
using LibraryManager.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryManager.Infrastructure.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly LibraryManagerDbContext _context;

        public UserRepository(LibraryManagerDbContext context)
        {
            _context = context;
        }

        public async Task<User> Add(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task Delete(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                user.SetAsDeletedAsync();
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> Exists(int id)
        {
            return await _context.Users
                .AnyAsync(u => u.Id == id && !u.IsDeleted);
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _context.Users
                .Where(u => !u.IsDeleted)
                .ToListAsync();
        }

        public async Task<User> GetByEmail(string email)
        {
            return await _context.Users
        .FirstOrDefaultAsync(u => u.Email == email && !u.IsDeleted);
        }

        public async Task<User> GetByEmailAndPassword(string email, string passwordHash)
        {
            return await _context.Users.SingleOrDefaultAsync(u => u.Email == email && u.Password == passwordHash);
        }

        public async Task<User> GetById(int id)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted);
        }

        public async Task<bool> HasActiveLoans(int userId)
        {
            return await _context.Loans
                .AnyAsync(l => l.UserId == userId &&
                              l.ActualReturnDate == null &&
                              !l.IsDeleted);
        }

        public async Task<User> Update(User user)
        {
            var existingUser = await _context.Users.FindAsync(user.Id);

            if (existingUser == null)
                return null;

            _context.Entry(existingUser).CurrentValues.SetValues(user);
            await _context.SaveChangesAsync();

            return existingUser;
        }
    }
}
