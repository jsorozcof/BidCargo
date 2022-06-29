using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Data;
using System.Security.Cryptography;
using System.IO;
using Newtonsoft.Json;
using System.Linq;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Text.RegularExpressions;
using System.Web.UI;

namespace BidCargo_.Controllers
{
    public class HomeController : Controller
    {
        private static Random random = new Random();
        public ActionResult Login(Models.Login model)
        {
            Session["idCliente"] = null;
            Session["idTypeCompany"] = null;
            Session["nombre"] = null;

            if (model.usuario != null)
            {
                ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
                CredencialesDeAcceso acceso = new CredencialesDeAcceso();
                DataTable dt = data.ValidarIngresoUsuario(model.usuario, GetMACAddress().ToString());

                DataRow row = dt.Rows[0];
                byte[] contrasena = (byte[])row["contrasena"];
                byte[] key = (byte[])row["key"];
                byte[] iv = (byte[])row["iv"];
                if (contrasena.Length > 2)
                {
                    string contrasenaFinal = acceso.DecryptStringFromBytes(contrasena, key, iv);
                    if (contrasenaFinal == model.contrasena)
                    {
                        Session["idCliente"] = row["idCliente"].ToString();
                        Session["nombre"] = row["nombre"].ToString();
                        Session["email"] = row["email"].ToString();
                        Session["idPerfil"] = row["idPerfil"].ToString();
                        Session["usuarioFacebook"] = row["usuarioFacebook"].ToString();
                        if (Convert.ToInt32(row["idPerfil"]) == 5)//esto es para los administradores
                        {
                            return RedirectToAction("Users");
                        }
                        if (Convert.ToInt32(row["idCalificacionCliente"])== 1)
                        {
                            return RedirectToAction("actualizarContrasena", new { @id=row["numeroCelular"]});
                        }
                        DataTable fe = data.getTypeCompanyByClient(Convert.ToInt32(row["idCliente"]));
                        bool entre = false;
                        foreach (dynamic ws in fe.Rows)
                        {
                            if (Convert.ToInt32(ws["idTypeCompany"]) == 1)
                            {
                                entre = true;
                                Session["principal"] = true;
                                if((Convert.ToInt32(ws["principal"])) == 1)
                                {
                                    Session["idTypeCompany"] = Convert.ToInt32(ws["idTypeCompany"]);
                                }
                            }
                            if (Convert.ToInt32(ws["idTypeCompany"]) == 2 || Convert.ToInt32(ws["idTypeCompany"]) == 3 || Convert.ToInt32(ws["idTypeCompany"]) == 4
                                             || Convert.ToInt32(ws["idTypeCompany"]) == 5 || Convert.ToInt32(ws["idTypeCompany"]) == 8)
                            {
                                if (entre == false)
                                    Session["oferta"] = true;
                                if ((Convert.ToInt32(ws["principal"])) == 1)
                                {
                                    Session["idTypeCompany"] = Convert.ToInt32(ws["idTypeCompany"]);
                                }
                            }
                            if (Convert.ToInt32(ws["idTypeCompany"]) == 6 || Convert.ToInt32(ws["idTypeCompany"]) == 7) { 
                                Session["factoring"] = true;
                                if ((Convert.ToInt32(ws["principal"])) == 1)
                                {
                                    Session["idTypeCompany"] = Convert.ToInt32(ws["idTypeCompany"]);
                                }
                            }
                        }
                        DataTable dd = data.getClientProfile(Convert.ToInt32(row["idCliente"]));
                        if (dd.Rows.Count == 0)//para saber si ya completo el perfil
                        {
                            Session.Remove("principal");
                            Session.Remove("factoring");
                            Session.Remove("oferta");
                            Session["message"] = "Por favor complete su registro en el sistema, gracias.";
                            Session["type"] = "info";
                            return RedirectToAction("completarRegistro");
                        }
                        if (Convert.ToInt32(row["idEstadoCliente"]) == 0)//en revision
                        {
                            Session.Remove("principal");
                            Session.Remove("factoring");
                            Session.Remove("oferta");
                            Session.Remove("idCliente");
                            Session.Remove("nombre");
                            Session.Remove("email");
                            Session.Remove("idPerfil");
                            Session.Remove("idTypeCompany");
                            Session["message"] = "Su perfil se encuentra en revision, contacte con el administrador, disculpe las molestias...";
                            Session["type"] = "info";
                            return View();
                        }
                        if (Convert.ToInt32(row["idEstadoCliente"]) == -1)//perfil no aprobado
                        {
                            Session.Remove("principal");
                            Session.Remove("factoring");
                            Session.Remove("oferta");
                            Session.Remove("idCliente");
                            Session.Remove("nombre");
                            Session.Remove("email");
                            Session.Remove("idPerfil");
                            Session.Remove("idTypeCompany");
                            Session["message"] = "Su perfil NO ESTA APROBADO, contacte con el administrador, disculpe las molestias...";
                            Session["type"] = "info";
                            return View();
                        }
                        if (Convert.ToInt32(row["idEstadoCliente"]) == -2)//usuario desactivado
                        {
                            Session.Remove("principal");
                            Session.Remove("factoring");
                            Session.Remove("oferta");
                            Session.Remove("idCliente");
                            Session.Remove("nombre");
                            Session.Remove("email");
                            Session.Remove("idPerfil");
                            Session.Remove("idTypeCompany");
                            Session["message"] = "Su perfil se encuentra DESACTIVADO, contacte con el administrador, disculpe las molestias...";
                            Session["type"] = "info";
                            return View();
                        }

                        if (Convert.ToInt32(Session["idTypeCompany"]) == 1)
                            return RedirectToAction("createOffer");
                        else
                            return RedirectToAction("offerShow");
                    }
                }
                Session["message"] = "Por favor validar el usuario y/o password que se encuentra digitando...";
                Session["type"] = "error";
            }
            return View();
        }


      

        /************ Administrador ***************/
        public ActionResult Users()
        {
            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            DataTable dd = data.consultarCliente(Convert.ToInt32(Session["idCliente"]));
            if (dd.Rows.Count > 0)
                ViewBag.data = dd.Rows[0];
            else
                return RedirectToAction("Index");

            dd = data.ObtenerData("SP_GetUsers");
            ViewBag.rows = dd.Rows;
            return View("/Views/Home/Users/table.cshtml");
        }

        public ActionResult DeActivateUsers(int id)//id del usuario
        {
            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            DataTable dd = data.DeActivateUsers(id);
            EnviarCorreos correoCreacion = new EnviarCorreos();
            string texto = "", bodyCorreo = "";
            if (Convert.ToInt32(dd.Rows[0]["idEstadoCliente"]) == 0)
            {
                texto = "Perfil de BidCargo Activado";
                bodyCorreo = correoCreacion.ArmarCorreoActiveProfile(dd.Rows[0], Session["urlHttp"].ToString(), 1);
            }
            else
            {
                texto = "Perfil de BidCargo Desactivado";
                bodyCorreo = correoCreacion.ArmarCorreoActiveProfile(dd.Rows[0], Session["urlHttp"].ToString(), 0);
            }
            
            //correoCreacion.EnviarCorreo("bcpruebagen1@gmail.com", texto, "cma010360@gmail.com", bodyCorreo, "contacto@bidcargo.com.co", "contacto@bidcargo.com.co", "Cid2306*", "");
            correoCreacion.EnviarCorreo(dd.Rows[0]["email"].ToString(), texto+ " - "+ dd.Rows[0]["email"].ToString(), "cma010360@gmail.com", bodyCorreo, "bidCargo@hotmail.com", "bidCargo@hotmail.com", "bidC#123", "");
            Session["message"] = "Modificacion Realizada con éxito.";

            return RedirectToAction("Users");
        }

        //Eliminar Empresa
        public ActionResult DeleteUsers(int id)//id del usuario
        {
            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            DataTable dd = data.DeleteUsers(id);
            Session["message"] = "El Registro fue eliminado con éxito.";

            return RedirectToAction("Users");
        }

