using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpaceConnect.Web.Services;

namespace SpaceConnect.Web.Controllers;

[Authorize]
public class CategoriasController : Controller
{
    private readonly ApiService _api;

    public CategoriasController(ApiService api)
    {
        _api = api;
    }

    public async Task<IActionResult> Index()
    {
        var categorias = await _api.ListarCategoriasDetalhadasAsync();
        return View(categorias);
    }
}
