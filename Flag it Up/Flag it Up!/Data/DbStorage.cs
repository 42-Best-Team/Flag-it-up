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

        // ����������� ��� ������������ � ��
        public async Task<List<User>> LoadUsersAsync() =>
            await _context.Users.ToListAsync();

        // �������� ������������ � ��
        public async Task SaveUserAsync(User user)
        {
            await _context.Users.AddAsync(user);  // ������ �����������
            await _context.SaveChangesAsync();  // �������� ���� � ��� �����
        }

        // �������� ����������� �� ���� username
        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        // ������� ��� �����������
        public async Task UpdateUserAsync(User user)
        {
            _context.Users.Update(user);  // ��������� �����������
            await _context.SaveChangesAsync();  // �������� ����
        }

        // ���������: �������� ����������� �� username
        public async Task DeleteUserAsync(string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user != null)
            {
                _context.Users.Remove(user); // �������� �����������
                await _context.SaveChangesAsync(); // �������� ����
            }
        }
    }
}
