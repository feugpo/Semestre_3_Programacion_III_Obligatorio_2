using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;

namespace EF
{
   public class PrestamosContext : DbContext
    {
        
            public DbSet<Proyecto> Proyectoes { get; set; }

            public DbSet<Usuario> Usuarios { get; set; }

            public DbSet<Financiamiento> Financiamientos { get; set; }

            public PrestamosContext() : base("miConexion")
            {
            }

        }
    }