        public ActionResult deleteOffer(int id)//1
        {
            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            DataTable dt = data.deleteOffer(id);
            ViewBag.offers = dt.Rows.Count;
            if (ViewBag.models == 0)
            {
                Session.Remove("codeOffer");
                Session.Remove("moreContainer");
                Session.Remove("model");
                Session.Remove("message");
            }

            //return Json(new { result = 1 }, JsonRequestBehavior.AllowGet);

            return RedirectToAction("AdminOffers");

        }

        public ActionResult DelUserSis(int id)//id del usuario
        {
            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            DataTable dd = data.SaveUsuarioSistema(0, id, 3, "", "", "","", 0, "",Convert.ToDateTime(null), 0, 0);
            Session["message"] = "El Registro fue eliminado con éxito.";

            return RedirectToAction("AdminUsers");
        }

        public ActionResult DeAprovedUser(int id)//id del usuario
        {
            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            DataTable dd = data.DeAprovedUser(id);
            EnviarCorreos correoCreacion = new EnviarCorreos();
            string texto = "", bodyCorreo = "";
            if (Convert.ToInt32(dd.Rows[0]["idEstadoCliente"]) == 1)
            {
                texto = "Perfil de BidCargo Aprobado";
                bodyCorreo = correoCreacion.ArmarCorreoAprovedProfile(dd.Rows[0], Session["urlHttp"].ToString(), 1);
            }
            else
            {
                texto = "Perfil de BidCargo No Aprobado";
                bodyCorreo = correoCreacion.ArmarCorreoAprovedProfile(dd.Rows[0], Session["urlHttp"].ToString(), 0);
            }
            
            //correoCreacion.EnviarCorreo("bcpruebagen1@gmail.com", texto, "cma010360@gmail.com", bodyCorreo, "contacto@bidcargo.com.co", "contacto@bidcargo.com.co", "Cid2306*", "");
            correoCreacion.EnviarCorreo(dd.Rows[0]["email"].ToString(), texto+ " - "+ dd.Rows[0]["email"].ToString(), "cma010360@gmail.com", bodyCorreo, "bidCargo@hotmail.com", "bidCargo@hotmail.com", "bidC#123", "");
            Session["message"] = "Modificacion Realizada con éxito.";

            return RedirectToAction("Users");
        }
        
        public ActionResult ShowProfile(int id)//id del usuario
        {
            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            DataTable dsd = data.consultarCliente(Convert.ToInt32(Session["idCliente"]));
            if (dsd.Rows.Count > 0)
                ViewBag.data = dsd.Rows[0];
            else
                return RedirectToAction("Index");

            DataTable dd = data.consultarCliente(id);
            ViewBag.row = dd.Rows[0];
            DataTable dj = data.getClientProfile(id);
            if (dj.Rows.Count > 0)
                ViewBag.dataProfile = dj.Rows[0];
            DataTable dt = data.getTypeCompanyByClient(id);
            ViewBag.profileTypeCompany = dt.Rows;
            CredencialesDeAcceso acceso = new CredencialesDeAcceso();
            ViewBag.passwordread = acceso.DecryptStringFromBytes(ViewBag.row["contrasena"], ViewBag.row["Key"], ViewBag.row["IV"]);
            int vals = 0;
            foreach(var row in ViewBag.profileTypeCompany)
            {
                if (Convert.ToInt32(row["principal"]) == 1)
                    vals = Convert.ToInt32(row["idTypeCompany"]);
            }
            ViewBag.documents = data.getDocuments(vals).Rows;
            ViewBag.myDocuments = data.getMyDocuments(id).Rows;

            return View("/Views/Home/Users/show.cshtml");
        }

        public ActionResult AdminOffers()
        {
            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            DataTable dd = data.consultarCliente(Convert.ToInt32(Session["idCliente"]));
            if (dd.Rows.Count > 0)
                ViewBag.data = dd.Rows[0];
            else
                return RedirectToAction("Index");

            ViewBag.offers = data.getOffer("",3).Rows;


            return View("/Views/Home/Users/tableOffers.cshtml");
        }

        public ActionResult AdminShowOffer(string id)
        {
            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            DataTable dd = data.consultarCliente(Convert.ToInt32(Session["idCliente"]));
            DataTable dt = data.getOffer(id, 4);


            DataTable ClientDs = data.getContracOffers(Convert.ToInt32(Session["idCliente"]), id);
            if (ClientDs.Rows.Count > 0)
                ViewBag.dt = dd.Rows;

            else
                ViewBag.dt = null;


            if (dd.Rows.Count > 0)
            {
                ViewBag.data = dd.Rows[0];
                ViewBag.idClientOffer = dt.Rows[0]["idClient"];
            }

            else
            {
                return RedirectToAction("Index");
            }


            DataTable idClientes = data.getContracOffers(0, id);
            ViewBag.dt = data.getContracOffers(0, id).Rows;
            ViewBag.myDocuments = data.getMyDocumentsOffer(Convert.ToInt32(ViewBag.idClientOffer), id, 0).Rows; // pendinte pasar id de cliente en oferta

            ViewBag.models = data.getOffer(id, 4).Rows;
            string[] listDocuments = null;
            DataTable dtc = new DataTable("myDocumentsOffer");
            foreach (dynamic row in ViewBag.models)
            {

                var splitSrcFile = Convert.ToString(row["srcFile"]);
                if (splitSrcFile != "")
                {
                    listDocuments = splitSrcFile.Split(new string[] { "@#" }, StringSplitOptions.None);

                    dtc.Columns.Add("srcFile");
                    foreach (string value in listDocuments)
                    {
                        dtc.Rows.Add(value);

                    }

                    ViewBag.myDocumentsOfferServiceCarga = dtc.Rows;

                }
                else {
                    ViewBag.myDocumentsOfferServiceCarga = null;
                }
            }

  
            if (idClientes.Rows.Count > 0)
            {
                foreach (dynamic row in idClientes.Rows)
                {
                    ViewBag.myDocumentsContraOffer = data.getMyDocumentsOffer(row["idClient"], id, 1).Rows;

                }
            }

            return View("/Views/Home/Users/showOffers.cshtml");
        }

        public ActionResult CloseOffer(string id)//codigo de la oferta
        {
            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            DataTable dd = data.consultarCliente(Convert.ToInt32(Session["idCliente"]));
            EnviarCorreos correoCreacion = new EnviarCorreos();
            if (dd.Rows.Count > 0)
                ViewBag.data = dd.Rows[0];
            else
                return RedirectToAction("Index");

            DataTable dt = data.closeOffer(id);
            DataTable dtt = data.NoAcceptContraOffer(id); // cantidad ofertas.
            if (dtt.Rows.Count > 0)
            {
                foreach (dynamic row in dtt.Rows)
                {
                    string bodyCorreo = correoCreacion.sendMailNOAccepOffer(row, Session["urlHttp"].ToString());
                    Console.Write(row["email"].ToString());
                    correoCreacion.EnviarCorreo(row["email"].ToString(), "Oferta no Seleccionada - " + row["email"].ToString(), "cma010360@gmail.com", bodyCorreo, "bidCargo@hotmail.com", "bidCargo@hotmail.com", "bidC#123", "");
                }
            }
            Session["mensaje"] = "Oferta Cerrada Exitosamente...";
            return RedirectToAction("AdminShowOffer", new { @id= id });
        }
        
