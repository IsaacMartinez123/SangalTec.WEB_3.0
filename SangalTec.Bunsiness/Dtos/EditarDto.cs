using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SangalTec.Bunsiness.Dtos
{
    public class EditarDto
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "El email es requerido")]
        [Display(Name = "Email", Order = -9,
        Prompt = "Ingrese el email", Description = "Email")]
        [EmailAddress(ErrorMessage = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }


        [Required(ErrorMessage = "El numero celular es requerido")]
        [Display(Name = "Numero de Celular", Order = -9,
        Prompt = "Ingrese el numero de celular", Description = "Numero de Celular")]
        [DataType(DataType.PhoneNumber)]
        public string NumeroCelular { get; set; }

        public bool Estado { get; set; }


       
    }
}
