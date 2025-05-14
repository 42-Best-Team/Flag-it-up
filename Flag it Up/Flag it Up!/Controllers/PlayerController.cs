using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlagItUpApp.Controllers;

[Authorize(Roles = "Player")]
public class PlayerController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}