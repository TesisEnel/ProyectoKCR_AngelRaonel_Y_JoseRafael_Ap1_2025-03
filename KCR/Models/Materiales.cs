using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KCR.Models;

public class Materiales
{
    [Key]
    public int IdMaterial { get; set; }
    public string Nombre { get; set; }
    public int Existencia { get; set; } = 0;
    [Column(TypeName = "decimal(10, 2)")]
    public decimal PrecioUnitario { get; set; }

    public ICollection<PreFacturaDetalles> PreFacturaDetalles { get; set; } = new List<PreFacturaDetalles>();
}
