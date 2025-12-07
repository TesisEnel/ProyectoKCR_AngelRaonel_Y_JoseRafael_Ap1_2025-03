using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KCR.Models;

public class PreFacturas
{
    [Key]
    public int IdPreFactura { get; set; }
    public string? NombreCliente { get; set; }

    public DateTime Fecha { get; set; }
    public string Estado { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Total { get; set; }

    [ForeignKey("Clientes")]
    public int? IdCliente { get; set; }
    public Clientes? Clientes { get; set; }

    [ForeignKey("Empleados")]
    public int IdEmpleado { get; set; }
    public Empleados? Empleado { get; set; }
    [ForeignKey("Turnos")]
    public int? IdTurno { get; set; }
    public Turnos? Turnos { get; set; }

    // Relaciones
    public ICollection<PreFacturaDetalles> PreFacturaDetalles { get; set; } = new List<PreFacturaDetalles>();
}
