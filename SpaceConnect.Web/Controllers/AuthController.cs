using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using SpaceConnect.Web.Models;
using SpaceConnect.Web.Services;

namespace SpaceConnect.Web.Controllers;

public class AuthController : Controller
{
    private readonly ApiService _api;

    public AuthController(ApiService api)
    {
        _api = api;
    }

    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        if (User.Identity?.IsAuthenticated == true) return RedirectToAction("Index", "Home");
        ViewBag.ReturnUrl = returnUrl;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
    {
        if (!ModelState.IsValid) return View(model);

        var resultado = await _api.LoginAsync(model.Email, model.Senha);

        if (resultado is null || !resultado.Sucesso)
        {
            ModelState.AddModelError("", resultado?.Mensagem ?? "Credenciais inválidas.");
            return View(model);
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, resultado.Id.ToString()),
            new(ClaimTypes.Name, resultado.Nome),
            new(ClaimTypes.Email, resultado.Email),
            new(ClaimTypes.Role, resultado.Perfil),
            new("Id", resultado.Id.ToString()),
            new("Perfil", resultado.Perfil)
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
            new AuthenticationProperties { IsPersistent = true });

        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            return Redirect(returnUrl);

        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult Cadastro()
    {
        if (User.Identity?.IsAuthenticated == true) return RedirectToAction("Index", "Home");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Cadastro(CadastroViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var resultado = await _api.CadastrarAsync(model);

        if (resultado is null || !resultado.Sucesso)
        {
            ModelState.AddModelError("", resultado?.Mensagem ?? "Erro ao criar conta.");
            return View(model);
        }

        TempData["Sucesso"] = "Conta criada! Faça login para continuar.";
        return RedirectToAction(nameof(Login));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction(nameof(Login));
    }

    public IActionResult AcessoNegado()
    {
        return View();
    }
}
