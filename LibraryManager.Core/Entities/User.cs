namespace LibraryManager.Core.Entities
{
    public class User : BaseEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }


        public User(string name, string email, string password, string role) : base()
        {
            Name = name;
            Email = email;
            Password = password;
            Role = role;
        }

        public void UpdatePassword(string password)
        {
            Password = password;
        }

        public override string ToString()
        {
            return $"ID: {Id} | Nome: {Name} | Email: {Email}";
        }
    }
}