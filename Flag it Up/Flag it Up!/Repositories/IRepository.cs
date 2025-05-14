
namespace FlagItUpApp.Repositories
{
    public interface IRepository<T>
    {
        void Add(T item);
        void Update(T item);
        void Delete(string id);  // ������������� int �� ID
        T? Get(string id);  // ������������� int �� ID
        List<T> GetAll();
        void Save();
        T GetById(int id);
    }
}
