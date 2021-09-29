using SangalTec.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SangalTec.Bunsiness.Abstract
{
    public interface IProductoBunsiness
    {
        Task<IEnumerable<Producto>> ObtenerProductos();

        Task<Producto> ObtenerProductoPorId(int? id);

        void Crear(Producto producto);

        void Editar(Producto producto);

        void Eliminar(Producto producto);
        Task<bool> GuardarCambios();
    }
}
