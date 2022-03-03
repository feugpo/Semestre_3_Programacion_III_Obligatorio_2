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
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de interfaz "IService1" en el código y en el archivo de configuración a la vez.
    [ServiceContract]
    public interface IServicioPrestamos
    {

        [OperationContract]
        bool PrecargaUsuario(List<DTOSolicitante> sol);


        [OperationContract]
        DTOSolicitante ObtenerDesdeString(string usu, string delimitador);


        [OperationContract]
        DTOProyecto ObtenerDesdeStringProyectos(string pro, string delimitador);


        [OperationContract]
        DTOUsuario Login(string cedula, string pass);

        [OperationContract]
        bool CambiarPass(string pass, string confpass, string id);

        // TODO: agregue aquí sus operaciones de servicio
    }


    // Utilice un contrato de datos, como se ilustra en el ejemplo siguiente, para agregar tipos compuestos a las operaciones de servicio.
    [DataContract]
    public class DTOSolicitante
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Nombre { get; set; }

        [DataMember]
        public string Apellido { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string Celular { get; set; }

        [DataMember]
        public string Cedula { get; set; }
        [DataMember]
        public DateTime FechaNacimiento { get; set; }

        [DataMember]
        public string Password { get; set; }

        [DataMember]
        public List<Proyecto> Proyectos { get; set; }

    }

    public class DTOUsuario
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Nombre {get; set; }
        [DataMember]
        public string Apellido { get; set; }
        [DataMember]
        public string Cedula { get; set; }
        [DataMember]
        public string rol { get; set; }
    }

    public class DTOProyecto
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int IdUsuario { get; set; }

        [DataMember]
        public string Titulo { get; set; }

        [DataMember]
        public string Descripcion { get; set; }
        [DataMember]
        public decimal MontoTotal { get; set; }
        [DataMember]
        public decimal MontoTotalConInteres { get; set; }
        [DataMember]
        public decimal MontoCuotas { get; set; }

        [DataMember]
        public int Cuotas { get; set; }

        [DataMember]
        public string Imagen { get; set; } //COMO ES EL FORMATO?

        [DataMember]
        public decimal TasaInteres { get; set; }

        [DataMember]
        public string Estado { get; set; }
        [DataMember]
        public string Experiencia { get; set; }
        [DataMember]
        public int CantidadIntegrantes { get; set; }
 
        [DataMember]
        public string Tipo { get; set; }
        

        [DataMember]
        public DateTime FechaCreacion { get; set; }
    }
}
