
namespace FlagItUpApp.Repositories
{
    public interface IRepository<T>
    {
        void Add(T item);
        void Update(T item);
        void Delete(string id);  // Використовуємо int як ID
        T? Get(string id);  // Використовуємо int як ID
        List<T> GetAll();
        void Save();
        T GetById(int id);
    }
}
