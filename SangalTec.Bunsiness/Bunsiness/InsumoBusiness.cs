using Microsoft.EntityFrameworkCore;
using SangalTec.Bunsiness.Abstract;
using SangalTec.DAL;
using SangalTec.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SangalTec.Bunsiness.Bunsiness
{
    public class InsumoBusiness : IInsumoBusiness
    {
        private readonly SangalDbContext _context;

        public InsumoBusiness(SangalDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Insumo>> ObtenerInsumos()
        {
            return await _context.Insumos.ToListAsync();
        }

        public async Task<Insumo> ObetenerInsumoPorId(int? id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            return await _context.Insumos.FirstOrDefaultAsync(e => e.InsumoId == id);
        }


        public void Crear(Insumo insumo)
        {
            if(insumo == null)
                throw new ArgumentNullException(nameof(insumo));
            insumo.Estado = true;
            _context.Add(insumo);
        }

        public void Editar(Insumo insumo)
        {
            if(insumo == null)
                throw new ArgumentNullException(nameof(insumo));

            _context.Update(insumo);
        }

        public void Eliminar(Insumo insumo)
        {
            if (insumo == null)
                throw new ArgumentNullException(nameof(insumo));

            _context.Remove(insumo);
        }

        public async Task<bool> GuardarCambios()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
