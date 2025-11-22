using System.ComponentModel.DataAnnotations;

namespace ProyectoKCR.Models;

public class Clientes
{
    [Key]
    public int IdCliente { get; set; }
    [Required, MaxLength(255)]
    public string Nombre { get; set; }
    [MaxLength(11)]
    public string? Cedula { get; set; }
    [MaxLength(10)]
    public string Telefono { get; set; }
    public DateTime Fecha { get; set; } 

    public ICollection<PreFacturas> PreFacturas { get; set; } = new List<PreFacturas>();
    public ICollection<Turnos> Turnos { get; set; } = new List<Turnos>();
}
