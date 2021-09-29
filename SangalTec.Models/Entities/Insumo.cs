using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SangalTec.Models.Entities
{
    public class Insumo
    {
        [Display(Name = "Id")]
        public int InsumoId { get; set; }

        public string Nombre { get; set; }

        [Display(Name = "Estado")]
        public bool Estado { get; set; }
    }
}
