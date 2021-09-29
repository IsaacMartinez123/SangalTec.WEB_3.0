using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SangalTec.Bunsiness.Dtos
{
    public class UsuarioDto
    {
        public string Id { get; set; }
        public string Email { get; set; }

        [Display (Name = "Numero de Celular")]
        public string NumeroCelular { get; set; }
        public bool Estado { get; set; }
    }
}
