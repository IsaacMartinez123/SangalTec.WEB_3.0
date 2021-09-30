using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SangalTec.Bunsiness.Dtos
{
    public class LoginDto
    {
        [Required(ErrorMessage = "El email es requerido")]
        [Display(Name = "Email", Order = -9,
        Prompt = "Ingrese el email", Description = "Email")]
        [EmailAddress(ErrorMessage = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }


        [Required(ErrorMessage = "La contraseña es requerida")]
        [Display(Name = "Contraseña", Order = -9,
        Prompt = "Ingrese la contraseña", Description = "Contraseña")]
        [DataType(DataType.Password)]
        [StringLength(20, ErrorMessage = "El {0} debe tener al menos {2} y maximo {1} caracteres.", MinimumLength = 5)]
        public string Password { get; set; }


        [Display(Name = "Recordarme")]
        public bool RecordarMe { get; set; }
    }
}
