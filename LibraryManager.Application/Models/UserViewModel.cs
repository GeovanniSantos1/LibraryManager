using LibraryManager.Core.Entities;

namespace LibraryManager.Application.Models
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public static UserViewModel FromEntity(User user)
        {
            return new UserViewModel
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email
            };
        }
    }
}
