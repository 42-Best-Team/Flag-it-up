using FlagItUpApp.Models;
using FlagItUpApp.Repositories;

namespace FlagItUpApp.Services
{
    public class FlagService
    {
        private readonly IRepository<Flag> _flagRepository;

        public FlagService(IRepository<Flag> flagRepository)
        {
            _flagRepository = flagRepository;
        }

        public void AddFlag(Flag flag)
        {
            _flagRepository.Add(flag);
        }

        public void DeleteFlag(int id)
        {
            _flagRepository.Delete(id.ToString());
        }

        public List<Flag> GetAllFlags()
        {
            return _flagRepository.GetAll();
        }

        public Flag? GetFlagById(int id)
        {
            return _flagRepository.Get(id.ToString());
        }
    }
}
