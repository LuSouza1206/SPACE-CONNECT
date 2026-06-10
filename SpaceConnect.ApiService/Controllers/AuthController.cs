using Microsoft.AspNetCore.Mvc;
using SpaceConnect.ApiService.Data.Interfaces;
using SpaceConnect.ApiService.DTOs;
using SpaceConnect.ApiService.Models;

namespace SpaceConnect.ApiService.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IUsuarioRepository _repo;

    public AuthController(IUsuarioRepository repo)
    {
        _repo = repo;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var usuario = await _repo.BuscarPorEmailAsync(dto.Email);
        if (usuario is null || !BCrypt.Net.BCrypt.Verify(dto.Senha, usuario.SenhaHash))
        {
            return Unauthorized(new AuthResponseDto(0, "", "", "", false, "E-mail ou senha incorretos."));
        }

        return Ok(new AuthResponseDto(usuario.Id, usuario.Nome, usuario.Email, usuario.Perfil, true, null));
    }

    [HttpPost("cadastro")]
    public async Task<IActionResult> Cadastro([FromBody] CadastroDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        if (await _repo.EmailExisteAsync(dto.Email))
        {
            return Conflict(new AuthResponseDto(0, "", "", "", false, "E-mail já cadastrado."));
        }

        var perfil = dto.Perfil == "Administrador" ? "Administrador" : "Pesquisador";

        var usuario = new Usuario
        {
            Nome = dto.Nome,
            Email = dto.Email,
            SenhaHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha),
            Perfil = perfil
        };

        var criado = await _repo.CriarAsync(usuario);
        return CreatedAtAction(null, new AuthResponseDto(criado.Id, criado.Nome, criado.Email, criado.Perfil, true, null));
    }
}
