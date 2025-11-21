using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoKCR.Models;

public class Turnos
{
    [Key]
    public int IdTurno { get; set; }
    [Required, MaxLength(50)]
    public string NumTurno { get; set; }
    [MaxLength(100)]
    public string? Servicio { get; set; }
    public DateTime Fecha { get; set; }

    [ForeignKey("Cliente")]
    public int IdCliente { get; set; }
    public Clientes Cliente { get; set; }
}
