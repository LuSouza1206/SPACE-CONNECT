using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpaceConnect.Web.Services;

namespace SpaceConnect.Web.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ApiService _api;

    public HomeController(ApiService api)
    {
        _api = api;
    }

    public async Task<IActionResult> Index()
    {
        ViewData["EditorialHero"] = true;
        var dashboard = await _api.ObterDashboardAsync();
        return View(dashboard);
    }

    [AllowAnonymous]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View();
    }
}
