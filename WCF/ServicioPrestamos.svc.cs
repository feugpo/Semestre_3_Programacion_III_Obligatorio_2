using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using Dominio;
using EF;

namespace WCF
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de clase "Service1" en el código, en svc y en el archivo de configuración.
    // NOTE: para iniciar el Cliente de prueba WCF para probar este servicio, seleccione Service1.svc o Service1.svc.cs en el Explorador de soluciones e inicie la depuración.
    public class ServicioPrestamos : IServicioPrestamos
    {

        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }



        #region CARGAR PROYECTOS

        public DTOProyecto ObtenerDesdeStringProyectos(string pro, string delimitador)
        {


            string[] vecDatos = pro.Split(delimitador.ToCharArray());


            if (pro != "")
            {
                string D = vecDatos[14].ToString();
                DateTime date = DateTime.Parse((D));

               
                    DTOProyecto nuevo = new DTOProyecto()
                    {
                        Id = int.Parse(vecDatos[0]),
                        Titulo = vecDatos[1],
                        Descripcion = vecDatos[2],
                        Imagen = vecDatos[3],
                        MontoTotal = decimal.Parse(vecDatos[4]),
                        MontoTotalConInteres = decimal.Parse(vecDatos[5]),
                        MontoCuotas = decimal.Parse(vecDatos[6]),
                        Cuotas = int.Parse(vecDatos[7]),
                        TasaInteres = decimal.Parse(vecDatos[8]),
                        Estado = vecDatos[9],
                        Experiencia = vecDatos[10],
                        CantidadIntegrantes = int.Parse(vecDatos[11]),
                        IdUsuario = int.Parse(vecDatos[12]),
                        Tipo = vecDatos[13],
                        FechaCreacion = date,

                    };

                    if (nuevo != null) return nuevo;
                
            }
            return null;
        }

        public bool PrecargaProyecto(List<DTOProyecto> proyectos)
        {
            bool ok = false;

            for (int x = 0; x < proyectos.Count; x++)
            {
                using (PrestamosContext db = new PrestamosContext())
                {
                    int ide = proyectos[x].IdUsuario;
                    var id = db.Usuarios.Where(u => u.Id == ide).SingleOrDefault();

                    if(proyectos[x].Tipo == "Personal") { 
                    Personal nuevoPersonal = new Personal()
                    {
                        Id = proyectos[x].Id,
                        Titulo = proyectos[x].Titulo,
                        Descripcion = proyectos[x].Descripcion,
                        Imagen = proyectos[x].Imagen,
                        MontoTotal = proyectos[x].MontoTotal,
                        MontoTotalConInteres = proyectos[x].MontoTotalConInteres,
                        MontoCuotas = proyectos[x].MontoCuotas,
                        Cuotas = proyectos[x].Cuotas,
                        TasaInteres = proyectos[x].TasaInteres,
                        Estado = proyectos[x].Estado,
                        Experiencia = proyectos[x].Experiencia,
                        Usuario = id,
                        FechaCreacion = proyectos[x].FechaCreacion,

                    };
                    if (nuevoPersonal == null) return ok = false;
                    db.Proyectoes.Add(nuevoPersonal);
                    db.SaveChanges();
                }

                if (proyectos[x].Tipo == "Cooperativo")
                    {
                        Cooperativo nuevoCoop = new Cooperativo()
                        {
                            Id = proyectos[x].Id,
                            Titulo = proyectos[x].Titulo,
                            Descripcion = proyectos[x].Descripcion,
                            Imagen = proyectos[x].Imagen,
                            MontoTotal = proyectos[x].MontoTotal,
                            MontoTotalConInteres = proyectos[x].MontoTotalConInteres,
                            MontoCuotas = proyectos[x].MontoCuotas,
                            Cuotas = proyectos[x].Cuotas,
                            TasaInteres = proyectos[x].TasaInteres,
                            Estado = proyectos[x].Estado,
                            CantidadIntegrantes = proyectos[x].CantidadIntegrantes,
                            Usuario = id,
                            FechaCreacion = proyectos[x].FechaCreacion,

                        };
                        if (nuevoCoop == null) return ok = false;
                        db.Proyectoes.Add(nuevoCoop);
                        db.SaveChanges();
                    }

                }
                ok = true;
            }
            return ok;
        }

    


        #endregion

        #region CARGAR SOLICITANTES
        public DTOSolicitante ObtenerDesdeString(string usu, string delimitador)
        {


            string[] vecDatos = usu.Split(delimitador.ToCharArray());

           

            if (vecDatos != null)
            {
                    string D = vecDatos[4].ToString();
                    DateTime date = DateTime.Parse((D));
                string nuevapass = contra(vecDatos[2], vecDatos[3], vecDatos[1]);
                

                DTOSolicitante nuevo = new DTOSolicitante
                {
                    Id = int.Parse(vecDatos[0]),
                    Cedula = vecDatos[1],
                    Nombre = vecDatos[2],
                    Apellido = vecDatos[3],
                    Password = nuevapass,
                    FechaNacimiento = date,
                    Celular = vecDatos[5],
                    Email = vecDatos[6]

                };

                if (nuevo != null) return nuevo;
            }
            return null;
        }

        public string contra(string nombre, string apellido, string pass)
        {
            string nom = nombre.ToLower();
            string ap = apellido.ToUpper();
            nom = nom.Substring(0, 1);
            ap = ap.Substring(0, 1);

            return nom + ap + pass;
        }

        public bool PrecargaUsuario(List<DTOSolicitante> sol)
        {
            bool ok = false;

            for (int x = 0; x < sol.Count; x++) {
                using (PrestamosContext db = new PrestamosContext())
                {
                    Solicitante nuevo = new Solicitante()
                    {
                        Id = sol[x].Id,
                        Nombre = sol[x].Nombre,
                        Apellido = sol[x].Apellido,
                        Email = sol[x].Email,
                        Cedula = sol[x].Cedula,
                        Celular = sol[x].Celular,
                        Password = sol[x].Password,
                        Proyectos = sol[x].Proyectos,
                        FechaNacimiento = sol[x].FechaNacimiento
                    };
                    if (nuevo == null) return ok = false;
                    db.Usuarios.Add(nuevo);
                    db.SaveChanges();
                }
                ok = true;
            }
            return ok;
        }
        #endregion




        public DTOUsuario Login(string cedula, string pass)
        {
         
            using (PrestamosContext db = new PrestamosContext())
            {
                DTOUsuario autenticado = new DTOUsuario();

                var au = db.Usuarios.Where(u => u.Cedula == cedula).Where(u => u.Password == pass).SingleOrDefault();

                if (au is Solicitante)
                {
                    DTOUsuario sol = new DTOUsuario
                    {
                        Id = au.Id,
                        Nombre = au.Nombre,
                        Apellido = au.Apellido,
                        Cedula= au.Cedula,
                        rol = "Solicitante"
                    };

                    return sol;
                }


                else if (au is Inversor)
                {
                    DTOUsuario Inv = new DTOUsuario
                    {
                        Id = au.Id,
                        Nombre = au.Nombre,
                        Apellido = au.Apellido,
                        rol = "Inversor"
                    };
                    return Inv;
                }


                return autenticado;
            }
        }

        public bool CambiarPass(string pass, string confpass, string id)
        {
            bool retorno = false;

            int idusuario = int.Parse(id);

            if (pass == confpass) {
                using (PrestamosContext db = new PrestamosContext())
                {
                    var usu = db.Usuarios.Where(u => u.Id == idusuario).SingleOrDefault();

                    db.Usuarios.Attach(usu);
                    usu.Password = pass;
                    db.SaveChanges();


                    retorno = true;
                }
            }
            return retorno;
        }


    }
}








       

