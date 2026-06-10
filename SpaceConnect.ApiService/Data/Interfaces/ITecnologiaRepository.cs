using SpaceConnect.ApiService.Models;

namespace SpaceConnect.ApiService.Data.Interfaces;

public interface ITecnologiaRepository
{
    Task<IEnumerable<Tecnologia>> ListarAsync();
    Task<Tecnologia?> BuscarPorIdAsync(int id);
    Task<Tecnologia> CriarAsync(Tecnologia tecnologia);
    Task<Tecnologia?> AtualizarAsync(int id, Tecnologia dados);
    Task<bool> RemoverAsync(int id);
    Task<TecnologiaStats> ObterStatsAsync();
}

public record TecnologiaStats(
    int TotalTecnologias,
    int TotalMissoes,
    int TotalCategorias,
    int TotalSetoresBeneficiados,
    IEnumerable<CategoriaStat> PorCategoria
);

public record CategoriaStat(int Id, string Nome, string CorHex, int Quantidade);
