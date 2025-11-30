using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KCR.Models;

public class Servicios
{
    [Key]
    public int IdServicio { get; set; }
    public string? Tipo { get; set; }
    public string Nombre { get; set; }
    [Column(TypeName = "decimal(4, 2)")]
    public decimal Precio { get; set; }
    [ForeignKey("Materiales")]
    public int? IdMaterial { get; set; }
    public Materiales? Materiales { get; set; }

    public ICollection<Turnos> Turnos { get; set; } = new List<Turnos>();
    public ICollection<PreFacturaDetalles> PreFacturaDetalles { get; set; } = new List<PreFacturaDetalles>();
}
