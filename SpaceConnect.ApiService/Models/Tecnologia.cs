using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpaceConnect.ApiService.Models;

[Table("tecnologias")]
public class Tecnologia
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    [Column("nome")]
    public string Nome { get; set; } = string.Empty;

    [Required]
    [Column("descricao")]
    public string Descricao { get; set; } = string.Empty;

    [Column("ano_origem")]
    public int? AnoOrigem { get; set; }

    [Column("aplicacao_terra")]
    public string? AplicacaoTerra { get; set; }

    [Required]
    [Column("categoria_id")]
    public int CategoriaId { get; set; }

    [Column("missao_id")]
    public int? MissaoId { get; set; }

    [Column("cadastrado_por")]
    public int? CadastradoPor { get; set; }

    [Column("criado_em")]
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;

    [ForeignKey(nameof(CategoriaId))]
    public CategoriaImpacto? Categoria { get; set; }

    [ForeignKey(nameof(MissaoId))]
    public Missao? Missao { get; set; }

    [ForeignKey(nameof(CadastradoPor))]
    public Usuario? Usuario { get; set; }
}
