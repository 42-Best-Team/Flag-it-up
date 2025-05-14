using FlagItUpApp.Data;
using FlagItUpApp.Models;
using FlagItUpApp.Repositories;

public class UserRepository : IRepository<User>
{
    private readonly DBContext _context;

    public UserRepository(DBContext context)
    {
        _context = context;
    }


    public void Add(User item)
    {
        _context.Set<User>().Add(item);
        Save();
    }

    public void Update(User item)
    {
        _context.Set<User>().Update(item);
        Save();
    }

    public void Delete(string id)
    {
        // ����������, �� ����� �������� id �� int
        if (int.TryParse(id, out int userId))
        {
            var user = Get(userId);  // ������������� int ��� ������ �����������
            if (user != null)
            {
                _context.Set<User>().Remove(user);  // ��������� �����������
                Save();
            }
        }
        else
        {
            throw new ArgumentException("Invalid ID format", nameof(id));  // ���� id �� ����� �������� �� int
        }
    }

    public User? Get(int id)
    {
        return _context.Set<User>().FirstOrDefault(u => u.Id == id);  // �������� ����������� �� int ID
    }

    // �������������� Get ��� ���� string
    public User? Get(string id)
    {
        if (int.TryParse(id, out int userId))  // ����������, �� ����� �������� id �� int
        {
            return _context.Set<User>().FirstOrDefault(u => u.Id == userId);  // ������������� int ��� ������
        }
        return null;
    }

    public List<User> GetAll()
    {
        return _context.Set<User>().ToList();
    }

    public void Save()
    {
        _context.SaveChanges();
    }

    public User GetById(int id)
    {
        return _context.Set<User>().FirstOrDefault(u => u.Id == id);
    }

    public IEnumerable<object> GetByCountry(string mode)
    {
        throw new NotImplementedException();
    }
}
