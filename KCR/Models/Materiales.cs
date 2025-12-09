using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KCR.Models;

public class Materiales
{
    [Key]
    public int IdMaterial { get; set; }
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ\s\.-]+$", ErrorMessage = "El nombre solo puede contener letras y espacios.")]
    public string Nombre { get; set; }
    [Required(ErrorMessage = "La existencia es requerido")]
    [Range(0, int.MaxValue, ErrorMessage = "La existencia deb ser mayor o igual a 0")]
    public int Existencia { get; set; } = 0;
    [Required(ErrorMessage = "El nombre del cliente es requerido")]
    [Range(0.01, double.MaxValue, ErrorMessage = "El total debe ser mayor que 0")]
    public double PrecioUnitario { get; set; }
    public bool Activo { get; set; } = true;

    public ICollection<PreFacturaDetalles> PreFacturaDetalles { get; set; } = new List<PreFacturaDetalles>();
}
