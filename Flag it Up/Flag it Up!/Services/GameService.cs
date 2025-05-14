using FlagItUpApp.Repositories;
using FlagItUpApp.Models;
using FlagItUpApp.Data;
using FlagItUpApp.Controllers;
using FlagItUpApp.Extensions;
using FlagItUpApp.Services;


using FlagItUpApp.Models;
using FlagItUpApp.Repositories;

namespace FlagItUpApp.Services
{
    public class GameService
    {
        private readonly IRepository<Flag> _flagRepo;

        public GameService(IRepository<Flag> flagRepo)
        {
            _flagRepo = flagRepo;
        }

        public Game StartGame(string mode)
        {
            var flags = _flagRepo.GetAll();

            if (flags == null || !flags.Any())
                throw new InvalidOperationException("No flags found.");

            return new Game
            {
                Mode = mode,
                TotalQuestions = 10,
                CorrectAnswers = 0,
                UsedFlagIds = new List<int>(),
                CurrentFlagId = flags.First().Id
            };
        }

        public bool CheckAnswer(Game game, string answer)
        {
            var flag = _flagRepo.GetById(game.CurrentFlagId);
            return flag != null && flag.Country.Equals(answer, StringComparison.OrdinalIgnoreCase);
        }

        public void UpdateScore(Game game, bool isCorrect)
        {
            if (isCorrect)
            {
                game.CorrectAnswers++;
            }
        }

        public Flag GetNextFlag(Game game)
        {
            var flags = _flagRepo.GetAll()
                .Where(f => !game.UsedFlagIds.Contains(f.Id))
                .ToList();

            if (!flags.Any()) return null;

            var next = flags[new Random().Next(flags.Count)];
            game.CurrentFlagId = next.Id;
            game.UsedFlagIds.Add(next.Id);

            return next;
        }

        public bool IsGameOver(Game game)
        {
            return game.UsedFlagIds.Count >= game.TotalQuestions;
        }
    }
}

