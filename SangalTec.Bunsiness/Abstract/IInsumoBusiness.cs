
using SangalTec.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SangalTec.Bunsiness.Abstract
{
    public interface IInsumoBusiness
    {
        Task<IEnumerable<Insumo>> ObtenerInsumos();
        Task<Insumo> ObetenerInsumoPorId(int? id);

        void Crear(Insumo insumo);

        void Editar(Insumo insumo);

        void Eliminar(Insumo insumo);

        Task<bool> GuardarCambios();
    }
}
