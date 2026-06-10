using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SpaceConnect.ApiService.Filters;
using SpaceConnect.ApiService.Data.Interfaces;
using SpaceConnect.ApiService.DTOs;
using SpaceConnect.ApiService.Models;

namespace SpaceConnect.ApiService.Controllers;

[ApiController]
[Route("api/tecnologias")]
public class TecnologiasController : ControllerBase
{
    private readonly ITecnologiaRepository _repo;

    public TecnologiasController(ITecnologiaRepository repo)
    {
        _repo = repo;
    }

    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        var lista = await _repo.ListarAsync();
        var resultado = lista.Select(MapToDto);
        return Ok(resultado);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> BuscarPorId(int id)
    {
        var tecnologia = await _repo.BuscarPorIdAsync(id);
        if (tecnologia is null) return NotFound(new { mensagem = "Tecnologia não encontrada." });
        return Ok(MapToDto(tecnologia));
    }

    [HttpPost]
    [ServiceFilter(typeof(AdminApiAuthorizationFilter))]
    public async Task<IActionResult> Criar([FromBody] CriarTecnologiaDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var tecnologia = new Tecnologia
        {
            Nome = dto.Nome,
            Descricao = dto.Descricao,
            AnoOrigem = dto.AnoOrigem,
            AplicacaoTerra = dto.AplicacaoTerra,
            CategoriaId = dto.CategoriaId,
            MissaoId = dto.MissaoId,
            CadastradoPor = dto.CadastradoPor
        };

        var criada = await _repo.CriarAsync(tecnologia);
        var resultado = await _repo.BuscarPorIdAsync(criada.Id);
        return CreatedAtAction(nameof(BuscarPorId), new { id = criada.Id }, MapToDto(resultado!));
    }

    [HttpPut("{id:int}")]
    [ServiceFilter(typeof(AdminApiAuthorizationFilter))]
    public async Task<IActionResult> Atualizar(int id, [FromBody] AtualizarTecnologiaDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var dados = new Tecnologia
        {
            Nome = dto.Nome,
            Descricao = dto.Descricao,
            AnoOrigem = dto.AnoOrigem,
            AplicacaoTerra = dto.AplicacaoTerra,
            CategoriaId = dto.CategoriaId,
            MissaoId = dto.MissaoId
        };

        var atualizada = await _repo.AtualizarAsync(id, dados);
        if (atualizada is null) return NotFound(new { mensagem = "Tecnologia não encontrada." });

        var resultado = await _repo.BuscarPorIdAsync(atualizada.Id);
        return Ok(MapToDto(resultado!));
    }

    [HttpDelete("{id:int}")]
    [ServiceFilter(typeof(AdminApiAuthorizationFilter))]
    public async Task<IActionResult> Remover(int id)
    {
        var removida = await _repo.RemoverAsync(id);
        if (!removida) return NotFound(new { mensagem = "Tecnologia não encontrada." });
        return NoContent();
    }

    [HttpGet("stats")]
    public async Task<IActionResult> Stats()
    {
        var stats = await _repo.ObterStatsAsync();
        return Ok(stats);
    }

    private static TecnologiaDto MapToDto(Tecnologia t) => new(
        t.Id,
        t.Nome,
        t.Descricao,
        t.AnoOrigem,
        t.AplicacaoTerra,
        t.CategoriaId,
        t.Categoria?.Nome,
        t.Categoria?.CorHex,
        t.MissaoId,
        t.Missao?.Nome,
        t.CriadoEm
    );
}
