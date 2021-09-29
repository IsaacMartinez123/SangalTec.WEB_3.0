using SangalTec.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SangalTec.Bunsiness.Abstract
{
    public interface ICategoriaBunsiness
    {
        Task<IEnumerable<Categoria>> ObtenerCategoria();

        void Crear(Categoria categoria);

        void Editar(Categoria categoria);

        void Eliminar(Categoria categoria);

        Task<Categoria> ObtenerCategoriaPorId(int? id);

        Task<bool> GuardarCambios();
    }
}
