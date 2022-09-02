using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using BidCargo_.Models;

public class ConnectionDataBase
{
    public class StoreProcediur
    {

        public DataTable validacionContrasenaActual(string numeroCelular)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
                SqlDataAdapter da = new SqlDataAdapter("SPValidacionContrasenaAntigua", con);
                da.SelectCommand.Parameters.Add("@numeroTelefonico", SqlDbType.VarChar).Value = numeroCelular;
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public DataTable guardarPrimerContacto(string nombrePrimerContacto, string telefonoPrimerContacto, string emailPrimerContacto, string razonSocial, string mensajeContacto)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
                SqlDataAdapter da = new SqlDataAdapter("SP_PrimerContacto", con);
                if (razonSocial != "") da.SelectCommand.Parameters.Add("@razonSocial", SqlDbType.VarChar).Value = razonSocial;
                if (nombrePrimerContacto != "") da.SelectCommand.Parameters.Add("@nombrePrimerContacto", SqlDbType.VarChar).Value = nombrePrimerContacto;
                if (emailPrimerContacto != "") da.SelectCommand.Parameters.Add("@emailPrimerContacto", SqlDbType.VarChar).Value = emailPrimerContacto;
                if (telefonoPrimerContacto != "") da.SelectCommand.Parameters.Add("@celularContacto", SqlDbType.VarChar).Value = telefonoPrimerContacto;
                if (mensajeContacto != "") da.SelectCommand.Parameters.Add("@mensajeContacto", SqlDbType.VarChar).Value = mensajeContacto;
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public DataTable actualizarContrasenaConfirmacion(string numeroCelular, byte[] contrasena, byte[] key, byte[] iv, byte[] contrasenaAntigua, byte[] keyAntigua, byte[] ivAntigua)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
                SqlDataAdapter da = new SqlDataAdapter("SPActualizarContrasenaConfirmacion", con);
                da.SelectCommand.Parameters.Add("@numeroCelular", SqlDbType.VarChar).Value = numeroCelular;
                da.SelectCommand.Parameters.Add("@contrasena", SqlDbType.VarBinary).Value = contrasena;
                da.SelectCommand.Parameters.Add("@Key", SqlDbType.VarBinary).Value = key;
                da.SelectCommand.Parameters.Add("@IV", SqlDbType.VarBinary).Value = iv;
                da.SelectCommand.Parameters.Add("@contrasenaAntigua", SqlDbType.VarBinary).Value = contrasenaAntigua;
                da.SelectCommand.Parameters.Add("@KeyAntigua", SqlDbType.VarBinary).Value = keyAntigua;
                da.SelectCommand.Parameters.Add("@IVAntigua", SqlDbType.VarBinary).Value = ivAntigua;
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public DataTable actualizarContrasenaUsuario(string numeroCelular, byte[] contrasena, byte[] key, byte[] iv, byte[] contrasenaAntigua, byte[] keyAntigua, byte[] ivAntigua)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
                SqlDataAdapter da = new SqlDataAdapter("SP_ActualizarContrasenaUsuario", con);
                da.SelectCommand.Parameters.Add("@numeroCelular", SqlDbType.VarChar).Value = numeroCelular;
                da.SelectCommand.Parameters.Add("@contrasena", SqlDbType.VarBinary).Value = contrasena;
                da.SelectCommand.Parameters.Add("@Key", SqlDbType.VarBinary).Value = key;
                da.SelectCommand.Parameters.Add("@IV", SqlDbType.VarBinary).Value = iv;
                da.SelectCommand.Parameters.Add("@contrasenaAntigua", SqlDbType.VarBinary).Value = contrasenaAntigua;
                da.SelectCommand.Parameters.Add("@KeyAntigua", SqlDbType.VarBinary).Value = keyAntigua;
                da.SelectCommand.Parameters.Add("@IVAntigua", SqlDbType.VarBinary).Value = ivAntigua;
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public DataTable getTypeCompanyByCustom(int op)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
                SqlDataAdapter da = new SqlDataAdapter("SP_getTypeCompany", con);
                da.SelectCommand.Parameters.Add("@op", SqlDbType.Int).Value = op;
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public DataTable GetPersonaByRol(int rol)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
                SqlDataAdapter da = new SqlDataAdapter("SP_ConsultarPersona", con);
                da.SelectCommand.Parameters.Add("@rol", SqlDbType.Int).Value = rol;
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;

            }
            catch (Exception)
            {

                throw;
            }
        }


        public DataTable ObtenerData(string SP)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
                SqlDataAdapter da = new SqlDataAdapter(SP, con);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public DataTable DeActivateUsers(int pidCliente = 0)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
                SqlDataAdapter da = new SqlDataAdapter("SP_DeActivateUsers", con);
                da.SelectCommand.Parameters.Add("@pidCliente", SqlDbType.Int).Value = pidCliente;
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public DataTable ConsultCity(int idDepartamento)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
                SqlDataAdapter da = new SqlDataAdapter("SP_CiudadXDepartamento", con);
                da.SelectCommand.Parameters.Add("@idDepartamento", SqlDbType.Int).Value = idDepartamento;
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public DataTable DeleteUsers(int pidCliente = 0)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
                SqlDataAdapter da = new SqlDataAdapter("SP_DeleteUsers", con);
                da.SelectCommand.Parameters.Add("@pidCliente", SqlDbType.Int).Value = pidCliente;
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public DataTable DeleteContraOfertaPropietario(int ofertaId)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
                SqlDataAdapter da = new SqlDataAdapter("SP_DeleteContraOfertaPropietario", con);
                da.SelectCommand.Parameters.Add("@ofertaId", SqlDbType.Int).Value = ofertaId;
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;

            }
            catch (Exception)
            {

                throw;
            }
        }

        

        public DataTable DeAprovedUser(int pidCliente = 0)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
                SqlDataAdapter da = new SqlDataAdapter("SP_DeAprovedUser", con);
                da.SelectCommand.Parameters.Add("@pidCliente", SqlDbType.Int).Value = pidCliente;
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public DataTable getCitybyDepatamento(string SP, int pidDepartamento)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
                SqlDataAdapter da = new SqlDataAdapter(SP, con);
                da.SelectCommand.Parameters.Add("@pidDepartamento", SqlDbType.Int).Value = pidDepartamento;
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;

            }
            catch (Exception)
            {

                throw;
            }
        }
        public DataTable actualizarLikes(int idPlan, int idCliente)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
                SqlDataAdapter da = new SqlDataAdapter("SP_ActualizarLikes", con);
                da.SelectCommand.Parameters.Add("@idPlanTuristico", SqlDbType.Int).Value = idPlan;
                da.SelectCommand.Parameters.Add("@idCliente", SqlDbType.Int).Value = idCliente;
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public DataTable ValidarUsuarioTabla(string usuario)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
                SqlDataAdapter da = new SqlDataAdapter("SP_ValidarUsuarioTabla", con);
                da.SelectCommand.Parameters.Add("@Usuario", SqlDbType.VarChar).Value = usuario;
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable ValidarNombreUsuario(string usuario)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
                SqlDataAdapter da = new SqlDataAdapter("SP_ValidacionNombreUsuario", con);
                da.SelectCommand.Parameters.Add("@nombreUsuario", SqlDbType.VarChar).Value = usuario;
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable ValidarIngresoPYC(string usuario, string macAddress)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
                SqlDataAdapter da = new SqlDataAdapter("SP_ValidacionIngresoUsuario", con);
                da.SelectCommand.Parameters.Add("@Usuario", SqlDbType.VarChar).Value = usuario;
                da.SelectCommand.Parameters.Add("@macAddress", SqlDbType.VarChar).Value = macAddress;
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch (Exception)
            {
                throw;
            }

        }


        public DataTable ValidarIngresoUsuario(string usuario, string macAddress)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
                SqlDataAdapter da = new SqlDataAdapter("SPValidacionIngresoCliente", con);
                da.SelectCommand.Parameters.Add("@Usuario", SqlDbType.VarChar).Value = usuario;
                da.SelectCommand.Parameters.Add("@macAddress", SqlDbType.VarChar).Value = macAddress;
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;

            }
            catch (Exception)
            {

                throw;
            }
        }
        public DataTable consultarCliente(int idCliente)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
                SqlDataAdapter da = new SqlDataAdapter("SP_ConsultarCliente", con);
                da.SelectCommand.Parameters.Add("@idCliente", SqlDbType.VarChar).Value = idCliente;
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public DataTable getUsuarioSistema(int idCliente, int userId)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
                SqlDataAdapter da = new SqlDataAdapter("SP_GetUsuarioSistema", con);
                da.SelectCommand.Parameters.Add("@CompanyId", SqlDbType.VarChar).Value = idCliente;
                da.SelectCommand.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;

            }
            catch (Exception)
            {

                throw;
            }
        }


        public DataTable getClientProfile(int idCliente)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
                SqlDataAdapter da = new SqlDataAdapter("SP_getDataProfileClient", con);
                da.SelectCommand.Parameters.Add("@pidCliente", SqlDbType.VarChar).Value = idCliente;
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;

            }
            catch (Exception)
            {

                throw;
            }
        }
        public DataTable getTypeCotizacion(int pidtypeCargo = 0)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
                SqlDataAdapter da = new SqlDataAdapter("SP_TipoDeCotizacion", con);
                da.SelectCommand.Parameters.Add("@pidTypeCargo", SqlDbType.Int).Value = pidtypeCargo;
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public DataTable getDocuments(int pidTypeCompany = 0)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
                SqlDataAdapter da = new SqlDataAdapter("[SP_getDocuments]", con);
                da.SelectCommand.Parameters.Add("@pidTypeCompany", SqlDbType.Int).Value = pidTypeCompany;
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;

            }
            catch (Exception)
            {

                throw;
            }
        }
        public DataTable getMyDocuments(int pidClient = 0, int pidType = 0)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
                SqlDataAdapter da = new SqlDataAdapter("SP_GetMyDocuments", con);
                da.SelectCommand.Parameters.Add("@pidClient", SqlDbType.Int).Value = pidClient;
                da.SelectCommand.Parameters.Add("@pidType", SqlDbType.Int).Value = pidType;
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;

            }
            catch (Exception)
            {

                throw;
            }
        }


        public DataTable getMyDocumentsOffer(int pidClient = 0, string pcodeOffer = "", int pseefrom = 0)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
                SqlDataAdapter da = new SqlDataAdapter("SP_GetMyDocumentsContraOffer", con);
                da.SelectCommand.Parameters.Add("@pidClient", SqlDbType.Int).Value = pidClient;
                da.SelectCommand.Parameters.Add("@pcodeOffer", SqlDbType.VarChar).Value = pcodeOffer;
                da.SelectCommand.Parameters.Add("@pseefrom", SqlDbType.Int).Value = pseefrom;
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;

            }
            catch (Exception)
            {

                throw;
            }
        }



        public DataTable CrearCliente(string nombre, string apellidoPaterno, string apellidoMaterno,
            int idPerfil, int idHobbie, int idGenero, int idCalificacionCLiente, string idProductoDeInteres,
            int idestadoCliente, int idMotivoConocimientoEmpresa, string numeroCelular, string comentarioAdicional,
            string usuario, byte[] contrasena, byte[] key, byte[] iv,
            string email, string fechaNacimiento, string usuarioFaceBook, string usuarioSnapchat,
            string usuarioInstagram, string imagen, int idEstiloCliente,
            int idClienteReferente, int idPais, int idCiudad, string direccion,
            string Edificio, string cargo, string cedula,
            int idLugar, string fechaInial, string fechaFinal, int pidTypeCompany)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
                SqlDataAdapter da = new SqlDataAdapter("SP_CreacionCliente", con);
                da.SelectCommand.Parameters.Add("@nombre", SqlDbType.VarChar).Value = nombre;
                if (apellidoPaterno != "") da.SelectCommand.Parameters.Add("@apellidoPaterno", SqlDbType.VarChar).Value = apellidoPaterno;
                if (apellidoMaterno != "") da.SelectCommand.Parameters.Add("@apellidoMaterno", SqlDbType.VarChar).Value = apellidoMaterno;
                if (cedula != "") da.SelectCommand.Parameters.Add("@cedula", SqlDbType.VarChar).Value = cedula;
                if (cargo != "") da.SelectCommand.Parameters.Add("@cargo", SqlDbType.VarChar).Value = cargo;
                if (idPerfil != 0) da.SelectCommand.Parameters.Add("@idPerfil", SqlDbType.Int).Value = idPerfil;
                if (idHobbie != 0) da.SelectCommand.Parameters.Add("@idHobbie", SqlDbType.Int).Value = idHobbie;
                if (idGenero != 0) da.SelectCommand.Parameters.Add("@idGenero", SqlDbType.Int).Value = idGenero;
                if (idCalificacionCLiente != 0) da.SelectCommand.Parameters.Add("@idCalificacionCliente", SqlDbType.Int).Value = idCalificacionCLiente;
                if (idProductoDeInteres != "") da.SelectCommand.Parameters.Add("@idProductoDeInteres", SqlDbType.VarChar).Value = idProductoDeInteres;
                if (idMotivoConocimientoEmpresa != 0) da.SelectCommand.Parameters.Add("@idMotivoConocimientoEmpresa", SqlDbType.Int).Value = idMotivoConocimientoEmpresa;
                da.SelectCommand.Parameters.Add("@numeroCelular", SqlDbType.VarChar).Value = numeroCelular;
                if (comentarioAdicional != "") da.SelectCommand.Parameters.Add("@comentarioAdicional", SqlDbType.VarChar).Value = comentarioAdicional;
                da.SelectCommand.Parameters.Add("@usuario", SqlDbType.VarChar).Value = usuario;
                da.SelectCommand.Parameters.Add("@contrasena", SqlDbType.VarBinary).Value = contrasena;
                da.SelectCommand.Parameters.Add("@Key", SqlDbType.VarBinary).Value = key;
                da.SelectCommand.Parameters.Add("@IV", SqlDbType.VarBinary).Value = iv;
                da.SelectCommand.Parameters.Add("@email", SqlDbType.VarChar).Value = email;
                da.SelectCommand.Parameters.Add("@usuarioFaceBook", SqlDbType.VarChar).Value = usuarioFaceBook;
                if (idEstiloCliente != 0) da.SelectCommand.Parameters.Add("@idEstiloCliente", SqlDbType.Int).Value = idEstiloCliente;
                if (idClienteReferente != 0) da.SelectCommand.Parameters.Add("@idClienteReferencia", SqlDbType.Int).Value = idClienteReferente;
                if (idPais != 0) da.SelectCommand.Parameters.Add("@idPais", SqlDbType.Int).Value = idPais;
                if (idCiudad != 0) da.SelectCommand.Parameters.Add("@idCiudad", SqlDbType.Int).Value = idCiudad;
                if (idLugar != 0) da.SelectCommand.Parameters.Add("@idLugar", SqlDbType.Int).Value = idLugar;
                if (Edificio != "") da.SelectCommand.Parameters.Add("@Edificio", SqlDbType.VarChar).Value = Edificio;
                da.SelectCommand.Parameters.Add("@direccion", SqlDbType.VarChar).Value = direccion;
                da.SelectCommand.Parameters.Add("@pidTypeCompany", SqlDbType.Int).Value = pidTypeCompany;
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public DataTable actualizarContrasena(string email, byte[] contrasena, byte[] key, byte[] iv)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
                SqlDataAdapter da = new SqlDataAdapter("SPActualizarContrasena", con);
                da.SelectCommand.Parameters.Add("@email", SqlDbType.VarChar).Value = email;
                da.SelectCommand.Parameters.Add("@contrasena", SqlDbType.VarBinary).Value = contrasena;
                da.SelectCommand.Parameters.Add("@Key", SqlDbType.VarBinary).Value = key;
                da.SelectCommand.Parameters.Add("@IV", SqlDbType.VarBinary).Value = iv;
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;

            }
            catch (Exception)
            {

                throw;
            }
        }
        public DataTable UpdateRegistryClient(BidCargo_.Models.completarRegistro model, int idClient)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
                SqlDataAdapter da = new SqlDataAdapter("SP_updateRegistryClient", con);
                da.SelectCommand.Parameters.Add("@pidCliente", SqlDbType.Int).Value = idClient;
                da.SelectCommand.Parameters.Add("@pidTypeCompany", SqlDbType.VarChar).Value = model.idTypeCompany;
                da.SelectCommand.Parameters.Add("@pnit", SqlDbType.VarChar).Value = model.nit;
                da.SelectCommand.Parameters.Add("@ppercentCommission", SqlDbType.Int).Value = model.percentCommission;
                da.SelectCommand.Parameters.Add("@pidDepartament", SqlDbType.Int).Value = model.idDepartamento;
                da.SelectCommand.Parameters.Add("@pjobPosition", SqlDbType.VarChar).Value = model.cargoContacto;
                da.SelectCommand.Parameters.Add("@pidCity", SqlDbType.Int).Value = model.idCiudad;
                da.SelectCommand.Parameters.Add("@profilefirstname", SqlDbType.VarChar).Value = model.nombreContacto;
                da.SelectCommand.Parameters.Add("@profilelastname", SqlDbType.VarChar).Value = model.apellidoContacto;
                da.SelectCommand.Parameters.Add("@profilephone", SqlDbType.VarChar).Value = model.celularContacto;
                da.SelectCommand.Parameters.Add("@pdireccion", SqlDbType.VarChar).Value = model.direccion;
                da.SelectCommand.Parameters.Add("@pphone", SqlDbType.VarChar).Value = model.telefono;
                da.SelectCommand.Parameters.Add("@prazonsocial", SqlDbType.VarChar).Value = model.razonSocial;
                da.SelectCommand.Parameters.Add("@pcorreoContrato", SqlDbType.VarChar).Value = model.correoContacto;
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch (Exception e)
            {
                string sas = e.Message;
                throw;
            }
        }
        public DataTable saveFile(string nameImg, string srcImg, string typeFile, int idCliente, int type, int typeDoct, string codeOffer, int idOp)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
                SqlDataAdapter da = new SqlDataAdapter("SP_saveFileClient", con);
                da.SelectCommand.Parameters.Add("@pnameImg", SqlDbType.VarChar).Value = nameImg;
                da.SelectCommand.Parameters.Add("@psrcImg", SqlDbType.VarChar).Value = srcImg;
                da.SelectCommand.Parameters.Add("@pfileType", SqlDbType.VarChar).Value = typeFile;
                da.SelectCommand.Parameters.Add("@pidCliente", SqlDbType.Int).Value = idCliente;
                da.SelectCommand.Parameters.Add("@ptype", SqlDbType.Int).Value = type;
                da.SelectCommand.Parameters.Add("@pidDocument", SqlDbType.Int).Value = typeDoct;
                da.SelectCommand.Parameters.Add("@codeOffer", SqlDbType.VarChar).Value = codeOffer;
                da.SelectCommand.Parameters.Add("@seefrom", SqlDbType.Int).Value = idOp;
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable StoreOffer(BidCargo_.Models.storeoffer model, int idCliente)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
                SqlDataAdapter da = new SqlDataAdapter("SP_storeOffers", con);
                string varnacdtaome = "";
                if (model.idNACDTAOMT != null)
                {
                    if (model.idNACDTAOMT.Count() > 0)
                    {
                        for (var i = 0; i < model.idNACDTAOMT.Count(); i++)
                        {
                            varnacdtaome += model.idNACDTAOMT[i];
                            if ((i + 1) < model.idNACDTAOMT.Count())
                                varnacdtaome += ",";
                        }
                    }
                }
                string varstuffLots = model.idTipoTransporte;
                if (model.idTipoTransporte != null)
                {
                    if (model.idTipoTransporte.Count() > 0)
                    {
                        for (var i = 0; i < model.idTipoTransporte.Count(); i++)
                        {
                            varstuffLots += model.idTipoTransporte[i];
                            if ((i + 1) < model.idTipoTransporte.Count())
                                varstuffLots += ",";
                        }
                    }
                }
                da.SelectCommand.Parameters.Add("@pidCliente", SqlDbType.Int).Value = idCliente;
                da.SelectCommand.Parameters.Add("@ptypeCargo", SqlDbType.VarChar).Value = model.typeCargo;
                da.SelectCommand.Parameters.Add("@pfrom", SqlDbType.VarChar).Value = model.from;
                da.SelectCommand.Parameters.Add("@pto", SqlDbType.VarChar).Value = model.to;
                da.SelectCommand.Parameters.Add("@pdayTraveler", SqlDbType.VarChar).Value = model.dayTraveler;
                da.SelectCommand.Parameters.Add("@pfrom2", SqlDbType.VarChar).Value = model.from2;
                da.SelectCommand.Parameters.Add("@pto2", SqlDbType.VarChar).Value = model.to2;
                da.SelectCommand.Parameters.Add("@pdayTraveler2", SqlDbType.VarChar).Value = model.dayTraveler2;
                da.SelectCommand.Parameters.Add("@pdirectionFrom", SqlDbType.VarChar).Value = model.directionFrom;
                da.SelectCommand.Parameters.Add("@pdirectionTo", SqlDbType.VarChar).Value = model.directionTo;
                da.SelectCommand.Parameters.Add("@ptypeMerchandise", SqlDbType.VarChar).Value = model.typeMerchandise;
                da.SelectCommand.Parameters.Add("@pbulkCargo", SqlDbType.VarChar).Value = model.bulkCargo;
                da.SelectCommand.Parameters.Add("@pcontainerType", SqlDbType.VarChar).Value = model.idTipoContenedor;
                da.SelectCommand.Parameters.Add("@pcontainerWeight", SqlDbType.VarChar).Value = model.containerWeight;
                da.SelectCommand.Parameters.Add("@pidWeightMeasured", SqlDbType.VarChar).Value = model.idWeightMeasured;
                da.SelectCommand.Parameters.Add("@pnacDtaOmt", SqlDbType.VarChar).Value = varnacdtaome;
                da.SelectCommand.Parameters.Add("@plongM", SqlDbType.VarChar).Value = model.longM;
                da.SelectCommand.Parameters.Add("@pwidthM", SqlDbType.VarChar).Value = model.widthM;
                da.SelectCommand.Parameters.Add("@phighM", SqlDbType.VarChar).Value = model.highM;
                da.SelectCommand.Parameters.Add("@ptied", SqlDbType.VarChar).Value = model.tied;
                da.SelectCommand.Parameters.Add("@pplates", SqlDbType.VarChar).Value = model.plates;
                da.SelectCommand.Parameters.Add("@ploose", SqlDbType.VarChar).Value = model.loose;
                da.SelectCommand.Parameters.Add("@pidDimensions", SqlDbType.VarChar).Value = model.idDimensions;
                da.SelectCommand.Parameters.Add("@pvalueMerchandise", SqlDbType.VarChar).Value = model.valueMerchandise;
                da.SelectCommand.Parameters.Add("@pvalueMerchandise2", SqlDbType.VarChar).Value = model.valueMerchandise2;
                da.SelectCommand.Parameters.Add("@pnumberUnits", SqlDbType.VarChar).Value = model.numberUnits;
                da.SelectCommand.Parameters.Add("@pnumberTons", SqlDbType.VarChar).Value = model.numberTons;
                da.SelectCommand.Parameters.Add("@pfactoring", SqlDbType.VarChar).Value = model.idFactoring;
                da.SelectCommand.Parameters.Add("@ppayDays", SqlDbType.VarChar).Value = model.payDays;
                da.SelectCommand.Parameters.Add("@pstuffLots", SqlDbType.VarChar).Value = varstuffLots;
                da.SelectCommand.Parameters.Add("@pobservation", SqlDbType.VarChar).Value = model.observation;
                da.SelectCommand.Parameters.Add("@pcodeOffer", SqlDbType.VarChar).Value = model.codeOffer;
                da.SelectCommand.Parameters.Add("@pvalueOffer", SqlDbType.Int).Value = model.valorOferta;
                da.SelectCommand.Parameters.Add("@pcoinTypesId", SqlDbType.Int).Value = model.coinTypesId;
                da.SelectCommand.Parameters.Add("@pdateOfServiceId", SqlDbType.Int).Value = model.dateOfServiceId;
                da.SelectCommand.Parameters.Add("@pidCotizacion", SqlDbType.Int).Value = model.idTipoDeCotizacion;
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch (Exception e)
            {
                string vaas = e.Message;
                throw;
            }
        }

        public DataTable storeContraOffer(int pidClient = 0, string pcodeOffer = "", decimal pContraOffer = 0, int pIdTypeCompany = 0, string pdescripcion = "")
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
                SqlDataAdapter da = new SqlDataAdapter("SP_StoreContraOffers", con);
                da.SelectCommand.Parameters.Add("@pidClient", SqlDbType.Int).Value = pidClient;
                da.SelectCommand.Parameters.Add("@pcodeOffer", SqlDbType.VarChar).Value = pcodeOffer;
                da.SelectCommand.Parameters.Add("@pContraOffer", SqlDbType.Decimal).Value = pContraOffer;
                da.SelectCommand.Parameters.Add("@pIdTypeCompany", SqlDbType.Int).Value = pIdTypeCompany;
                da.SelectCommand.Parameters.Add("@pdescripcion", SqlDbType.VarChar).Value = pdescripcion;
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch (Exception e)
            {
                string vaas = e.Message;
                throw;
            }
        }
        public DataTable getOffer(string pcodeOffer = "", int ptype = 0, int pidClient = 0)
        {
            try
            {

                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
                SqlDataAdapter da = new SqlDataAdapter("SP_getOffer", con);
                da.SelectCommand.Parameters.Add("@pcodeOffer", SqlDbType.VarChar).Value = pcodeOffer;
                da.SelectCommand.Parameters.Add("@ptype", SqlDbType.Int).Value = ptype;
                da.SelectCommand.Parameters.Add("@pidClient", SqlDbType.Int).Value = pidClient;
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch (Exception e)
            {
                string vaas = e.Message;
                throw;
            }
        }

        public DataTable getOfferByTypeCompany(string pcodeOffer = "", int ptype = 0, int pidClient = 0)
        {
            try
            {

                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
                SqlDataAdapter da = new SqlDataAdapter("SP_getOffer", con);
                da.SelectCommand.Parameters.Add("@pcodeOffer", SqlDbType.VarChar).Value = pcodeOffer;
                da.SelectCommand.Parameters.Add("@ptype", SqlDbType.Int).Value = ptype;
                da.SelectCommand.Parameters.Add("@pidClient", SqlDbType.Int).Value = pidClient;
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch (Exception e)
            {
                string vaas = e.Message;
                throw;
            }
        }



        public DataTable GetViewExportToExcel(int id)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
                SqlDataAdapter da = new SqlDataAdapter("GetViewExportToExcel", con);
                da.SelectCommand.Parameters.Add("@type", SqlDbType.Int).Value = id;
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch (Exception e)
            {
                string vaas = e.Message;
                throw;
            }
        }



        public DataTable getCompaniesAcceptedByCodeOffers(string pcodeOffer = "")
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
                SqlDataAdapter da = new SqlDataAdapter("SP_getCompaniesAcceptedByCodeOffers", con);
                if (pcodeOffer != "") da.SelectCommand.Parameters.Add("@pcodeOffer", SqlDbType.VarChar).Value = pcodeOffer;
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch (Exception e)
            {
                string vaas = e.Message;
                throw;
            }
        }

        public DataTable getContracOffers(int pidClient = 0, string pcodeOffer = "")
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
                SqlDataAdapter da = new SqlDataAdapter("SP_getContracOffers", con);
                da.SelectCommand.Parameters.Add("@pidClient", SqlDbType.Int).Value = pidClient;
                da.SelectCommand.Parameters.Add("@pcodeOffer", SqlDbType.VarChar).Value = pcodeOffer;
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch (Exception e)
            {
                string vaas = e.Message;
                throw;
            }
        }


        public DataTable getContraOfferEdit(int idContraOffers)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
                SqlDataAdapter da = new SqlDataAdapter("SP_getContraOfferEdit", con);
                da.SelectCommand.Parameters.Add("@idContraOffers", SqlDbType.Int).Value = idContraOffers;
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch (Exception e)
            {
                string vaas = e.Message;
                throw;
            }
        }

        public DataTable UpdateContracOffers(int pidClient, int idContraOffers, int pContraOffer, string descripcion)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
                SqlDataAdapter da = new SqlDataAdapter("SP_UpdateContracOffers", con);
                da.SelectCommand.Parameters.Add("@pidClient", SqlDbType.Int).Value = pidClient;
                da.SelectCommand.Parameters.Add("@pidContraOffers", SqlDbType.Int).Value = idContraOffers;
                da.SelectCommand.Parameters.Add("@pContraOffer", SqlDbType.Decimal).Value = Convert.ToDecimal(pContraOffer);
                da.SelectCommand.Parameters.Add("@pdescripcion", SqlDbType.VarChar).Value = descripcion;
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch (Exception e)
            {
                string vaas = e.Message;
                throw;
            }
        }

        public DataTable obtenerCorreos(int idTipoClasificacion)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
                SqlDataAdapter da = new SqlDataAdapter("SPObtenerCorreosInteresados", con);
                da.SelectCommand.Parameters.Add("@idTipoClasificacion", SqlDbType.VarChar).Value = idTipoClasificacion;
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch (Exception e)
            {
                string vaas = e.Message;
                throw;
            }
        }
        public DataTable deleteOffer(int pidClientOffer)//1
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
                SqlDataAdapter da = new SqlDataAdapter("SP_deleteOffer", con);
                da.SelectCommand.Parameters.Add("@pidClientOffer", SqlDbType.Int).Value = pidClientOffer;
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch (Exception e)
            {
                string vaas = e.Message;
                throw;
            }
        }
        public DataTable getTypeContainer(int pidtypeCargo = 0, string parameters = "")
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
                SqlDataAdapter da = new SqlDataAdapter("SP_TipoContenedor", con);
                da.SelectCommand.Parameters.Add("@pidtypeCargo", SqlDbType.Int).Value = pidtypeCargo;
                da.SelectCommand.Parameters.Add("@parameters", SqlDbType.VarChar).Value = parameters;
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch (Exception e)
            {
                string vaas = e.Message;
                throw;
            }
        }
        public DataTable getCotizacion(int pidtypeCargo = 0, string parameters = "")
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
                SqlDataAdapter da = new SqlDataAdapter("SP_TipoDeCotizacion", con);
                da.SelectCommand.Parameters.Add("@pidtypeCargo", SqlDbType.Int).Value = pidtypeCargo;
                da.SelectCommand.Parameters.Add("@params", SqlDbType.VarChar).Value = parameters;
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch (Exception e)
            {
                string vaas = e.Message;
                throw;
            }
        }
        public DataTable getTypeMeasured(int pidtypeCargo = 0, int ptype = 0, string parameters = "")
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
                SqlDataAdapter da = new SqlDataAdapter("SP_getTypeMeasured", con);
                da.SelectCommand.Parameters.Add("@pidtypeCargo", SqlDbType.Int).Value = pidtypeCargo;
                da.SelectCommand.Parameters.Add("@ptype", SqlDbType.Int).Value = ptype;
                da.SelectCommand.Parameters.Add("@params", SqlDbType.VarChar).Value = parameters;
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch (Exception e)
            {
                string vaas = e.Message;
                throw;
            }
        }
        public DataTable acceptContraOffer(int pidContraOffer = 0, int pstatus = 0)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
                SqlDataAdapter da = new SqlDataAdapter("SP_acceptContraOffer", con);
                da.SelectCommand.Parameters.Add("@pidContraOffer", SqlDbType.Int).Value = pidContraOffer;
                da.SelectCommand.Parameters.Add("@pstatus", SqlDbType.Int).Value = pstatus;
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch (Exception e)
            {
                string vaas = e.Message;
                throw;
            }
        }

        public DataTable NoAcceptContraOffer(string pContraOffer)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
                SqlDataAdapter da = new SqlDataAdapter("SP_NoAcceptContraOffer", con);
                da.SelectCommand.Parameters.Add("@pContraOffer", SqlDbType.NVarChar).Value = pContraOffer;
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch (Exception e)
            {
                string vaas = e.Message;
                throw;
            }
        }

        public DataTable getCliente(int idCliente)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
                SqlDataAdapter da = new SqlDataAdapter("SP_getCliente", con);
                da.SelectCommand.Parameters.Add("@idCliente", SqlDbType.Int).Value = idCliente;
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch (Exception e)
            {
                string vaas = e.Message;
                throw;
            }
        }
        public DataTable getTypeCompanyMinusIdclient(int pidClient = 0)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
                SqlDataAdapter da = new SqlDataAdapter("SP_getTypeCompanyMinusIdClient", con);
                da.SelectCommand.Parameters.Add("@pidClient", SqlDbType.Int).Value = pidClient;
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch (Exception e)
            {
                string vaas = e.Message;
                throw;
            }
        }
        public DataTable getTypeCompanyByClient(int pidClient = 0)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
                SqlDataAdapter da = new SqlDataAdapter("SP_getTypeCompanyByClient", con);
                da.SelectCommand.Parameters.Add("@pidClient", SqlDbType.Int).Value = pidClient;
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch (Exception e)
            {
                string vaas = e.Message;
                throw;
            }
        }



        public DataTable getUsersTypeCompanyByOffers(int pidOffers = 0)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
                SqlDataAdapter da = new SqlDataAdapter("SP_getUsersTypeCompanyByOffers", con);
                da.SelectCommand.Parameters.Add("@pidOffers", SqlDbType.Int).Value = pidOffers;
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch (Exception e)
            {
                string vaas = e.Message;
                throw;
            }
        }

        public DataTable ShowContraOffers(string pcodeOffer = "")
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
                SqlDataAdapter da = new SqlDataAdapter("SP_ShowContraOffers", con);
                da.SelectCommand.Parameters.Add("@pcodeOffer", SqlDbType.VarChar).Value = pcodeOffer;
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch (Exception e)
            {
                string vaas = e.Message;
                throw;
            }
        }

        public DataTable getDataProfiles(int idCliente)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
                SqlDataAdapter da = new SqlDataAdapter("SP_getDataProfile", con);
                da.SelectCommand.Parameters.Add("@pidCliente", SqlDbType.Int).Value = idCliente;
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public DataTable CreateOrUpdateProfile(BidCargo_.Models.Perfil model, int idCliente, byte[] profipassword = null, byte[] key = null, byte[] iv = null)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
                SqlDataAdapter da = new SqlDataAdapter("SP_ActualizarPerfil", con);
                da.SelectCommand.Parameters.Add("@pidCliente", SqlDbType.Int).Value = idCliente;
                if (model.nombre != null) da.SelectCommand.Parameters.Add("@nombre", SqlDbType.VarChar).Value = model.nombre;
                if (model.apellidoPaterno != null) da.SelectCommand.Parameters.Add("@apellidoPaterno", SqlDbType.VarChar).Value = model.apellidoPaterno;
                if (model.cedula != null) da.SelectCommand.Parameters.Add("@cedula", SqlDbType.VarChar).Value = model.cedula;
                if (model.email != null) da.SelectCommand.Parameters.Add("@email", SqlDbType.VarChar).Value = model.email;
                if (model.numeroCelular != null) da.SelectCommand.Parameters.Add("@numeroCelular", SqlDbType.VarChar).Value = model.numeroCelular;
                if (Convert.ToDateTime(model.fechanacimiento).Year > DateTime.Now.Year - 200) da.SelectCommand.Parameters.Add("@fechanacimiento", SqlDbType.DateTime).Value = model.fechanacimiento;
                if (model.idPais != 0) da.SelectCommand.Parameters.Add("@idPais", SqlDbType.Int).Value = model.idPais;
                if (model.idCiudad != 0) da.SelectCommand.Parameters.Add("@idCiudad", SqlDbType.Int).Value = model.idCiudad;
                if (model.idDepartamento != 0) da.SelectCommand.Parameters.Add("@idDepartament", SqlDbType.Int).Value = model.idDepartamento;
                if (model.idTypeCompany != "") da.SelectCommand.Parameters.Add("@idTypeCompany", SqlDbType.VarChar).Value = model.idTypeCompany;
                if (profipassword != null) da.SelectCommand.Parameters.Add("@contraseña", SqlDbType.VarBinary).Value = profipassword;
                if (key != null) da.SelectCommand.Parameters.Add("@Key", SqlDbType.VarBinary).Value = key;
                if (iv != null) da.SelectCommand.Parameters.Add("@IV", SqlDbType.VarBinary).Value = iv;
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public DataTable storeQualificationContraOffer(int idUserQualifying = 0, int pidContraOffer = 0, int pQualification = 0, string pComments = "")
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
                SqlDataAdapter da = new SqlDataAdapter("SP_storeQualificationContraOffer", con);
                da.SelectCommand.Parameters.Add("@idUserQualifying", SqlDbType.Int).Value = idUserQualifying;
                da.SelectCommand.Parameters.Add("@pidContraOffer", SqlDbType.Int).Value = pidContraOffer;
                da.SelectCommand.Parameters.Add("@pQualification", SqlDbType.Int).Value = pQualification;
                da.SelectCommand.Parameters.Add("@pComments", SqlDbType.VarChar).Value = pComments;
                da.SelectCommand.Parameters.Add("@pDateQualification", SqlDbType.VarChar).Value = DateTime.Now.ToString("dd/MM/yyyy");
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable getQualificationByContraOffer(int pidContraOffer = 0)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
                SqlDataAdapter da = new SqlDataAdapter("SP_getQualificationByContraOffer", con);
                da.SelectCommand.Parameters.Add("@pidContraOffer", SqlDbType.Int).Value = pidContraOffer;
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public DataTable closeOffer(string pcodeOffer = "")
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
                SqlDataAdapter da = new SqlDataAdapter("SP_closeOffer", con);
                da.SelectCommand.Parameters.Add("@pcodeOffer", SqlDbType.VarChar).Value = pcodeOffer;
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public DataTable verifyCelular(string pcelular = "")
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
                SqlDataAdapter da = new SqlDataAdapter("SP_verifyCelular", con);
                da.SelectCommand.Parameters.Add("@pcelular", SqlDbType.VarChar).Value = pcelular;
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch (Exception e)
            {
                string sas = e.Message; throw;
            }
        }
        public DataTable verifyFijo(string pcelular = "")
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
                SqlDataAdapter da = new SqlDataAdapter("SP_verifyFijo", con);
                da.SelectCommand.Parameters.Add("@pcelular", SqlDbType.VarChar).Value = pcelular;
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch (Exception e)
            {
                string sas = e.Message; throw;
            }
        }


        /*------ USUARIO SISTEMA --------- */
        public DataTable SaveUsuarioSistema(int Opcion, int Id,
            int AccionId, string Nombre, string Apellido,
            string Celular, string Contrasena,
            int Estado, string Correo, DateTime Creado, int CompanyId, int PerfilId)
        {

            string[] newUser = Correo.Split('@');
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
                SqlDataAdapter da = new SqlDataAdapter("SP_UsuarioSistema", con);
                da.SelectCommand.Parameters.Add("@Opcion", SqlDbType.Int).Value = Opcion;
                da.SelectCommand.Parameters.Add("@Id", SqlDbType.Int).Value = Id;
                da.SelectCommand.Parameters.Add("@AccionId", SqlDbType.Int).Value = AccionId;
                da.SelectCommand.Parameters.Add("@nombre", SqlDbType.VarChar).Value = Nombre;
                da.SelectCommand.Parameters.Add("@apellidoPaterno", SqlDbType.VarChar).Value = Apellido;
                da.SelectCommand.Parameters.Add("@celular", SqlDbType.VarChar).Value = Celular;
                da.SelectCommand.Parameters.Add("@usuario", SqlDbType.VarChar).Value = newUser[0].ToString();
                da.SelectCommand.Parameters.Add("@contrasena", SqlDbType.VarChar).Value = Contrasena;
                da.SelectCommand.Parameters.Add("@estado", SqlDbType.Int).Value = Estado;
                da.SelectCommand.Parameters.Add("@correo", SqlDbType.VarChar).Value = Correo;
                da.SelectCommand.Parameters.Add("@creado", SqlDbType.DateTime).Value = Creado;
                da.SelectCommand.Parameters.Add("@companyId", SqlDbType.Int).Value = CompanyId;
                da.SelectCommand.Parameters.Add("@perfilId", SqlDbType.Int).Value = PerfilId;

                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch (Exception)
            {
                throw;
            }
        }



        /*******admin reportes ****/


        public DataTable Offertas()
        {
            try
            {

                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
                string Query = "select * from TB_ClientOffers";
                SqlCommand command = new SqlCommand(Query, con);
                SqlDataAdapter data = new SqlDataAdapter(command);
                DataTable tablevehiculos = new DataTable();
                data.Fill(tablevehiculos);
                return tablevehiculos;

            }
            catch (Exception)
            {

                throw;
            }


        }

        public DataTable ReporteVehiculo()
        {

            try
            {

                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
                SqlDataAdapter da = new SqlDataAdapter("SP_VehiculoPerfil", con);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable tableConductor = new DataTable();
                da.Fill(tableConductor);
                return tableConductor;

            }
            catch (Exception)
            {

                throw;
            }

        }



        public DataTable ReportePersonaJuridica()
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
                SqlDataAdapter da = new SqlDataAdapter("SP_ConsultarPropietarioJuridico", con);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable tableConductor = new DataTable();
                da.Fill(tableConductor);
                return tableConductor;


            }
            catch (Exception)
            {

                throw;
            }

        }
        public DataTable ReportePersonaNaturales()
        {
            try
            {

                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
                SqlDataAdapter da = new SqlDataAdapter("SP_ConsultarPropietarioNatural", con);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable tableConductor = new DataTable();
                da.Fill(tableConductor);
                return tableConductor;

            }
            catch (Exception)
            {

                throw;
            }

        }
        public DataTable ReporteConductor()
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
                SqlDataAdapter da = new SqlDataAdapter("SP_ConsultarConductor", con);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable tableConductor = new DataTable();
                da.Fill(tableConductor);
                return tableConductor;


            }
            catch (Exception)
            {

                throw;
            }

        }
        public DataTable ReporteConductor2(int tipo)
        {
            try
            {
                if (tipo == 1)
                {
                    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
                    string Query = $"select * from TB_Conductor tc inner join  TB_PropietarioNatural tpj on tc.idPropietario = tpj.fk_usuario inner join TB_Usuario tu on tu.sk_usuario = tpj.fk_usuario";
                    SqlCommand command = new SqlCommand(Query, con);
                    SqlDataAdapter data = new SqlDataAdapter(command);
                    DataTable tableConductor = new DataTable();
                    data.Fill(tableConductor);
                    return tableConductor;



     

                }
                else {
                    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
                    string Query = $" select * from TB_Conductor tc inner join  TB_PropietarioJuridico tpj on tc.idPropietario = tpj.fk_usuario inner join TB_Usuario tu on tu.sk_usuario = tpj.fk_usuario ";
                    SqlCommand command = new SqlCommand(Query, con);
                    SqlDataAdapter data = new SqlDataAdapter(command);
                    DataTable tableConductor = new DataTable();
                    data.Fill(tableConductor);
                    return tableConductor;


                }

            }
            catch (Exception)
            {

                throw;
            }

        }

        public DataTable ReporteDefinidoConductor( int sk_conductor)
        {
            try
            {
                
                    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
                    string Query = $" select * from TB_Conductor tc inner join  TB_PropietarioJuridico tpj on tc.idPropietario = tpj.fk_usuario inner join TB_Usuario tu on tu.sk_usuario = tpj.fk_usuario where tc.sk_conductor = {sk_conductor}";
                    SqlCommand command = new SqlCommand(Query, con);
                    SqlDataAdapter data = new SqlDataAdapter(command);
                    DataTable tableConductor = new DataTable();
                    data.Fill(tableConductor);
                    return tableConductor;


                

            }
            catch (Exception)
            {

                throw;
            }

        }




        /************fin reportes **/






        /***vehiculo***/

        public DataTable Estadovehiculo(int Id, string estado)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
                SqlDataAdapter da = new SqlDataAdapter("SP_ActualizarVehiculos", con);
                da.SelectCommand.Parameters.Add("@idVehiculo ", SqlDbType.Int).Value = Id;
                da.SelectCommand.Parameters.Add("@Estado", SqlDbType.VarChar).Value = estado;
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch (Exception e)
            {
                string vaas = e.Message;
                throw;
            }


        }

        public DataTable EliminarVehiculo(int id)
        {
            try
            {

                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
                SqlDataAdapter da = new SqlDataAdapter("SP_EliminarVehiculo", con);
                da.SelectCommand.Parameters.Add("@idVehiculo ", SqlDbType.Int).Value = id;
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;

            }
            catch (Exception e)
            {
                string vaas = e.Message;
                throw;

            }

        }
        public DataTable DrowlistTipoVehiculo()
        {

            string query = "select * from  TB_TipoDeVehiculo";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
            SqlCommand command = new SqlCommand(query, con);
            SqlDataAdapter data = new SqlDataAdapter(command);
            DataTable table = new DataTable();
            data.Fill(table);
            return table;



        }
        public DataTable GuardarVehiculo(VehiculoViewModels vehiculoViewModels)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
                SqlDataAdapter da = new SqlDataAdapter("SP_insertarVehiculo", con);
                da.SelectCommand.Parameters.Add("@Placa", SqlDbType.NVarChar).Value = vehiculoViewModels.Placa;
                da.SelectCommand.Parameters.Add("@TipoVehivulo", SqlDbType.Int).Value = vehiculoViewModels.TipoVehiculo;
                da.SelectCommand.Parameters.Add("@Marca", SqlDbType.NVarChar).Value = vehiculoViewModels.Marca;
                da.SelectCommand.Parameters.Add("@Modelo", SqlDbType.NVarChar).Value = vehiculoViewModels.Modelo;
                da.SelectCommand.Parameters.Add("@Año", SqlDbType.NVarChar).Value = vehiculoViewModels.Linea;
                da.SelectCommand.Parameters.Add("@Operador", SqlDbType.NVarChar).Value = vehiculoViewModels.Operador;
                da.SelectCommand.Parameters.Add("@Usuario", SqlDbType.NVarChar).Value = vehiculoViewModels.Usuario;
                da.SelectCommand.Parameters.Add("@Contraseña", SqlDbType.NVarChar).Value = vehiculoViewModels.Contraseña;
                da.SelectCommand.Parameters.Add("@SoportePropiedad", SqlDbType.NVarChar).Value = vehiculoViewModels.SoportePropiedad;
                da.SelectCommand.Parameters.Add("@SoporteSoat", SqlDbType.NVarChar).Value = vehiculoViewModels.SoporteSoat;
                da.SelectCommand.Parameters.Add("@SoporteMecanica", SqlDbType.NVarChar).Value = vehiculoViewModels.SoporteMecanica;
                da.SelectCommand.Parameters.Add("@SoporteTodoRiezdo", SqlDbType.NVarChar).Value = vehiculoViewModels.SoportetodoRiezgo;
                da.SelectCommand.Parameters.Add("@Estado", SqlDbType.NVarChar).Value = vehiculoViewModels.Estado;
                da.SelectCommand.Parameters.Add("@IdPropietario", SqlDbType.Int).Value = vehiculoViewModels.IdPropietario;
                da.SelectCommand.Parameters.Add("@PlacaRemolque", SqlDbType.NVarChar).Value = vehiculoViewModels.PlacaRemolque;
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch (Exception)
            {

                throw;
            }


        }
        /******fin Vehiculos******/

        public DataTable CrearUsuario(UsuarioInputModel usuarioInputModel)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
                SqlDataAdapter da = new SqlDataAdapter("SP_CreacionUsuario", con);
                da.SelectCommand.Parameters.Add("@nombreUsuario", SqlDbType.VarChar).Value = usuarioInputModel.nombreUsusario;
                da.SelectCommand.Parameters.Add("@correo", SqlDbType.VarChar).Value = usuarioInputModel.correoContacto;
                da.SelectCommand.Parameters.Add("@telefonoMovil", SqlDbType.VarChar).Value = usuarioInputModel.telefonoMovil;
                da.SelectCommand.Parameters.Add("@contraseña", SqlDbType.VarBinary).Value = usuarioInputModel.contraseña;
                da.SelectCommand.Parameters.Add("@Key", SqlDbType.VarBinary).Value = usuarioInputModel.KEY;
                da.SelectCommand.Parameters.Add("@IV", SqlDbType.VarBinary).Value = usuarioInputModel.IV;
                da.SelectCommand.Parameters.Add("@rol", SqlDbType.VarChar).Value = usuarioInputModel.rol;
                da.SelectCommand.Parameters.Add("@idEstadoCliente", SqlDbType.Int).Value = usuarioInputModel.idEstadoCliente;


                //da.SelectCommand.Parameters.Add("@tipoDocumento", SqlDbType.Int).Value = 1;
                //da.SelectCommand.Parameters.Add("@numeroDocumento", SqlDbType.VarChar).Value = conductor.numeroDocumento;
                //da.SelectCommand.Parameters.Add("@nombre", SqlDbType.VarChar).Value = conductor.nombre;
                //da.SelectCommand.Parameters.Add("@apellido", SqlDbType.VarChar).Value = conductor.apellido;


                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch (Exception)
            {

                throw;
            }
        }


        public DataTable CrearConductor(ConductorInputModel conductor, int idUsuario,  int idpropietario)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
                SqlDataAdapter da = new SqlDataAdapter("SP_CreacionConductor", con);
                da.SelectCommand.Parameters.Clear();
                da.SelectCommand.Parameters.Add("@tipoDocumento", SqlDbType.Int).Value = conductor.tipoDocumento;
                da.SelectCommand.Parameters.Add("@numeroDocumento", SqlDbType.VarChar).Value = conductor.numeroDocumento;
                da.SelectCommand.Parameters.Add("@nombre", SqlDbType.VarChar).Value = conductor.nombre;
                da.SelectCommand.Parameters.Add("@apellido", SqlDbType.VarChar).Value = conductor.apellido;
                da.SelectCommand.Parameters.Add("@numeroLicencia", SqlDbType.VarChar).Value = conductor.numeroLicencia;
                da.SelectCommand.Parameters.Add("@direccion", SqlDbType.VarChar).Value = conductor.direccion;
                da.SelectCommand.Parameters.Add("@codigoCiudad", SqlDbType.Int).Value = conductor.codigoCiudad;
                da.SelectCommand.Parameters.Add("@telefonoFijo", SqlDbType.VarChar).Value = conductor.telefonoFijo;
                da.SelectCommand.Parameters.Add("@codigoUsuario", SqlDbType.Int).Value = idUsuario;
                da.SelectCommand.Parameters.Add("@telefonomovil", SqlDbType.VarChar).Value = conductor.telefonoMovil;
                da.SelectCommand.Parameters.Add("@correo", SqlDbType.VarChar).Value = conductor.correoContacto; 
                da.SelectCommand.Parameters.Add("@idpropietario", SqlDbType.VarChar).Value = idpropietario;

                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public DataTable CrearPropietarioNatural(PropNaturalInputModel propNaturalInput)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
                SqlDataAdapter da = new SqlDataAdapter("SP_CreacionPropietarioNatural", con);
                da.SelectCommand.Parameters.Add("@nombres", SqlDbType.NVarChar).Value = propNaturalInput.nombres;
                da.SelectCommand.Parameters.Add("@apellido", SqlDbType.NVarChar).Value = propNaturalInput.apellidos;
                da.SelectCommand.Parameters.Add("@fk_tipodocumento", SqlDbType.Int).Value = propNaturalInput.fk_TipoDocumento;
                da.SelectCommand.Parameters.Add("@numerodocumento", SqlDbType.VarChar).Value = propNaturalInput.numeroDocumento;
                da.SelectCommand.Parameters.Add("@direccion", SqlDbType.VarChar).Value = propNaturalInput.direccion;
                da.SelectCommand.Parameters.Add("@telefonoFijo", SqlDbType.VarChar).Value = propNaturalInput.telefonoFijo;
                da.SelectCommand.Parameters.Add("@fk_idciudad", SqlDbType.VarChar).Value = propNaturalInput.fk_idciudad;
                da.SelectCommand.Parameters.Add("@fk_usuario", SqlDbType.VarChar).Value = propNaturalInput.fk_usuario;
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public DataTable CrearPropietarioJuridico(propJuridicoInputModel propJuridicoInput)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
                SqlDataAdapter da = new SqlDataAdapter("SP_CreacionPropietarioJuridico", con);
                da.SelectCommand.Parameters.Add("@nombrecontacto", SqlDbType.NVarChar).Value = propJuridicoInput.nombreContacto;
                da.SelectCommand.Parameters.Add("@apellidocontacto", SqlDbType.NVarChar).Value = propJuridicoInput.apellidoContacto;
                da.SelectCommand.Parameters.Add("@nit", SqlDbType.VarChar).Value = propJuridicoInput.nit;
                da.SelectCommand.Parameters.Add("@razonsocial", SqlDbType.VarChar).Value = propJuridicoInput.razonSocial;
                da.SelectCommand.Parameters.Add("@direccion", SqlDbType.VarChar).Value = propJuridicoInput.direccion;
                da.SelectCommand.Parameters.Add("@telefonoFijo", SqlDbType.VarChar).Value = string.IsNullOrEmpty(propJuridicoInput.telefonoFijo) ? "0000" : propJuridicoInput.telefonoFijo;
                da.SelectCommand.Parameters.Add("@fk_idciudad", SqlDbType.VarChar).Value = propJuridicoInput.fk_idciudad;
                da.SelectCommand.Parameters.Add("@fk_usuario", SqlDbType.VarChar).Value = propJuridicoInput.fk_usuario;
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public DataTable ObtenerContraseña(string telefonoMovil)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
                SqlDataAdapter da = new SqlDataAdapter("SP_ObtenerContrasena", con);
                da.SelectCommand.Parameters.Add("@telefonoMovil", SqlDbType.NVarChar).Value = telefonoMovil;
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch (Exception)
            {

                throw;
            }
        }


        public DataTable ConsultarUsuarioPropietario(int sk_usuario)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
                SqlDataAdapter da = new SqlDataAdapter("SP_ConsultarUsuarioPropietario", con);
                da.SelectCommand.Parameters.Add("@sk_usuario", SqlDbType.Int).Value = sk_usuario;
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public DataTable ConsultarUsuarioPropietarioPerfil(int sk_usuario)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
                SqlDataAdapter da = new SqlDataAdapter("SP_ConsultarUsuarioPropietarioPerfil", con);
                da.SelectCommand.Parameters.Add("@sk_usuario", SqlDbType.Int).Value = sk_usuario;
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public DataTable CambioEstadoPropietario(int id, int estado)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
            SqlDataAdapter da = new SqlDataAdapter("SP_CambioEstado", con);
            da.SelectCommand.Parameters.Add("@idUsuario", SqlDbType.Int).Value = id;
            da.SelectCommand.Parameters.Add("@nuevoEstado", SqlDbType.Int).Value = estado;
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }
        public DataTable CambioEstadoConductor(int id, int estado)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
            SqlDataAdapter da = new SqlDataAdapter("SP_cambioestadoConductor2", con);
            da.SelectCommand.Parameters.Add("@idUsuario", SqlDbType.Int).Value = id;
            da.SelectCommand.Parameters.Add("@nuevoEstado", SqlDbType.Int).Value = estado;
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }  

        public DataTable CrearSoportePropietarioNatural(PropietarioPathInptModel propietarioPath)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
                SqlDataAdapter da = new SqlDataAdapter("SP_CrearSoportePropietarioNatural", con);
                da.SelectCommand.Parameters.Add("@fk_propietario", SqlDbType.Int).Value = propietarioPath.IdPropietario;
                da.SelectCommand.Parameters.Add("@pk_usuario", SqlDbType.Int).Value = propietarioPath.idUsuario;
                da.SelectCommand.Parameters.Add("@rut", SqlDbType.NVarChar).Value = propietarioPath.RUT;
                da.SelectCommand.Parameters.Add("@certificacionbancaria", SqlDbType.VarChar).Value = propietarioPath.CertificacionBancaria;
                da.SelectCommand.Parameters.Add("@cedulaciudad", SqlDbType.VarChar).Value = propietarioPath.CC;
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public DataTable CrearSoportePropietarioJudicial(PropietarioPathInptModel propietarioPath)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
                SqlDataAdapter da = new SqlDataAdapter("SP_CrearSoportePropietarioJudicial", con);
                da.SelectCommand.Parameters.Add("@fk_propietariojuridico", SqlDbType.Int).Value = propietarioPath.IdPropietario;
                da.SelectCommand.Parameters.Add("@pk_usuario", SqlDbType.Int).Value = propietarioPath.idUsuario;
                da.SelectCommand.Parameters.Add("@certificacionbancaria", SqlDbType.NVarChar).Value = propietarioPath.CertificacionBancaria;
                da.SelectCommand.Parameters.Add("@nit", SqlDbType.NVarChar).Value = propietarioPath.NIT;
                da.SelectCommand.Parameters.Add("@cedularepresentante", SqlDbType.NVarChar).Value = propietarioPath.CC;
                da.SelectCommand.Parameters.Add("@camaracomercio", SqlDbType.NVarChar).Value = propietarioPath.CamaraComercio;
                da.SelectCommand.Parameters.Add("@rut", SqlDbType.NVarChar).Value = propietarioPath.RUT;
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public DataTable ConsultarUsuarioConductor(int sk_usuario)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
                SqlDataAdapter da = new SqlDataAdapter("SP_ConsultarUsuarioConductor", con);
                da.SelectCommand.Parameters.Add("@sk_usuario", SqlDbType.Int).Value = sk_usuario;
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public DataTable RegistrarSoportesConductor(ConductorPathInputModel conductorPath)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
                SqlDataAdapter da = new SqlDataAdapter("SP_RegistrarSoportesConductor", con);
                da.SelectCommand.Parameters.Add("@idConductor", SqlDbType.Int).Value = conductorPath.idConductor;
                da.SelectCommand.Parameters.Add("@cedula", SqlDbType.NVarChar).Value = conductorPath.pathCedula;
                da.SelectCommand.Parameters.Add("@licencia", SqlDbType.NVarChar).Value = conductorPath.pathLicencia;
                da.SelectCommand.Parameters.Add("@seguridad", SqlDbType.NVarChar).Value = conductorPath.pathSeguridadSoc;
                da.SelectCommand.Parameters.Add("@cursos", SqlDbType.NVarChar).Value = conductorPath.pathCursos;
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public DataTable VisualizacionVehiculo(string id)
        {
            try
            {

                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
                string Query = $"select * from TB_Vehiculo ve inner join TB_TipoDeVehiculo Us on ve.TipoVehivulo = Us.idTipoDeVehiculo where Placa = '{id}'";
                SqlCommand command = new SqlCommand(Query, con);
                SqlDataAdapter data = new SqlDataAdapter(command);
                DataTable tablejuridicos = new DataTable();
                data.Fill(tablejuridicos);
                return tablejuridicos;

            }
            catch (Exception)
            {

                throw;
            }








        }






        public DataTable listaofertas()
        {
            try
            {

                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
                string Query = "select * from TB_ClientOffers where status = 2";
                SqlCommand command = new SqlCommand(Query, con);
                SqlDataAdapter data = new SqlDataAdapter(command);
                DataTable tablejuridicos = new DataTable();
                data.Fill(tablejuridicos);
                return tablejuridicos;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public DataTable consultarempresa(int idcliente) {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
            string Query = $"  select * from TB_cliente where idCliente = {idcliente}";
            SqlCommand command = new SqlCommand(Query, con);
            SqlDataAdapter data = new SqlDataAdapter(command);
            DataTable dt = new DataTable();
            data.Fill(dt);
            return dt;
        }

        public DataTable ListaContraOfertaPropietario()
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
            //FORMAT([MyDateTime],'yyyyMMdd')
            //string Query = @"
            //             T TCOP.Sk_ContraOferta, TCOP.UbicacionVehiculo, FORMAT(TCOP.FechaHora,'yyyy/MM/dd') as FechaHora,TCOP.estado , TCOP.Costo, TCO.codeOffer, TC.nombre, TC.apellido, TV.Marca, TV.Modelo,  TCOP.fk_usuario
						      //  FROM TB_ContraOfertaPropietario TCOP
						      //  inner join TB_ClientOffers TCO on TCOP.Oferta_fk = TCO.idClientOffer
						      //  inner join TB_Vehiculo TV on TCOP.Vechiculo_id_fk = TV.idVehiculo
						      //  inner join TB_Conductor TC on TCOP.Conductor_id_fk = TC.sk_conductor;
            //                ";
            //SqlCommand command = new SqlCommand(Query, con);
            //SqlDataAdapter data = new SqlDataAdapter(command);
            //DataTable ContraOferta = new DataTable();
            //data.Fill(ContraOferta);
            //return ContraOferta;


            SqlDataAdapter da = new SqlDataAdapter("SP_ListaContraoferta ", con);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;


        }

        public DataTable AceptarRechazarContraofertas(int aceptar , int estado , int fkoferta , int fkusuario){
            try
            {   

                string Query = "";
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
                if (aceptar == 1) {
                    Query = $" update TB_ContraOfertaPropietario set estado = {estado} where Sk_ContraOferta = {fkoferta} AND fk_usuario = { fkusuario} ";
                }
                else{
                    Query = $" update TB_ContraOfertaPropietario set estado = {estado} where Sk_ContraOferta = {fkoferta} AND fk_usuario != {fkusuario} ";
                }
                SqlCommand command = new SqlCommand(Query, con);
                SqlDataAdapter data = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                data.Fill(dt);
                return dt;

            }
            catch (Exception)
            {

                throw;
            }

        }



        public DataTable PersonasParaNotificar(int aceptar,  int fkusuario)
        {
            try
            {
                string Query = "";
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
                if (aceptar == 1)
                {
                    Query = $"select * from TB_Usuario  where sk_usuario = { fkusuario} ";
                }
                else
                {
                    Query = $" select * from TB_Usuario  where sk_usuario !=  {fkusuario} ";
                }
                SqlCommand command = new SqlCommand(Query, con);
                SqlDataAdapter data = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                data.Fill(dt);
                return dt;

            }
            catch (Exception)
            {

                throw;
            }

        }
        public DataTable EmpresaAnotificar( int fkusuario)
        {
            try
            {
                string Query = "";
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
               
                    Query = $"select * from TB_cliente c inner join  TB_ContraOfertaPropCliente cpc on c.IdCliente = cpc.IdCliente inner join  TB_ClientOffers co on co.idClient = c.IdCliente inner join  TB_ContraOfertaPropietario cop on cpc.IdContraOferta = cop.Sk_ContraOferta where cop.Sk_ContraOferta = {fkusuario} ";
               
                SqlCommand command = new SqlCommand(Query, con);
                SqlDataAdapter data = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                data.Fill(dt);
                return dt;

            }
            catch (Exception)
            {

                throw;
            }

        }



        public DataTable ListaContraOfertaPropietario2()
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
            //FORMAT([MyDateTime],'yyyyMMdd')
            SqlDataAdapter da = new SqlDataAdapter("SP_ListaContraoferta ", con);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;

        }
        public DataTable ListaContraOfertaPropietario3(int id)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
            //FORMAT([MyDateTime],'yyyyMMdd')
            string Query = @" select * from TB_ContraOfertaPropietario tc 
                                  inner join TB_ClientOffers TCO on tc.Oferta_fk = TCO.idClientOffer
                                  inner join TB_ContraOfertaPropCliente tcc on 
                                  tcc.IdContraOferta = tc.Sk_ContraOferta
                                  inner join TB_Vehiculo TV on tc.Vechiculo_id_fk = TV.idVehiculo"+
                                  $" inner join TB_Conductor TCr on tc.Conductor_id_fk = TCr.sk_conductor where tcc.idCliente = '{id}';";
            SqlCommand command = new SqlCommand(Query, con);
            SqlDataAdapter data = new SqlDataAdapter(command);
            DataTable dt = new DataTable();
            data.Fill(dt);
            return dt;

        }

        public DataTable ListaContraOfertaPropietarioPorUsuario(int pk_usuario)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
            //FORMAT([MyDateTime],'yyyyMMdd')
            string Query = @"
                            SELECT TCOP.Sk_ContraOferta, TCOP.UbicacionVehiculo, FORMAT(TCOP.FechaHora,'yyyy/MM/dd') as FechaHora, TCOP.Costo, TCO.codeOffer, TC.nombre, TC.apellido, TV.Marca, TV.Modelo, TCOP.fk_usuario
						        FROM TB_ContraOfertaPropietario TCOP
						        inner join TB_ClientOffers TCO on TCOP.Oferta_fk = TCO.idClientOffer
						        inner join TB_Vehiculo TV on TCOP.Vechiculo_id_fk = TV.idVehiculo"+
						        $" inner join TB_Conductor TC on TCOP.Conductor_id_fk = TC.sk_conductor WHERE TCOP.fk_usuario = '{pk_usuario}';";
            SqlCommand command = new SqlCommand(Query, con);
            SqlDataAdapter data = new SqlDataAdapter(command);
            DataTable ContraOferta = new DataTable();
            data.Fill(ContraOferta);
            return ContraOferta;
        }




        public DataTable ConsultarInformacionAdmin(int id)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);

            SqlDataAdapter da = new SqlDataAdapter("SP_CosnsultarInformacionConductor", con);
            da.SelectCommand.Parameters.Add("@idUsuario", SqlDbType.Int).Value = id;
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;


        }


        public DataTable ConsultarInformacionAdminpj(int id)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);

            SqlDataAdapter da = new SqlDataAdapter("SP_CosnsultarInformacionPersonaJ", con);
            da.SelectCommand.Parameters.Add("@idUsuario", SqlDbType.Int).Value = id;
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;


        }

        public DataTable ConsultarInformacionAdminpn(int id)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);

            SqlDataAdapter da = new SqlDataAdapter("SP_CosnsultarInformacionPersonaN", con);
            da.SelectCommand.Parameters.Add("@idUsuario", SqlDbType.Int).Value = id;
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;


        }



        public DataTable ReporteVehiculo2(int idusuario)
        {

            try
            {

                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
                string Query = $"select * from TB_Vehiculo ve inner join   TB_TipoDeVehiculo Us on ve.TipoVehivulo = Us.idTipoDeVehiculo where  IdPropietario = {idusuario}";
                SqlCommand command = new SqlCommand(Query, con);
                SqlDataAdapter data = new SqlDataAdapter(command);
                DataTable tablevehiculo = new DataTable();
                data.Fill(tablevehiculo);
                return tablevehiculo;
            }
            catch (Exception)
            {

                throw;
            }


        }

        public DataTable visualizarinfovehiculoPropietario(int idusuario)
        {

            try
            {

                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
                string Query = $"select * from TB_Vehiculo ve inner join   TB_TipoDeVehiculo Us on ve.TipoVehivulo = Us.idTipoDeVehiculo inner join TB_Usuario tu  on ve.IdPropietario = tu.sk_usuario where idVehiculo = {idusuario}";
                SqlCommand command = new SqlCommand(Query, con);
                SqlDataAdapter data = new SqlDataAdapter(command);
                DataTable tablevehiculo = new DataTable();
                data.Fill(tablevehiculo);
                return tablevehiculo;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public DataTable ListaContraOffShearch(string value)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
            string Query = @"
                            SELECT TCOP.Sk_ContraOferta, TCOP.UbicacionVehiculo, FORMAT(TCOP.FechaHora,'yyyy/MM/dd') as FechaHora, TCOP.Costo, TCOP.PathDocumento, TCO.codeOffer, TC.nombre, TC.apellido, TV.Marca, TV.Modelo,TCOP.Sk_ContraOferta, TCOP.fk_usuario
						        FROM TB_ContraOfertaPropietario TCOP
						        inner join TB_ClientOffers TCO on TCOP.Oferta_fk = TCO.idClientOffer
						        inner join TB_Vehiculo TV on TCOP.Vechiculo_id_fk = TV.idVehiculo
						        inner join TB_Conductor TC on TCOP.Conductor_id_fk = TC.sk_conductor
						        WHERE TCOP.Sk_ContraOferta =  '" + value + "';";

            SqlCommand command = new SqlCommand(Query, con);
            SqlDataAdapter data = new SqlDataAdapter(command);
            DataTable ContraOferta = new DataTable();
            data.Fill(ContraOferta);
            return ContraOferta;
        }

        public DataTable UpdateCostoContraOferta(decimal NuevoCosto, int Sk_ContraOfertaPropiertario)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);

            SqlDataAdapter da = new SqlDataAdapter("SP_UpdateCostoContraOferta", con);
            da.SelectCommand.Parameters.Add("@NuevoCosto", SqlDbType.Money).Value = NuevoCosto;
            da.SelectCommand.Parameters.Add("@Sk_ContraOfertaPropiertario", SqlDbType.VarChar).Value = Sk_ContraOfertaPropiertario;
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }





        public DataTable ConsultarVehiculosPropietario(int idPropietario)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
                SqlDataAdapter da = new SqlDataAdapter("SP_ConsultarVehiculosPropietario", con);
                da.SelectCommand.Parameters.Add("@id_propietario", SqlDbType.VarChar).Value = idPropietario;
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public DataTable ConsultarInfoVehiculos(int idVehiculo)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
                SqlDataAdapter da = new SqlDataAdapter("SP_ConsultarInfoVehiculos", con);
                da.SelectCommand.Parameters.Add("@idVehiculo", SqlDbType.VarChar).Value = idVehiculo;
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public DataTable ConsultarConductores(int idUsuario)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
                SqlDataAdapter da = new SqlDataAdapter("SP_ConsultarConductores", con);
                da.SelectCommand.Parameters.Add("@idUsuario", SqlDbType.VarChar).Value = idUsuario;
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public DataTable ConsultarConductor(int idConductor)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
                SqlDataAdapter da = new SqlDataAdapter("SP_ConsultarUnConductor", con);
                da.SelectCommand.Parameters.Add("@idConductor", SqlDbType.VarChar).Value = idConductor;
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public DataTable ConsultarEmpresaTransporte(int idClient)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
                SqlDataAdapter da = new SqlDataAdapter("SP_ConsultarEmpresaTransporte", con);
                da.SelectCommand.Parameters.Add("@idClient", SqlDbType.Int).Value = idClient;
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public DataTable ObtenerIdOferta(string codigoOferta)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
                SqlDataAdapter da = new SqlDataAdapter("SP_ObtenerIdOferta", con);
                da.SelectCommand.Parameters.Add("@codigoOferta", SqlDbType.VarChar).Value = codigoOferta;
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public DataTable CreacionContraOfertaPropietario(ContraOfertaPropietario contraOferta)
         {
            try
            {
                DateTime DateObject = DateTime.ParseExact(contraOferta.FechaHora, "MM/dd/yyyy", null);

                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
                SqlDataAdapter da = new SqlDataAdapter("SP_CreacionContraOfertaPropietario", con);
                da.SelectCommand.Parameters.Add("@UbicacionVehiculo", SqlDbType.NVarChar).Value = contraOferta.UbicacionVehiculo;
                da.SelectCommand.Parameters.Add("@FechaHora", SqlDbType.Date).Value = DateObject;
                da.SelectCommand.Parameters.Add("@Observacion", SqlDbType.NVarChar).Value = contraOferta.Observacion;
                da.SelectCommand.Parameters.Add("@Vechiculo_id_fk", SqlDbType.Int).Value = contraOferta.IdVehiculo;
                da.SelectCommand.Parameters.Add("@PathDocumento", SqlDbType.NVarChar).Value = contraOferta.PathDocumento;
                da.SelectCommand.Parameters.Add("@Conductor_id_fk", SqlDbType.Int).Value = contraOferta.IdConductor;
                da.SelectCommand.Parameters.Add("@Costo", SqlDbType.Money).Value = contraOferta.Costo;
                da.SelectCommand.Parameters.Add("@Oferta_fk", SqlDbType.Int).Value = contraOferta.CodigoOferta;
                da.SelectCommand.Parameters.Add("@Estado", SqlDbType.Int).Value = contraOferta.Estado;
                da.SelectCommand.Parameters.Add("@Fk_usuario", SqlDbType.Int).Value = contraOferta.Fk_Usuario;
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                da.Fill(dt);

                DataRow row = dt.Rows[0];

                foreach (var item in contraOferta.Empresas)
                {
                    ContraOfertaPropietario_Cliente(int.Parse(row["idContraferta"].ToString()), item);
                }

                return dt;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void ContraOfertaPropietario_Cliente(int IdContraOferta, int IdCliente)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
                SqlDataAdapter da = new SqlDataAdapter("SP_ContraOfertaPropietario_Cliente", con);
                da.SelectCommand.Parameters.Add("@IdContraOferta", SqlDbType.Int).Value = IdContraOferta;
                da.SelectCommand.Parameters.Add("@IdCliente", SqlDbType.Int).Value = IdCliente;
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                da.Fill(dt);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public DataTable CambioEstadoOferta(string codeOffer, int estado)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
            SqlDataAdapter da = new SqlDataAdapter("SP_CambioEstado", con);
            da.SelectCommand.Parameters.Add("@idUsuario", SqlDbType.VarChar).Value = codeOffer;
            da.SelectCommand.Parameters.Add("@nuevoEstado", SqlDbType.Int).Value = estado;
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        public DataTable CambioEstadoContrOfertPropietario(int Sk_ContraOferta, int nuevoEstado)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
            SqlDataAdapter da = new SqlDataAdapter("SP_CambioEstadoContraOfertaPropietario", con);
            da.SelectCommand.Parameters.Add("@Sk_ContraOferta", SqlDbType.Int).Value = Sk_ContraOferta;
            da.SelectCommand.Parameters.Add("@nuevoEstado", SqlDbType.Int).Value = nuevoEstado;
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        public DataTable ActualizarVehiculo(VehiculoViewModels vehiculoViewModels)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
                SqlDataAdapter da = new SqlDataAdapter("ActualizardocuementosVehiculos ", con);
                da.SelectCommand.Parameters.Add("@placa", SqlDbType.NVarChar).Value = vehiculoViewModels.Placa;
                da.SelectCommand.Parameters.Add("@tarjetapropiedad", SqlDbType.NVarChar).Value = vehiculoViewModels.SoportePropiedad;
                da.SelectCommand.Parameters.Add("@soat", SqlDbType.NVarChar).Value = vehiculoViewModels.SoporteSoat;
                da.SelectCommand.Parameters.Add("@tecnomecanica", SqlDbType.NVarChar).Value = vehiculoViewModels.SoporteMecanica;
                da.SelectCommand.Parameters.Add("@todoriesgo", SqlDbType.NVarChar).Value = vehiculoViewModels.SoportetodoRiezgo;
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch (Exception)
            {

                throw;
            }


        }

        public DataTable ConsultarCorreoPropietarios()
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
                SqlDataAdapter da = new SqlDataAdapter("SP_ConsultarCorreosPropietarios", con);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public DataTable GetGeneradoresDeCarga()
        {
            try
            {

                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
                string Query = "SELECT * FROM TB_cliente clie INNER JOIN TB_ClientTypeCompany tc ON tc.idClient = clie.idCliente WHERE idEstadoCliente > 0 AND tc.IdTypeCompany = 1";
                SqlCommand command = new SqlCommand(Query, con);
                SqlDataAdapter data = new SqlDataAdapter(command);
                DataTable tableConductor = new DataTable();
                data.Fill(tableConductor);
                return tableConductor;




            }
            catch (Exception)
            {

                throw;
            }

        }

        public DataTable ConductorPropietario(int id)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);

            SqlDataAdapter da = new SqlDataAdapter("SP_ConductoresPropietarios", con);
            da.SelectCommand.Parameters.Clear();
            da.SelectCommand.Parameters.Add("@IdPropietario", SqlDbType.Int).Value = id;
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;


        }

        public DataTable BuscarCorreoUsuario(string correo)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);

            SqlDataAdapter da = new SqlDataAdapter("SP_BuscarCorreoUsuario", con);
            da.SelectCommand.Parameters.Add("@correo", SqlDbType.VarChar).Value = correo;
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;


        }

        public DataTable actualizarContrasenaUsuarioCambio(string email, byte[] contrasena, byte[] key, byte[] iv)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["BidcargoConnectionString"].ConnectionString);
                SqlDataAdapter da = new SqlDataAdapter("SP_ActualizarContrasenaUsuarioCambio", con);
                da.SelectCommand.Parameters.Add("@email", SqlDbType.VarChar).Value = email;
                da.SelectCommand.Parameters.Add("@contrasena", SqlDbType.VarBinary).Value = contrasena;
                da.SelectCommand.Parameters.Add("@Key", SqlDbType.VarBinary).Value = key;
                da.SelectCommand.Parameters.Add("@IV", SqlDbType.VarBinary).Value = iv;
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;

            }
            catch (Exception)
            {

                throw;
            }
        }


    }
}