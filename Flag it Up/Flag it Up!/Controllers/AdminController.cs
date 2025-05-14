using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FlagItUpApp.Models;
using FlagItUpApp.Data;

namespace FlagItUpApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly DbStorage _storage;

        public AdminController(DbStorage storage)
        {
            _storage = storage;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _storage.LoadUsersAsync();
            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string username)
        {
            await _storage.DeleteUserAsync(username);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(string username)
        {
            var user = await _storage.GetUserByUsernameAsync(username);
            if (user == null)
                return NotFound();

            ViewData["SelectedRole"] = user.Role;
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(User updatedUser)
        {
            var user = await _storage.GetUserByUsernameAsync(updatedUser.Username);
            if (user == null)
                return NotFound();

            user.Role = updatedUser.Role;
            await _storage.UpdateUserAsync(user);

            return RedirectToAction("Index");
        }
    }
}
