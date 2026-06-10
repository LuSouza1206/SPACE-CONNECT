using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpaceConnect.Web.Models;
using SpaceConnect.Web.Services;

namespace SpaceConnect.Web.Controllers;

[Authorize]
public class TecnologiasController : Controller
{
    private readonly ApiService _api;

    public TecnologiasController(ApiService api)
    {
        _api = api;
    }

    public async Task<IActionResult> Index(int? categoriaId, int? missaoId)
    {
        var lista = await _api.ListarTecnologiasAsync();

        if (categoriaId.HasValue)
        {
            lista = lista.Where(t => t.CategoriaId == categoriaId.Value).ToList();
            ViewBag.CategoriaId = categoriaId.Value;
            ViewBag.CategoriaNome = lista.FirstOrDefault()?.CategoriaNome
                ?? (await _api.ListarCategoriasAsync()).FirstOrDefault(c => c.Id == categoriaId.Value)?.Nome;
        }

        if (missaoId.HasValue)
        {
            lista = lista.Where(t => t.MissaoId == missaoId.Value).ToList();
            ViewBag.MissaoId = missaoId.Value;
            ViewBag.MissaoNome = lista.FirstOrDefault()?.MissaoNome
                ?? (await _api.ListarMissoesAsync()).FirstOrDefault(m => m.Id == missaoId.Value)?.Nome;
        }

        return View(lista);
    }

    public async Task<IActionResult> Detalhes(int id)
    {
        var tecnologia = await _api.BuscarTecnologiaAsync(id);
        if (tecnologia is null) return NotFound();
        return View(tecnologia);
    }

    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Criar()
    {
        var model = new CriarTecnologiaViewModel
        {
            Categorias = await _api.ListarCategoriasAsync(),
            Missoes = await _api.ListarMissoesAsync()
        };
        return View(model);
    }

    [HttpPost]
    [Authorize(Roles = "Administrador")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Criar(CriarTecnologiaViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.Categorias = await _api.ListarCategoriasAsync();
            model.Missoes = await _api.ListarMissoesAsync();
            return View(model);
        }

        var usuarioId = int.TryParse(User.FindFirst("Id")?.Value, out var uid) ? uid : 0;
        var criado = await _api.CriarTecnologiaAsync(model, usuarioId);

        if (!criado)
        {
            ModelState.AddModelError("", "Não foi possível criar a tecnologia. Verifique os dados.");
            model.Categorias = await _api.ListarCategoriasAsync();
            model.Missoes = await _api.ListarMissoesAsync();
            return View(model);
        }

        TempData["Sucesso"] = "Tecnologia cadastrada com sucesso.";
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Editar(int id)
    {
        var tecnologia = await _api.BuscarTecnologiaAsync(id);
        if (tecnologia is null) return NotFound();

        var model = new CriarTecnologiaViewModel
        {
            Nome = tecnologia.Nome,
            Descricao = tecnologia.Descricao,
            AnoOrigem = tecnologia.AnoOrigem,
            AplicacaoTerra = tecnologia.AplicacaoTerra,
            CategoriaId = tecnologia.CategoriaId,
            MissaoId = tecnologia.MissaoId,
            Categorias = await _api.ListarCategoriasAsync(),
            Missoes = await _api.ListarMissoesAsync()
        };

        ViewBag.TecnologiaId = id;
        return View(model);
    }

    [HttpPost]
    [Authorize(Roles = "Administrador")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(int id, CriarTecnologiaViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.Categorias = await _api.ListarCategoriasAsync();
            model.Missoes = await _api.ListarMissoesAsync();
            ViewBag.TecnologiaId = id;
            return View(model);
        }

        var atualizado = await _api.AtualizarTecnologiaAsync(id, model);
        if (!atualizado)
        {
            ModelState.AddModelError("", "Não foi possível atualizar a tecnologia.");
            model.Categorias = await _api.ListarCategoriasAsync();
            model.Missoes = await _api.ListarMissoesAsync();
            ViewBag.TecnologiaId = id;
            return View(model);
        }

        TempData["Sucesso"] = "Tecnologia atualizada com sucesso.";
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Excluir(int id)
    {
        var tecnologia = await _api.BuscarTecnologiaAsync(id);
        if (tecnologia is null) return NotFound();
        return View(tecnologia);
    }

    [HttpPost]
    [Authorize(Roles = "Administrador")]
    [ValidateAntiForgeryToken]
    [ActionName("Excluir")]
    public async Task<IActionResult> ConfirmarExclusao(int id)
    {
        var removido = await _api.RemoverTecnologiaAsync(id);
        if (!removido)
        {
            TempData["Erro"] = "Não foi possível remover a tecnologia.";
            return RedirectToAction(nameof(Index));
        }

        TempData["Sucesso"] = "Tecnologia removida com sucesso.";
        return RedirectToAction(nameof(Index));
    }
}
