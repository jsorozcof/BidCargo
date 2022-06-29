using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BidCargo_.Models
{
    public class ConductorInputModel : UsuarioInputModel
    {
        public string tipoDocumento { get; set; }
        public string numeroDocumento { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string direccion { get; set; }
        public string codigoDepartamento { get; set; }
        public string codigoCiudad { get; set; }
        public string numeroLicencia { get; set; }
        public string telefonoFijo { get; set; }
        public bool terminosYCondiciones { get; set; }
        public bool autorizacionDatosPersonales { get; set; }
        public bool politicaTratamientoDatos { get; set; }
        public HttpPostedFileBase cedula { get; set; }
        public HttpPostedFileBase licencia { get; set; }
        public HttpPostedFileBase seguridadSocial { get; set; }
        public HttpPostedFileBase cursos { get; set; }
    }

    public class ConductorViewModel
    {
        public int idConductor { get; set; }
        public int tipoDocumento { get; set; }
        public string nombreTipoDocumento
        {
            get
            {
                if (tipoDocumento == 1)
                {
                    return "Cédula de ciudadanía";
                }
                else if (tipoDocumento == 2)
                {
                    return "Cédula extranjera";
                }
                else
                {
                    return "Pasaporte";
                }
            }
        }
        public string numeroDocumento { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string correo { get; set; }
        public string telefonoMovil { get; set; }
    }

    public class ConductorFileInputModel
    {
        [Required(ErrorMessage = "La cedula es requerida")]
        public HttpPostedFileBase cedula { get; set; }
        [Required(ErrorMessage = "La licencia es requerida")]
        public HttpPostedFileBase licencia { get; set; }
        [Required(ErrorMessage = "La seguridad social es requerida")]
        public HttpPostedFileBase seguridadSocial { get; set; }
        public HttpPostedFileBase cursos { get; set; }
    }

    public class ConductorPathInputModel
    {
        public int idConductor { get; set; }
        public string pathCedula { get; set; }
        public string pathLicencia { get; set; }
        public string pathSeguridadSoc { get; set; }
        public string pathCursos { get; set; }
    }



    public class CondutorvisualizacionComplera
    {



        public string tipoDocumento { get; set; }
        public string numeroDocumento { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string direccion { get; set; }
        public string codigoDepartamento { get; set; }
        public string codigoCiudad { get; set; }
        public string numeroLicencia { get; set; }
        public string telefonoFijo { get; set; }
        public string pathCedula { get; set; }
        public string pathLicencia { get; set; }
        public string pathSeguridadSoc { get; set; }
        public string pathCursos { get; set; }

        public HttpPostedFileBase cedula { get; set; }
        public HttpPostedFileBase licencia { get; set; }
        public HttpPostedFileBase seguridadSocial { get; set; }
        public HttpPostedFileBase cursos { get; set; }



    }


    public class ConductorExel
    {
        public int sk_conductor { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string nroLicencia { get; set; }
        public string telefonofijo { get; set; }
        public int idEstadoCliente { get; set; }
        public string idEstadoClientestr { get; set; }


    }



}