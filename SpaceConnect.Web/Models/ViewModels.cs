using System.ComponentModel.DataAnnotations;

namespace SpaceConnect.Web.Models;

public class TecnologiaViewModel
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public int? AnoOrigem { get; set; }
    public string? AplicacaoTerra { get; set; }
    public int CategoriaId { get; set; }
    public string? CategoriaNome { get; set; }
    public string? CategoriaCorHex { get; set; }
    public int? MissaoId { get; set; }
    public string? MissaoNome { get; set; }
    public DateTime CriadoEm { get; set; }
}

public class CriarTecnologiaViewModel
{
    [Required(ErrorMessage = "Nome obrigatório")]
    [MaxLength(200, ErrorMessage = "Máximo 200 caracteres")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "Descrição obrigatória")]
    public string Descricao { get; set; } = string.Empty;

    [Range(1900, 2100, ErrorMessage = "Ano inválido")]
    public int? AnoOrigem { get; set; }

    public string? AplicacaoTerra { get; set; }

    [Required(ErrorMessage = "Categoria obrigatória")]
    public int CategoriaId { get; set; }

    public int? MissaoId { get; set; }

    public List<CategoriaSelectItem> Categorias { get; set; } = [];
    public List<MissaoSelectItem> Missoes { get; set; } = [];
}

public class DashboardViewModel
{
    public int TotalTecnologias { get; set; }
    public int TotalMissoes { get; set; }
    public int TotalCategorias { get; set; }
    public int TotalSetoresBeneficiados { get; set; }
    public List<CategoriaStat> PorCategoria { get; set; } = [];
    public List<TecnologiaViewModel> Recentes { get; set; } = [];
}

public record CategoriaStat(int Id, string Nome, string CorHex, int Quantidade);

public record CategoriaSelectItem(int Id, string Nome, string CorHex);
public record MissaoSelectItem(int Id, string Nome, string Agencia, int Ano);

public record CategoriaViewModel(int Id, string Nome, string? Descricao, string? Icone, string CorHex, int TotalTecnologias);
public record MissaoViewModel(int Id, string Nome, string Agencia, int Ano, string? Descricao, int TotalTecnologias);

public class LoginViewModel
{
    [Required(ErrorMessage = "E-mail obrigatório")]
    [EmailAddress(ErrorMessage = "E-mail inválido")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Senha obrigatória")]
    public string Senha { get; set; } = string.Empty;
}

public class CadastroViewModel
{
    [Required(ErrorMessage = "Nome obrigatório")]
    [MaxLength(150)]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "E-mail obrigatório")]
    [EmailAddress(ErrorMessage = "E-mail inválido")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Senha obrigatória")]
    [MinLength(6, ErrorMessage = "Mínimo 6 caracteres")]
    public string Senha { get; set; } = string.Empty;

    [Compare(nameof(Senha), ErrorMessage = "Senhas não conferem")]
    public string ConfirmaSenha { get; set; } = string.Empty;

    public string Perfil { get; set; } = "Pesquisador";
}
