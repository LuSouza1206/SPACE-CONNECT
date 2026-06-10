using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpaceConnect.Web.Services;

namespace SpaceConnect.Web.Controllers;

[Authorize]
public class MissoesController : Controller
{
    private readonly ApiService _api;

    public MissoesController(ApiService api)
    {
        _api = api;
    }

    public async Task<IActionResult> Index()
    {
        var missoes = await _api.ListarMissoesDetalhadasAsync();
        return View(missoes);
    }
}
