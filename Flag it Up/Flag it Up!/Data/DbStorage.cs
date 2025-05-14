using FlagItUpApp.Data;
using FlagItUpApp.Models;
using Microsoft.EntityFrameworkCore;

namespace FlagItUpApp.Data
{
    public class DbStorage
    {
        private readonly DBContext _context;

        public DbStorage(DBContext context)
        {
            _context = context;
        }

        // Завантажити всіх користувачів з БД
        public async Task<List<User>> LoadUsersAsync() =>
            await _context.Users.ToListAsync();

        // Зберегти користувачів в БД
        public async Task SaveUserAsync(User user)
        {
            await _context.Users.AddAsync(user);  // Додаємо користувача
            await _context.SaveChangesAsync();  // Зберігаємо зміни в базі даних
        }

        // Отримати користувача за його username
        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        // Оновити дані користувача
        public async Task UpdateUserAsync(User user)
        {
            _context.Users.Update(user);  // Оновлюємо користувача
            await _context.SaveChangesAsync();  // Зберігаємо зміни
        }

        // Додатково: Видалити користувача за username
        public async Task DeleteUserAsync(string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user != null)
            {
                _context.Users.Remove(user); // Видалити користувача
                await _context.SaveChangesAsync(); // Зберегти зміни
            }
        }
    }
}
