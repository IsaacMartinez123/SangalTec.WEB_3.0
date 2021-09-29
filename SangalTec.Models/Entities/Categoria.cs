using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SangalTec.Models.Entities
{
    public class Categoria
    {
        [Display(Name = "Id")]
        public int CategoriaId { get; set; }
        
        [Required(ErrorMessage = "Ingresa un nombre para la categoría")]
        [Display(Name = "Categoría")]
        public string Nombre { get; set; }

    }
}
