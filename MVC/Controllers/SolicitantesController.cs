using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Dominio;
using EF;

namespace MVC.Controllers
{
    public class SolicitantesController : Controller
    {
        private PrestamosContext db = new PrestamosContext();

        // GET: Inversores
        public ActionResult Index()
        {
            if (Session["rol"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            if (Session["rol"].ToString() != "Solicitante")
            {
                return RedirectToAction("Index", "Inversor");
            }
            int id = int.Parse(Session["id"].ToString());

            return View(db.Proyectoes.Where(p => p.Usuario.Id == id).ToList());
           
        }

        public ActionResult Details(int? id)
        {
            if (Session["rol"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            if (Session["rol"].ToString() != "Solicitante")
            {
                return RedirectToAction("Index", "Inversor");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Proyecto proyecto = db.Proyectoes.Find(id);
            if (proyecto == null)
            {
                return HttpNotFound();
            }
            return View(proyecto);
        }



        // GET: Solicitantes/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Solicitantes/Create
        //// Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        //// más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "Id,Cedula,Nombre,Apellido,FechaNacimiento,Celular,Email,Password")] Solicitante solicitante)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Usuarios.Add(solicitante);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(solicitante);
        //}

        //// GET: Solicitantes/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Solicitante solicitante = db.Usuarios.Find(id);
        //    if (solicitante == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(solicitante);
        //}

        //// POST: Solicitantes/Edit/5
        //// Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        //// más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Id,Cedula,Nombre,Apellido,FechaNacimiento,Celular,Email,Password")] Solicitante solicitante)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(solicitante).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(solicitante);
        //}

        //// GET: Solicitantes/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Solicitante solicitante = db.Usuarios.Find(id);
        //    if (solicitante == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(solicitante);
        //}

        //// POST: Solicitantes/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Solicitante solicitante = db.Usuarios.Find(id);
        //    db.Usuarios.Remove(solicitante);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
