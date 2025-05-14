using FlagItUpApp.Data;
using FlagItUpApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FlagItUpApp.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly DbStorage _storage;

        public ProfileController(DbStorage storage)
        {
            _storage = storage;
        }

        public async Task<IActionResult> Index()
        {
            var username = User.Identity?.Name;
            var user = await _storage.GetUserByUsernameAsync(username); // використання асинхронного методу
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Update(string email, string phone)
        {
            var username = User.Identity?.Name;
            var user = await _storage.GetUserByUsernameAsync(username); // використання асинхронного методу
            if (user != null)
            {
                user.Email = email;
                await _storage.UpdateUserAsync(user); // використання асинхронного методу
            }
            return RedirectToAction("Index");
        }
    }
}
