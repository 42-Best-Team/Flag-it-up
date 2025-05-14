using FlagItUpApp.Models;
using FlagItUpApp.Services;
using Microsoft.AspNetCore.Mvc;
using FlagItUpApp.Extensions;

namespace FlagItUpApp.Controllers
{
    public class GameController : Controller
    {
        private readonly GameService _gameService;

        public GameController(GameService gameService)
        {
            _gameService = gameService;
        }

        [HttpGet]
        public IActionResult Start(string mode)
        {
            if (string.IsNullOrWhiteSpace(mode))
            {
                ViewBag.Error = "Оберіть режим гри.";
                return View(); // повертає Start.cshtml
            }

            var game = _gameService.StartGame(mode.Trim());
            HttpContext.Session.Set("Game", game);
            return RedirectToAction("Question");
        }

        [HttpGet]
        public IActionResult Question()
        {
            var game = HttpContext.Session.Get<Game>("Game");
            if (game == null)
                return RedirectToAction("Start");

            var flag = _gameService.GetNextFlag(game);
            HttpContext.Session.Set("Game", game);
            return View(flag);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Answer(string answer)
        {
            var game = HttpContext.Session.Get<Game>("Game");
            if (game == null)
                return RedirectToAction("Start");

            // Перевірка на наявність чисел
            if (answer.Any(char.IsDigit))
            {
                // Якщо є числа, просто вивести помилку, не змінюючи рахунок
                TempData["Error"] = "Не можна вводити числа у відповідь!";
                return RedirectToAction("Question"); // Повертаємось на сторінку питання
            }

            // Якщо число не введено, перевіряємо відповідь
            var isCorrect = _gameService.CheckAnswer(game, answer);

            // Якщо відповідь правильна, оновлюємо рахунок
            if (isCorrect)
            {
                _gameService.UpdateScore(game, isCorrect);
            }

            HttpContext.Session.Set("Game", game);

            // Переходимо до наступного питання, якщо гра не завершена
            if (_gameService.IsGameOver(game))
            {
                return RedirectToAction("GameResults", new
                {
                    correctAnswers = game.CorrectAnswers,
                    totalQuestions = game.TotalQuestions
                });
            }

            return RedirectToAction("Question");
        }





        [HttpGet]
        public IActionResult GameResults(int correctAnswers, int totalQuestions)
        {
            var model = new Game
            {
                CorrectAnswers = correctAnswers,
                TotalQuestions = totalQuestions
            };
            return View(model);
        }
    }
}
