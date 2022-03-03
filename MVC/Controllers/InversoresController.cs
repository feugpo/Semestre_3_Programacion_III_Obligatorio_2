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
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Text;

namespace MVC.Controllers
{
    public class InversoresController : Controller
    {
        private PrestamosContext db = new PrestamosContext();

        // GET: Inversores
        public ActionResult Index()
        {  if (Session["rol"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            if (Session["rol"].ToString() != "Inversor")
            {
                return RedirectToAction("Index", "Solicitante");
            }
          
            return View(db.Proyectoes.ToList());
        }

        public ActionResult Details(int? id)
        {
              if (Session["rol"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            if (Session["rol"] == null || Session["rol"].ToString() != "Inversor")
            {
                return RedirectToAction("Index", "Solicitante");
            }
          
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Proyecto proyecto = db.Proyectoes.Include("Usuario").Where(p => p.Id == id).SingleOrDefault();
            if (proyecto == null)
            {
                return HttpNotFound();
            }

            var inversion = db.Financiamientos.Where(f => f.Proyecto.Id == proyecto.Id).Select(f => f.Monto).ToList().ToArray();

            decimal total = 0;

            for(int x = 0; x<inversion.Count(); x++)
            {
               
              total += decimal.Parse(inversion[x].ToString());
               
            }

            ViewBag.monto = proyecto.MontoTotal - total;
            return View(proyecto);
        }

       // GET: Inversores/Create
        public ActionResult Create()
        {
            if (Session["rol"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            if (Session["rol"].ToString() != "Inversor")
            {
                return RedirectToAction("Index", "Solicitante");
            }
            return View();
        }

        //POST: Inversores/Create
        //Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse.Para obtener
        //más información vea https://go.microsoft.com/fwlink/?LinkId=317598.

       [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Cedula,Nombre,Apellido,FechaNacimiento,Celular,Email,ConfirmPassword,Password,MontoMaximo,Presentacion")] Inversor inversor)
        {
            if (ModelState.IsValid)
            {
                db.Usuarios.Add(inversor);
                db.SaveChanges();

                return RedirectToAction("Login", "Home");
            }

            return RedirectToAction("Login","Home");
        }

        [HttpPost]
        public ActionResult Details(int? id, string monto)  
        {
          
            Proyecto proyecto = db.Proyectoes.Include("Usuario").Where(p => p.Id == id).SingleOrDefault();
            var inversion = db.Financiamientos.Where(f => f.Proyecto.Id == proyecto.Id).Select(f => f.Monto).ToList().ToArray();

            decimal Monto = decimal.Parse(monto);
            decimal total = 0;

            for (int x = 0; x < inversion.Count(); x++)
            {

                total += decimal.Parse(inversion[x].ToString());

            }

            int inv = int.Parse(Session["id"].ToString());
            Inversor usu = db.Usuarios.Where(u => u.Id == inv).SingleOrDefault() as Inversor;
       
            if (Monto <= usu.MontoMaximo && Monto <= proyecto.MontoTotal-total && Monto <= proyecto.MontoTotal)
            {
                Financiamiento finan = new Financiamiento
                {
                    FechaFinanciacion = DateTime.Today,
                    FKInversor = int.Parse(Session["id"].ToString()),
                    FKProyecto = proyecto.Id,
                    Monto = decimal.Parse(monto)
                  
                };

                HttpClient financiacion = new HttpClient();
                Uri url = new Uri("http://localhost:55766/api/Financiamientos");

                Task<HttpResponseMessage> respuesta = financiacion.PostAsJsonAsync(url, finan);
                respuesta.Wait();

                if (respuesta.Result.IsSuccessStatusCode)
                {
                    Task<string> contenidoRespuesta = respuesta.Result.Content.ReadAsStringAsync();
                    contenidoRespuesta.Wait();

                    string json = contenidoRespuesta.Result;
                    Financiamiento dadoDeAlta = JsonConvert.DeserializeObject<Financiamiento>(json);
                    return RedirectToAction("Index");
                }
                return View(proyecto);
            }
            return View(proyecto);
        }

        











        public ActionResult VerMisInversiones()
        {
            if (Session["rol"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            if (Session["rol"].ToString() != "Inversor")
            {
                return RedirectToAction("Index", "Solicitante");
            }
            // GetFinanciamiento
            int id = int.Parse(Session["id"].ToString());

            string ide = JsonConvert.SerializeObject(id);
            var StringContent = new StringContent(ide);
            HttpClient financiacion = new HttpClient();
            Uri url = new Uri("http://localhost:55766/api/Financiamientos?id="+id);

            List<Financiamiento> lista = new List<Financiamiento>();

            Task<HttpResponseMessage> respuesta = financiacion.GetAsync(
                url);
            respuesta.Wait();
            if (respuesta.Result.IsSuccessStatusCode)
            {
                Task<string> contenidoRespuesta = respuesta.Result.Content.ReadAsStringAsync();
                contenidoRespuesta.Wait();

                string json = contenidoRespuesta.Result;
               
                    var formatter = new JsonSerializerSettings()
                    {
                        TypeNameHandling = TypeNameHandling.Auto
                    };

                   
                    lista = JsonConvert.DeserializeObject<List<Financiamiento>>(json, formatter);

                    return View(lista);
                }
            return View(lista);
            }

        



        public ActionResult FiltrarProyecto()
         {
            int id = int.Parse(Session["id"].ToString());

            if (Session["rol"].ToString() == "Solicitante")
            {
                return View(db.Proyectoes.Where(p => p.Usuario.Id == id).ToList());
            }

            return View(db.Proyectoes.ToList());
        }


        [HttpPost]
        public ActionResult FiltrarProyecto(string estado, decimal? monto, DateTime? desde, DateTime? hasta, string textoTitulo, string textoDesc, string cedula)
        {
            int id = int.Parse(Session["id"].ToString());
            var usuario = db.Usuarios.Where(u => u.Id == id).SingleOrDefault();


            if (Session["rol"].ToString() == "Solicitante") cedula = usuario.Cedula;

            var filtrados = db.Proyectoes.Include("Usuario").ToList();

            if (estado == null && monto == 0 && desde == null && hasta == null && textoDesc == null && textoTitulo == null && cedula == null)
            {
                var sinfiltro = db.Proyectoes.Include("Usuario").ToList();
                return View(sinfiltro);
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
            return View(filtrados);
        }

        //#region WEBAPI FILTRADO
        //[HttpPost]
        //public ActionResult FiltrarProyecto(string estado, decimal? monto, DateTime? desde, DateTime? hasta, string textoTitulo, string textoDesc, string cedula)
        //{

        //    //var filtrados = db.Proyectoes.Include("Usuario").ToList();

        //    var proyecto = new
        //    {
        //        Estado = estado,
        //        MontoTotal = monto,
        //        FechaCreacion = desde,
        //        Descripcion = textoDesc,
        //        Titulo = textoTitulo,
        //        Usuario = db.Usuarios.Where(u => u.Cedula == cedula).SingleOrDefault()
        //    };

        //    HttpClient filtrar = new HttpClient();
        //    Uri url = new Uri("http://localhost:55766/api/Proyectos?estado="+estado+"&monto="+monto+"&desde="+desde+"&hasta="+hasta+"&textoTitulo="+textoTitulo+"&textoDesc="+textoDesc+"&cedula="+cedula);

        //    List<Proyecto> lista = new List<Proyecto>();

        //    Task<HttpResponseMessage> respuesta = filtrar.GetAsync(url);
        //    respuesta.Wait();

        //    if (respuesta.Result.IsSuccessStatusCode)
        //    {
        //        Task<string> contenidoRespuesta = respuesta.Result.Content.ReadAsStringAsync();
        //        contenidoRespuesta.Wait();

        //        string json = contenidoRespuesta.Result;

        //        var formatter = new JsonSerializerSettings()
        //        {
        //            TypeNameHandling = TypeNameHandling.Auto
        //        };


        //        lista = JsonConvert.DeserializeObject<List<Proyecto>>(json, formatter);

        //        return View(lista);
        //    }
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
