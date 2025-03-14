using System;
using System.Collections.Generic;
using System.Linq;

public interface IRepository<T> where T : BaseEntity
{
    void Add(T entity);
    void Remove(T entity);
    T GetById(string id);
    List<T> GetAll();
}

public class Repository<T> : IRepository<T> where T : BaseEntity
{
    private List<T> items = new List<T>();
    private Dictionary<string, string> flags = new Dictionary<string, string>();

    // Окремі списки для гравців та адміністраторів
    private List<Player> players = new List<Player>();
    private List<Admin> admins = new List<Admin>();

    public void Add(T entity)
    {
        if (!items.Any(x => x.Id == entity.Id))
        {
            items.Add(entity);

            if (entity is Admin admin)
                admins.Add(admin);
            else if (entity is Player player)
                players.Add(player);
        }
    }

    public void Remove(T entity)
    {
        items.Remove(entity);

        if (entity is Admin admin)
            admins.Remove(admin);
        else if (entity is Player player)
            players.Remove(player);
    }

    public T GetById(string id)
    {
        return items.FirstOrDefault(x => x.Id == id);
    }

    public List<T> GetAll()
    {
        return items.ToList();
    }

    // 🔥 Повертаємо окремий список гравців
    public List<Player> GetAllPlayers()
    {
        return players.ToList();
    }

    // 🔥 Повертаємо окремий список адміністраторів
    public List<Admin> GetAllAdmins()
    {
        return admins.ToList();
    }

    // 🔥 Методи для роботи з прапорами
    public void AddFlag(string country, string flag)
    {
        if (!flags.ContainsKey(country))
            flags[country] = flag;
    }

    public void EditFlag(string country, string flag)
    {
        if (flags.ContainsKey(country))
            flags[country] = flag;
    }

    public void RemoveFlag(string country)
    {
        if (flags.ContainsKey(country))
            flags.Remove(country);
    }

    public Dictionary<string, string> GetAllFlags()
    {
        return new Dictionary<string, string>(flags);
    }
}
