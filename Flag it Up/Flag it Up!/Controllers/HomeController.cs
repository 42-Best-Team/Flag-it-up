using Microsoft.AspNetCore.Mvc;
using FlagItUpApp.Models;
using FlagItUpApp.Data;

namespace FlagItUpApp.Controllers;

public class HomeController : Controller
{
    private readonly DbStorage _storage;

    public HomeController(DbStorage storage) => _storage = storage;

}
