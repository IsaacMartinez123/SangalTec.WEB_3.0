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
    public class CategoriaBunsiness : ICategoriaBunsiness
    {
        private readonly SangalDbContext _context;

        public CategoriaBunsiness(SangalDbContext context)
        {
            _context = context;
        }


      
        public async Task<IEnumerable<Categoria>> ObtenerCategoria()
        {
            return await _context.Categorias.ToListAsync();
        }

        public async Task<Categoria> ObtenerCategoriaPorId(int? id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            return await _context.Categorias.FirstOrDefaultAsync(x => x.CategoriaId == id);
        }

        public void Crear(Categoria categoria)
        {
            if (categoria == null)
                throw new ArgumentNullException(nameof(categoria));
            _context.Add(categoria);
        }

        public  void Editar(Categoria categoria)
        {
            if(categoria == null)
                throw new ArgumentNullException(nameof(categoria));

            _context.Update(categoria);
        }


        public void Eliminar(Categoria categoria)
        {
            if(categoria == null)
                throw new ArgumentNullException(nameof(categoria));
            _context.Remove(categoria);
        }

       

        public async Task<bool> GuardarCambios()
        {
            return await _context.SaveChangesAsync() > 0;
        } 


    }
}
