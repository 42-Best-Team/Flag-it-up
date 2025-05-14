using Microsoft.EntityFrameworkCore;
using FlagItUpApp.Models;
using FlagItUpApp.Repositories;
using FlagItUpApp.Data;

public class FlagRepository : IRepository<Flag>
{
    private readonly DBContext _context;

    public FlagRepository(DBContext context)
    {
        _context = context;
    }


    public void Add(Flag item)
    {
        _context.Set<Flag>().Add(item);
        Save();
    }

    public void Update(Flag item)
    {
        _context.Set<Flag>().Update(item);
        Save();
    }
    public Flag? Get(int id)
    {
        return _context.Set<Flag>().Find(id);
    }


    // Оновлений метод Delete для роботи з string id
    public void Delete(string id)
    {
        if (int.TryParse(id, out int flagId))  // Перевіряємо, чи можна перетворити id на int
        {
            var flag = Get(flagId);  // Використовуємо int для пошуку
            if (flag != null)
            {
                _context.Set<Flag>().Remove(flag);
                Save();
            }
        }
        else
        {
            throw new ArgumentException("Invalid ID format", nameof(id));  // Якщо id не можна перетворити на int
        }
    }

    public Flag? Get(string id)  // Оновлений метод Get для роботи з string id
    {
        if (int.TryParse(id, out int flagId))  // Перевіряємо, чи можна перетворити id на int
        {
            return _context.Set<Flag>().FirstOrDefault(f => f.Id == flagId);  // Використовуємо int для порівняння
        }
        return null;  // Якщо id не можна перетворити на int, повертаємо null
    }

    public List<Flag> GetAll()
    {
        return _context.Set<Flag>().ToList();
    }

    public void Save()
    {
        _context.SaveChanges();
    }

    public Flag GetById(int id)
    {
        return _context.Set<Flag>().FirstOrDefault(f => f.Id == id);
    }

    public IEnumerable<object> GetByCountry(string mode)
    {
        throw new NotImplementedException();
    }
}
