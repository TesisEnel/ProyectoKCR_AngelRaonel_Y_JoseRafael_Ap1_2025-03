using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KCR.Models;

public class PreFacturaDetalles
{
    [Key]
    public int IdDetalle { get; set; }
    public int Cantidad { get; set; }
    [Column(TypeName = "decimal(10, 2)")]
    public decimal PrecioUnitario { get; set; }

    [ForeignKey("Servicios")]
    public int? IdServicio { get; set; }
    public Servicios? Servicios{ get; set; }

    [ForeignKey("PreFacturas")]
    public int IdPreFactura { get; set; }
    public PreFacturas PreFacturas { get; set; }

    [ForeignKey("Materiales")]
    public int? IdMaterial { get; set; }
    public Materiales? Materiales { get; set; }

}