        public JsonResult SaveQualification(int id)
        {
            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            int valos = 0;
            ViewBag.rows = data.storeQualificationContraOffer(Convert.ToInt32(Session["idCliente"]), id, Convert.ToInt32(Request["qual"]), Request["comments"].ToString()).Rows;
            if (Convert.ToInt32(ViewBag.rows[0]["respuesta"]) > 0)
                valos = 1;
            return Json(new { status= valos }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult getQualification(int id)//id contraoferta
        {
            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            DataTable dt = data.getQualificationByContraOffer(id);
            ViewBag.result = datatabletojson(dt);
            return Json(ViewBag.result, JsonRequestBehavior.AllowGet);
        }
        
        /*********** Fin Administrador *****************/
        public ActionResult Index()
        {
            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            DataTable dd = data.consultarCliente(Convert.ToInt32(Session["idCliente"]));
            if (dd.Rows.Count > 0)
                ViewBag.data = dd.Rows[0];

            Session["mensaje"] = "";
            Session.Remove("codeOffer");
            Session.Remove("moreContainer");
            Session.Remove("model");
            //Session.Remove("message");

            return View();
        }
        public JsonResult ObtenerCiudadLista(int idDepartamentos)
        {
            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            DataTable dt = data.getCitybyDepatamento("SP_getCityByDepartament",idDepartamentos);
            ViewBag.ListaCiudades = ToSelectList(dt, "idCiudad", "Ciudad");
            //dt.Configuration.ProxyCreationEnabled = false;
            return Json(ViewBag.ListaCiudades, JsonRequestBehavior.AllowGet);
        }
        public ActionResult completarRegistro()
        {
            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            DataTable dd = data.consultarCliente(Convert.ToInt32(Session["idCliente"]));
            if (dd.Rows.Count > 0)
                ViewBag.data = dd.Rows[0];
            else
                return RedirectToAction("Index");

            Session.Remove("mensaje");

            ViewBag.data = dd.Rows[0];
            ViewBag.usuarioFacebook = dd.Rows[0]["usuarioFacebook"].ToString();
            ViewBag.apellidoPaterno = dd.Rows[0]["apellidoPaterno"].ToString();
            ViewBag.Nit = dd.Rows[0]["cedula"].ToString();
            ViewBag.nombre = dd.Rows[0]["nombre"].ToString();
            ViewBag.numeroCelular = dd.Rows[0]["numeroCelular"].ToString();
            ViewBag.telefono = dd.Rows[0]["Edificio"].ToString();
            ViewBag.correoContacto = dd.Rows[0]["email"].ToString();

            DataTable dt = data.ObtenerData("SP_departamentos");
            ViewBag.ListaDepartamento = ToSelectList(dt, "idDepartamentos", "departamentos");

            dt = data.ObtenerData("SP_getTypeCompany");
            ViewBag.ListTypeCompany = dt.Rows;

            dt = data.ObtenerData("SP_Ciudad");
            ViewBag.ListCities = ToSelectList(dt, "idCiudad", "ciudad");

            ViewBag.ListCompany = data.getTypeCompanyByClient(Convert.ToInt32(Session["idCliente"])).Rows;
            dt = data.getClientProfile(Convert.ToInt32(Session["idCliente"]));
            if (dt.Rows.Count > 0)
            {
                ViewBag.cargoContacto = dt.Rows[0]["jobPosition"];
                ViewBag.departament = dt.Rows[0]["idDepartament"];
                ViewBag.percent = dt.Rows[0]["percentCommission"];
            }
            else
            {
                ViewBag.cargoContacto ="";
                ViewBag.departament = "";
                ViewBag.percent = 1;
            }

            ViewBag.documents = data.getDocuments(Convert.ToInt32(Session["idTypeCompany"])).Rows;
            return View();
        }
        public SelectList ToSelectList(DataTable table, string valueField, string textField)
        {
            List<SelectListItem> list = new List<SelectListItem>();

            foreach (DataRow row in table.Rows)
            {
                list.Add(new SelectListItem()
                {
                    Text = row[textField].ToString(),
                    Value = row[valueField].ToString()
                });
            }

            return new SelectList(list, "Value", "Text");
        }

        public string GetMACAddress()
        {
            System.Net.NetworkInformation.NetworkInterface[] nics = System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces();
            String sMacAddress = string.Empty;
            foreach (System.Net.NetworkInformation.NetworkInterface adapter in nics)
            {
                if (sMacAddress == String.Empty)// only return MAC Address from first card  
                {
                    System.Net.NetworkInformation.IPInterfaceProperties properties = adapter.GetIPProperties();
                    sMacAddress = adapter.GetPhysicalAddress().ToString();
                }
            }
            return sMacAddress;
        }

        public ActionResult actualizarContrasenaActual(Models.actualizarPassword model)
        {
            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            CredencialesDeAcceso acceso = new CredencialesDeAcceso();
            RijndaelManaged myRijndael = new RijndaelManaged();
            myRijndael.GenerateKey();
            myRijndael.GenerateIV();
            if (model.newpassword == model.confirmnewpassword)
            {
                string sub = Session["customerId"].ToString();
                Byte[] contrasenaEncriptada = acceso.EncryptStringToBytes(model.newpassword, myRijndael.Key, myRijndael.IV);
                DataTable dtd = data.validacionContrasenaActual(sub);
                DataRow rowd = dtd.Rows[0];
                byte[] contrasena = (byte[])rowd["contrasena"];
                byte[] key = (byte[])rowd["key"];
                byte[] iv = (byte[])rowd["iv"];
                string contrasenaFinal = acceso.DecryptStringFromBytes(contrasena, key, iv);
                if (contrasenaFinal != model.newpassword)
                {
                    Byte[] contrasenaEncriptadaAntigua = acceso.EncryptStringToBytes(model.actual, myRijndael.Key, myRijndael.IV);
                    DataTable dt = data.actualizarContrasenaConfirmacion(sub, contrasenaEncriptada, myRijndael.Key, myRijndael.IV, contrasenaEncriptadaAntigua, myRijndael.Key, myRijndael.IV);
                    DataRow row = dt.Rows[0];
                    if (dt.Rows.Count > 0)
                    {
                        Session["message"] = "Contraseña actualizada con exito, por favor ingrese con su usuario y nueva contraseña";
                        Session["type"] = "success";
                        return RedirectToAction("Login");

                    }
                }
                else
                {
                    Session["message"] = "Contraseña nueva es igual es la contraseña guardada...";
                }
            }
            return RedirectToAction("actualizarContrasena", new { @id = Session["customerId"] });
        }
        public JsonResult sendMailPhone()
        {
            EnviarCorreos correoCreacion = new EnviarCorreos();
            string bodyCorreo = correoCreacion.ArmarCorreoPhone(Request["phonemailsss"].ToString()); 
            //correoCreacion.EnviarCorreo("bcpruebagen1@gmail.com", "Posible generador de carga o prestador de Servicios en Transportes", "cma010360@gmail.com", bodyCorreo, "contacto@bidcargo.com.co", "contacto@bidcargo.com.co", "Cid2306*", "");
            correoCreacion.EnviarCorreo("cma010360@gmail.com", "Posible generador de carga o prestador de Servicios en Transportes", "cma010360@gmail.com", bodyCorreo, "bidCargo@hotmail.com", "bidCargo@hotmail.com", "bidC#123", "");
            return Json(1, JsonRequestBehavior.AllowGet);

        }

        public ActionResult recuperarContrasena(Models.olvidoSuContrasena model)
        {
            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            string respuesta = "";
            CredencialesDeAcceso acceso = new CredencialesDeAcceso();
            string contrasena = acceso.creacionContrasena();
            RijndaelManaged myRijndael = new RijndaelManaged();
            myRijndael.GenerateKey();
            myRijndael.GenerateIV();
            Byte[] contrasenaEncriptada = acceso.EncryptStringToBytes(contrasena, myRijndael.Key, myRijndael.IV);
            DataTable dt = data.actualizarContrasena(model.email.Trim(), contrasenaEncriptada, myRijndael.Key, myRijndael.IV);
            if (dt.Rows.Count == 1)
            {
                DataRow row = dt.Rows[0];
                string[] nombreTelefono = respuesta.Split('-');
                EnviarCorreos correoCreacion = new EnviarCorreos();
                string bodyCorreo = correoCreacion.ArmarCorreoRecuperacionContrasena(row["Nombre"].ToString(), contrasena, 0, row["numeroCelular"].ToString(), Session["urlHttp"].ToString());
                //correoCreacion.EnviarCorreo("bcpruebagen1@gmail.com", "Recuperación de contraseña", "cma010360@gmail.com", bodyCorreo, "contacto@bidcargo.com.co", "contacto@bidcargo.com.co", "Cid2306*", "");
                //correoCreacion.EnviarCorreo("contacto@bidcargo.com.co", "Recuperación de contraseña - " + row["email"].ToString(), "cma010360@gmail.com", bodyCorreo, "jose.escobar@metnet.co", "jose.escobar@metnet.co", "13A132b17#", "");

                correoCreacion.EnviarCorreo(row["email"].ToString(), "Recuperación de contraseña", "cma010360@gmail.com", bodyCorreo, "bidCargo@hotmail.com", "bidCargo@hotmail.com", "bidC#123", "");


                Session["message"] = "Se ha enviado un correo con la nueva contraseña, por favor validar";
            }
            else
            {

                Session["message"] = "Por favor validar el correo que se encuentra digitando";
                return RedirectToAction("olvidoSuContrasena");
            }
            return RedirectToAction("Index");
        }

        public ActionResult actualizarContrasena(string id)
        {
            Session.RemoveAll();
            Session["customerId"] = id;
            return View("~/Views/Home/actualizarContrasena.cshtml");
        }
        public ActionResult olvidoSuContrasena()
        {
            return View("");
        }
        public ActionResult registroInicial()
        {
            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            DataTable dd = data.consultarCliente(Convert.ToInt32(Session["idCliente"]));
            if (dd.Rows.Count > 0)
                ViewBag.data = dd.Rows[0];
            if (Session["mensaje"] != null)
            {
                ViewBag.Message = Session["mensaje"].ToString();
            }
            else
            {
                ViewBag.Message = "";
            }
            dd = data.ObtenerData("SP_getTypeCompany");
            ViewBag.ListTypeCompany = ToSelectList(dd, "idTypeCompany", "name");
            return View();
        }
        public ActionResult About()
        {
            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            DataTable dd = data.consultarCliente(Convert.ToInt32(Session["idCliente"]));
            if (dd.Rows.Count > 0)
                ViewBag.data = dd.Rows[0];

            return View();
        }

        public ActionResult Contact()
        {
            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            DataTable dd = data.consultarCliente(Convert.ToInt32(Session["idCliente"]));
            if (dd.Rows.Count > 0)
                ViewBag.data = dd.Rows[0];

            return View("/Views/Home/Contact.cshtml");
        }
        public ActionResult Logout()
        {
            Session.RemoveAll();
            return RedirectToAction("Index");
        }


        /************ Usuario Sistema ***************/
        public ActionResult AdminUsers()
        {
            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            DataTable dd = data.getUsuarioSistema(Convert.ToInt32(Session["idCliente"]), 0);
            DataTable dt = data.ObtenerData("SP_departamentos");

            if (dd.Rows.Count > 0)
                ViewBag.data = dd.Rows[0];
            else
                return RedirectToAction("Index");

            dt = data.ObtenerData("SP_getTypeCompany");
            if (dt.Rows.Count > 0)
                ViewBag.ListTypeCompany = dt.Rows;


            ViewBag.AdminUsers = dd.Rows;
            return View("/Views/Home/Users/AdminUsers.cshtml");
        }


        public ActionResult Add()
        {
            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            DataTable dd = data.consultarCliente(Convert.ToInt32(Session["idCliente"]));
            DataTable dt = data.ObtenerData("SP_departamentos");

            if (dd.Rows.Count > 0)
                ViewBag.data = dd.Rows[0];
            else
                return RedirectToAction("Index");


            dt = data.ObtenerData("SP_getTypeCompany");
            if (dt.Rows.Count > 0)
                ViewBag.ListTypeCompany = dt.Rows;



            ViewBag.AdminUsers = dd.Rows;
            return View("/Views/Home/Users/Add.cshtml");
        }

        public ActionResult detailUsers(int id)
        {
            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            DataTable dsd = data.getUsuarioSistema(Convert.ToInt32(Session["idCliente"]),id);
            DataTable dt = data.ObtenerData("SP_departamentos");
            if (dsd.Rows.Count > 0)
                ViewBag.data = dsd.Rows[0];
            else
                return RedirectToAction("Index");

            ViewBag.row = dt.Rows[0];
            dt = data.ObtenerData("SP_getTypeCompany");
            ViewBag.ListTypeCompany = ToSelectList(dt, "idTypeCompany", "name");

            return View("/Views/Home/Users/datailusers.cshtml");
        }



        public ActionResult SaveUsuarioSistema(Models.UsuarioSistema model)
        {
            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            CredencialesDeAcceso acceso = new CredencialesDeAcceso();
            string contrasena = model.Contrasena;
            RijndaelManaged myRijndael = new RijndaelManaged();
            myRijndael.GenerateKey();
            myRijndael.GenerateIV();
            model.CompanyId  = Convert.ToInt32(Session["idCliente"]);

            string contrasenaEncriptada = acceso.EncryptStringToBytes(contrasena, myRijndael.Key, myRijndael.IV).ToString();
           

            DataTable dt = data.SaveUsuarioSistema(0,0, 1, model.Nombre, model.Apellido, model.Celular, contrasenaEncriptada, 1, model.Correo, DateTime.Now, model.CompanyId, 1);
            DataRow row = dt.Rows[0];
            Console.WriteLine(row);
            if (dt.Rows.Count == 1)
            {
                if (row["respuesta"].ToString() == "0")
                {
                    ///RedirectToAction("Add.cshtml");
                    Session["error"] = "error";
                    Session["mensaje"] = "El usuario con el Email " + model.Correo + " ya se encuentra registrado en el sistema";
                }
            }


            return RedirectToAction("AdminUsers");
        }

        public ActionResult verPerfil()
        {
            if (Convert.ToInt32(Session["idCliente"]) > 0)
            {
                try
                {
                    ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();

                    DataTable dd = data.consultarCliente(Convert.ToInt32(Session["idCliente"]));
                    if (dd.Rows.Count > 0)
                        ViewBag.data = dd.Rows[0];
                    else
                        return RedirectToAction("Index");

                    Session.Remove("mensaje");

                    ViewBag.data = dd.Rows[0];
                    DataTable dt = data.ObtenerData("SP_departamentos");
                    if (dt.Rows.Count > 0)
                        ViewBag.ListaDepartamento = dt.Rows;


                    dt = data.ObtenerData("SP_getTypeCompany");
                    if(dt.Rows.Count > 0)
                        ViewBag.ListTypeCompany = dt.Rows;

                    dt = data.ObtenerData("SP_Ciudad"); 
                    if (dt.Rows.Count > 0)
                        ViewBag.ListCities = dt.Rows;
                    DataTable dj = data.getClientProfile(Convert.ToInt32(Session["idCliente"]));
                    if (dj.Rows.Count > 0)
                        ViewBag.dataProfile = dj.Rows[0];
                    else
                        return RedirectToAction("completarRegistro");
                    

                    dt = data.getTypeCompanyByClient(Convert.ToInt32(Session["idCliente"]));
                    ViewBag.profileTypeCompany= dt.Rows;

                    ViewBag.documents = data.getDocuments(Convert.ToInt32(Session["idTypeCompany"])).Rows;

                    ViewBag.myDocuments = data.getMyDocuments(Convert.ToInt32(Session["idCliente"])).Rows;

                    return View("~/Views/Home/PageProfile.cshtml");
                }
                catch (Exception e)
                {
                    return RedirectToAction("Index");
                }
            }
            else
                return RedirectToAction("Index");
        }



        public ActionResult createOffer()
        {
            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            
            DataTable dt = data.ObtenerData("SP_TipoContenedor");
            ViewBag.listaTipoContenedor = ToSelectList(dt, "idTipoContenedor", "tipoContenedor");

            dt = data.ObtenerData("SP_factoring");
            ViewBag.listaFactoring = ToSelectList(dt, "idFactoring", "factoring");

            dt = data.ObtenerData("SP_NACDTAOMT");
            ViewBag.listaNACDTAOMT = ToSelectList(dt, "idNACDTAOMT", "NACDTAOMT");

      

            dt = data.ObtenerData("SP_getTypeCompany");

            dt = data.getTypeCompanyByClient(Convert.ToInt32(Session["idCliente"]));
            ViewBag.profileTypeCompany = dt.Rows;
           
            dt = data.ObtenerData("SP_getTypeCompany");
            if (dt.Rows.Count > 0)
                ViewBag.ListTypeCompany = dt.Rows;
            //ViewBag.ListTypeCompany = ToSelectList(dt, "idTypeCompany", "name");

            dt = data.getTypeCompanyByCustom(Convert.ToInt32(5));
            if (dt.Rows.Count > 0)
                ViewBag.ListTypeCompanyByCustom = dt.Rows;



            dt = data.ObtenerData("SP_TipoDeVehiculo");
            ViewBag.listaTipoDeVehiculo = ToSelectList(dt, "idTipoDeVehiculo", "TipoDeVehiculo");

            dt = data.ObtenerData("SP_Organizar");
            ViewBag.listaOrganizar = ToSelectList(dt, "idOrganizar", "Organizar");

            dt = data.getTypeCotizacion(1);
            ViewBag.listaTipoDeCotizacion = ToSelectList(dt, "idTipoDeCotizacion", "TipoDeCotizacion");

            dt = data.getTypeCotizacion(2);
            ViewBag.listaTipoDeCotizacion2 = dt;

            DataTable dd = data.consultarCliente(Convert.ToInt32(Session["idCliente"]));
            if (dd.Rows.Count > 0)
                ViewBag.data = dd.Rows[0];
            else
                return RedirectToAction("Index");

            if(Session["moreContainer"] == null)
                Session["moreContainer"] = 0;
            else if(Convert.ToInt32(Session["moreContainer"]) == 1)
            {
                ViewBag.allModels = data.getOffer(Session["codeOffer"].ToString(),-1).Rows;
                ViewBag.models = ViewBag.allModels[0];
            }
            dt = data.ObtenerData("SP_departamentos");
            ViewBag.departament = dt.Rows;

            dt = data.ObtenerData("SP_GetCoinTypes");
            ViewBag.getCoinTypes = ToSelectList(dt, "Id", "Code");

            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem() { Text = "Fecha Estimada", Value = "1" });
            items.Add(new SelectListItem() { Text = "Fecha Confirmada", Value = "2" });
            ViewBag.dateOfServiceId = items;

            return View("/Views/Home/Offers/create.cshtml");
        }

        public ActionResult Contactando(Models.contactanos model)
        {
            EnviarCorreos correoCreacion = new EnviarCorreos();
            string bodyCorreo = correoCreacion.ArmarCorreoElectronico(model.nombreContacto, 2, model.celularContacto, model.comentariosContacto);
            //correoCreacion.EnviarCorreo("bcpruebagen1@gmail.com", "Usuario BidCargo", "cma010360@gmail.com,jose.escobar@metnet.co", bodyCorreo, "contacto@bidcargo.com.co", "contacto@bidcargo.com.co", "Cid2306*","");
            correoCreacion.EnviarCorreo(model.correoContacto, "Usuario BidCargo - "+ model.correoContacto, "cma010360@gmail.com, jose.escobar@metnet.co", bodyCorreo, "bidCargo@hotmail.com", "bidCargo@hotmail.com", "bidC#123", "");
            
            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            data.guardarPrimerContacto(model.correoContacto, model.celularContacto, model.correoContacto,model.razonSocial, model.comentariosContacto);

            Session["message"] = "Su mensaje fue enviado exitosamente.";
            return RedirectToAction("Index");
        }

        public ActionResult ActualizarContrasenaView(Models.actualizarContrasena model)
        {
            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            CredencialesDeAcceso acceso = new CredencialesDeAcceso();
            RijndaelManaged myRijndael = new RijndaelManaged();
            myRijndael.GenerateKey();
            myRijndael.GenerateIV();
            var file = model.numeroCelular;
            if (model.contrasenaActual == model.confirmarContrasena)
            {
                var fileName = Path.GetFileName(file);
                var extention = Path.GetExtension(file);
                var filenamewithoutextention = Path.GetFileNameWithoutExtension(file);

                Byte[] contrasenaEncriptada = acceso.EncryptStringToBytes(model.contrasenaNueva, myRijndael.Key, myRijndael.IV);
                DataTable dtd = data.validacionContrasenaActual(model.contrasenaActual);
                DataRow rowd = dtd.Rows[0];
                byte[] contrasena = (byte[])rowd["contrasena"];
                byte[] key = (byte[])rowd["key"];
                byte[] iv = (byte[])rowd["iv"];
                string contrasenaFinal = acceso.DecryptStringFromBytes(contrasena, key, iv);
                if (contrasenaFinal == model.contrasenaActual)
                {
                    Byte[] contrasenaEncriptadaAntigua = acceso.EncryptStringToBytes(model.contrasenaActual, myRijndael.Key, myRijndael.IV);
                    DataTable dt = data.actualizarContrasenaConfirmacion(fileName, contrasenaEncriptada, myRijndael.Key, myRijndael.IV, contrasenaEncriptadaAntigua, myRijndael.Key, myRijndael.IV);
                    DataRow row = dt.Rows[0];
                    if (dt.Rows.Count == 1)
                    {
                        Session["idCliente"] = row["idCliente"].ToString();
                        Session["idPerfil"] = row["idPerfil"].ToString();
                        Session["nombre"] = row["Nombre"].ToString();
                        if (Convert.ToInt32(row["idPerfil"].ToString()) == 1)
                        {

                            Response.Redirect("/Presentacion/AlquilaEscritorio.aspx");
                        }
                        else
                        {
                            Response.Redirect("/Presentacion/AlquilaEscritorio.aspx");
                        }
                    }
                }
            }
            return View();
        }
        public ActionResult Completar(Models.completarRegistro model)
        {
            try { 
               ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
                var nomaas = Request.Files;
                string[] docs = Request["fileDoc[]"].Split(',');
                if(Request["prueba[]"] != null)
                {
                    model.idTypeCompany = Request["prueba[]"];
                }
                //model.idDepartamento = Request["idDepartamento"];
                //model.idCiudad = Request["idCiudad"];
                for (var i = 0; i < nomaas.Count; i++)
                {
                    if (nomaas[i].ContentLength > 0)
                    {
                        string FileName = Path.GetFileNameWithoutExtension(nomaas[i].FileName).Replace("+", "");
                        string FileExtension = Path.GetExtension(nomaas[i].FileName);
                        FileName = DateTime.Now.ToString("yyyyMMdd") + "_"+ RandomString(5) + FileName.Trim() + FileExtension;
                        //string srcFile = Path.Combine(System.Web.HttpRuntime.AppDomainAppPath, "/Content/uploads/" + FileName);
                        string srcFile = "/Content/uploads/" + FileName;
                        //nomaas[i].SaveAs(srcFile);
                        nomaas[i].SaveAs(System.Web.HttpContext.Current.Server.MapPath("/Content/uploads/") + FileName);
                        data.saveFile(FileName, srcFile, FileExtension, Convert.ToInt32(Session["idCliente"]), 0, Convert.ToInt32(docs[i]),"",0);
                    }
                }
                DataTable dt = data.UpdateRegistryClient(model, Convert.ToInt32(Session["idCliente"]));
                if (dt.Rows.Count > 0)
                {
                    if(Request["profile"] != null)
                    {
                        Session["message"] = "Registro Actualizado con exito";
                        Session["type"] = "success";
                        return RedirectToAction("verPerfil");
                    }
                    else
                    {
                        Session["good"] = 1;
                        Session.Remove("idCliente");
                        Session.Remove("nombre");
                        Session.Remove("email");
                        Session.Remove("idPerfil");
                        Session["message"] = "Registro Actualizado. Su perfil entrara en periodo de verificacion por parte de los administradores, cuando finalice, sera notificado via email...";
                        Session["type"] = "success";
                        return RedirectToAction("Index");
                    }

                    //if (Convert.ToInt32(Session["idTypeCompany"]) == 1)
                    //    return RedirectToAction("createOffer");
                    //else
                    //    return RedirectToAction("offerShow");
                }
                else
                {
                    if (Request["profile"] != null)
                    {
                        Session["message"] = "Problemas Actualizando el registro...";
                        Session["type"] = "error";
                        return RedirectToAction("verPerfil");
                    }
                    else
                    {
                        Session["message"] = "Problemas Actualizando el registro...";
                        Session["type"] = "error";
                        return RedirectToAction("completarRegistro");
                    }
                }
            }
            catch (Exception e)
            {
                string messanje = e.Message;
                Session["message"] = e.Message;
                Session["type"] = "error";
                return RedirectToAction("completarRegistro");
            }
        }

        public ActionResult SaveRecord(Models.preregistroInicial model)
        {
            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            string fileName = "";
            string fechaNacimiento = "1920-01-01";
            CredencialesDeAcceso acceso = new CredencialesDeAcceso();
            string usuario = acceso.creacionUsuario(model.nombreContacto, model.apellido).ToLower();
            string contrasena = model.telefono;
            RijndaelManaged myRijndael = new RijndaelManaged();
            myRijndael.GenerateKey();
            myRijndael.GenerateIV();

            Byte[] contrasenaEncriptada = acceso.EncryptStringToBytes(contrasena, myRijndael.Key, myRijndael.IV);
            DataTable dt = data.CrearCliente(model.nombreContacto, model.apellido,"", 
                1,  0, 2, 0, "0", 1, 1, model.celularContacto.Trim(), "Realiza una donacion", usuario.Trim(),
                contrasenaEncriptada, myRijndael.Key,myRijndael.IV, 
                model.correoContacto.Trim(), fechaNacimiento, model.razonSocial,
                "", "", fileName, 0, 0, 3, 0, "", 
                model.telefono, "", model.nit, 0, DateTime.Now.ToString(), DateTime.Now.ToString(), model.idTypeCompany);

            DataRow row = dt.Rows[0];

            if (dt.Rows.Count == 1)
            {
                if (row["respuesta"].ToString() == "0")
                {
                    RedirectToAction("registroInicial");
                    Session["error"] = "error";
                    Session["mensaje"] = "El usuario con el Email " + model.correoContacto + " ya se encuentra registrado en el sistema";
                }
                else
                {
                    Session["idCliente"] = row["idCliente"].ToString();
                    Session["idTypeCompany"] = row["idTypeCompany"].ToString();
                    EnviarCorreos correoCreacion = new EnviarCorreos();
                    string bodyCorreo = correoCreacion.ArmarCorreoElectronicoPrimerContacto(model.nombreContacto, "", contrasena, usuario, model.celularContacto, Session["urlHttp"].ToString());

                    correoCreacion.EnviarCorreo(model.correoContacto, "PreRegistro BidCargo - " + model.correoContacto, "cma010360@gmail.com, bidCargo@hotmail.com", bodyCorreo, "bidCargo@hotmail.com", "bidCargo@hotmail.com", "bidC#123", "");
                    Session["mensaje"] = "El usuario " + model.nombreContacto + " ha sido registrado de forma exitosa, por favor validar su correo electrónico";
                    RedirectToAction("Index");
                 
                }
            }


            return RedirectToAction("registroInicial");
        }

        public ActionResult StoreOffer(Models.storeoffer model)
        {
            try
            {
                ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
                int VidCliente = Convert.ToInt32(Session["idCliente"]);
                DataTable dt = data.StoreOffer(model, VidCliente);

                if (Convert.ToInt32(model.typeCargo) == 3 || Convert.ToInt32(model.typeCargo) == 4)
                {
                    var nomaas = Request.Files;
                    string FileExtension = "";
                    string newsrcFile = string.Empty;
                    string newnombre = string.Empty;
                    List<string> listsrcFile = new List<string>();
                    List<string> listnombre = new List<string>();
                    for (var i = 0; i < nomaas.Count; i++)
                    {
                        if (nomaas[i].ContentLength > 0)
                        {
                            string FileName = Path.GetFileNameWithoutExtension(nomaas[i].FileName).Replace("+", "");
                            FileExtension = Path.GetExtension(nomaas[i].FileName);
                            FileName = DateTime.Now.ToString("yyyyMMdd") + "_" + FileName.Trim() + FileExtension;
                            string srcFile = "/Content/uploads/" + FileName;
                            nomaas[i].SaveAs(System.Web.HttpContext.Current.Server.MapPath("/Content/uploads/") + FileName);
                            listsrcFile.Add(srcFile);
                            listnombre.Add(FileName);
                            newsrcFile = string.Join("@#", listsrcFile.ToArray());
                            newnombre = string.Join("@#", listnombre.ToArray());
                        }
                    }

                     data.saveFile(newnombre, newsrcFile, FileExtension, VidCliente, 1, 0, dt.Rows[0]["codeOffer"].ToString(), 0);
                }
                model.idTipoTransporte = Request["prueba[]"];
                if (model.typeMerchandise == null && (Request["morecontainer"] == "1"))
                {
                    Session["message"] = "Su Oferta se ha publicado satisfactoriamente...";
                    return RedirectToAction("VerifyInfo");
                }
                Session["model"] = model;
                Session["message"] = "Solicitud ha sido generada exitosamente";
                Session["codeOffer"] = dt.Rows[0]["codeOffer"].ToString();
                if(Request["morecontainer"] == "1")
                {
                    Session["moreContainer"] = 1;

                    return RedirectToAction("createOffer");
                }
                return RedirectToAction("VerifyInfo");
            }
            catch (Exception e)
            {
                string messanje = e.Message;
                return RedirectToAction("createOffer");
            }
        }
        public ActionResult VerifyInfo()
        {
            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            int VidCliente = Convert.ToInt32(Session["idCliente"]);
            string vemail = Session["email"].ToString();
            string vhttp = Session["urlHttp"].ToString();
            string vcodeOffer = Session["codeOffer"].ToString();
            DataTable dd = data.consultarCliente(VidCliente);
            if (dd.Rows.Count > 0){ViewBag.data = dd.Rows[0];}
            else{return RedirectToAction("Index");}

            ViewBag.models = data.getOffer(vcodeOffer, 4).Rows;

            EnviarCorreos correoCreacion = new EnviarCorreos();
            string bodyCorreo = correoCreacion.sendOfferMail(ViewBag.models, ViewBag.data["usuarioFaceBook"], vhttp);
            //DataTable dtd = data.obtenerCorreos(int idTipoClasificacion);
            //correoCreacion.EnviarCorreo("bcpruebagen1@gmail.com", "Oferta de Carga", "cma010360@gmail.com", bodyCorreo, "contacto@bidcargo.com.co", "contacto@bidcargo.com.co", "Cid2306*", "");

            correoCreacion.EnviarCorreo(vemail, "Oferta de Carga - " + vemail, "cma010360@gmail.com", bodyCorreo, "bidCargo@hotmail.com", "bidCargo@hotmail.com", "bidC#123", "");

            //ViewBag.myDocuments = data.getMyDocuments(VidCliente,0).Rows;

            foreach (dynamic row in ViewBag.models)
            {
                DataTable kl = data.getUsersTypeCompanyByOffers(row["idClientOffer"]);
                if(kl.Rows.Count > 0) { 
                    foreach(dynamic ks in kl.Rows) {
                        dynamic sa = data.getTypeCompanyByClient(Convert.ToInt32(ks["idCliente"])).Rows;
                        foreach(dynamic oo in sa)
                        {
                            string fffff = correoCreacion.sendOfferMailTypeCompany(Convert.ToInt32(oo["idClient"]),ViewBag.models, ViewBag.data["usuarioFaceBook"],Convert.ToInt32(oo["idTypeCompany"]), vhttp);
                            //correoCreacion.EnviarCorreo("bcpruebagen1@gmail.com", "Oferta de Carga", "cma010360@gmail.com", fffff, "contacto@bidcargo.com.co", "contacto@bidcargo.com.co", "Cid2306*", "");
                            correoCreacion.EnviarCorreo(ks["email"].ToString(), "Oferta de Carga - "+ ks["email"].ToString(), "cma010360@gmail.com", fffff, "bidCargo@hotmail.com", "bidCargo@hotmail.com", "bidC#123", "");
                        }
                    }
                }
            }
            ViewBag.gogo = 1;

            Session["mensaje"] = "Su registro ha sido realizado, por favor validar su correo";
            Session.Remove("moreContainer");
            Session.Remove("codeOffer");
            return View("/Views/Home/Offers/show.cshtml");
        }



        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public string datatabletojson(DataTable table)
        {
            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(table);
            return JSONString;
        }
        public JsonResult searchtypecontaiener(int id = 0, string parameter ="")
        {
            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            DataTable dt = data.getTypeContainer(id, parameter);
            ViewBag.result = datatabletojson(dt);
            return Json(ViewBag.result, JsonRequestBehavior.AllowGet);
        }


        public JsonResult searchcotizar(int id = 0, string parameter = "")
        {
            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            DataTable dt = data.getCotizacion(id, parameter);
            ViewBag.result = datatabletojson(dt);
            return Json(ViewBag.result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult searchTypeMeaused(int id = 0, int ptype = 0, string parameter = "")
        {
            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            DataTable dt = data.getTypeMeasured(id, ptype, parameter);
            ViewBag.result = datatabletojson(dt);
            return Json(ViewBag.result, JsonRequestBehavior.AllowGet);
        }
        //prueba
        public JsonResult acceptContraOffer(string id = "")
        {
            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            string[] ksa = id.Split('-');
            DataTable dt = data.acceptContraOffer(Convert.ToInt32(ksa[0]), Convert.ToInt32(ksa[1]));// Cliente que fue seleccionado por su oferta, 
            DataTable dd = data.consultarCliente(Convert.ToInt32(Session["idCliente"])); // Cliente que monto la oferta
            DataTable dtt = data.NoAcceptContraOffer(dt.Rows[0]["codeOffer"].ToString()); // cantidad ofertas.

            //pendiente traer los datos de las contraofertas que no fueron seleccionadas.

            ViewBag.result = datatabletojson(dt);
            ViewBag.NoAcceptContraOffer = datatabletojson(dtt);

            EnviarCorreos correoCreacion = new EnviarCorreos();
            if (Convert.ToInt32(ksa[1]) == 0)
            {
              
                string bodyCorreo = correoCreacion.sendMailAccepOffer(dt.Rows[0], dd.Rows[0], Session["urlHttp"].ToString());
                //DataTable dtd = data.obtenerCorreos(int idTipoClasificacion);
                //correoCreacion.EnviarCorreo("bcpruebagen1@gmail.com", "Aceptacion de Oferta", "cma010360@gmail.com", bodyCorreo, "contacto@bidcargo.com.co", "contacto@bidcargo.com.co", "Cid2306*", "");
                correoCreacion.EnviarCorreo(dt.Rows[0]["email"].ToString(), "Aceptacion de Oferta - " + dt.Rows[0]["email"].ToString(), "cma010360@gmail.com", bodyCorreo, "bidCargo@hotmail.com", "bidCargo@hotmail.com", "bidC#123", "");

            }

            //Envio Correo Empresas rechazadas, ::Pendiente validar cuando la empresa es diferente de trasporte terrestre.
            if (dtt.Rows.Count > 0)
            {
                foreach (dynamic row in dtt.Rows)
                {
                    string bodyCorreo = correoCreacion.sendMailNOAccepOffer(row, Session["urlHttp"].ToString());
                    Console.Write(row["email"].ToString());
                    correoCreacion.EnviarCorreo(row["email"].ToString(), "Oferta no Seleccionada - " + row["email"].ToString(), "cma010360@gmail.com", bodyCorreo, "bidCargo@hotmail.com", "bidCargo@hotmail.com", "bidC#123", "");
                }
            }
            

            return Json(ViewBag.result, JsonRequestBehavior.AllowGet);
        }




        public ActionResult myofferShow()
        {
            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            DataTable dd = data.consultarCliente(Convert.ToInt32(Session["idCliente"]));
            if (dd.Rows.Count > 0)
                ViewBag.data = dd.Rows[0];
            else
                return RedirectToAction("Index");

            ViewBag.offers = data.getOffer("",1, Convert.ToInt32(Session["idCliente"])).Rows;

            Session.Remove("moreContainer");
            Session.Remove("codeOffer");
            return View("/Views/Home/Offers/table.cshtml");
        }
        public ActionResult offerShow()
        {
            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            DataTable dd = data.consultarCliente(Convert.ToInt32(Session["idCliente"]));
            if (dd.Rows.Count > 0)
                ViewBag.data = dd.Rows[0];
            else
                return RedirectToAction("Index");

            ViewBag.offers = data.getOffer("", 2, Convert.ToInt32(Session["idCliente"])).Rows;
            if(Convert.ToInt32(Session["idTypeCompany"]) == 7)
            {
                ViewBag.companies = data.getCompaniesAcceptedByCodeOffers().Rows;
            }

            return View("/Views/Home/Offers/table.cshtml");
        }

        public ActionResult factoringShow()
        {
            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            DataTable dd = data.consultarCliente(Convert.ToInt32(Session["idCliente"]));
            if (dd.Rows.Count > 0)
                ViewBag.data = dd.Rows[0];
            else
                return RedirectToAction("Index");

            /*ViewBag.offers = data.getOffer("", 2, Convert.ToInt32(Session["idCliente"]));
            if (ViewBag.offers.Rows.Count > 0)
                ViewBag.offers = ViewBag.offers.Rows;*/


            return View("/Views/Home/showFactoring.cshtml");
        }


        public ActionResult ShowOffer(string id)//este id es el codigo de la oferta
        {
            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            DataTable dd = data.consultarCliente(Convert.ToInt32(Session["idCliente"]));
            if (dd.Rows.Count > 0)
                ViewBag.data = dd.Rows[0];
            else
                return RedirectToAction("Index");

            if (Convert.ToInt32(Session["idTypeCompany"]) == 1)
            {
                dd = data.ShowContraOffers(id);
                ViewBag.dt = dd.Rows;
                if (dd.Rows.Count > 0)
                {
                    foreach (dynamic row in dd.Rows)
                    {
                        ViewBag.myDocumentsContraOfferServiceCarga = data.getMyDocumentsOffer(row["idClient"], id, 1).Rows;
                    }
                }
              
            }
            else
            {
                dd = data.getContracOffers(Convert.ToInt32(Session["idCliente"]), id);
                if (dd.Rows.Count > 0)
                    ViewBag.dt = dd.Rows;
                else
                    ViewBag.dt = null;
            }

            //ViewBag.dt = data.getContracOffers(0, id).Rows;// AQUI ESTA EL ERROR TRAE TODAS LAS OFERTAS
            ViewBag.idClient = Session["idCliente"];
            ViewBag.idTypeCompany = Session["idTypeCompany"];

            ViewBag.models = data.getOffer(id, 4).Rows;
            
            string[] listDocuments = null;
            foreach (dynamic row in ViewBag.models)
            {

                var splitSrcFile = Convert.ToString(row["srcFile"]);
                if (splitSrcFile != "")
                {
                    listDocuments = splitSrcFile.Split(new string[] { "@#" }, StringSplitOptions.None);

                    DataTable dt = new DataTable("myDocumentsOffer");
                    dt.Columns.Add("srcFile");

                    if (listDocuments.Length > 0)
                    {
                        foreach (string value in listDocuments)
                        {
                            dt.Rows.Add(value);
                        }

                        ViewBag.myDocumentsOfferServiceCarga = dt.Rows;
                    }
                }

                else {
                    ViewBag.myDocumentsOfferServiceCarga = null;
                }
            }


            
            DataTable idClientes = data.getContracOffers(Convert.ToInt32(ViewBag.idClient), id);
            ViewBag.myDocuments = data.getMyDocumentsOffer(Convert.ToInt32(ViewBag.idClientOffer), id, 0).Rows; // pendinte pasar id de cliente en oferta
            ViewBag.myDocumentsContraOffer = null;
            if (idClientes.Rows.Count > 0)
            {
                foreach (dynamic row in idClientes.Rows)
                {
                    ViewBag.myDocumentsContraOffer = data.getMyDocumentsOffer(row["idClient"], id, 1).Rows;
                }
            }

            

            return View("/Views/Home/Offers/show.cshtml");
        }

        public ActionResult OfferEdit(string id)//este id es el codigo de la oferta
        {
            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            DataTable dd = data.consultarCliente(Convert.ToInt32(Session["idCliente"]));
            if (dd.Rows.Count > 0)
                ViewBag.data = dd.Rows[0];
            else
                return RedirectToAction("Index");

            if (Convert.ToInt32(Session["idTypeCompany"]) == 1)
            {
                dd = data.ShowContraOffers(id);
                ViewBag.dt = dd.Rows;
            }
            else
            {
                dd = data.getContracOffers(Convert.ToInt32(Session["idCliente"]), id);
                if (dd.Rows.Count > 0)
                    ViewBag.dt = dd.Rows[0];
                else
                    ViewBag.dt = null;
            }

            ViewBag.idClient = Session["idCliente"];
            ViewBag.idTypeCompany = Session["idTypeCompany"];

            ViewBag.models = data.getOffer(id, 4).Rows;


            return View("/Views/Home/Offers/offerEdit.cshtml");
        }




        public JsonResult getContraOfferEdit(int id)//este id es el codigo de la oferta
        {
            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            DataTable dd = data.consultarCliente(Convert.ToInt32(Session["idCliente"]));

            DataTable dt = data.getContraOfferEdit(id);
            ViewBag.result = datatabletojson(dt);
            return Json(ViewBag.result, JsonRequestBehavior.AllowGet);
        }


        public JsonResult UpdateContracOffers(int id)
        {
            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            int valos = 0;
            ViewBag.rows = data.UpdateContracOffers(Convert.ToInt32(Session["idCliente"]), id, Convert.ToInt32(Request["ContraOffer"]), Request["descripcion"].ToString()).Rows;
            if (Convert.ToInt32(ViewBag.rows[0]["idContraOffers"]) > 0)
                valos = 1;
            return Json(new { status = valos }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult OffersToken(string id)
        {
            Session.RemoveAll();
            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            //DataTable dd = data.consultarCliente(Convert.ToInt32(Session["idCliente"]));
            //if (dd.Rows.Count > 0)
            //    ViewBag.data = dd.Rows[0];
            //else
            //    return RedirectToAction("Index");

            string[] asd = id.Split('k');

            ViewBag.models = data.getOffer(asd[3], 4).Rows;
            ViewBag.idClient = asd[1];
            ViewBag.idTypeCompany = asd[5];

            return View("/Views/Home/Offers/procedure.cshtml");
        }



        public ActionResult storeContraOffers()
        {
            try
            {
                ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
                string descis = "";
                if (Request["descripcion"] != null)
                    descis = Request["descripcion"].ToString();

                var nomaas = Request.Files;
                for (var i = 0; i < nomaas.Count; i++)
                {
                    if (nomaas[i].ContentLength > 0)
                    {
                        string FileName = Path.GetFileNameWithoutExtension(nomaas[i].FileName).Replace("+", "");
                        string FileExtension = Path.GetExtension(nomaas[i].FileName);
                        FileName = DateTime.Now.ToString("yyyyMMdd") + "_" + FileName.Trim() + FileExtension;
                        string srcFile = "/Content/uploads/" + FileName;
                        nomaas[i].SaveAs(System.Web.HttpContext.Current.Server.MapPath("/Content/uploads/") + FileName);
                        data.saveFile(FileName, srcFile, FileExtension, Convert.ToInt32(Request["idClient"]), 1, nomaas.Count, Request["codeOffer"].ToString(),1);
                    }
                }

                int idclientess = Convert.ToInt32(Request["idClient"]);
                Console.WriteLine(idclientess);
                DataTable dd = data.storeContraOffer(Convert.ToInt32(Request["idClient"]), Request["codeOffer"].ToString(), Convert.ToDecimal(Request["contraOffer"]), Convert.ToInt32(Request["idTypeCompany"]), descis);

                if (Convert.ToInt32(dd.Rows[0]["respuesta"]) > 0)
                {
                    Session["message"] = "Oferta Guardada Exitosamente.";
                    Session["type"] = "success";

                    EnviarCorreos correoCreacion = new EnviarCorreos();
                    string bodyCorreo = correoCreacion.sendMailContraOffer(Request["codeOffer"].ToString(), Session["urlHttp"].ToString());
                    //DataTable dtd = data.obtenerCorreos(int idTipoClasificacion);
                    //correoCreacion.EnviarCorreo("bcpruebagen1@gmail.com", "Oferta de Servicios", "cma010360@gmail.com", bodyCorreo, "contacto@bidcargo.com.co", "contacto@bidcargo.com.co", "Cid2306*", "");
                    correoCreacion.EnviarCorreo(dd.Rows[0]["email"].ToString(), "Oferta de Servicios - " + dd.Rows[0]["email"].ToString(), "cma010360@gmail.com", bodyCorreo, "bidCargo@hotmail.com", "bidCargo@hotmail.com", "bidC#123", "");

                }
                else
                {
                    Session["message"] = "No se pudo guardar su oferta, intentelo mas tarde...";
                    Session["type"] = "error";
                }
                if (Request["show"] != null)
                    return RedirectToAction("ShowOffer", new { @id = Request["codeOffer"].ToString() });
                else
                    return RedirectToAction("Index");
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }



        public ActionResult GuardarPerfil(Models.completarRegistro model)
        {
            int i = 0;
            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            try
            {
                string js = Request["passwordnew"];
                string ks = Request["passwordnewconfirm"];
                if (js.Length > 0)
                {
                    if (!js.Equals(ks))
                        return RedirectToAction("verPerfil");
                }
                if (js.Length > 0)
                {
                    CredencialesDeAcceso acceso = new CredencialesDeAcceso();
                    RijndaelManaged myRijndael = new RijndaelManaged();
                    myRijndael.GenerateKey();
                    myRijndael.GenerateIV();

                    Byte[] contrasenaEncriptada = acceso.EncryptStringToBytes(js, myRijndael.Key, myRijndael.IV);
                    //DataTable dt = data.CreateOrUpdateProfile(Convert.ToInt32(Session["idCliente"]), contrasenaEncriptada, myRijndael.Key, myRijndael.IV);
                }
                else
                {
                    //model.idTypeCompany = Request["prueba[]"];
                    DataTable dt = data.UpdateRegistryClient(model, Convert.ToInt32(Session["idCliente"]));
                    Session["message"] = "Registro Actualizado con Exito...";
                    return RedirectToAction("createOffer");
                }
                Session["message"] = "Your Profile Has been Updated....";
                Session["title"] = "Very Well...";
                Session["type"] = "success";
            }
            catch (Exception ex)
            {
                Response.Write("Error al Cargar la imagen Principal. " + ex.Message);
            }
            return RedirectToAction("verPerfil");
        }
        public JsonResult verifyCelular(string id)
        {
            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            DataTable dd = data.verifyCelular(id);
            ViewBag.result = datatabletojson(dd);

            return Json(ViewBag.result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult verifyFijo(string id)
        {
            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            DataTable dd = data.verifyFijo(id);
            ViewBag.result = datatabletojson(dd);

            return Json(ViewBag.result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult exportToExcel(int id)
        {

            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            var fileName = "Excel_" + DateTime.Now.ToString("yyyyMMddHHmm") + ".xls";
            try
            {
                DataTable dt = data.GetViewExportToExcel(id);
                Helpers.ExportToExcel(dt, null);

            }
            catch (Exception)
            {

                throw;
            }

            return new JsonResult()
            {
                Data = new { FileName = fileName }
            };

        }

    }
}