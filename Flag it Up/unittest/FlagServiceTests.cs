using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using FlagItUpApp.Models;
using FlagItUpApp.Repositories;
using FlagItUpApp.Services;

namespace FlagItUpApp.Tests
{
    [TestClass]
    public class FlagServiceTests
    {
        private Mock<IRepository<Flag>> _mockRepo;
        private FlagService _service;

        [TestInitialize]
        public void Setup()
        {
            _mockRepo = new Mock<IRepository<Flag>>();
            _service = new FlagService(_mockRepo.Object);
        }

        [TestMethod]
        public void AddFlag_ShouldCallRepositoryAdd()
        {
            var flag = new Flag { Id = 1, Country = "Ukraine", ImagePath = "ukraine.png" };

            _service.AddFlag(flag);

            _mockRepo.Verify(r => r.Add(flag), Times.Once);
        }

        [TestMethod]
        public void DeleteFlag_ShouldCallRepositoryDelete_WithCorrectId()
        {
            int id = 5;

            _service.DeleteFlag(id);

            _mockRepo.Verify(r => r.Delete("5"), Times.Once);
        }

        [TestMethod]
        public void GetAllFlags_ShouldReturnAllFlags()
        {
            var flags = new List<Flag>
            {
                new Flag { Id = 1, Country = "Ukraine" },
                new Flag { Id = 2, Country = "Poland" }
            };

            _mockRepo.Setup(r => r.GetAll()).Returns(flags);

            var result = _service.GetAllFlags();

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("Ukraine", result[0].Country);
            Assert.AreEqual("Poland", result[1].Country);
        }

        [TestMethod]
        public void GetFlagById_ShouldReturnCorrectFlag()
        {
            var flag = new Flag { Id = 3, Country = "France" };
            _mockRepo.Setup(r => r.Get("3")).Returns(flag);

            var result = _service.GetFlagById(3);

            Assert.IsNotNull(result);
            Assert.AreEqual("France", result.Country);
        }

        [TestMethod]
        public void GetFlagById_ShouldReturnNull_IfNotFound()
        {
            _mockRepo.Setup(r => r.Get("99")).Returns((Flag)null);

            var result = _service.GetFlagById(99);

            Assert.IsNull(result);
        }
    }
}
