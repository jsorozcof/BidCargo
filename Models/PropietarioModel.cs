using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace BidCargo_.Models
{
    public class PropietarioInputModel
    {
        public string nit { get; set; }
        public int tipoPersona { get; set; }
        public string razonSolical { get; set; }
        public string nombreContacto { get; set; }
        public string apellidoContacto { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string numeroDocumento { get; set; }
        public int tipoDocumento_id { get; set; }
        public int ciudad_id { get; set; }
        public int idDepartamento = 0;
        public string direccion { get; set; }
        public string telefonoFijo { get; set; }
        public string telefonoCelular { get; set; }
        public string correoElectronico { get; set; }
        public bool terminosYCondiciones { get; set; }
        public bool autorizacionDatosPersonales { get; set; }
        public bool politicaTratamientoDatos { get; set; }
    }
     
    public class PropNaturalInputModel
    {
        public string nombres { get; set; }
        public string apellidos { get; set; }
        public int fk_TipoDocumento { get; set; }
        public string numeroDocumento { get; set; }
        public string direccion { get; set; }
        public string telefonoFijo { get; set; }
        public int fk_idciudad { get; set; }
        public int fk_usuario { get; set; }
    }

    public class propJuridicoInputModel
    {
        public string razonSocial { get; set; }
        public string nit { get; set; }
        public string nombreContacto { get; set; }
        public string apellidoContacto { get; set; }
        public string direccion { get; set; }
        public string telefonoFijo { get; set; }
        public int fk_idciudad { get; set; }
        public int fk_usuario { get; set; }
    }

    public class PropietarioViewModel
    {
        public int tipoUsuario { get; set; }
        public string nombreTipo
        {
            get
            {
                if (tipoUsuario == 1)
                {
                    return "Natural";
                }
                else
                {
                    return "Juridico";
                }
            }
        }
        public string nombres { get; set; }
        public string apellidos { get; set; }
        public string telefono { get; set; }
        public string cedula { get; set; }
        public string correo { get; set; }
    }

    public class PropNaturalSoporteInputModel
    {
        public string Rut { get; set; }
        public string cc { get; set; }
        public string certificacionBancaria { get; set; }
    }

    public class PropJuridicoSoporteInputModel
    {
        public string Rut { get; set; }
        public string CcRepresentante { get; set; }
        public string CamaraComercio { get; set; }
        public string Nit { get; set; }
        public string certificacionBancaria { get; set; }
    }

    public class PropietarioFileInptModel
    {
        //Natural
        [DisplayName("RUT")]
        public HttpPostedFileBase RUTNatural { get; set; }
        [DisplayName("CC")]
        public HttpPostedFileBase CCNatural { get; set; }
        [DisplayName("Certificación Bancaria")]
        public HttpPostedFileBase CBNatural { get; set; }

        //Juridico
        [DisplayName("RUT")]
        public HttpPostedFileBase RUTJuridica { get; set; }
        [DisplayName("CC")]
        public HttpPostedFileBase CCJuridica { get; set; }
        [DisplayName("Certificación Bancaria")]
        public HttpPostedFileBase CBJuridica { get; set; }
        [DisplayName("Camara de Comercio")]
        public HttpPostedFileBase CamaraComercio { get; set; }
        [DisplayName("NIT")]
        public HttpPostedFileBase NIT { get; set; }
        
    }

    public class PropietarioPathInptModel
    {
        public int IdPropietario { get; set; }
        public int idUsuario { get; set; }
        public string RUT { get; set; }
        public string CC { get; set; }
        public string CertificacionBancaria { get; set; }
        public string CamaraComercio { get; set; }
        public string NIT { get; set; }
    }
     
    public class ContraOfertaPropietario
    {
        public int Codigo { get; set; }
        public string UbicacionVehiculo { get; set; }
        public string FechaHora { get; set; }
        public string Observacion { get; set; }
        public int IdVehiculo { get; set; }
        public int IdConductor { get; set; }
        public int Costo { get; set; }
        public string PathDocumento { get; set; }
        public int CodigoOferta { get; set; }
        public HttpPostedFileBase FileDocumento { get; set; }
        public List<int> Empresas { get; set; }
        public int Estado { get; set; }
        public int Fk_Usuario { get; set; }

    }

}