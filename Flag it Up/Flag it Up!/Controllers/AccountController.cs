using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using FlagItUpApp.Models;
using FlagItUpApp.Data;

namespace FlagItUpApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly DbStorage _storage;

        public AccountController(DbStorage storage)
        {
            _storage = storage;
        }

        public IActionResult Login() => View();
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error = "Усі поля обов'язкові для заповнення.";
                return View();
            }

            var user = await _storage.GetUserByUsernameAsync(username);
            if (user != null && VerifyPassword(password, user.PasswordHash))
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.Role)
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                return RedirectToAction("Index", user.Role == "Admin" ? "Admin" : "Home");
            }

            ViewBag.Error = "Невірні дані для входу";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string username, string password, string email, string phone)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error = "Усі поля обов'язкові для заповнення.";
                return View();
            }

            var existingUser = await _storage.GetUserByUsernameAsync(username);
            if (existingUser != null)
            {
                ViewBag.Error = "Користувач вже існує";
                return View();
            }

            if (!PasswordIsValid(password))
            {
                ViewBag.Error = "Пароль має містити хоча б одну велику літеру, одну малу літеру та бути не менше 6 символів.";
                return View();
            }

            var user = new User
            {
                Username = username,
                PasswordHash = HashPassword(password),
                Email = email,
                Role = "Player"
            };

            await _storage.SaveUserAsync(user);
            return RedirectToAction("Login");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login");
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToHexString(bytes).ToLower();
        }

        private bool VerifyPassword(string password, string storedHash) =>
            HashPassword(password) == storedHash;

        private bool PasswordIsValid(string password)
        {
            return password.Length >= 6 &&
                   password.Any(char.IsUpper) &&
                   password.Any(char.IsLower);
        }
    }
}
