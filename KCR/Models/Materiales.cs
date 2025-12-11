using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KCR.Models;

public class Materiales
{
    [Key]
    public int IdMaterial { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    public string Nombre { get; set; }

    [Required(ErrorMessage = "La existencia es requerida")]
    [Range(0.0, double.MaxValue, ErrorMessage = "La existencia debe ser mayor o igual a 0")]
    public double Existencia { get; set; } = 0.0;

    [Required(ErrorMessage = "El precio unitario es requerido")]
    [Range(0.01, double.MaxValue, ErrorMessage = "El precio unitario debe ser mayor que 0")]
    public double PrecioUnitario { get; set; }

    public bool Activo { get; set; } = true;

    public ICollection<PreFacturaDetalles> PreFacturaDetalles { get; set; } = new List<PreFacturaDetalles>();
}
