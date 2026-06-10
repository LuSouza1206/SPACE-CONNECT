using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpaceConnect.ApiService.Data;
using SpaceConnect.ApiService.DTOs;

namespace SpaceConnect.ApiService.Controllers;

[ApiController]
[Route("api/categorias")]
public class CategoriasController : ControllerBase
{
    private readonly SpaceConnectContext _context;

    public CategoriasController(SpaceConnectContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        var categorias = await _context.Categorias
            .Include(c => c.Tecnologias)
            .OrderBy(c => c.Nome)
            .Select(c => new CategoriaDto(c.Id, c.Nome, c.Descricao, c.Icone, c.CorHex, c.Tecnologias.Count))
            .ToListAsync();

        return Ok(categorias);
    }
}
