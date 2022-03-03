using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Dominio;
using EF;

namespace WebApi.Controllers
{
    public class ProyectoesController : ApiController
    {
        private PrestamosContext db = new PrestamosContext();

        // GET: api/Proyectoes
        public IQueryable<Proyecto> GetProyectoes()
        {
            return db.Proyectoes;
        }

        // GET: api/Proyectoes/5
        [ResponseType(typeof(Proyecto))]
        public IHttpActionResult GetProyecto(string estado, decimal? monto, DateTime? desde, DateTime? hasta, string textoTitulo, string textoDesc, string cedula)
        {
            var filtrados = db.Proyectoes.Include("Usuario").ToList();

            if (estado == null && monto == 0 && desde == null && hasta == null && textoDesc == null && textoTitulo == null && cedula == null)
            {
                var sinfiltro = db.Proyectoes.Include("Usuario").ToList();
                return Ok(sinfiltro);
            }

            if (estado != null && estado.Length > 0)
            {
                filtrados = filtrados.Where(p => p.Estado == estado).ToList();
            }
            if (desde != null && hasta != null)
            {
                filtrados = filtrados.Where(p => p.FechaCreacion > desde).Where(p => p.FechaCreacion < hasta).ToList();
            }

            if (monto > 0)
            {
                filtrados = filtrados.Where(p => p.MontoTotal <= monto).ToList();
            }

            if (cedula != null && cedula.Length > 0)
            {
                filtrados = filtrados.Where(p => p.Usuario.Cedula == cedula).ToList();
            }


            if (textoTitulo != null && textoTitulo.Length > 0)
            {
                filtrados = filtrados.Where(p => p.Titulo.Contains(textoTitulo)).ToList();
            }

            if (textoDesc != null && textoDesc.Length > 0)
            {
                filtrados = filtrados.Where(x => x.Descripcion.Contains(textoDesc)).ToList();
            }
            return Ok(filtrados);
        
        }

        // PUT: api/Proyectoes/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutProyecto(int id, Proyecto proyecto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != proyecto.Id)
            {
                return BadRequest();
            }

            db.Entry(proyecto).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProyectoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Proyectoes
        [ResponseType(typeof(Proyecto))]
        public IHttpActionResult PostProyecto(Proyecto proyecto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Proyectoes.Add(proyecto);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = proyecto.Id }, proyecto);
        }

        // DELETE: api/Proyectoes/5
        [ResponseType(typeof(Proyecto))]
        public IHttpActionResult DeleteProyecto(int id)
        {
            Proyecto proyecto = db.Proyectoes.Find(id);
            if (proyecto == null)
            {
                return NotFound();
            }

            db.Proyectoes.Remove(proyecto);
            db.SaveChanges();

            return Ok(proyecto);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProyectoExists(int id)
        {
            return db.Proyectoes.Count(e => e.Id == id) > 0;
        }
    }
}