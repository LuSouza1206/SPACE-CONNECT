using System.Net.Http;
using System.Text;
using System.Text.Json;
using SpaceConnect.Web.Models;

namespace SpaceConnect.Web.Services;

public class ApiService
{
    private readonly HttpClient _http;
    private static readonly JsonSerializerOptions _json = new() { PropertyNameCaseInsensitive = true };

    public ApiService(HttpClient http)
    {
        _http = http;
    }

    private static async Task<T?> DeserializeUtf8Async<T>(HttpResponseMessage response)
    {
        var bytes = await response.Content.ReadAsByteArrayAsync();
        return JsonSerializer.Deserialize<T>(bytes, _json);
    }

    public async Task<List<TecnologiaViewModel>> ListarTecnologiasAsync()
    {
        var resp = await _http.GetAsync("/api/tecnologias");
        resp.EnsureSuccessStatusCode();
        return await DeserializeUtf8Async<List<TecnologiaViewModel>>(resp) ?? [];
    }

    public async Task<TecnologiaViewModel?> BuscarTecnologiaAsync(int id)
    {
        var resp = await _http.GetAsync($"/api/tecnologias/{id}");
        if (resp.StatusCode == System.Net.HttpStatusCode.NotFound) return null;
        resp.EnsureSuccessStatusCode();
        return await DeserializeUtf8Async<TecnologiaViewModel>(resp);
    }

    public async Task<bool> CriarTecnologiaAsync(CriarTecnologiaViewModel model, int usuarioId)
    {
        var payload = new
        {
            model.Nome,
            model.Descricao,
            model.AnoOrigem,
            model.AplicacaoTerra,
            model.CategoriaId,
            model.MissaoId,
            CadastradoPor = usuarioId
        };

        var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
        var resp = await _http.PostAsync("/api/tecnologias", content);
        return resp.IsSuccessStatusCode;
    }

    public async Task<bool> AtualizarTecnologiaAsync(int id, CriarTecnologiaViewModel model)
    {
        var payload = new
        {
            model.Nome,
            model.Descricao,
            model.AnoOrigem,
            model.AplicacaoTerra,
            model.CategoriaId,
            model.MissaoId
        };

        var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
        var resp = await _http.PutAsync($"/api/tecnologias/{id}", content);
        return resp.IsSuccessStatusCode;
    }

    public async Task<bool> RemoverTecnologiaAsync(int id)
    {
        var resp = await _http.DeleteAsync($"/api/tecnologias/{id}");
        return resp.IsSuccessStatusCode;
    }

    public async Task<DashboardViewModel> ObterDashboardAsync()
    {
        var statsTask = _http.GetAsync("/api/tecnologias/stats");
        var recentesTask = _http.GetAsync("/api/tecnologias");

        await Task.WhenAll(statsTask, recentesTask);

        var stats = await DeserializeUtf8Async<StatsResponse>(statsTask.Result);
        var recentes = await DeserializeUtf8Async<List<TecnologiaViewModel>>(recentesTask.Result) ?? [];

        return new DashboardViewModel
        {
            TotalTecnologias = stats?.TotalTecnologias ?? 0,
            TotalMissoes = stats?.TotalMissoes ?? 0,
            TotalCategorias = stats?.TotalCategorias ?? 0,
            TotalSetoresBeneficiados = stats?.TotalSetoresBeneficiados ?? 0,
            PorCategoria = stats?.PorCategoria?.Select(x => new CategoriaStat(x.Id, x.Nome, x.CorHex, x.Quantidade)).ToList() ?? [],
            Recentes = recentes.Take(8).ToList()
        };
    }

    public async Task<List<CategoriaViewModel>> ListarCategoriasDetalhadasAsync()
    {
        var resp = await _http.GetAsync("/api/categorias");
        resp.EnsureSuccessStatusCode();
        return await DeserializeUtf8Async<List<CategoriaRaw>>(resp) is { } raw
            ? raw.Select(c => new CategoriaViewModel(c.Id, c.Nome, c.Descricao, c.Icone, c.CorHex, c.TotalTecnologias)).ToList()
            : [];
    }

    public async Task<List<MissaoViewModel>> ListarMissoesDetalhadasAsync()
    {
        var missoesTask = _http.GetAsync("/api/missoes");
        var tecnologiasTask = _http.GetAsync("/api/tecnologias");
        await Task.WhenAll(missoesTask, tecnologiasTask);

        var missoes = await DeserializeUtf8Async<List<MissaoRaw>>(missoesTask.Result) ?? [];
        var tecnologias = await DeserializeUtf8Async<List<TecnologiaViewModel>>(tecnologiasTask.Result) ?? [];

        return missoes
            .Select(m => new MissaoViewModel(
                m.Id,
                m.Nome,
                m.Agencia,
                m.Ano,
                m.Descricao,
                tecnologias.Count(t => t.MissaoId == m.Id)))
            .ToList();
    }

    public async Task<List<CategoriaSelectItem>> ListarCategoriasAsync()
    {
        var resp = await _http.GetAsync("/api/categorias");
        resp.EnsureSuccessStatusCode();
        return await DeserializeUtf8Async<List<CategoriaRaw>>(resp) is { } raw
            ? raw.Select(c => new CategoriaSelectItem(c.Id, c.Nome, c.CorHex)).ToList()
            : [];
    }

    public async Task<List<MissaoSelectItem>> ListarMissoesAsync()
    {
        var resp = await _http.GetAsync("/api/missoes");
        resp.EnsureSuccessStatusCode();
        return await DeserializeUtf8Async<List<MissaoRaw>>(resp) is { } raw
            ? raw.Select(m => new MissaoSelectItem(m.Id, m.Nome, m.Agencia, m.Ano)).ToList()
            : [];
    }

    public async Task<AuthResultViewModel?> LoginAsync(string email, string senha)
    {
        var payload = new { Email = email, Senha = senha };
        var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
        var resp = await _http.PostAsync("/api/auth/login", content);
        return await DeserializeUtf8Async<AuthResultViewModel>(resp);
    }

    public async Task<AuthResultViewModel?> CadastrarAsync(CadastroViewModel model)
    {
        var payload = new { model.Nome, model.Email, model.Senha, model.Perfil };
        var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
        var resp = await _http.PostAsync("/api/auth/cadastro", content);
        return await DeserializeUtf8Async<AuthResultViewModel>(resp);
    }

    private record StatsResponse(int TotalTecnologias, int TotalMissoes, int TotalCategorias, int TotalSetoresBeneficiados, List<CategoriaStatRaw> PorCategoria);
    private record CategoriaStatRaw(int Id, string Nome, string CorHex, int Quantidade);
    private record CategoriaRaw(int Id, string Nome, string? Descricao, string? Icone, string CorHex, int TotalTecnologias);
    private record MissaoRaw(int Id, string Nome, string Agencia, int Ano, string? Descricao);
}

public class AuthResultViewModel
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Perfil { get; set; } = string.Empty;
    public bool Sucesso { get; set; }
    public string? Mensagem { get; set; }
}
