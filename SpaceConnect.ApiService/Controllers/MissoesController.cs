using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpaceConnect.ApiService.Data;
using SpaceConnect.ApiService.DTOs;

namespace SpaceConnect.ApiService.Controllers;

[ApiController]
[Route("api/missoes")]
public class MissoesController : ControllerBase
{
    private readonly SpaceConnectContext _context;

    public MissoesController(SpaceConnectContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        var missoes = await _context.Missoes
            .OrderBy(m => m.Ano)
            .Select(m => new MissaoDto(m.Id, m.Nome, m.Agencia, m.Ano, m.Descricao))
            .ToListAsync();

        return Ok(missoes);
    }
}
