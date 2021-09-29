using SangalTec.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SangalTec.Bunsiness.Abstract
{
    public interface IProveedorBusiness
    {
        Task<IEnumerable<Proveedor>> ObtenerProveedores();
        Task<Proveedor> ObtenerProveedorPorId(int? id);
        void Crear(Proveedor proveedor);
        void Editar(Proveedor proveedor);
        void Eliminar(Proveedor proveedor);
        Task<bool> GuardarCambios();
    }
}
