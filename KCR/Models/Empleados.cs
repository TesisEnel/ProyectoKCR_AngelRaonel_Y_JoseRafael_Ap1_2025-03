using System.ComponentModel.DataAnnotations;

namespace KCR.Models;

public class Empleados
{
    [Key]
    public int IdEmpleado { get; set; }
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ\s\.-]+$", ErrorMessage = "El nombre solo puede contener letras y espacios.")]
    public string Nombre { get; set; }

    [Required(ErrorMessage = "El usuario (Email) es obligatorio.")]
    [EmailAddress(ErrorMessage = "El formato del usuario debe ser un correo electrónico válido.")]
    public string Usuario { get; set; }

    [Required, MaxLength(255)]
    public string Clave { get; set; }

    [StringLength(11, MinimumLength = 11, ErrorMessage = "La Cédula debe tener 11 dígitos.")]
    [RegularExpression(@"^\d{11}$", ErrorMessage = "Solo se permiten 11 dígitos numéricos.")]
    public string? Cedula { get; set; }
    [Required(ErrorMessage = "El cargo es obligatorio.")]
    [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ\s\.-]+$", ErrorMessage = "El cargo solo puede contener letras y espacios.")]
    public string? Cargo { get; set; }

    public ICollection<PreFacturas> PreFacturas { get; set; } = new List<PreFacturas>();
}
