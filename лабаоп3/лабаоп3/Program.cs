using System;
using System.Collections.Generic;
using System.Linq;

// 🔹 Абстрактний клас для сутностей із унікальним ідентифікатором
abstract class BaseEntity
{
    public int Id { get; }
    protected BaseEntity(int id) => Id = id;
}

// 🔹 Інтерфейс для гри
interface IPlayable
{
    void Play();
}

// 🔹 Абстрактний клас режиму гри
abstract class GameMode : BaseEntity, IPlayable
{
    protected Dictionary<string, string> Flags = new Dictionary<string, string>();

    protected GameMode(int id) : base(id) { }

    // Додаємо метод для адміністратора, щоб він міг додавати прапори
    public void AddFlag(string country, string flag)
    {
        Flags[country] = flag;
    }

    public abstract void Play();
}

// 🔹 Конкретні режими гри
class EuropeMode : GameMode
{
    public EuropeMode(int id) : base(id)
    {
        Flags.Add("France", "French flag");
        Flags.Add("Germany", "Deutsch flag");
        Flags.Add("Italy", "Italian flag");
    }

    public override void Play()
    {
        Console.WriteLine("Режим: Європа");
        Console.WriteLine("Доступнi прапори:");
        foreach (var flag in Flags)
        {
            Console.WriteLine($"{flag.Key}: {flag.Value}");
        }
    }
}

class AsiaMode : GameMode
{
    public AsiaMode(int id) : base(id)
    {
        Flags.Add("Japan", "Jopan flag");
        Flags.Add("China", "Chinaa flag");
        Flags.Add("India", "Indida flag");
    }

    public override void Play()
    {
        Console.WriteLine("Режим: Азiя");
        Console.WriteLine("Доступнi прапори:");
        foreach (var flag in Flags)
        {
            Console.WriteLine($"{flag.Key}: {flag.Value}");
        }
    }
}

// 🔹 Клас "Користувач"
class Player : BaseEntity
{
    public string Name { get; }

    public Player(int id, string name) : base(id)
    {
        Name = name;
    }
}

// 🔹 Клас "Адміністратор"
class Admin : Player
{
    public Admin(int id, string name) : base(id, name) { }

    // Використовуємо публічний метод AddFlag у GameMode
    public void AddFlag(GameMode mode, string country, string flag)
    {
        mode.AddFlag(country, flag);
        Console.WriteLine("Адмiнiстратор {Name} додав прапор {country}: {flag}");
    }
}

// 🔹 Інтерфейс для репозиторію
public interface IRepository<T>
{
    void Add(T entity);
    void Remove(T entity);
    T GetById(int id);
    List<T> GetAll();
}

// 🔹 Реалізація репозиторію
class Repository<T> : IRepository<T> where T : BaseEntity
{
    private List<T> items = new List<T>();

    public void Add(T item) => items.Add(item);

    public void Remove(T item) => items.Remove(item);

    public T GetById(int id) => items.FirstOrDefault(x => x.Id == id);

    public List<T> GetAll() => items.ToList();

    // Додамо LINQ для фільтрації
    public List<T> GetByCondition(Func<T, bool> condition) =>
        items.Where(condition).ToList();

    // Додамо LINQ для сортування
    public List<T> GetSorted(Func<T, object> keySelector) =>
        items.OrderBy(keySelector).ToList();
}

// 🔹 Основний клас для демонстрації роботи
class Program
{
    static void Main()
    {
        Console.WriteLine("Вiтаємо у грi 'Flag it up!'");
        Console.WriteLine("Оберiть режим гри: 1 - Європа, 2 - Азiя");

        string choice = Console.ReadLine();
        GameMode mode;

        if (choice == "1")
            mode = new EuropeMode(1);
        else if (choice == "2")
            mode = new AsiaMode(2);
        else
        {
            Console.WriteLine("Невiрний вибiр! Автоматично обрано режим Європи.");
            mode = new EuropeMode(1);
        }

        mode.Play();

        // Додамо адміністратора для перевірки
        Admin admin = new Admin(999, "SuperAdmin");
        admin.AddFlag(mode, "Ukraine", "Zhivchik flag");

        Console.WriteLine("\nОновлений список прапорiв:");
        mode.Play();

        // Репозиторій для гравців
        Repository<Player> playerRepo = new Repository<Player>();
        playerRepo.Add(new Player(1, "John"));
        playerRepo.Add(new Player(2, "Jane"));
        playerRepo.Add(new Player(3, "Jack"));
        // Відображення всіх гравців
        Console.WriteLine("\nСписок гравцiв:");
        var allPlayers = playerRepo.GetAll();
        foreach (var player in allPlayers)
        {
            Console.WriteLine("ID: {player.Id}, Name: {player.Name}");
        }

        // Пошук за умовою через LINQ
        Console.WriteLine("\nГравцi, iмена яких починаються на 'J':");
        var filteredPlayers = playerRepo.GetByCondition(p => p.Name.StartsWith("J"));
        foreach (var player in filteredPlayers)
        {
            Console.WriteLine($"ID: {player.Id}, Name: {player.Name}");
        }

        // Перевірка операторів is та as
        object obj = new Player(10, "Max");
        if (obj is Player playerObj)
        {
            Console.WriteLine("\nГравець через 'is': {playerObj.Name}");
        }

        Player castedPlayer = obj as Player;
        if (castedPlayer != null)
        {
            Console.WriteLine($"Гравець через 'as': {castedPlayer.Name}");
        }

        // Додавання нового гравця
        playerRepo.Add(new Player(4, "Michael"));
        Console.WriteLine("\nДодано нового гравця");

        // Видалення гравця
        var playerToRemove = playerRepo.GetById(2);
        if (playerToRemove != null)
        {
            playerRepo.Remove(playerToRemove);
            Console.WriteLine("Видалено гравця з ID 2");
        }

        // Відображення оновленого списку
        Console.WriteLine("\nОновлений список гравцiв:");
        foreach (var player in playerRepo.GetAll())
        {
            Console.WriteLine($"ID: {player.Id}, Name: {player.Name}");
        }

        // Відсортуємо гравців за ім'ям
        var sortedPlayers = playerRepo.GetSorted(p => p.Name);
        Console.WriteLine("\nВiдсортованi гравцi:");
        foreach (var player in sortedPlayers)
        {
            Console.WriteLine($"ID: {player.Id}, Name: {player.Name}");
        }
    }
}