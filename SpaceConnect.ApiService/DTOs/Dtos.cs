using System.ComponentModel.DataAnnotations;

namespace SpaceConnect.ApiService.DTOs;

public record TecnologiaDto(
    int Id,
    string Nome,
    string Descricao,
    int? AnoOrigem,
    string? AplicacaoTerra,
    int CategoriaId,
    string? CategoriaNome,
    string? CategoriaCorHex,
    int? MissaoId,
    string? MissaoNome,
    DateTime CriadoEm
);

public record CriarTecnologiaDto(
    [Required][MaxLength(200)] string Nome,
    [Required] string Descricao,
    int? AnoOrigem,
    string? AplicacaoTerra,
    [Required] int CategoriaId,
    int? MissaoId,
    int? CadastradoPor
);

public record AtualizarTecnologiaDto(
    [Required][MaxLength(200)] string Nome,
    [Required] string Descricao,
    int? AnoOrigem,
    string? AplicacaoTerra,
    [Required] int CategoriaId,
    int? MissaoId
);

public record LoginDto(
    [Required][EmailAddress] string Email,
    [Required] string Senha
);

public record CadastroDto(
    [Required][MaxLength(150)] string Nome,
    [Required][EmailAddress] string Email,
    [Required][MinLength(6)] string Senha,
    string Perfil = "Pesquisador"
);

public record AuthResponseDto(
    int Id,
    string Nome,
    string Email,
    string Perfil,
    bool Sucesso,
    string? Mensagem
);

public record CategoriaDto(
    int Id,
    string Nome,
    string? Descricao,
    string? Icone,
    string CorHex,
    int TotalTecnologias
);

public record MissaoDto(
    int Id,
    string Nome,
    string Agencia,
    int Ano,
    string? Descricao
);
