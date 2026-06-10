using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpaceConnect.ApiService.Models;

[Table("missoes")]
public class Missao
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [MaxLength(150)]
    [Column("nome")]
    public string Nome { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    [Column("agencia")]
    public string Agencia { get; set; } = string.Empty;

    [Required]
    [Column("ano")]
    public int Ano { get; set; }

    [Column("descricao")]
    public string? Descricao { get; set; }

    [Column("criado_em")]
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;

    public ICollection<Tecnologia> Tecnologias { get; set; } = [];
}
