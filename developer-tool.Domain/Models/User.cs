namespace Domain.Models
{
    public class User 
    {
        public string Username { get; private set; }
        public string Password { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string FullName { get; private set; }

        public static User Create(string username, string password, string firstName, string lastName) 
        {
            return new User {
                Username = username,
                Password = password,
                FirstName = firstName,
                LastName = lastName
            };
        }
    }
}