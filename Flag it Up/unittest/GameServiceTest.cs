using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using FlagItUpApp.Models;
using FlagItUpApp.Services;
using FlagItUpApp.Repositories;

namespace FlagItUpApp.Tests
{
    [TestClass]
    public class GameServiceTests
    {
        private Mock<IRepository<Flag>> _mockRepo;
        private GameService _service;

        [TestInitialize]
        public void Setup()
        {
            _mockRepo = new Mock<IRepository<Flag>>();
            _mockRepo.Setup(r => r.GetAll()).Returns(new List<Flag>
            {
                new Flag { Id = 1, Country = "Ukraine" },
                new Flag { Id = 2, Country = "Poland" },
                new Flag { Id = 3, Country = "France" },
            });

            _mockRepo.Setup(r => r.GetById(It.IsAny<int>())).Returns<int>(id =>
                _mockRepo.Object.GetAll().FirstOrDefault(f => f.Id == id));

            _service = new GameService(_mockRepo.Object);
        }

        [TestMethod]
        public void StartGame_ShouldInitializeGameCorrectly()
        {
            var game = _service.StartGame("hard");

            Assert.IsNotNull(game);
            Assert.AreEqual("hard", game.Mode);
            Assert.AreEqual(10, game.TotalQuestions);
            Assert.AreEqual(0, game.CorrectAnswers);
            Assert.IsTrue(game.UsedFlagIds.Count == 0);
            Assert.IsTrue(game.CurrentFlagId != 0);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void StartGame_ShouldThrowIfNoFlags()
        {
            _mockRepo.Setup(r => r.GetAll()).Returns(new List<Flag>());

            var service = new GameService(_mockRepo.Object);
            service.StartGame("easy");
        }

        [TestMethod]
        public void CheckAnswer_ShouldReturnTrue_WhenCorrect()
        {
            var game = _service.StartGame("easy");
            var flag = _mockRepo.Object.GetById(game.CurrentFlagId);

            var result = _service.CheckAnswer(game, flag.Country);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CheckAnswer_ShouldReturnFalse_WhenIncorrect()
        {
            var game = _service.StartGame("easy");

            var result = _service.CheckAnswer(game, "WrongCountry");

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void UpdateScore_ShouldIncreaseCorrectAnswers_IfCorrect()
        {
            var game = _service.StartGame("easy");
            _service.UpdateScore(game, true);

            Assert.AreEqual(1, game.CorrectAnswers);
        }

        [TestMethod]
        public void UpdateScore_ShouldNotIncreaseCorrectAnswers_IfIncorrect()
        {
            var game = _service.StartGame("easy");
            _service.UpdateScore(game, false);

            Assert.AreEqual(0, game.CorrectAnswers);
        }

        [TestMethod]
        public void GetNextFlag_ShouldReturnNewFlag_AndAddToUsed()
        {
            var game = _service.StartGame("easy");

            var firstUsedId = game.CurrentFlagId;
            game.UsedFlagIds.Add(firstUsedId);

            var nextFlag = _service.GetNextFlag(game);

            Assert.IsNotNull(nextFlag);
            Assert.AreNotEqual(firstUsedId, nextFlag.Id);
            Assert.IsTrue(game.UsedFlagIds.Contains(nextFlag.Id));
        }

        [TestMethod]
        public void GetNextFlag_ShouldReturnNull_IfNoFlagsLeft()
        {
            var game = _service.StartGame("easy");
            var allFlags = _mockRepo.Object.GetAll();

            // Вручну використаємо всі прапори
            foreach (var flag in allFlags)
                game.UsedFlagIds.Add(flag.Id);

            var nextFlag = _service.GetNextFlag(game);

            Assert.IsNull(nextFlag);
        }

        [TestMethod]
        public void IsGameOver_ShouldReturnTrue_WhenEnoughFlagsUsed()
        {
            var game = _service.StartGame("easy");
            for (int i = 0; i < 10; i++)
                game.UsedFlagIds.Add(i);

            var result = _service.IsGameOver(game);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsGameOver_ShouldReturnFalse_IfLessFlagsUsed()
        {
            var game = _service.StartGame("easy");
            for (int i = 0; i < 5; i++)
                game.UsedFlagIds.Add(i);

            var result = _service.IsGameOver(game);

            Assert.IsFalse(result);
        }
    }
}
