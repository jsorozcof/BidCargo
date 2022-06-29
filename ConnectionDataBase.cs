using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public class ConnectionDataBase
{
    public class StoreProcediur
    {

        public DataTable validacionContrasenaActual(string numeroCelular)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CustomerDataConnectionString"].ConnectionString);
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
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CustomerDataConnectionString"].ConnectionString);
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
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CustomerDataConnectionString"].ConnectionString);
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
        public DataTable ObtenerData(string SP)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CustomerDataConnectionString"].ConnectionString);
                SqlDataAdapter da = new SqlDataAdapter(SP, con);
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
        
        public DataTable DeActivateUsers(int pidCliente = 0)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CustomerDataConnectionString"].ConnectionString);
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
        public DataTable DeAprovedUser(int pidCliente = 0)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CustomerDataConnectionString"].ConnectionString);
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
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CustomerDataConnectionString"].ConnectionString);
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
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CustomerDataConnectionString"].ConnectionString);
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

        public DataTable ValidarIngresoUsuario(string usuario, string macAddress)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CustomerDataConnectionString"].ConnectionString);
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
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CustomerDataConnectionString"].ConnectionString);
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
        public DataTable getClientProfile(int idCliente)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CustomerDataConnectionString"].ConnectionString);
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
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CustomerDataConnectionString"].ConnectionString);
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
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CustomerDataConnectionString"].ConnectionString);
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
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CustomerDataConnectionString"].ConnectionString);
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
        
        public DataTable CrearCliente(string nombre, string apellidoPaterno, string apellidoMaterno, 
            int idPerfil, int idHobbie, int idGenero,int idCalificacionCLiente, string idProductoDeInteres, 
            int idestadoCliente, int idMotivoConocimientoEmpresa,string numeroCelular, string comentarioAdicional, 
            string usuario, byte[] contrasena, byte[] key, byte[] iv, 
            string email,string fechaNacimiento, string usuarioFaceBook, string usuarioSnapchat, 
            string usuarioInstagram, string imagen,int idEstiloCliente, 
            int idClienteReferente, int idPais, int idCiudad, string direccion, 
            string Edificio, string cargo,string cedula, 
            int idLugar,  string fechaInial, string fechaFinal, int pidTypeCompany)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CustomerDataConnectionString"].ConnectionString);
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
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CustomerDataConnectionString"].ConnectionString);
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
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CustomerDataConnectionString"].ConnectionString);
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
        public DataTable saveFile(string nameImg, string srcImg, string typeFile, int idCliente, int type, int typeDoct)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CustomerDataConnectionString"].ConnectionString);
                SqlDataAdapter da = new SqlDataAdapter("SP_saveFileClient", con);
                da.SelectCommand.Parameters.Add("@pnameImg", SqlDbType.VarChar).Value = nameImg;
                da.SelectCommand.Parameters.Add("@psrcImg", SqlDbType.VarChar).Value = srcImg;
                da.SelectCommand.Parameters.Add("@pfileType", SqlDbType.VarChar).Value = typeFile;
                da.SelectCommand.Parameters.Add("@pidCliente", SqlDbType.Int).Value = idCliente;
                da.SelectCommand.Parameters.Add("@ptype", SqlDbType.Int).Value = type;
                da.SelectCommand.Parameters.Add("@pidDocument", SqlDbType.Int).Value = typeDoct;
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
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CustomerDataConnectionString"].ConnectionString);
                SqlDataAdapter da = new SqlDataAdapter("SP_storeOffers", con);
                string varnacdtaome = "";
                if (model.idNACDTAOMT != null) {
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
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CustomerDataConnectionString"].ConnectionString);
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
        public DataTable getOffer(string pcodeOffer = "", int ptype = 0, int pidClient = 0 )
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CustomerDataConnectionString"].ConnectionString);
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
        public DataTable getCompaniesAcceptedByCodeOffers(string pcodeOffer = "")
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CustomerDataConnectionString"].ConnectionString);
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
        
        public DataTable getContracOffers(int pidClient = 0 , string pcodeOffer = "")
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CustomerDataConnectionString"].ConnectionString);
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

        public DataTable obtenerCorreos(int idTipoClasificacion)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CustomerDataConnectionString"].ConnectionString);
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
        public DataTable deleteOffer(int pidClientOffer)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CustomerDataConnectionString"].ConnectionString);
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
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CustomerDataConnectionString"].ConnectionString);
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
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CustomerDataConnectionString"].ConnectionString);
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
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CustomerDataConnectionString"].ConnectionString);
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
        public DataTable acceptContraOffer(int pidContraOffer = 0, int pstatus =0)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CustomerDataConnectionString"].ConnectionString);
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
        public DataTable getCliente(int idCliente)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CustomerDataConnectionString"].ConnectionString);
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
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CustomerDataConnectionString"].ConnectionString);
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
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CustomerDataConnectionString"].ConnectionString);
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
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CustomerDataConnectionString"].ConnectionString);
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
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CustomerDataConnectionString"].ConnectionString);
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
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CustomerDataConnectionString"].ConnectionString);
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
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CustomerDataConnectionString"].ConnectionString);
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

        public DataTable storeQualificationContraOffer(int idUserQualifying = 0,int pidContraOffer = 0, int pQualification = 0, string pComments = "")
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CustomerDataConnectionString"].ConnectionString);
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

        public DataTable getQualificationByContraOffer( int pidContraOffer = 0)
        {
            try
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CustomerDataConnectionString"].ConnectionString);
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
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CustomerDataConnectionString"].ConnectionString);
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
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CustomerDataConnectionString"].ConnectionString);
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
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CustomerDataConnectionString"].ConnectionString);
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
    }
}
