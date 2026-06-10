using Microsoft.EntityFrameworkCore;
using SpaceConnect.ApiService.Data.Interfaces;
using SpaceConnect.ApiService.Models;

namespace SpaceConnect.ApiService.Data.Repositories;

public class TecnologiaRepository : ITecnologiaRepository
{
    private readonly SpaceConnectContext _context;

    public TecnologiaRepository(SpaceConnectContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Tecnologia>> ListarAsync()
    {
        return await _context.Tecnologias
            .Include(t => t.Categoria)
            .Include(t => t.Missao)
            .OrderByDescending(t => t.CriadoEm)
            .ToListAsync();
    }

    public async Task<Tecnologia?> BuscarPorIdAsync(int id)
    {
        return await _context.Tecnologias
            .Include(t => t.Categoria)
            .Include(t => t.Missao)
            .Include(t => t.Usuario)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<Tecnologia> CriarAsync(Tecnologia tecnologia)
    {
        tecnologia.CriadoEm = DateTime.UtcNow;
        _context.Tecnologias.Add(tecnologia);
        await _context.SaveChangesAsync();
        return tecnologia;
    }

    public async Task<Tecnologia?> AtualizarAsync(int id, Tecnologia dados)
    {
        var existente = await _context.Tecnologias.FindAsync(id);
        if (existente is null) return null;

        existente.Nome = dados.Nome;
        existente.Descricao = dados.Descricao;
        existente.AnoOrigem = dados.AnoOrigem;
        existente.AplicacaoTerra = dados.AplicacaoTerra;
        existente.CategoriaId = dados.CategoriaId;
        existente.MissaoId = dados.MissaoId;

        await _context.SaveChangesAsync();
        return existente;
    }

    public async Task<bool> RemoverAsync(int id)
    {
        var tecnologia = await _context.Tecnologias.FindAsync(id);
        if (tecnologia is null) return false;

        _context.Tecnologias.Remove(tecnologia);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<TecnologiaStats> ObterStatsAsync()
    {
        var totalTecnologias = await _context.Tecnologias.CountAsync();
        var totalMissoes = await _context.Missoes.CountAsync();
        var totalCategorias = await _context.Categorias.CountAsync();
        var totalSetores = await _context.Tecnologias
            .Select(t => t.CategoriaId)
            .Distinct()
            .CountAsync();

        var porCategoriaRaw = await _context.Categorias
            .Select(c => new { c.Id, c.Nome, c.CorHex, Quantidade = c.Tecnologias.Count })
            .OrderByDescending(x => x.Quantidade)
            .ToListAsync();

        var porCategoria = porCategoriaRaw
            .Select(x => new CategoriaStat(x.Id, x.Nome, x.CorHex, x.Quantidade))
            .ToList();

        return new TecnologiaStats(
            totalTecnologias,
            totalMissoes,
            totalCategorias,
            totalSetores,
            porCategoria
        );
    }
}
