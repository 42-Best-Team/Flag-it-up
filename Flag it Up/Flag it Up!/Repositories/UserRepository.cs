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
        // ѕерев≥р€Їмо, чи можна привести id до int
        if (int.TryParse(id, out int userId))
        {
            var user = Get(userId);  // ¬икористовуЇмо int дл€ пошуку користувача
            if (user != null)
            {
                _context.Set<User>().Remove(user);  // ¬идал€Їмо користувача
                Save();
            }
        }
        else
        {
            throw new ArgumentException("Invalid ID format", nameof(id));  // якщо id не можна привести до int
        }
    }

    public User? Get(int id)
    {
        return _context.Set<User>().FirstOrDefault(u => u.Id == id);  // ќтримуЇмо користувача за int ID
    }

    // ѕеревантаженн€ Get дл€ типу string
    public User? Get(string id)
    {
        if (int.TryParse(id, out int userId))  // ѕерев≥р€Їмо, чи можна привести id до int
        {
            return _context.Set<User>().FirstOrDefault(u => u.Id == userId);  // ¬икористовуЇмо int дл€ пошуку
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
