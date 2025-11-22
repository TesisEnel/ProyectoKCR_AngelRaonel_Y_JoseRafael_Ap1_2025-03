using System.ComponentModel.DataAnnotations;

namespace ProyectoKCR.Models;

public class Materiales
{
    [Key]
    public int IdMaterial { get; set; }

    [Required, MaxLength(255)]
    public string Nombre { get; set; }

    public int Existencia { get; set; }

    [Required]
    public decimal PrecioUnitario { get; set; }

    public ICollection<Servicios> servicios { get; set; } = new List<Servicios>();
}