using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KCR.Models;

public class Turnos
{
    [Key]
    public int IdTurno { get; set; }

    [Required, MaxLength(50)]
    public string? NumTurno { get; set; }

    public DateTime Fecha { get; set; }

    [Required, MaxLength(20)]
    public string Estado { get; set; } = "En Espera";

    [ForeignKey("Servicios")]
    public int? IdServicio { get; set; }
    public Servicios? Servicios { get; set; }

    [ForeignKey("Clientes")]
    public int IdCliente { get; set; }
    public Clientes Clientes { get; set; }
}
