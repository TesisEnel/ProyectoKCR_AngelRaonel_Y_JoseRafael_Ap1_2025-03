using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace KCR.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        [Required(ErrorMessage = "Todos los campos son obligatotios")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "Todos los campos son obligatotios")]
        public string Cedula { get; set; }
        [Required(ErrorMessage = "Todos los campos son obligatotios")]
        public string Cargo { get; set; }
    }

}
