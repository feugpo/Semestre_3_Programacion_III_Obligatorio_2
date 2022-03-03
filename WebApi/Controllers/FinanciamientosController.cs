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
    public class FinanciamientosController : ApiController
    {
        private PrestamosContext db = new PrestamosContext();

        // GET: api/Financiamientos
        public IQueryable<Financiamiento> GetFinanciamientos()
        {
            return db.Financiamientos;
        }

        // GET: api/Financiamientos/5
        [ResponseType(typeof(Financiamiento))]
       public IHttpActionResult GetFinanciamiento(string id)
        {
            
            int ide = int.Parse(id);
            List<Financiamiento> financiamientos = db.Financiamientos.Where(f => f.Inversor.Id == ide).ToList();
            if (financiamientos != null)
            {
                foreach (var finan in financiamientos) { 

                finan.Inversor = db.Usuarios.Where(u => u.Id == finan.FKInversor).SingleOrDefault() as Inversor;
                finan.Proyecto = db.Proyectoes.Where(p => p.Id == finan.FKProyecto).SingleOrDefault();
            }
            }

            return Ok(financiamientos);
        }

        // PUT: api/Financiamientos/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutFinanciamiento(int id, Financiamiento financiamiento)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != financiamiento.Id)
            {
                return BadRequest();
            }

            db.Entry(financiamiento).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FinanciamientoExists(id))
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

        // POST: api/Financiamientos
        [ResponseType(typeof(Financiamiento))]
        public IHttpActionResult PostFinanciamiento(Financiamiento financiamiento)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Financiamientos.Add(financiamiento);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = financiamiento.Id }, financiamiento);
        }

        // DELETE: api/Financiamientos/5
        [ResponseType(typeof(Financiamiento))]
        public IHttpActionResult DeleteFinanciamiento(int id)
        {
            Financiamiento financiamiento = db.Financiamientos.Find(id);
            if (financiamiento == null)
            {
                return NotFound();
            }

            db.Financiamientos.Remove(financiamiento);
            db.SaveChanges();

            return Ok(financiamiento);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FinanciamientoExists(int id)
        {
            return db.Financiamientos.Count(e => e.Id == id) > 0;
        }
    }
}