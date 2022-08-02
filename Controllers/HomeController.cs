using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Data;
using System.Security.Cryptography;
using System.IO;
using Newtonsoft.Json;
using System.Linq;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using BidCargo_.Models;
using System.Reflection;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using System.Net;
using Microsoft.Owin.Security;
using System.Web;

namespace BidCargo_.Controllers
{
    public class HomeController : Controller
    {
        EnviarCorreos correoCreacion = new EnviarCorreos();
        private static Random random = new Random();
        public ActionResult Login(Models.Login model)
        {
            Session["idCliente"] = null;
            Session["idTypeCompany"] = null;
            Session["nombre"] = null;
            Session["estado"] = null;
            AuthenticationManager.SignOut();

            if (model.usuario != null)
            {
                ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
                CredencialesDeAcceso acceso = new CredencialesDeAcceso();

                DataTable dataTable = data.ValidarUsuarioTabla(model.usuario);
                DataRow dataRow = dataTable.Rows[0];
                string respuesta = dataRow["Respuesta"].ToString();

                if (respuesta == "1") //Tabla usuario
                {
                    DataTable dataTablePYC = data.ValidarIngresoPYC(model.usuario, GetMACAddress().ToString());
                    DataRow dataRowPYC = dataTablePYC.Rows[0];
                    byte[] contrasena = (byte[])dataRowPYC["contraceña"];
                    byte[] key = (byte[])dataRowPYC["KEY"];
                    byte[] iv = (byte[])dataRowPYC["IV"];
                    string telefonoMovil = dataRowPYC["telefonoMovil"].ToString();

                    string contrasenaPYC = acceso.DecryptStringFromBytes(contrasena, key, iv);

                    if (model.contrasena != contrasenaPYC)
                    {

                        Session["message"] = "Por favor validar el usuario y/o password que se encuentra digitando...";
                        Session["type"] = "error";
                        return View();

                    } else if (model.contrasena == telefonoMovil)
                    {
                        Session.Remove("principal");
                        Session.Remove("factoring");
                        Session.Remove("oferta");
                        Session.Remove("idCliente");
                        Session.Remove("nombre");
                        Session.Remove("email");
                        Session.Remove("idPerfil");
                        Session.Remove("idTypeCompany");
                        Session.Remove("IdUsuario");
                        Session.Remove("estado");
                        Session.Remove("rol");
                        Session["message"] = "Por favor, cambie su contraseña...";
                        Session["type"] = "info";
                    }
                    else if (model.contrasena == contrasenaPYC) //igual a la contraseña desencriptada
                    {
                        //ValidarEstadoUsuario(dataRowPYC);
                        int estado = Convert.ToInt32(dataRowPYC["idEstadoCliente"]);
                        int rol = Convert.ToInt32(dataRowPYC["rol"]);
                        int idCiente = Convert.ToInt32(dataRowPYC["IdUsuario"]);
                        if (estado == 0)
                        {
                            Session["estado"] = 0;

                            if (rol == 4) // rol del conductor
                            {
                                Session["IdUsuario"] = idCiente;
                                Session["role"] = "4";

                                authSingInUsers(idCiente.ToString(), "4");

                                return Redirect("~/Conductor/CargueDocumento");
                            }
                            else if (rol == 2) // rol Propietario
                            {
                                Session["IdUsuario"] = idCiente;
                                Session["role"] = "2";
                                authSingInUsers(idCiente.ToString(), "2");

                                return Redirect("~/Propietario/CargueDocumento");
                            }

                        }
                        else if (estado == 1)//en revision
                        {
                            Session.Remove("principal");
                            Session.Remove("factoring");
                            Session.Remove("oferta");
                            Session.Remove("idCliente");
                            Session.Remove("nombre");
                            Session.Remove("email");
                            Session.Remove("idPerfil");
                            Session.Remove("idTypeCompany");
                            Session.Remove("IdUsuario");
                            Session.Remove("estado");
                            Session.Remove("rol");
                            Session["message"] = "Su perfil se encuentra en revision, contacte con el administrador, disculpe las molestias...";
                            Session["type"] = "info";
                            return View();
                        }
                        else if (estado == 2)//aprobado
                        {

                            Session["IdUsuario"] = idCiente;
                            Session["idPerfil"] = rol;

                            if (rol == 4) // rol del conductor
                            {

                                authSingInUsers(idCiente.ToString(), "4");

                                return Redirect("~/Conductor/MenuConductor");
                            }
                            else if (rol == 2) // rol Propietario
                            {
                                authSingInUsers(idCiente.ToString(), "2");

                                return Redirect("~/Propietario/MenuPropietario");
                            }

                        }
                        if (estado == -1)//perfil no aprobado
                        {
                            Session.Remove("principal");
                            Session.Remove("factoring");
                            Session.Remove("oferta");
                            Session.Remove("idCliente");
                            Session.Remove("nombre");
                            Session.Remove("email");
                            Session.Remove("idPerfil");
                            Session.Remove("idTypeCompany");
                            Session.Remove("IdUsuario");
                            Session.Remove("estado");
                            Session.Remove("rol");
                            Session["message"] = "Su perfil NO ESTA APROBADO, contacte con el administrador, disculpe las molestias...";
                            Session["type"] = "info";
                            return View();
                        }
                        if (estado == -2)//usuario desactivado
                        {
                            Session.Remove("principal");
                            Session.Remove("factoring");
                            Session.Remove("oferta");
                            Session.Remove("idCliente");
                            Session.Remove("nombre");
                            Session.Remove("email");
                            Session.Remove("idPerfil");
                            Session.Remove("idTypeCompany");
                            Session.Remove("IdUsuario");
                            Session.Remove("estado");
                            Session.Remove("rol");
                            Session["message"] = "Su perfil se encuentra DESACTIVADO, contacte con el administrador, disculpe las molestias...";
                            Session["type"] = "info";
                            return View();
                        }
                        return View();
                    }

                }
                else //tabla cliente
                {
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
                            if (Convert.ToInt32(row["idCalificacionCliente"]) == 1)
                            {
                                return RedirectToAction("actualizarContrasena", new { @id = row["numeroCelular"] });
                            }
                            DataTable fe = data.getTypeCompanyByClient(Convert.ToInt32(row["idCliente"]));
                            bool entre = false;
                            foreach (dynamic ws in fe.Rows)
                            {
                                if (Convert.ToInt32(ws["idTypeCompany"]) == 1)
                                {
                                    entre = true;
                                    Session["principal"] = true;
                                    if ((Convert.ToInt32(ws["principal"])) == 1)
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
                                if (Convert.ToInt32(ws["idTypeCompany"]) == 6 || Convert.ToInt32(ws["idTypeCompany"]) == 7)
                                {
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
            }
            return View();
        }

        //private void ValidarEstadoUsuario(DataRow dataRow)
        //{
        //    int idEstadoCliente = Convert.ToInt32(dataRow["idEstadoCliente"]);
        //    int rol = Convert.ToInt32(dataRow["rol"]);

        //    EstadoCliente(idEstadoCliente, rol);

        //    //return View();
        //}

        //private ActionResult EstadoCliente(int estado, int rol)
        //{
        //    if (estado == 0) 
        //    {
        //        if (rol == 4) // rol del conductor
        //        {
        //            return RedirectToAction("/Propietario/CargeDocumento");
        //        }
        //        else if (rol == 2) // rol Propietario
        //        {
        //            return RedirectToAction("/Propietario/CargeDocumento");
        //        }
        //    }
        //    else if(estado == 1)//en revision
        //    {
        //        Session.Remove("principal");
        //        Session.Remove("factoring");
        //        Session.Remove("oferta");
        //        Session.Remove("idCliente");
        //        Session.Remove("nombre");
        //        Session.Remove("email");
        //        Session.Remove("idPerfil");
        //        Session.Remove("idTypeCompany");
        //        Session["message"] = "Su perfil se encuentra en revision, contacte con el administrador, disculpe las molestias...";
        //        Session["type"] = "info";
        //        return View();
        //    }
        //    if (estado == -1)//perfil no aprobado
        //    {
        //        Session.Remove("principal");
        //        Session.Remove("factoring");
        //        Session.Remove("oferta");
        //        Session.Remove("idCliente");
        //        Session.Remove("nombre");
        //        Session.Remove("email");
        //        Session.Remove("idPerfil");
        //        Session.Remove("idTypeCompany");
        //        Session["message"] = "Su perfil NO ESTA APROBADO, contacte con el administrador, disculpe las molestias...";
        //        Session["type"] = "info";
        //        return View();
        //    }
        //    if (estado == -2)//usuario desactivado
        //    {
        //        Session.Remove("principal");
        //        Session.Remove("factoring");
        //        Session.Remove("oferta");
        //        Session.Remove("idCliente");
        //        Session.Remove("nombre");
        //        Session.Remove("email");
        //        Session.Remove("idPerfil");
        //        Session.Remove("idTypeCompany");
        //        Session["message"] = "Su perfil se encuentra DESACTIVADO, contacte con el administrador, disculpe las molestias...";
        //        Session["type"] = "info";
        //        return View();
        //    }
        //    return View();
        //}



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
            correoCreacion.EnviarCorreo(dd.Rows[0]["email"].ToString(), texto + " - " + dd.Rows[0]["email"].ToString(), "cma010360@gmail.com", bodyCorreo, "bidCargo@hotmail.com", "bidCargo@hotmail.com", "bidC#123", "");
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
            DataTable dd = data.SaveUsuarioSistema(0, id, 3, "", "", "", "", 0, "", Convert.ToDateTime(null), 0, 0);
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
            correoCreacion.EnviarCorreo(dd.Rows[0]["email"].ToString(), texto + " - " + dd.Rows[0]["email"].ToString(), "cma010360@gmail.com", bodyCorreo, "bidCargo@hotmail.com", "bidCargo@hotmail.com", "bidC#123", "");
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
            foreach (var row in ViewBag.profileTypeCompany)
            {
                if (Convert.ToInt32(row["principal"]) == 1)
                    vals = Convert.ToInt32(row["idTypeCompany"]);
            }
            ViewBag.documents = data.getDocuments(vals).Rows;
            ViewBag.myDocuments = data.getMyDocuments(id).Rows;
            ViewBag.idClientePerfil = Convert.ToInt32(Session["idCliente"]);

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

            ViewBag.offers = data.getOffer("", 3).Rows;


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
            return RedirectToAction("AdminShowOffer", new { @id = id });
        }

        public JsonResult SaveQualification(int id)
        {
            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            int valos = 0;
            ViewBag.rows = data.storeQualificationContraOffer(Convert.ToInt32(Session["idCliente"]), id, Convert.ToInt32(Request["qual"]), Request["comments"].ToString()).Rows;
            if (Convert.ToInt32(ViewBag.rows[0]["respuesta"]) > 0)
                valos = 1;
            return Json(new { status = valos }, JsonRequestBehavior.AllowGet);
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
            DataTable dt = data.getCitybyDepatamento("SP_getCityByDepartament", idDepartamentos);
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
                ViewBag.cargoContacto = "";
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
            DataTable dtUsuario = data.BuscarCorreoUsuario(model.email.Trim());
            DataRow rowUsuario = dtUsuario.Rows[0];

            DataTable dt = null;

            if (rowUsuario["respuesta"].ToString() == "1")
            {
                dt = data.actualizarContrasenaUsuarioCambio(model.email.Trim(), contrasenaEncriptada, myRijndael.Key, myRijndael.IV);
            }
            else
            {
                dt = data.actualizarContrasena(model.email.Trim(), contrasenaEncriptada, myRijndael.Key, myRijndael.IV);
            }

            if (dt.Rows.Count == 1)
            {
                DataRow row = dt.Rows[0];
                string[] nombreTelefono = respuesta.Split('-');
                EnviarCorreos correoCreacion = new EnviarCorreos();

                string bodyCorreo = "";

                if (rowUsuario["respuesta"].ToString() == "1")
                {
                    bodyCorreo = correoCreacion.ArmarCorreoRecuperacionContrasena(row["Nombre"].ToString(), contrasena, 0, row["numeroCelular"].ToString(), Session["urlHttp"].ToString(), true);
                }
                else
                {
                    bodyCorreo = correoCreacion.ArmarCorreoRecuperacionContrasena(row["Nombre"].ToString(), contrasena, 0, row["numeroCelular"].ToString(), Session["urlHttp"].ToString(), false);
                }

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
            AuthenticationManager.SignOut();
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
            DataTable dsd = data.getUsuarioSistema(Convert.ToInt32(Session["idCliente"]), id);
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
            model.CompanyId = Convert.ToInt32(Session["idCliente"]);

            string contrasenaEncriptada = acceso.EncryptStringToBytes(contrasena, myRijndael.Key, myRijndael.IV).ToString();


            DataTable dt = data.SaveUsuarioSistema(0, 0, 1, model.Nombre, model.Apellido, model.Celular, contrasenaEncriptada, 1, model.Correo, DateTime.Now, model.CompanyId, 1);
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
                    if (dt.Rows.Count > 0)
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
                    ViewBag.profileTypeCompany = dt.Rows;

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

            if (Session["moreContainer"] == null)
                Session["moreContainer"] = 0;
            else if (Convert.ToInt32(Session["moreContainer"]) == 1)
            {
                ViewBag.allModels = data.getOffer(Session["codeOffer"].ToString(), -1).Rows;
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

            if (!string.IsNullOrEmpty(Convert.ToString(Session["codeOffer"])))
            {
                var dtPersona = data.GetPersonaByRol(2);
                var dtEmpresasTransporte = data.GetPersonaByRol(0);

                string vhttp = Session["urlHttp"].ToString();
                ViewBag.models = data.getOffer(Session["codeOffer"].ToString(), 4).Rows;
                string textoCorreo = correoCreacion.sendOfferMail(ViewBag.models, ViewBag.data["usuarioFaceBook"], vhttp);

                if (dtPersona.Rows.Count > 0)
                {
                    for (int i = 0; i < dtPersona.Rows.Count; i++)
                    {
                        string correo = dtPersona.Rows[i]["correo"].ToString();
                        correoCreacion.EnviarCorreo(correo, "Oferta de Carga - " + correo, "cma010360@gmail.com", textoCorreo, "bidCargo@hotmail.com", "bidCargo@hotmail.com", "bidC#123", "");

                    }
                }


                if (dtEmpresasTransporte.Rows.Count > 0)
                {
                    for (int i = 0; i < dtEmpresasTransporte.Rows.Count; i++)
                    {
                        string correo = dtEmpresasTransporte.Rows[i]["email"].ToString();
                        correoCreacion.EnviarCorreo(correo, "Oferta de Carga - " + correo, "cma010360@gmail.com", textoCorreo, "bidCargo@hotmail.com", "bidCargo@hotmail.com", "bidC#123", "");

                    }
                }
            }

           

            return View("/Views/Home/Offers/create.cshtml");
        }

        public ActionResult Contactando(Models.contactanos model)
        {
            EnviarCorreos correoCreacion = new EnviarCorreos();
            string bodyCorreo = correoCreacion.ArmarCorreoElectronico(model.nombreContacto, 2, model.celularContacto, model.comentariosContacto);
            //correoCreacion.EnviarCorreo("bcpruebagen1@gmail.com", "Usuario BidCargo", "cma010360@gmail.com,jose.escobar@metnet.co", bodyCorreo, "contacto@bidcargo.com.co", "contacto@bidcargo.com.co", "Cid2306*","");
            correoCreacion.EnviarCorreo(model.correoContacto, "Usuario BidCargo - " + model.correoContacto, "cma010360@gmail.com,jsorozcof@gmail.com", bodyCorreo, "bidCargo@hotmail.com", "bidCargo@hotmail.com", "bidC#123", "");

            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            data.guardarPrimerContacto(model.correoContacto, model.celularContacto, model.correoContacto, model.razonSocial, model.comentariosContacto);

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
                if (Request["prueba[]"] != null)
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
                        FileName = DateTime.Now.ToString("yyyyMMdd") + "_" + RandomString(5) + FileName.Trim() + FileExtension;
                        //string srcFile = Path.Combine(System.Web.HttpRuntime.AppDomainAppPath, "/Content/uploads/" + FileName);
                        string srcFile = "/Content/uploads/" + FileName;
                        //nomaas[i].SaveAs(srcFile);
                        nomaas[i].SaveAs(System.Web.HttpContext.Current.Server.MapPath("/Content/uploads/") + FileName);
                        data.saveFile(FileName, srcFile, FileExtension, Convert.ToInt32(Session["idCliente"]), 0, Convert.ToInt32(docs[i]), "", 0);
                    }
                }

                string bodyCorreo = $"Administrativo bidcargo una nueva entidad ha realizado el cargue de los documentos de la entidad {model.razonSocial}  en su plataforma ";
                correoCreacion.EnviarCorreo("cma010360@gmail.com", "Revision de documentos. ", "cma010360@gmail.com", bodyCorreo, "Bidcargo@hotmail.com", "Bidcargo@hotmail.com", "bidC#123", "");
                correoCreacion.EnviarCorreo("Bidcargo@hotmail.com", "Revision de documentos. ", "Bidcargo@hotmail.com", bodyCorreo, "Bidcargo@hotmail.com", "Bidcargo@hotmail.com", "bidC#123", "");

                DataTable dt = data.UpdateRegistryClient(model, Convert.ToInt32(Session["idCliente"]));


                if (dt.Rows.Count > 0)
                {
                    if (Request["profile"] != null)

                    {
                        string bodyCorreo2 = $"Administrativo bidcargo una nueva entidad ha realizado el cargue de los documentos de la entidad {model.razonSocial}  en su plataforma ";
                        correoCreacion.EnviarCorreo("cma010360@gmail.com", "Revision de documentos. ", "cma010360@gmail.com", bodyCorreo2, "Bidcargo@hotmail.com", "Bidcargo@hotmail.com", "bidC#123", "");


                        Session["message"] = "Registro Actualizado con exito";
                        Session["type"] = "success";
                        return RedirectToAction("verPerfil");
                    }
                    else
                    {
                        string bodyCorreo3 = $"Administrativo bidcargo una nueva entidad ha realizado el cargue de los documentos de la entidad {model.razonSocial}  en su plataforma ";
                        correoCreacion.EnviarCorreo("cma010360@gmail.com", "Revision de documentos. ", "cma010360@gmail.com", bodyCorreo3, "Bidcargo@hotmail.com", "Bidcargo@hotmail.com", "bidC#123", "");

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
            DataTable dt = data.CrearCliente(model.nombreContacto, model.apellido, "",
                1, 0, 2, 0, "0", 1, 1, model.celularContacto.Trim(), "Realiza una donacion", usuario.Trim(),
                contrasenaEncriptada, myRijndael.Key, myRijndael.IV,
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
                    string bodyCorreo2 = $"Administrativo bidcargo una nueva entidad ha realizado el pre-registro inicial a la plataforma, la entidad es:  {model.razonSocial} ";
                    correoCreacion.EnviarCorreo("cma010360@gmail.com", "Registro inicial de entidad. ", "cma010360@gmail.com", bodyCorreo2, "Bidcargo@hotmail.com", "Bidcargo@hotmail.com", "bidC#123", "");


                    string bodyCorreo = correoCreacion.ArmarCorreoElectronicoPrimerContacto(model.nombreContacto, "", contrasena, usuario, model.celularContacto, Session["urlHttp"].ToString() + "/Home/actualizarContrasena");

                    correoCreacion.EnviarCorreo(model.correoContacto, "PreRegistro BidCargo - " + model.correoContacto, "Bidcargo@hotmail.com", bodyCorreo, "Bidcargo@hotmail.com", "Bidcargo@hotmail.com", "bidC#123", "");
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
                EnviarCorreos correoCreacion = new EnviarCorreos();


                string bodyCorreo = correoCreacion.ArmarCorreoNuevaOferta(model);
                DataTable dta = data.ConsultarCorreoPropietarios();
                if (dta.Rows.Count > 0)
                {
                    correoCreacion.EnviarCorreoPropietarios(dta, "Nueva oferta BidCargo", "Bidcargo@hotmail.com", bodyCorreo, "Bidcargo@hotmail.com", "Bidcargo@hotmail.com", "bidC#123", "");
                }
               
                if (Request["morecontainer"] == "1")
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
            if (dd.Rows.Count > 0) { ViewBag.data = dd.Rows[0]; }
            else { return RedirectToAction("Index"); }

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
                if (kl.Rows.Count > 0) {
                    foreach (dynamic ks in kl.Rows) {
                        dynamic sa = data.getTypeCompanyByClient(Convert.ToInt32(ks["idCliente"])).Rows;
                        foreach (dynamic oo in sa)
                        {
                            string fffff = correoCreacion.sendOfferMailTypeCompany(Convert.ToInt32(oo["idClient"]), ViewBag.models, ViewBag.data["usuarioFaceBook"], Convert.ToInt32(oo["idTypeCompany"]), vhttp);
                            //correoCreacion.EnviarCorreo("bcpruebagen1@gmail.com", "Oferta de Carga", "cma010360@gmail.com", fffff, "contacto@bidcargo.com.co", "contacto@bidcargo.com.co", "Cid2306*", "");
                            correoCreacion.EnviarCorreo(ks["email"].ToString(), "Oferta de Carga - " + ks["email"].ToString(), "cma010360@gmail.com", fffff, "bidCargo@hotmail.com", "bidCargo@hotmail.com", "bidC#123", "");
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
        public JsonResult searchtypecontaiener(int id = 0, string parameter = "")
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

            ViewBag.offers = data.getOffer("", 1, Convert.ToInt32(Session["idCliente"])).Rows;

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

            var fullOffer = data.getOffer("", 2, Convert.ToInt32(Session["idCliente"]));
            
            //Consultar si Session["idCliente"] es una empresa de transporte.
            if (Convert.ToInt32(Session["idTypeCompany"]) == 8)
            {
                DataTable dtTypeCompany = data.getTypeCompanyByClient(0);
                DataView dv = new DataView(dtTypeCompany);
                dv.RowFilter = "idTypeCompany = 8";

                if (fullOffer.Rows.Count > 0)
                {
                    for (var i = 0; i < dv.Count; i++)
                    {
                        int idClientCompany = (int)dv[i]["idClient"];
                        List<DataRow> rowsToDelete = new List<DataRow>();

                        foreach (DataRow row in fullOffer.Rows)
                        {
                            if (row["idClient"].ToString().Contains(Convert.ToString(idClientCompany)))
                            {
                                rowsToDelete.Add(row);
                            }
                        }

                        foreach (DataRow row in rowsToDelete)
                        {
                            fullOffer.Rows.Remove(row);
                        }

                    }

                    fullOffer.AcceptChanges();
                    ViewBag.offers = fullOffer.Rows;
                }
             }
            else
            {
                ViewBag.offers = data.getOffer("", 2, Convert.ToInt32(Session["idCliente"])).Rows;
            }



            if (Convert.ToInt32(Session["idTypeCompany"]) == 7)
            {
                ViewBag.companies = data.getCompaniesAcceptedByCodeOffers().Rows;
            }

            return View("/Views/Home/Offers/table.cshtml");
        }

        public List<T> ConvertToList<T>(DataTable dt)
        {
            var columnNames = dt.Columns.Cast<DataColumn>()
                .Select(c => c.ColumnName)
                .ToList();
            var properties = typeof(T).GetProperties();
            return dt.AsEnumerable().Select(row =>
            {
                var objT = Activator.CreateInstance<T>();
                foreach (var pro in properties)
                {
                    if (columnNames.Contains(pro.Name))
                        pro.SetValue(objT, row[pro.Name]);
                }
                return objT;
            }).ToList();
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
                        data.saveFile(FileName, srcFile, FileExtension, Convert.ToInt32(Request["idClient"]), 1, nomaas.Count, Request["codeOffer"].ToString(), 1);
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


        private static List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }
        private static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    //    if(r["idPropiedad"] != DBNull.Value)
                    if (pro.Name == column.ColumnName)
                        pro.SetValue(obj, dr[column.ColumnName], null);


                    else
                        continue;
                }
            }
            return obj;
        }


        public ActionResult Download()
        {
            var fileName = "Excel_BidCargo" + DateTime.Now.ToString("yyyyMMddHHmm") + ".xlsx";
            if (Session["DownloadExcel_FileManager"] != null)
            {

                byte[] data = Session["DownloadExcel_FileManager"] as byte[];
                return File(data, "application/octet-stream", fileName);
            }
            else
            {
                return new EmptyResult();
            }
        }

        [HttpGet]
        public ActionResult exportToExcel(int id)
        {

            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            DataTable dt = data.GetViewExportToExcel(id);
            DataTable Dt = new DataTable();
            try
            {


                if (id == 1)
                {
                    List<ExportOfferModels> exportOfferList = new List<ExportOfferModels>();
                    exportOfferList = ConvertDataTable<ExportOfferModels>(dt);


                    Dt.Columns.Add("ID", typeof(int));
                    Dt.Columns.Add("CODIGO", typeof(string));
                    Dt.Columns.Add("EMPRESA", typeof(string));
                    Dt.Columns.Add("VALOR_OFERTA", typeof(float));
                    Dt.Columns.Add("TRAYECTO", typeof(string));
                    Dt.Columns.Add("FACTORING", typeof(string));
                    Dt.Columns.Add("FECHA_OFERTA", typeof(string));
                    Dt.Columns.Add("ESTADO", typeof(string));

                    foreach (var d in exportOfferList)
                    {
                        DataRow row = Dt.NewRow();
                        row[0] = d.ID;
                        row[1] = d.CODIGO;
                        row[2] = d.EMPRESA;
                        row[3] = d.VALOR_OFERTA;
                        row[4] = d.TRAYECTO;
                        row[5] = d.FACTORING;
                        row[6] = d.FECHA_OFERTA;
                        row[7] = d.ESTADO;
                        Dt.Rows.Add(row);

                    }
                }

                else if (id == 3)
                {

                    DataTable dt2 = data.ReportePersonaNaturales();
                    DataTable Dt2 = new DataTable();
                    List<PropNaturalInputModel> exportOfferList = new List<PropNaturalInputModel>();
                    exportOfferList = ConvertDataTable<PropNaturalInputModel>(dt2);
                    Dt2.Columns.Add("fk_usuario", typeof(int));
                    Dt2.Columns.Add("nombres", typeof(string));
                    Dt2.Columns.Add("apellido", typeof(string));
                    Dt2.Columns.Add("direccion", typeof(string));
                    Dt2.Columns.Add("telefonoFijo", typeof(string));

                    foreach (var d in exportOfferList)
                    {
                        DataRow row = Dt.NewRow();
                        row[0] = d.fk_usuario;
                        row[1] = d.nombres;
                        row[2] = d.apellidos;
                        row[3] = d.direccion;
                        row[4] = d.telefonoFijo;
                        Dt.Rows.Add(row);
                    }

                }
                else
                {

                    List<ExportCompanyModels> exportCompanyList = new List<ExportCompanyModels>();
                    exportCompanyList = ConvertDataTable<ExportCompanyModels>(dt);


                    Dt.Columns.Add("ID", typeof(int));
                    Dt.Columns.Add("RAZON_SOCIAL", typeof(string));
                    Dt.Columns.Add("NIT", typeof(string));
                    Dt.Columns.Add("CONTACTO", typeof(string));
                    Dt.Columns.Add("TIPO_EMPRESA_PRINCIPAL", typeof(string));
                    Dt.Columns.Add("ESTADO", typeof(string));


                    foreach (var d in exportCompanyList)
                    {
                        DataRow row = Dt.NewRow();
                        row[0] = d.ID;
                        row[1] = d.RAZON_SOCIAL;
                        row[2] = d.NIT;
                        row[3] = d.CONTACTO;
                        row[4] = d.TIPO_EMPRESA_PRINCIPAL;
                        row[5] = d.ESTADO;

                        Dt.Rows.Add(row);

                    }
                }


                var memoryStream = new MemoryStream();
                using (var excelPackage = new ExcelPackage(memoryStream))
                {
                    var worksheet = excelPackage.Workbook.Worksheets.Add("Sheet1");
                    worksheet.Cells["A1"].LoadFromDataTable(Dt, true, TableStyles.None);
                    worksheet.Cells["A1:AN1"].Style.Font.Bold = true;
                    worksheet.DefaultRowHeight = 18;


                    worksheet.Column(2).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    worksheet.Column(6).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.Column(7).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.DefaultColWidth = 20;
                    worksheet.Column(2).AutoFit();

                    Session["DownloadExcel_FileManager"] = excelPackage.GetAsByteArray();
                    return Json("", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public ActionResult TableVehiculos()
        {
            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            DataTable dd = data.ReporteVehiculo();
            if (dd.Rows.Count > 0)
                ViewBag.data = dd.Rows[0];
            else
                dd = data.ReporteVehiculo();
            ViewBag.rows = dd.Rows;
            return View();
        }

        public ActionResult TablePnaturales()
        {
            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            DataTable dd = data.ReportePersonaNaturales();
            if (dd.Rows.Count > 0)
                ViewBag.data = dd.Rows[0];
            else
                dd = data.ReportePersonaNaturales();
            ViewBag.rows = dd.Rows;
            return View();
        }

        public ActionResult TablePjuridicas()
        {
            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            DataTable dd = data.ReportePersonaJuridica();
            if (dd.Rows.Count > 0)
                ViewBag.data = dd.Rows[0];
            else
                dd = data.ReportePersonaJuridica();
            ViewBag.rows = dd.Rows;
            return View();
        }

        public ActionResult TableConductor()
        {
            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            DataTable dd = data.ReporteConductor();
            if (dd.Rows.Count > 0)
                ViewBag.data = dd.Rows[0];
            else
                dd = data.ReporteConductor();
            ViewBag.rows = dd.Rows;
            return View();
        }
      

        [HttpGet]
        public ActionResult ExportExelVehiculo()
        {

            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            DataTable dt = data.ReporteVehiculo();
            DataTable Dt = new DataTable();
            try
            {
                List<VehiculoViewModels> exportOfferList = new List<VehiculoViewModels>();
                exportOfferList = ConvertDataTable<VehiculoViewModels>(dt);
                Dt.Columns.Add("Placa", typeof(string));
                Dt.Columns.Add("TipoVehivulo", typeof(string));
                Dt.Columns.Add("Marca", typeof(string));
                Dt.Columns.Add("Año", typeof(string));
                Dt.Columns.Add("Estado", typeof(string));
                Dt.Columns.Add("Operador", typeof(string));
                Dt.Columns.Add("Usuario", typeof(string));
                Dt.Columns.Add("Contraseña", typeof(string));

                foreach (var d in exportOfferList)
                {
                    DataRow row = Dt.NewRow();
                    row[0] = d.Placa;
                    row[1] = d.TipoVehiculo;
                    row[2] = d.Marca;
                    row[3] = d.Linea;

                    if (d.Estado == "0")
                    {
                        d.Estado = "Revision";
                        row[4] = d.Estado;
                    }
                    else if (d.Estado == "-2")
                    {
                        d.Estado = "Desactivado";
                        row[4] = d.Estado;
                    }
                    else if (d.Estado == "-1")
                    {
                        d.Estado = "No aprovado";
                        row[4] = d.Estado;
                    }
                    else
                    {
                        d.Estado = "Aprovado";
                        row[4] = d.Estado;
                    }
                    row[5] = d.Operador;
                    row[6] = d.Usuario;
                    row[7] = d.Contraseña;
                    Dt.Rows.Add(row);
                }
                var memoryStream = new MemoryStream();
                using (var excelPackage = new ExcelPackage(memoryStream))
                {
                    var worksheet = excelPackage.Workbook.Worksheets.Add("Sheet1");
                    worksheet.Cells["A1"].LoadFromDataTable(Dt, true, TableStyles.None);
                    worksheet.Cells["A1:AN1"].Style.Font.Bold = true;
                    worksheet.DefaultRowHeight = 18;


                    worksheet.Column(2).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    worksheet.Column(6).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.Column(7).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.DefaultColWidth = 20;
                    worksheet.Column(2).AutoFit();

                    Session["DownloadExcel_FileManager"] = excelPackage.GetAsByteArray();
                    return Json("", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public ActionResult actualizarContrasenaUsuarios(string id)
        {
            Session.RemoveAll();
            Session["customerId"] = id;
            return View("~/Views/Home/actualizarContrasenaUsuarios.cshtml");
        }

        public ActionResult ActualizarContrasenaNuevaUsuario(Models.actualizarPassword model)
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
                DataTable dtd = data.ObtenerContraseña(sub);
                DataRow rowd = dtd.Rows[0];
                byte[] contraseña = (byte[])rowd["contraseña"];
                byte[] key = (byte[])rowd["KEY"];
                byte[] iv = (byte[])rowd["IV"];
                string contrasenaFinal = acceso.DecryptStringFromBytes(contraseña, key, iv);
                if (contrasenaFinal != model.newpassword)
                {
                    Byte[] contrasenaEncriptadaAntigua = acceso.EncryptStringToBytes(model.actual, myRijndael.Key, myRijndael.IV);
                    DataTable dt = data.actualizarContrasenaUsuario(sub, contrasenaEncriptada, myRijndael.Key, myRijndael.IV, contrasenaEncriptadaAntigua, myRijndael.Key, myRijndael.IV);
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
            return RedirectToAction("ActualizarContrasenaUsuarios", new { @id = Session["customerId"] });
        }



        public ActionResult EstadoAprobado(int id, string correo, string nombre)
        {
            string estado = "1";
            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            DataTable deta = data.Estadovehiculo(id, estado);

            string bodyCorreo = correoCreacion.ArmarCorreoVehiculoAprobado(nombre, "", 1);
            correoCreacion.EnviarCorreo(correo, "Cambio de estado " + correo, "Bidcargo@hotmail.com", bodyCorreo, "Bidcargo@hotmail.com", "Bidcargo@hotmail.com", "bidC#123", "");

            Session["message"] = "Modificacion Realizada con éxito.";

            return RedirectToAction("TableVehiculos");
        }

        public ActionResult EstadoDesAprobado(int id, string correo, string nombre)
        {
            string estado = "-1";
            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            DataTable deta = data.Estadovehiculo(id, estado);

            string bodyCorreo = correoCreacion.ArmarCorreoVehiculoAprobado(nombre, "", 0);
            correoCreacion.EnviarCorreo(correo, "Cambio de estado " + correo, "Bidcargo@hotmail.com", bodyCorreo, "Bidcargo@hotmail.com", "Bidcargo@hotmail.com", "bidC#123", "");

            Session["message"] = "Modificacion Realizada con éxito.";

            return RedirectToAction("TableVehiculos");
        }

        public ActionResult EstadoDesActivado(int id, string correo, string nombre)
        {
            string estado = "-2";
            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            DataTable deta = data.Estadovehiculo(id, estado);
            string bodyCorreo = correoCreacion.ArmarCorreoVehiculoActivado(nombre, "", 0);
            correoCreacion.EnviarCorreo(correo, "Cambio de estado " + correo, "Bidcargo@hotmail.com", bodyCorreo, "Bidcargo@hotmail.com", "Bidcargo@hotmail.com", "bidC#123", "");

            Session["message"] = "Modificacion Realizada con éxito.";
            return RedirectToAction("TableVehiculos");
        }

        public ActionResult ElimarVehiculo(int id)
        {
            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            DataTable deta = data.EliminarVehiculo(id);
            Session["message"] = "Modificacion Realizada con éxito.";
            return RedirectToAction("TableVehiculos");

        }

        public ActionResult ContraOfertaTransportador()
        {

            return View();
        }


        private void authSingInUsers(string idUsuario, string role)
        {
            ClaimsIdentity identity = new ClaimsIdentity(DefaultAuthenticationTypes.ApplicationCookie);

            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, idUsuario));

            identity.AddClaim(new Claim(ClaimTypes.Role, role));

            identity.AddClaim(new Claim(ClaimTypes.Name, idUsuario));

            AuthenticationManager.SignIn(identity);
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        public ActionResult ContraOfertaPropietarioPempresas()
        {
            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            DataTable dd = data.ListaContraOfertaPropietario3(Convert.ToInt32(Session["idCliente"]));
            
            if (dd.Rows.Count > 0)
                ViewBag.data = dd.Rows[0];
            else
                dd = data.ListaContraOfertaPropietario3(Convert.ToInt32(Session["idCliente"]));
            ViewBag.rows = dd.Rows;
            return View();
        }

        public ActionResult ViewContraOfertaPropietario(string Sk_ContraOferta)
        {
            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            DataTable dataTable = data.ListaContraOffShearch(Sk_ContraOferta);

            if (dataTable.Rows.Count > 0)
            {
                DataRow dataRow = dataTable.Rows[0];
                int fk_usuario = int.Parse(dataRow["fk_usuario"].ToString());

                DataTable dataUsuario = data.ConsultarUsuarioPropietarioPerfil(fk_usuario);
                ViewBag.tipoUsuario = dataUsuario.Rows[0];
                ViewBag.OfferPropietario = dataTable.Rows[0];
                string id = ViewBag.OfferPropietario["codeOffer"].ToString();
                ViewBag.models = data.getOffer(id, 4).Rows;
            }
            else
            {
                return RedirectToAction("ContraOfertaPropietario");
            }

            return View();
        }
        public ActionResult ViewContraOfertaPropietarioAdmin(string Sk_ContraOferta)
        {
            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            DataTable dataTable = data.ListaContraOffShearch(Sk_ContraOferta);

            if (dataTable.Rows.Count > 0)
            {
                DataRow dataRow = dataTable.Rows[0];
                int fk_usuario = int.Parse(dataRow["fk_usuario"].ToString());

                DataTable dataUsuario = data.ConsultarUsuarioPropietarioPerfil(fk_usuario);
                ViewBag.tipoUsuario = dataUsuario.Rows[0];
                ViewBag.OfferPropietario = dataTable.Rows[0];
                string id = ViewBag.OfferPropietario["codeOffer"].ToString();
                ViewBag.models = data.getOffer(id, 4).Rows;
            }
            else
            {
                return RedirectToAction("ContraOfertaPropietario");
            }

            return View();
        }






        public ActionResult EstadoAprobadoUsuario(int id, int estado, string correo, string nombre)
        {
            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            DataTable deta = data.CambioEstadoPropietario(id, estado);
            string ruta = "";
            int estadostring;
            if (estado == 2)
            {
                estadostring = 2;
                string bodyCorreo2 = $"Administrativo bidcargo un Propietario {nombre} fue aceptado  en su plataforma ";
                correoCreacion.EnviarCorreo("cma010360@gmail.com", "Cambio de Estado Propietario. ", "cma010360@gmail.com", bodyCorreo2, "Bidcargo@hotmail.com", "Bidcargo@hotmail.com", "bidC#123", "");
                string bodyCorreo = correoCreacion.ArmarCorreoPropietarioAprobado(nombre, ruta, estadostring);
                correoCreacion.EnviarCorreo(correo, "Cambio de estado " + correo, "Bidcargo@hotmail.com", bodyCorreo, "Bidcargo@hotmail.com", "Bidcargo@hotmail.com", "bidC#123", "");

            }
            if (estado == -1)
            {
                estadostring = 0;
                string bodyCorreo2 = $"Administrativo bidcargo un Propietario {nombre} No fue aceptado  en su plataforma ";
                correoCreacion.EnviarCorreo("cma010360@gmail.com", "Cambio de Estado Propietario. ", "cma010360@gmail.com", bodyCorreo2, "Bidcargo@hotmail.com", "Bidcargo@hotmail.com", "bidC#123", "");
                string bodyCorreo = correoCreacion.ArmarCorreoPropietarioAprobado(nombre, ruta, estadostring);
                correoCreacion.EnviarCorreo(correo, "Cambio de estado " + correo, "Bidcargo@hotmail.com", bodyCorreo, "Bidcargo@hotmail.com", "Bidcargo@hotmail.com", "bidC#123", "");

            }
            if (estado == -2)
            {
                estadostring = 0;
                string bodyCorreo2 = $"Administrativo bidcargo un Propietario {nombre} fue Desactivado  en su plataforma ";
                correoCreacion.EnviarCorreo("cma010360@gmail.com", "Cambio de Estado Propietario. ", "cma010360@gmail.com", bodyCorreo2, "Bidcargo@hotmail.com", "Bidcargo@hotmail.com", "bidC#123", "");
                string bodyCorreo = correoCreacion.ArmarCorreoPropietarioActivado(nombre, ruta, estadostring);
                correoCreacion.EnviarCorreo(correo, "Cambio de estado " + correo, "Bidcargo@hotmail.com", bodyCorreo, "Bidcargo@hotmail.com", "Bidcargo@hotmail.com", "bidC#123", "");

            }

            Session["message"] = "Modificacion Realizada con éxito.";
            return RedirectToAction("TablePjuridicas");


        }
        public ActionResult EstadoAprobadoUsuario2(int id, int estado, string correo, string nombre)
        {

            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            DataTable deta = data.CambioEstadoPropietario(id, estado);
            string ruta = "";
            int estadostring;
            if (estado == 2)
            {
                estadostring = 2;
                string bodyCorreo2 = $"Administrativo bidcargo un Propietario {nombre} fue aceptado  en su plataforma ";
                correoCreacion.EnviarCorreo("cma010360@gmail.com", "Cambio de Estado Propietario. ", "cma010360@gmail.com", bodyCorreo2, "Bidcargo@hotmail.com", "Bidcargo@hotmail.com", "bidC#123", "");
                string bodyCorreo = correoCreacion.ArmarCorreoPropietarioAprobado(nombre, ruta, estadostring);
                correoCreacion.EnviarCorreo(correo, "Cambio de estado " + correo, "Bidcargo@hotmail.com", bodyCorreo, "Bidcargo@hotmail.com", "Bidcargo@hotmail.com", "bidC#123", "");

            }
            if (estado == -1)
            {
                estadostring = 0;
                string bodyCorreo2 = $"Administrativo bidcargo un Propietario {nombre} No fue aceptado  en su plataforma ";
                correoCreacion.EnviarCorreo("cma010360@gmail.com", "Cambio de Estado Propietario. ", "cma010360@gmail.com", bodyCorreo2, "Bidcargo@hotmail.com", "Bidcargo@hotmail.com", "bidC#123", "");
                string bodyCorreo = correoCreacion.ArmarCorreoPropietarioAprobado(nombre, ruta, estadostring);
                correoCreacion.EnviarCorreo(correo, "Cambio de estado " + correo, "Bidcargo@hotmail.com", bodyCorreo, "Bidcargo@hotmail.com", "Bidcargo@hotmail.com", "bidC#123", "");

            }
            if (estado == -2)
            {
                estadostring = 0;
                string bodyCorreo2 = $"Administrativo bidcargo un Propietario {nombre} fue Desactivado  en su plataforma ";
                correoCreacion.EnviarCorreo("cma010360@gmail.com", "Cambio de Estado Propietario. ", "cma010360@gmail.com", bodyCorreo2, "Bidcargo@hotmail.com", "Bidcargo@hotmail.com", "bidC#123", "");
                string bodyCorreo = correoCreacion.ArmarCorreoPropietarioActivado(nombre, ruta, estadostring);
                correoCreacion.EnviarCorreo(correo, "Cambio de estado " + correo, "Bidcargo@hotmail.com", bodyCorreo, "Bidcargo@hotmail.com", "Bidcargo@hotmail.com", "bidC#123", "");

            }


            Session["message"] = "Modificacion Realizada con éxito.";

            return RedirectToAction("TablePnaturales");
        }
        public ActionResult EstadoAprobadoUsuario3(int id, int estado, string correo, string nombre)
        {

            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            DataTable deta = data.CambioEstadoConductor(id, estado);
            string ruta = "";
            int estadostring;

            if (estado == 2)
            {
                estadostring = 2;
                string bodyCorreo = correoCreacion.ArmarCorreoConductorAprobado(nombre, ruta, estadostring);
                correoCreacion.EnviarCorreo(correo, "Cambio de estado " + correo, "Bidcargo@hotmail.com", bodyCorreo, "Bidcargo@hotmail.com", "Bidcargo@hotmail.com", "bidC#123", "");

            }
            if (estado == -1)
            {
                estadostring = 0;
                string bodyCorreo = correoCreacion.ArmarCorreoConductorAprobado(nombre, ruta, estadostring);
                correoCreacion.EnviarCorreo(correo, "Cambio de estado " + correo, "Bidcargo@hotmail.com", bodyCorreo, "Bidcargo@hotmail.com", "Bidcargo@hotmail.com", "bidC#123", "");

            }
            if (estado == -2)
            {
                estadostring = 0;
                string bodyCorreo = correoCreacion.ArmarCorreoConductorActivado(nombre, ruta, estadostring);
                correoCreacion.EnviarCorreo(correo, "Cambio de estado " + correo, "Bidcargo@hotmail.com", bodyCorreo, "Bidcargo@hotmail.com", "Bidcargo@hotmail.com", "bidC#123", "");

            }




            Session["message"] = "Modificacion Realizada con éxito.";

            return RedirectToAction("TableConductor");
        }

        public ActionResult ConsultarInformacionConducor(int id, int tipo)
        {

            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            DataTable deta = data.ConsultarInformacionAdmin(id);
            DataRow dataRow = deta.Rows[0];
            CondutorvisualizacionComplera conductorView = new CondutorvisualizacionComplera();

            int idpropietario = Convert.ToInt32(dataRow["tipoDocumento"].ToString());
            ViewBag.p = data.ConsultarInformacionAdminpn(idpropietario).Rows;


            conductorView.tipoDocumento = dataRow["tipoDocumento"].ToString();
            if (dataRow["tipoDocumento"].ToString() == "1")
            {
                conductorView.tipoDocumento = "Cédula de ciudadanía";
            }
            else if (dataRow["tipoDocumento"].ToString() == "2")
            {
                conductorView.tipoDocumento = "Cédula extranjera";
            }
            else
            {
                conductorView.tipoDocumento = "Pasaporte";
            }
            conductorView.numeroLicencia = dataRow["nrolicencia"].ToString();
            conductorView.nombre = dataRow["nombre"].ToString();
            conductorView.apellido = dataRow["apellido"].ToString();
            conductorView.telefonoFijo = dataRow["telefonofijo"].ToString();
            conductorView.direccion = dataRow["direccion"].ToString();
            conductorView.codigoCiudad = dataRow["ciudad"].ToString();
            conductorView.codigoDepartamento = dataRow["departamentos"].ToString();
            conductorView.numeroDocumento = dataRow["numerodocumento"].ToString();

            conductorView.pathCedula = dataRow["cedulaciudadania"].ToString();
            conductorView.pathLicencia = dataRow["licenciacoducion"].ToString();
            conductorView.pathSeguridadSoc = dataRow["planillapagos"].ToString();
            conductorView.pathCursos = dataRow["cursos"].ToString();

            ViewBag.conductorView = conductorView;

            DataTable dt = data.ConsultarUsuarioPropietario(Convert.ToInt32(dataRow["idPropietario"].ToString()));
            DataRow dataRow2 = dt.Rows[0];
            PropietarioViewModel propietarioView = new PropietarioViewModel();
            //propietarioView.tipoUsuario = Convert.ToInt32(dataRow["tipoUsuario"]);
            propietarioView.nombres = dataRow2["nombres"].ToString();
            propietarioView.apellidos = dataRow2["apellidos"].ToString();
            propietarioView.telefono = dataRow2["telefono"].ToString();
            propietarioView.correo = dataRow2["correo"].ToString();
            propietarioView.cedula = dataRow2["cedula"].ToString();

            ViewBag.propietarioView = propietarioView;

            return View();
        }

        public void mostrarCredenciales(int id, int tipo) {
            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            if (tipo == 1)
            {
                DataTable dd = data.ConsultarInformacionAdminpj(id);
                ViewBag.data = dd.Rows[0];
                string usurario = dd.Rows[0]["nombreUsuario"].ToString();
                Session["message"] = $"El usuario es {usurario}. ";

            }
            else {
                DataTable dd = data.ConsultarInformacionAdminpn(id);
                ViewBag.data = dd.Rows[0];
                string usurario = dd.Rows[0]["nombreUsusario"].ToString();
                Session["message"] = $"El usuario es {usurario}. ";
                Session["type"] = "success";
            }

        }


        public ActionResult ConsultarInformacionPjuridoca(int id)
        {
            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            DataTable dd = data.ConsultarInformacionAdminpj(id);
            if (dd.Rows.Count > 0) {
                ViewBag.data = dd.Rows[0];
                CredencialesDeAcceso acceso = new CredencialesDeAcceso();
                byte[] contrasena = (byte[])ViewBag.data["contraseña"];
                byte[] key = (byte[])ViewBag.data["KEY"];
                byte[] iv = (byte[])ViewBag.data["IV"];
                ViewBag.data["idEstadoCliente"] = acceso.DecryptStringFromBytes(contrasena, key, iv);
            }
            else
                dd = data.ConsultarInformacionAdminpj(id);
            ViewBag.rows = dd.Rows;
            return View();
        }

        public ActionResult ConsultarInformacionPnatural(int id)
        {

            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            DataTable dd = data.ConsultarInformacionAdminpn(id);
            if (dd.Rows.Count > 0) {
                ViewBag.data = dd.Rows[0];
                CredencialesDeAcceso acceso = new CredencialesDeAcceso();
                byte[] contrasena = (byte[])ViewBag.data["contraseña"];
                byte[] key = (byte[])ViewBag.data["KEY"];
                byte[] iv = (byte[])ViewBag.data["IV"];
                ViewBag.data["idEstadoCliente"] = acceso.DecryptStringFromBytes(contrasena, key, iv);
            }
               
            else
                dd = data.ConsultarInformacionAdminpn(id);
            ViewBag.rows = dd.Rows;

            return View();
        }
        public ActionResult AceptarContraofertas(int oferta, int usuario) {

            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            
            DataTable dda = data.AceptarRechazarContraofertas( 1 , 1, oferta, usuario );
            DataTable ddd = data.AceptarRechazarContraofertas(2, 2, oferta, usuario);
            enviarnotificaciones(oferta ,usuario);
            Session["message"] = "Modificacion Realizada con éxito.";
            return RedirectToAction("ContraOfertaPropietarioPempresas");
        }

        public ActionResult AceptarContraofertas2(int oferta, int usuario)
        {

            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();

            DataTable dda = data.AceptarRechazarContraofertas(6, 6, oferta, usuario);
            Session["message"] = "Modificacion Realizada con éxito.";
            return RedirectToAction("ContraOfertaPropietario");
        }

        //Eliminar ContraOfertas Propietario
        public ActionResult DeleteContraOfertaPropietario(int ofertaId)
        {
            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            DataTable dd = data.DeleteContraOfertaPropietario(ofertaId);
            ViewBag.offers = dd.Rows.Count;
            if (ViewBag.models == 0)
            {
                Session.Remove("codeOffer");
                Session.Remove("moreContainer");
                Session.Remove("model");
                Session["message"] = "El Registro fue eliminado con éxito.";
            }

            return RedirectToAction("ContraOfertaPropietario");
        }


        public void enviarnotificaciones(int oferta, int usuario) 
        {
            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            EnviarCorreos correoCreacion = new EnviarCorreos();
            DataTable person = data.PersonasParaNotificar(1, usuario);
            DataTable companies = data.EmpresaAnotificar(oferta);

            string bodyCorreo = correoCreacion.sendMailAccepOffer3(person, companies, "");
            correoCreacion.EnviarCorreo(companies.Rows[0]["email"].ToString(), "Oferta Aceptada", "Bidcargo@hotmail.com", bodyCorreo, "Bidcargo@hotmail.com", "Bidcargo@hotmail.com", "bidC#123", "");
           
            
            DataTable ddoa = data.PersonasParaNotificar(2, usuario);
            foreach (dynamic row in ddoa.Rows)
            {
                string bodyCorreo2 = correoCreacion.sendMailNOAccepOffer3(row, "");
                correoCreacion.EnviarCorreo(row["Correo"].ToString(), "Oferta Rechazada", "Bidcargo@hotmail.com", bodyCorreo, "Bidcargo@hotmail.com", "Bidcargo@hotmail.com", "bidC#123", "");


            }



        }








        public FileResult DescargarArchivos( string ruta ) 
        {
            var nombrearchivo = Path.GetFileName(ruta);
            var archivo = Server.MapPath("~/Content/Files/" + nombrearchivo);
            return File(archivo, "application/pdf", Path.GetFileName(archivo));
        }

        public ActionResult VisualizarVehiculohome(int placa)
        {
            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            DataTable dd = data.visualizarinfovehiculoPropietario(placa);
            if (dd.Rows.Count > 0)
                ViewBag.data = dd.Rows[0];
            else
                dd = data.visualizarinfovehiculoPropietario(placa);
            ViewBag.rows = dd.Rows;
            return View();

        }
        public ActionResult ContraOfertaPropietario()
        {
            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            DataTable dd = data.ListaContraOfertaPropietario();
            if (dd.Rows.Count > 0)
                ViewBag.data = dd.Rows[0];
            else
                dd = data.ListaContraOfertaPropietario();
            ViewBag.rows = dd.Rows;
            return View(); 
        }
       




        [HttpGet]
        public ActionResult ExportexelConductor()
        {

            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            DataTable dt = data.ReporteConductor();
            DataTable Dt = new DataTable();
            try
            {
                List<CondutorvisualizacionComplera> exportOfferList = new List<CondutorvisualizacionComplera>();
                exportOfferList = ConvertDataTable<CondutorvisualizacionComplera>(dt);
                Dt.Columns.Add("nombre", typeof(string));
                Dt.Columns.Add("apellido", typeof(string));
                Dt.Columns.Add("tipoDocumento", typeof(string));
                Dt.Columns.Add("cedula", typeof(string));
                Dt.Columns.Add("direccion", typeof(string));
                Dt.Columns.Add("telefonofijo", typeof(string));
               

                foreach (var d in exportOfferList)
                {
                    DataRow row = Dt.NewRow();
                    row[0] = d.nombre;
                    row[1] = d.apellido;
                    row[2] = d.tipoDocumento;
                    row[3] = d.cedula;
                    row[4] = d.direccion;
                    row[5] = d.telefonoFijo;
                    Dt.Rows.Add(row);
                }
                var memoryStream = new MemoryStream();
                using (var excelPackage = new ExcelPackage(memoryStream))
                {
                    var worksheet = excelPackage.Workbook.Worksheets.Add("Sheet1");
                    worksheet.Cells["A1"].LoadFromDataTable(Dt, true, TableStyles.None);
                    worksheet.Cells["A1:AN1"].Style.Font.Bold = true;
                    worksheet.DefaultRowHeight = 18;


                    worksheet.Column(2).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    worksheet.Column(6).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.Column(7).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.DefaultColWidth = 20;
                    worksheet.Column(2).AutoFit();

                    Session["DownloadExcel_FileManager"] = excelPackage.GetAsByteArray();
                    return Json("", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [HttpGet]
        public ActionResult ExportexelPnatural()
        {
            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            DataTable dt = data.ReportePersonaNaturales();
            DataTable Dt = new DataTable();
            try
            {
                List<PropNaturalInputModel> exportOfferList = new List<PropNaturalInputModel>();
                exportOfferList = ConvertDataTable<PropNaturalInputModel>(dt);
                Dt.Columns.Add("nombre", typeof(string));
                Dt.Columns.Add("apellido", typeof(string));
                Dt.Columns.Add("tipoDocumento", typeof(string));
                Dt.Columns.Add("numeroDocuemto", typeof(string));
                Dt.Columns.Add("direccion", typeof(string));
                Dt.Columns.Add("telefonofijo", typeof(string));


                foreach (var d in exportOfferList)
                {
                    DataRow row = Dt.NewRow();
                    row[0] = d.nombres;
                    row[1] = d.apellidos;
                    row[2] = d.fk_TipoDocumento;
                    row[3] = d.numeroDocumento;
                    row[4] = d.direccion;
                    row[5] = d.telefonoFijo;
                    Dt.Rows.Add(row);
                }
                var memoryStream = new MemoryStream();
                using (var excelPackage = new ExcelPackage(memoryStream))
                {
                    var worksheet = excelPackage.Workbook.Worksheets.Add("Sheet1");
                    worksheet.Cells["A1"].LoadFromDataTable(Dt, true, TableStyles.None);
                    worksheet.Cells["A1:AN1"].Style.Font.Bold = true;
                    worksheet.DefaultRowHeight = 18;


                    worksheet.Column(2).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    worksheet.Column(6).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.Column(7).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.DefaultColWidth = 20;
                    worksheet.Column(2).AutoFit();

                    Session["DownloadExcel_FileManager"] = excelPackage.GetAsByteArray();
                    return Json("", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public ActionResult AceptarContraOferta(int Sk_ContraOferta, int fk_usuario)
        {
            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            DataRow dataRow = data.ConsultarUsuarioPropietario(fk_usuario).Rows[0];
            string correo = dataRow["correo"].ToString();


            DataTable deta = data.CambioEstadoContrOfertPropietario(Sk_ContraOferta, 1);
            string bodyCorreo = "Su contra oferta ha sido aceptada por favor ponerce en contacto con:";
            EnviarCorreos correoCreacion = new EnviarCorreos();
            correoCreacion.EnviarCorreo(correo, "Cambio de estado " + correo, "Bidcargo@hotmail.com", bodyCorreo, "Bidcargo@hotmail.com", "Bidcargo@hotmail.com", "bidC#123", "");

            //Session["message"] = "Modificacion Realizada con éxito.";

            return RedirectToAction("TablePnaturales");
        }



        [HttpGet]
        public ActionResult ExportexelPjuridico()
        {
            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            DataTable dt = data.ReportePersonaNaturales();
            DataTable Dt = new DataTable();
            try
            {
                List<propJuridicoInputModel> exportOfferList = new List<propJuridicoInputModel>();
                exportOfferList = ConvertDataTable<propJuridicoInputModel>(dt);
                Dt.Columns.Add("Rozon_Social", typeof(string));
                Dt.Columns.Add("Nit", typeof(string));
                Dt.Columns.Add("nombreContacto", typeof(string));
                Dt.Columns.Add("apellidoContacto", typeof(string));
                Dt.Columns.Add("direccion", typeof(string));
                Dt.Columns.Add(" telefonoFijo ", typeof(string));


                foreach (var d in exportOfferList)
                {
                    DataRow row = Dt.NewRow();
                    row[0] = d.razonSocial;
                    row[1] = d.nit;
                    row[2] = d.nombreContacto;
                    row[3] = d.apellidoContacto;
                    row[4] = d.direccion;
                    row[5] = d.telefonoFijo;
                    Dt.Rows.Add(row);
                }
                var memoryStream = new MemoryStream();
                using (var excelPackage = new ExcelPackage(memoryStream))
                {
                    var worksheet = excelPackage.Workbook.Worksheets.Add("Sheet1");
                    worksheet.Cells["A1"].LoadFromDataTable(Dt, true, TableStyles.None);
                    worksheet.Cells["A1:AN1"].Style.Font.Bold = true;
                    worksheet.DefaultRowHeight = 18;


                    worksheet.Column(2).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    worksheet.Column(6).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.Column(7).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.DefaultColWidth = 20;
                    worksheet.Column(2).AutoFit();

                    Session["DownloadExcel_FileManager"] = excelPackage.GetAsByteArray();
                    return Json("", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }



      





        public ActionResult TableConductorjuridicos()
        {
            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            DataTable dd = data.ReporteConductor2(0);
            if (dd.Rows.Count > 0)
                ViewBag.data = dd.Rows[0];
            else
                dd = data.ReporteConductor2(0);
            ViewBag.rows = dd.Rows;
            return View();
        }
        public ActionResult TableConductorNaturales()
        {
            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            DataTable dd = data.ReporteConductor2(1);
            if (dd.Rows.Count > 0)
                ViewBag.data = dd.Rows[0];
            else
                dd = data.ReporteConductor2(1);
            ViewBag.rows = dd.Rows;
            return View();
        }


        public ActionResult ConsultarInformacionConducorju(int id)
        {

            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            DataTable deta = data.ConsultarInformacionAdmin(id);
            DataRow dataRow = deta.Rows[0];
            CondutorvisualizacionComplera conductorView = new CondutorvisualizacionComplera();

            int idpropietario = Convert.ToInt32(dataRow["tipoDocumento"].ToString());
            ViewBag.p = data.ConsultarInformacionAdminpn(idpropietario).Rows;


            conductorView.tipoDocumento = dataRow["tipoDocumento"].ToString();
            if (dataRow["tipoDocumento"].ToString() == "1")
            {
                conductorView.tipoDocumento = "Cédula de ciudadanía";
            }
            else if (dataRow["tipoDocumento"].ToString() == "2")
            {
                conductorView.tipoDocumento = "Cédula extranjera";
            }
            else
            {
                conductorView.tipoDocumento = "Pasaporte";
            }
            conductorView.numeroLicencia = dataRow["nrolicencia"].ToString();
            conductorView.nombre = dataRow["nombre"].ToString();
            conductorView.apellido = dataRow["apellido"].ToString();
            conductorView.telefonoFijo = dataRow["telefonofijo"].ToString();
            conductorView.direccion = dataRow["direccion"].ToString();
            conductorView.codigoCiudad = dataRow["ciudad"].ToString();
            conductorView.codigoDepartamento = dataRow["departamentos"].ToString();
            conductorView.numeroDocumento = dataRow["numerodocumento"].ToString();

            conductorView.pathCedula = dataRow["cedulaciudadania"].ToString();
            conductorView.pathLicencia = dataRow["licenciacoducion"].ToString();
            conductorView.pathSeguridadSoc = dataRow["planillapagos"].ToString();
            conductorView.pathCursos = dataRow["cursos"].ToString();

            ViewBag.conductorView = conductorView;


            DataTable dt = data.ConsultarUsuarioPropietario(Convert.ToInt32(dataRow["idPropietario"].ToString()));
            DataRow dataRow2 = dt.Rows[0];
            PropietarioViewModel propietarioView = new PropietarioViewModel();
            //propietarioView.tipoUsuario = Convert.ToInt32(dataRow["tipoUsuario"]);
            propietarioView.nombres = dataRow2["nombres"].ToString();
            propietarioView.apellidos = dataRow2["apellidos"].ToString();
            propietarioView.telefono = dataRow2["telefono"].ToString();
            propietarioView.correo = dataRow2["correo"].ToString();
            propietarioView.cedula = dataRow2["cedula"].ToString();

            ViewBag.propietarioView = propietarioView;


            return View();
        }


























    }
}