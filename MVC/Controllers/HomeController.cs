
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EF;
using WCF;

namespace MVC.Controllers
{
    public class HomeController : Controller
    {

        private PrestamosContext db = new PrestamosContext();
        public ActionResult Index()
        {
            if (Session["rol"].ToString() != null)
            {

                string session = Session["rol"].ToString();

                if (session == "Inversor")
                {
                    return Redirect("Inversores/Index");
                }
                if (session == "Solicitante")
                {
                    return Redirect("Solicitante/Index");
                }
            }
            return View();
        }



      






        public ActionResult Login(string mensaje)
        {
            ViewBag.Mensaje = mensaje;
            Session.Abandon();
            return View();

        }

        [HttpPost]
        public ActionResult Login(string cedula, string password)
        {

            string errormensaje = "";

            ServicioPrestamos proxy = new ServicioPrestamos();

            var autenticado = proxy.Login(cedula, password);
            var verificarPass = proxy.contra(autenticado.Nombre, autenticado.Apellido, cedula);

            if (autenticado != null)
            {
                Session["rol"] = autenticado.rol;
                Session["nombre"] = autenticado.Nombre;
                Session["id"] = autenticado.Id;
               
                if(verificarPass == password)
                {
                    return RedirectToAction("CambiarPass", new { mensaje = errormensaje });
                }

                if(autenticado.rol =="Solicitante") return RedirectToAction("Index", "Solicitantes");

                else if (autenticado.rol == "Inversor") return RedirectToAction("Index", "Inversores");

            }
            
                errormensaje = "Usuario o contraseña incorrectos";
                return RedirectToAction("Login", new { mensaje = errormensaje });
            
        }



        public ActionResult PreCarga()
        {
            
            string rutaAplicacion = AppDomain.CurrentDomain.BaseDirectory;
            string nombreArchivo = "Solicitantes.txt";
            string rutaCompleta = Path.Combine(rutaAplicacion, nombreArchivo);

            FileStream fs = new FileStream(rutaCompleta, FileMode.Open);
            StreamReader sr = new StreamReader(fs);


            ServicioPrestamos proxy = new ServicioPrestamos();

            List<DTOSolicitante> retorno = new List<DTOSolicitante>();

            string linea = sr.ReadLine();
            while ((linea != null))
            {
                DTOSolicitante unS = proxy.ObtenerDesdeString(linea, "|");
                if (unS != null)
                {
                    linea = sr.ReadLine();
                    retorno.Add(unS);
                }

            }

            bool ok = proxy.PrecargaUsuario(retorno);
            if (ok) return Redirect("/Home/Login");
            else return RedirectToAction("Create");
        }
    

        public ActionResult PreCargaProyectos()
        {



            string rutaAplicacion = AppDomain.CurrentDomain.BaseDirectory;
            string nombreArchivo = "Proyectos.txt";
            string rutaCompleta = Path.Combine(rutaAplicacion, nombreArchivo);

            FileStream fs1 = new FileStream(rutaCompleta, FileMode.Open);
            StreamReader sr = new StreamReader(fs1);


            ServicioPrestamos proxy = new ServicioPrestamos();

            List<DTOProyecto> retorno = new List<DTOProyecto>();

            string linea = sr.ReadLine();
            while ((linea != ""))
            {
                DTOProyecto unP = proxy.ObtenerDesdeStringProyectos(linea, "|");
                if (unP != null)
                {
                    linea = sr.ReadLine();
                    retorno.Add(unP);
                }

            }

            bool ok = proxy.PrecargaProyecto(retorno);
            if (ok) return Redirect("/Home/Login");
            else return RedirectToAction("Create");
        }


        public ActionResult CambiarPass(string error)
        {
            ViewBag.Mensaje = error;
            return View();
        }
        [HttpPost]
        public ActionResult CambiarPass (string Password, string ConfPassword)
        {
            ServicioPrestamos proxy = new ServicioPrestamos();
            string id = Session["id"].ToString();

            bool confirmar = proxy.CambiarPass(Password, ConfPassword, id);

            if (confirmar) return Redirect("Index");

            else return RedirectToAction("CambiarPass", new { mensaje = "Algo salio mal, intentalo de nuevo" });
        }

    }

   

}