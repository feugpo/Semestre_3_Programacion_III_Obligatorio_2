using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;
using Repositorios;

namespace Servicios
{
    public class FabricaRepositorios
    {
        IRepositorio<Cliente> ObtenerRepoClientes()
        {

            return new RepoClientesEF();
        }

        IRepositorio<Producto> ObtenerRepoProductos()
        {
            return new RepoProductos();
        }
    }
}
