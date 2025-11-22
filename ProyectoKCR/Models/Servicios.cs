using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoKCR.Models;

public class Servicios
{
    [Key]
    public int IdServicios { get; set; }
    public string TipoServicio { get; set; }
    public string NombreServicio { get; set; }
    public decimal PrecioServicio { get; set; }

    [ForeignKey("Material")]
    public int IdMaterial { get; set; }
    public Materiales Material { get; set; }

    public ICollection<DetallePreFactura> DetallesPreFactura { get; set; } = new List<DetallePreFactura>();

}
