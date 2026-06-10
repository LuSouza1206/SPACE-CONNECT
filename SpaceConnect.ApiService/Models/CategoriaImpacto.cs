using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpaceConnect.ApiService.Models;

[Table("categorias_impacto")]
public class CategoriaImpacto
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    [Column("nome")]
    public string Nome { get; set; } = string.Empty;

    [Column("descricao")]
    public string? Descricao { get; set; }

    [MaxLength(50)]
    [Column("icone")]
    public string? Icone { get; set; }

    [MaxLength(7)]
    [Column("cor_hex")]
    public string CorHex { get; set; } = "#ef4444";

    [Column("criado_em")]
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;

    public ICollection<Tecnologia> Tecnologias { get; set; } = [];
}
