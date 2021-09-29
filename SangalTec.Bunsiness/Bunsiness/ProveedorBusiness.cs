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
    public class ProveedorBusiness : IProveedorBusiness
    {
        private readonly SangalDbContext _context; 

        public ProveedorBusiness(SangalDbContext context)
        {
            _context = context;
        }


        public async Task<IEnumerable<Proveedor>> ObtenerProveedores()
        {
            return await _context.Proveedores.Include(e => e.Insumo).ToListAsync();
        }

        public async Task<Proveedor> ObtenerProveedorPorId(int? id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            return await _context.Proveedores.Include(e => e.Insumo).FirstOrDefaultAsync(e => e.ProveedorId == id);
        }


        public void Crear(Proveedor proveedor)
        {
            if (proveedor == null)
                throw new ArgumentNullException(nameof(proveedor));

            proveedor.Estado = true;
            _context.Add(proveedor);
        }

        public void Editar(Proveedor proveedor)
        {
            if (proveedor == null)
                throw new ArgumentNullException(nameof(proveedor));

            _context.Update(proveedor);
        }


        public void Eliminar(Proveedor proveedor)
        {
            if (proveedor == null)
                throw new ArgumentNullException(nameof(proveedor));

            _context.Remove(proveedor);
        }

        public async Task<bool> GuardarCambios()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
