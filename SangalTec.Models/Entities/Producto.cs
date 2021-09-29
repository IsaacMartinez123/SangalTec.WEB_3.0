using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SangalTec.Models.Entities
{
    public class Producto
    {
        [Display(Name = "Id")]
        public int ProductoId { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [Display(Name = "Nombre del producto")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La cantidad es obligatoria")]
        [Display(Name = "Cantidad")]
        public int Cantidad { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio")]
        [Display(Name = "Precio")]
        public double Precio { get; set; }


        [Display(Name = "Estado")]
        public bool Estado { get; set; }

        [Required(ErrorMessage = "La categoría es obligatorio")]
        [Display(Name = "Categoría")]
        public int CategoriaId { get; set; }

        public Categoria Categoria { get; set; }
    }
}
