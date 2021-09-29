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
    public class ProductoBunsiness : IProductoBunsiness
    {
        private readonly SangalDbContext _context;

        public ProductoBunsiness(SangalDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Producto>> ObtenerProductos()
        {
            return await _context.Productos.Include(x => x.Categoria).ToListAsync();
        }

        public async Task<Producto> ObtenerProductoPorId(int? id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            return await _context.Productos.Include(x => x.Categoria).FirstOrDefaultAsync(x => x.ProductoId == id);
        }

        public void Crear(Producto producto)
        {
            if (producto == null)
                throw new ArgumentNullException(nameof(producto));
            producto.Estado = true;
            _context.Add(producto);
        }

        public void Editar(Producto producto)
        {
            if(producto == null)
                throw new ArgumentNullException(nameof(producto));

            _context.Update(producto);
        }
       

        public void Eliminar(Producto producto)
        {
            if (producto == null)
                throw new ArgumentNullException(nameof(producto));

            _context.Remove(producto);
        }

        public async Task<bool> GuardarCambios()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
