using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using FlagItUpApp.Services;
using FlagItUpApp.Models;
using FlagItUpApp.Repositories;

[TestClass]
public class UserServiceTests
{
    private Mock<IRepository<User>> _userRepoMock;
    private UserService _userService;

    [TestInitialize]
    public void Setup()
    {
        _userRepoMock = new Mock<IRepository<User>>();
        _userService = new UserService(_userRepoMock.Object);
    }

    [TestMethod]
    public void ValidateUser_ShouldReturnTrue_WhenCredentialsAreValid()
    {
        var username = "admin";
        var password = "secret";

        var hashed = Hash(password);
        var user = new User { Username = username, PasswordHash = hashed };

        _userRepoMock.Setup(r => r.Get(username)).Returns(user);

        var result = _userService.ValidateUser(username, password);

        Assert.IsTrue(result);
    }

    [TestMethod]
    public void ValidateUser_ShouldReturnFalse_WhenUserDoesNotExist()
    {
        _userRepoMock.Setup(r => r.Get("ghost")).Returns((User)null);

        var result = _userService.ValidateUser("ghost", "any");

        Assert.IsFalse(result);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void RegisterUser_ShouldThrow_WhenUserExists()
    {
        _userRepoMock.Setup(r => r.Get("admin")).Returns(new User());

        _userService.RegisterUser("admin", "password");
    }

    [TestMethod]
    public void RegisterUser_ShouldAddUser_WhenNew()
    {
        _userRepoMock.Setup(r => r.Get("newUser")).Returns((User)null);

        _userService.RegisterUser("newUser", "pass123");

        _userRepoMock.Verify(r => r.Add(It.Is<User>(u => u.Username == "newUser")), Times.Once);
        _userRepoMock.Verify(r => r.Save(), Times.Once);
    }

    // Допоміжний метод, щоб симулювати хешування (має бути такий же, як у UserService)
    private string Hash(string password)
    {
        using var sha = System.Security.Cryptography.SHA256.Create();
        var bytes = sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        return Convert.ToHexString(bytes).ToLower();
    }
}
