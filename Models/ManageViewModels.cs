using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace BidCargo_.Models
{
    public class IndexViewModel
    {
        public bool HasPassword { get; set; }
        public IList<UserLoginInfo> Logins { get; set; }
        public string PhoneNumber { get; set; }
        public bool TwoFactor { get; set; }
        public bool BrowserRemembered { get; set; }
    }

    public class actualizarPassword
    {
        public string actual { get; set; }
        public string newpassword { get; set; }
        public string confirmnewpassword { get; set; }
    }

    public class preregistroInicial
    {
        public string razonSocial { get; set; }
        public string apellido { get; set; }
        public string nit { get; set; }
        public string telefono { get; set; }
        public string nombreContacto { get; set; }
        public string celularContacto { get; set; }
        public string correoContacto { get; set; }
        public int idTipoTransporte { get; set; }
        public int idTypeCompany { get; set; }
        public bool terminosYCondiciones { get; set; }
        public bool autorizacionDatosPersonales { get; set; }
        public bool politicaTratamientoDatos { get; set; }
    }

    public class PreRegistroPropietario
    {
        public string nit { get; set; }
        public int tipoPersona_id { get; set; }
        public string razonSolical { get; set; }
        public string nombreContacto { get; set; }
        public string apellidoContacto { get; set; }
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
    
    public class olvidoSuContrasena
    {
        public string email { get; set; }
    }
    public class contactanos
    {
        public string razonSocial { get; set; }
        public string nombreContacto { get; set; }
        public string celularContacto { get; set; }
        public string correoContacto { get; set; }
        public string comentariosContacto { get; set; }
    }

    public class Login
    {
        public string usuario { get; set; }
        public string contrasena { get; set; }
    }

    public class actualizarContrasena
    {
        public string numeroCelular { get; set; }
        public string contrasenaActual { get; set; }
        public string contrasenaNueva { get; set; }
        public string confirmarContrasena { get; set; }
    }
    public class dinalizarPerfil
    {
        public string usuario { get; set; }
        public string contrasena { get; set; }
    }

    public class storeoffer{
        public string idCliente { get; set; }
        public string codeOffer { get; set; }
        public int valorOferta { get; set; }
        public int coinTypesId { get; set; }
        public int dateOfServiceId { get; set; }
        public string typeCargo { get; set; }
        public string from { get; set; }
        public string to { get; set; }
        public string dayTraveler { get; set; }
        public string from2 { get; set; }
        public string to2 { get; set; }
        public string dayTraveler2 { get; set; }
        public string directionFrom { get; set; }
        public string directionTo { get; set; }
        public string typeMerchandise { get; set; }
        public string bulkCargo { get; set; }
        public string containerType { get; set; }
        public string containerWeight { get; set; }
        public int idWeightMeasured { get; set; }
        public string[] nacDtaOmt { get; set; }
        public string longM { get; set; }
        public string widthM { get; set; }
        public string highM { get; set; }
        public string tied { get; set; }
        public string plates { get; set; }
        public string loose { get; set; }
        public int idDimensions { get; set; }
        public string valueMerchandise { get; set; }
        public string valueMerchandise2 { get; set; }
        public string numberUnits { get; set; }
        public string numberTons { get; set; }
        public string factoring { get; set; }
        public string payDays { get; set; }
        public string[] stuffLots { get; set; }
        public string observation { get; set; }
        public int idTipoContenedor { get; set; }
        public int idFactoring { get; set; }
        public string[] idNACDTAOMT { get; set; }
        public string idTipoTransporte { get; set; }
        public int idTipoDeVehiculo { get; set; }
        public int idTipoOrganizar { get; set; }
        public int idTipoDeCotizacion { get; set; }
        public int Id { get; set; }
        public string Description { get; set; }



    }
    public class completarRegistro
    {
        public string razonSocial { get; set; }
        public string nit { get; set; }
        public string telefono { get; set; }
        public string direccion { get; set; }
        public string nombreContacto { get; set; }
        public string apellidoContacto { get; set; }
        public string celularContacto { get; set; }
        public string correoContacto { get; set; }
        public string cargoContacto { get; set; }
        public string idDepartamento { get; set; }
        public string idCiudad { get; set; }
        public string idTypeCompany { get; set; }
        public string email { get; set; }
        public string fechanacimiento { get; set; }

        public int percentCommission { get; set; }
        public string[] archivoCargado { get; set; }
    }

    
    public class ManageLoginsViewModel
    {
        public IList<UserLoginInfo> CurrentLogins { get; set; }
        public IList<AuthenticationDescription> OtherLogins { get; set; }
    }

    public class FactorViewModel
    {
        public string Purpose { get; set; }
    }

    public class SetPasswordViewModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "{0} debe tener al menos {2} caracteres de longitud.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña nueva")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirme la contraseña nueva")]
        [Compare("NewPassword", ErrorMessage = "La contraseña nueva y la contraseña de confirmación no coinciden.")]
        public string ConfirmPassword { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña actual")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} debe tener al menos {2} caracteres de longitud.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña nueva")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirme la contraseña nueva")]
        [Compare("NewPassword", ErrorMessage = "La contraseña nueva y la contraseña de confirmación no coinciden.")]
        public string ConfirmPassword { get; set; }
    }

    public class AddPhoneNumberViewModel
    {
        [Required]
        [Phone]
        [Display(Name = "Número de teléfono")]
        public string Number { get; set; }
    }

    public class VerifyPhoneNumberViewModel
    {
        [Required]
        [Display(Name = "Código")]
        public string Code { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Número de teléfono")]
        public string PhoneNumber { get; set; }
    }

    public class ConfigureTwoFactorViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
    }

    public class Perfil
    {
        public string nombre { get; set; }
        public string apellidoPaterno { get; set; }
        public string cedula { get; set; }
        public int idGenero { get; set; }
        public string numeroCelular { get; set; }
        public string email  { get; set; }
        public string fechanacimiento { get; set; }
        public string usuarioFaceBook { get; set; }
        public string usuarioSnapchat { get; set; }
        public string usuarioInstagram { get; set; }
        public string direccion { get; set; }
        public int idPais { get; set; }
        public int idCiudad { get; set; }
        public string profileTypeCompany { get; set; }
        public string contrasena { get; set; }
        public int idDepartamento { get; set; }
        public string idTypeCompany { get; set; }
    }

    public class UsuarioSistema
    {
        public int Id { get; set; }
        public int AccionId { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Celular { get; set; }
        public string Correo { get; set; }
        public string Usuario { get; set; }
        public string Contrasena { get; set; }
        public int Estado { get; set; }
        public int CompanyId { get; set; }
        public int PerfilId { get; set; }
        public DateTime Creado { get; set; }


    }
}