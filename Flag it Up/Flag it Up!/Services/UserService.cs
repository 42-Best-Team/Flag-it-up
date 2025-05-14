using FlagItUpApp.Models;
using FlagItUpApp.Repositories;
using System.Security.Cryptography;
using System.Text;

namespace FlagItUpApp.Services
{
    public class UserService
    {
        private readonly IRepository<User> _userRepository;

        public UserService(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public bool ValidateUser(string username, string password)
        {
            var user = _userRepository.Get(username); // Отримуємо користувача за username (string)
            return user != null && VerifyPassword(password, user.PasswordHash);
        }

        public void RegisterUser(string username, string password)
        {
            if (_userRepository.Get(username) != null)
                throw new InvalidOperationException("User already exists");

            var user = new User
            {
                Username = username,
                PasswordHash = HashPassword(password),
                Role = "Player"
            };

            _userRepository.Add(user);
            _userRepository.Save();
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToHexString(bytes).ToLower(); // Або ToUpper(), головне — послідовність
        }

        private bool VerifyPassword(string password, string storedHash)
        {
            return HashPassword(password) == storedHash;
        }
    }
}
