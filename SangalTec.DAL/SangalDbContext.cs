using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SangalTec.Models.Entities;
using SangalTec.Models.EntitiesUsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SangalTec.DAL
{
    public class SangalDbContext : IdentityDbContext
    {

        public SangalDbContext()
        {

        }

        //opciones como la cadena de conexion 
        public SangalDbContext(DbContextOptions<SangalDbContext> options) : base(options)
        {

        }


        //BdSet o representacion de nuestras tablas
        public DbSet<Usuario> Usuarios { get; set; }

        //public DbSet<TipoDocumento> TipoDocumento { get; set; }

        public DbSet<Producto> Productos { get; set; }

        public DbSet<Categoria> Categorias { get; set; }

        public DbSet<Proveedor> Proveedores { get; set; }

        public DbSet<Insumo> Insumos { get; set; }
    }
}
