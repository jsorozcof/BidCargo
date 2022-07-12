using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BidCargo_.Models;
using System.Security.Cryptography;
using System.IO;

namespace BidCargo_.Controllers
{
    [Authorize(Roles = "2")]
    public class PropietarioController : Controller
    {
        [AllowAnonymous]
        // GET: Propietario
        public ActionResult PreRegistro()
        {
            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            DataTable dt = data.ObtenerData("SP_departamentos");
            ViewBag.ListaDepartamento = ToSelectList(dt, "idDepartamentos", "departamentos");

            dt = data.ObtenerData("SP_ConsultarTipoDocumento");
            ViewBag.ListTipoDocumento = ToSelectList(dt, "sk_tipoDocumento", "nombre");
            ViewBag.ListPropietario = ToListTipoUsuario();

            return View();
        }

        public ActionResult CargueDocumento()
        {
            if (Session["IdUsuario"] == null)
            {
                return Redirect("~/Home/Login");
            }
            ViewBag.propietarioView = ConsultarInformacion();

            return View();
        }

        public PropietarioViewModel ConsultarInformacion()
        {
            int IdUsuario = Convert.ToInt32(Session["IdUsuario"]);

            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            DataTable dt = data.ConsultarUsuarioPropietario(IdUsuario);
            DataRow dataRow = dt.Rows[0];

            PropietarioViewModel propietarioView = new PropietarioViewModel();
            propietarioView.tipoUsuario = Convert.ToInt32(dataRow["tipoUsuario"]);
            propietarioView.nombres = dataRow["nombres"].ToString();
            propietarioView.apellidos = dataRow["apellidos"].ToString();
            propietarioView.telefono = dataRow["telefono"].ToString();
            propietarioView.correo = dataRow["correo"].ToString();
            propietarioView.cedula = dataRow["cedula"].ToString();

            ViewBag.propietarioView = propietarioView;

            @TempData["nombre"] = propietarioView.nombres + " " + propietarioView.apellidos;
            @TempData["correo"] = propietarioView.correo;

            @TempData["IdPropietario"] = dataRow["IdPropietario"];
            @TempData["TipoUsuario"] = dataRow["tipoUsuario"];
            @TempData["IdUsuario"] = Session["IdUsuario"];

            return propietarioView;
        }

        [HttpPost]
        public ActionResult SaveFiles(PropietarioFileInptModel SoporteModel)
        {
            PropietarioPathInptModel PropietarioPath = MapPropietarioFile(SoporteModel);
            int TipoUsuario = Convert.ToInt32(TempData["TipoUsuario"]);
            string respuesta = "";
            EnviarCorreos correoCreacion = new EnviarCorreos();
            PropietarioViewModel propietarioView = new PropietarioViewModel();
            propietarioView = ConsultarInformacion2();

            if (TipoUsuario == 1) //Natural
            {
                respuesta = SaveFilePropNatural(SoporteModel, PropietarioPath);
                if (respuesta  =="1")
                {
                    
                    string bodyCorreoAdmin = correoCreacion.CarguePN(propietarioView);
                    correoCreacion.EnviarCorreo("cma010360@gmail.com", "PreRegistro BidCargo - " + TempData["correo"].ToString(), "cma010360@gmail.com", bodyCorreoAdmin, "Bidcargo@hotmail.com", "Bidcargo@hotmail.com", "bidC#123", "");


                }
            }
            else if (TipoUsuario == 2) //Juridica
            {
                respuesta = SaveFilePropJuridico(SoporteModel, PropietarioPath);

                if (respuesta == "1")
                {
                    string bodyCorreoAdmin = correoCreacion.CarguePN(propietarioView);
                    correoCreacion.EnviarCorreo("cma010360@gmail.com", "PreRegistro BidCargo - " + TempData["correo"].ToString(), "cma010360@gmail.com", bodyCorreoAdmin, "Bidcargo@hotmail.com", "Bidcargo@hotmail.com", "bidC#123", "");

                }
            }

            if (respuesta == "1")
            {
                
                Session["message"] = "La carga de los documentos fue exitosa. Su perfil se encuentra en revision, contacte con el administrador, disculpe las molestias...";
                Session["type"] = "success";

              
                Session.Remove("IdUsuario");
            }
            else
            {
                Session["message"] = "No se cargo información se cargo exitosamente, si tiene inconvenientes contacte con en administrador, disculpe la molestia";
                Session["type"] = "info";

                Session.Remove("IdUsuario");
            }

            return Redirect("~/Home/Login");
        }
        public PropietarioViewModel ConsultarInformacion2()
        {
            int IdUsuario = Convert.ToInt32(Session["IdUsuario"]);

            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            DataTable dt = data.ConsultarUsuarioPropietario(IdUsuario);
            DataRow dataRow = dt.Rows[0];

            PropietarioViewModel propietarioView = new PropietarioViewModel();
            propietarioView.tipoUsuario = Convert.ToInt32(dataRow["tipoUsuario"]);
            propietarioView.nombres = dataRow["nombres"].ToString();
            propietarioView.apellidos = dataRow["apellidos"].ToString();
            propietarioView.telefono = dataRow["telefono"].ToString();
            propietarioView.correo = dataRow["correo"].ToString();
            propietarioView.cedula = dataRow["cedula"].ToString();
            return propietarioView;
        }
        private string SaveFilePropNatural(PropietarioFileInptModel SoporteModel, PropietarioPathInptModel PropietarioPath)
        {
            string Ruta = "/Content/Files/";
            SoporteModel.CCNatural.SaveAs(PropietarioPath.CC);
            SoporteModel.RUTNatural.SaveAs(PropietarioPath.RUT);
            SoporteModel.CBNatural.SaveAs(PropietarioPath.CertificacionBancaria);

            int IdPropietario = Convert.ToInt32(TempData["IdPropietario"]);
            int IdUsuario = Convert.ToInt32(TempData["IdUsuario"]);

            PropietarioPathInptModel propietarioPathInptModel = new PropietarioPathInptModel();
            propietarioPathInptModel.IdPropietario = IdPropietario;
            propietarioPathInptModel.idUsuario = IdUsuario;
            propietarioPathInptModel.CC = Path.Combine(Ruta + Path.GetFileName(PropietarioPath.CC));
            propietarioPathInptModel.RUT = Path.Combine(Ruta + Path.GetFileName(PropietarioPath.RUT));
            propietarioPathInptModel.CertificacionBancaria = Path.Combine(Ruta + Path.GetFileName(PropietarioPath.CertificacionBancaria));

            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            DataTable dt = data.CrearSoportePropietarioNatural(propietarioPathInptModel);

            DataRow row = dt.Rows[0];

            return row["respuesta"].ToString();
        }

        private string SaveFilePropJuridico(PropietarioFileInptModel SoporteModel, PropietarioPathInptModel PropietarioPath)
        {
            string Ruta = "/Content/Files/";
            SoporteModel.CCJuridica.SaveAs(PropietarioPath.CC);
            SoporteModel.NIT.SaveAs(PropietarioPath.NIT);
            SoporteModel.RUTJuridica.SaveAs(PropietarioPath.RUT);
            SoporteModel.CamaraComercio.SaveAs(PropietarioPath.CamaraComercio);
            SoporteModel.CBJuridica.SaveAs(PropietarioPath.CertificacionBancaria);

            int IdPropietario = Convert.ToInt32(TempData["IdPropietario"]);
            int IdUsuario = Convert.ToInt32(TempData["IdUsuario"]);

            PropietarioPathInptModel propietarioPathInptModel = new PropietarioPathInptModel();
            propietarioPathInptModel.IdPropietario = IdPropietario;
            propietarioPathInptModel.idUsuario = IdUsuario;
            propietarioPathInptModel.CC = Path.Combine(Ruta + Path.GetFileName(PropietarioPath.CC));
            propietarioPathInptModel.RUT = Path.Combine(Ruta + Path.GetFileName(PropietarioPath.RUT));
            propietarioPathInptModel.CamaraComercio = Path.Combine(Ruta + Path.GetFileName(PropietarioPath.CamaraComercio));
            propietarioPathInptModel.CertificacionBancaria = Path.Combine(Ruta + Path.GetFileName(PropietarioPath.CertificacionBancaria));
            propietarioPathInptModel.NIT = Path.Combine(Ruta + Path.GetFileName(PropietarioPath.NIT));

            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            DataTable dt = data.CrearSoportePropietarioJudicial(propietarioPathInptModel);

            DataRow row = dt.Rows[0];

            return row["respuesta"].ToString();
        }

        public PropietarioPathInptModel MapPropietarioFile(PropietarioFileInptModel SoporteModel)
        {
            PropietarioPathInptModel sopPropietarioPath = new PropietarioPathInptModel();

            string RutaSitio = Path.Combine(Server.MapPath("~/") + "Content/Files/");
            string Date = DateTime.Now.ToString("dd-MM-yyyy");
            string hour = DateTime.Now.ToString("ss-mm-hh");
            string Ruta = RutaSitio + "_" + Date + "_" + hour;

            int TipoUsuario = Convert.ToInt32(TempData["TipoUsuario"]);

            if (TipoUsuario == 1) //Natural
            {
                string pathRUT = Path.Combine(Ruta + Path.GetFileName(SoporteModel.RUTNatural.FileName));
                string pathCC = Path.Combine(Ruta + Path.GetFileName(SoporteModel.CCNatural.FileName));
                string pathCertificacion = Path.Combine(Ruta + Path.GetFileName(SoporteModel.CBNatural.FileName));

                sopPropietarioPath.RUT = pathRUT;
                sopPropietarioPath.CC = pathCC;
                sopPropietarioPath.CertificacionBancaria = pathCertificacion;
            }
            else if (TipoUsuario == 2) //juridico
            {
                string pathRUT = Path.Combine(Ruta + Path.GetFileName(SoporteModel.RUTJuridica.FileName));
                string pathCC = Path.Combine(Ruta + Path.GetFileName(SoporteModel.CCJuridica.FileName));
                string pathCertificacion = Path.Combine(Ruta + Path.GetFileName(SoporteModel.CBJuridica.FileName));
                string pathCamaraComercio = Path.Combine(Ruta + Path.GetFileName(SoporteModel.CamaraComercio.FileName));
                string pathNIT = Path.Combine(Ruta + Path.GetFileName(SoporteModel.NIT.FileName));

                sopPropietarioPath.RUT = pathRUT;
                sopPropietarioPath.CC = pathCC;
                sopPropietarioPath.NIT = pathNIT;
                sopPropietarioPath.CamaraComercio = pathCamaraComercio;
                sopPropietarioPath.CertificacionBancaria = pathCertificacion;
            }

            return sopPropietarioPath;
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

        public SelectList ToListTipoUsuario()
        {
            List<SelectListItem> list = new List<SelectListItem>();

            list.Add(new SelectListItem()
            {
                Text = "Propietario Juridico",
                Value = 1.ToString()
            });

            list.Add(new SelectListItem()
            {
                Text = "Propietario Natural",
                Value = 2.ToString()
            });

            return new SelectList(list, "Value", "Text");
        }


        [HttpPost]
        [AllowAnonymous]
        public ActionResult SavePropietario(Models.PropietarioInputModel model)
        {
            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            CredencialesDeAcceso acceso = new CredencialesDeAcceso();
            RespuestaUsuario respuesta = new RespuestaUsuario();

            respuesta = SaveUsuario(data, model);
            if (!respuesta.respuestaErrorUsuario)
            {
                if (model.tipoPersona == 1) // persona juridica
                {
                    propJuridicoInputModel PersonaJuridica = new propJuridicoInputModel();
                    PersonaJuridica.fk_usuario = respuesta.idUsuario;
                    PersonaJuridica.nombreContacto = model.nombreContacto;
                    PersonaJuridica.apellidoContacto = model.apellidoContacto;
                    PersonaJuridica.nit = model.nit;
                    PersonaJuridica.razonSocial = model.razonSolical;
                    PersonaJuridica.telefonoFijo = model.telefonoFijo;
                    PersonaJuridica.fk_idciudad = model.ciudad_id;
                    PersonaJuridica.direccion = model.direccion;

                    DataTable dt = data.CrearPropietarioJuridico(PersonaJuridica);

                    if (dt.Rows.Count == 1)
                    {
                        DataRow row = dt.Rows[0];
                        if (row["respuesta"].ToString() == "1")
                        {
                            EnviarCorreos correoCreacion = new EnviarCorreos();
                            string bodyCorreo2 = $"Administrativo bidcargo un Propietario {model.razonSolical} juridico realizo el Pre-registro  en su plataforma ";
                            correoCreacion.EnviarCorreo("cma010360@gmail.com", "Pre-registro Propietario. ", "cma010360@gmail.com", bodyCorreo2, "Bidcargo@hotmail.com", "Bidcargo@hotmail.com", "bidC#123", "");

                            string bodyCorreo = correoCreacion.ArmarCorreoElectronicoPrimerContacto(model.nombreContacto, model.apellidoContacto, model.telefonoCelular, respuesta.usuario, model.telefonoCelular, Session["urlHttp"].ToString() + "/Home/ActualizarContrasenaUsuarios");

                            correoCreacion.EnviarCorreo(model.correoElectronico, "PreRegistro BidCargo - " + model.correoElectronico, "Bidcargo@hotmail.com", bodyCorreo, "Bidcargo@hotmail.com", "Bidcargo@hotmail.com", "bidC#123", "");
                            Session["mensaje"] = "El usuario " + model.nombreContacto + " ha sido registrado de forma exitosa, por favor validar su correo electrónico";
                            //return Redirect("~/Home/Login");
                        }
                        else
                        {
                            Session["error"] = "error";
                            Session["mensaje"] = "El usuario con el Email " + model.correoElectronico + " ya se encuentra registrado en el sistema";
                        }
                    }

                }
                else if (model.tipoPersona == 2)// persona natural
                {
                    PropNaturalInputModel PersonaNatural = new PropNaturalInputModel();
                    PersonaNatural.fk_usuario = respuesta.idUsuario;
                    PersonaNatural.nombres = model.nombre;
                    PersonaNatural.apellidos = model.apellido;
                    PersonaNatural.numeroDocumento = model.numeroDocumento;
                    PersonaNatural.telefonoFijo = model.telefonoFijo;
                    PersonaNatural.direccion = model.direccion;
                    PersonaNatural.fk_idciudad = model.ciudad_id;
                    PersonaNatural.fk_TipoDocumento = model.tipoDocumento_id;

                    DataTable dt = data.CrearPropietarioNatural(PersonaNatural);

                    if (dt.Rows.Count == 1)
                    {
                        DataRow row = dt.Rows[0];
                        if (row["respuesta"].ToString() == "1")
                        {
                            EnviarCorreos correoCreacion = new EnviarCorreos();

                            string bodyCorreo2 = $"Administrativo bidcargo un Propietario {model.nombre} natural realizo el Pre-registro  en su plataforma ";
                            correoCreacion.EnviarCorreo("cma010360@gmail.com", "Pre-registro Propietario. ", "cma010360@gmail.com", bodyCorreo2, "Bidcargo@hotmail.com", "Bidcargo@hotmail.com", "bidC#123", "");

                            string bodyCorreo = correoCreacion.ArmarCorreoElectronicoPrimerContacto(model.nombre, model.apellido, model.telefonoCelular, respuesta.usuario, model.telefonoCelular, Session["urlHttp"].ToString() + "/Home/ActualizarContrasenaUsuarios");

                            correoCreacion.EnviarCorreo(model.correoElectronico, "PreRegistro BidCargo - " + model.correoElectronico, "Bidcargo@hotmail.com", bodyCorreo, "Bidcargo@hotmail.com", "Bidcargo@hotmail.com", "bidC#123", "");
                            
                            Session["mensaje"] = "El usuario " + model.nombreContacto + " ha sido registrado de forma exitosa, por favor validar su correo electrónico";
                            //return Redirect("~/Home/Login");
                        }
                        else
                        {
                            Session["error"] = "error";
                            Session["mensaje"] = "El usuario con el Email " + model.correoElectronico + " ya se encuentra registrado en el sistema";
                        }
                    }
                }
            }

            return Redirect("PreRegistro");
        }

        public RespuestaUsuario SaveUsuario(ConnectionDataBase.StoreProcediur data, Models.PropietarioInputModel model)
        {
            CredencialesDeAcceso acceso = new CredencialesDeAcceso();

            string NombreUsuario = "";

            if (model.tipoPersona == 1) // persona juridica
            {
                NombreUsuario = acceso.creacionUsuario(model.nombreContacto, model.apellidoContacto).ToLower();
            }
            else if (model.tipoPersona == 2)// persona natural
            {
                NombreUsuario = acceso.creacionUsuario(model.nombre, model.nombre).ToLower();
            }

            string contrasena = model.telefonoCelular;
            RijndaelManaged myRijndael = new RijndaelManaged();
            myRijndael.GenerateKey();
            myRijndael.GenerateIV();
            Byte[] contrasenaEncriptada = acceso.EncryptStringToBytes(contrasena, myRijndael.Key, myRijndael.IV);

            UsuarioInputModel usuarioInput = new UsuarioInputModel();
            usuarioInput.telefonoMovil = model.telefonoCelular;
            usuarioInput.nombreUsusario = NombreUsuario;
            usuarioInput.contraseña = contrasenaEncriptada;
            usuarioInput.correoContacto = model.correoElectronico;
            usuarioInput.KEY = myRijndael.Key;
            usuarioInput.IV = myRijndael.IV;
            usuarioInput.rol = 2;
            usuarioInput.idEstadoCliente = 0;

            DataTable dt = data.CrearUsuario(usuarioInput);
            DataRow row = dt.Rows[0];
            RespuestaUsuario respuesta = new RespuestaUsuario();
            if (dt.Rows.Count == 1)
            {
                if (row["respuestaErrorCorreo"].ToString() == "0")
                {
                    RedirectToAction("PreRegistro");
                    Session["error"] = "error";
                    Session["mensaje"] = "El usuario con el Email " + model.correoElectronico + " ya se encuentra registrado en el sistema";
                    respuesta.respuestaErrorUsuario = true;
                }
                else if (row["idUsuario"].ToString() != "0")
                {
                    respuesta.usuario = NombreUsuario;
                    respuesta.idUsuario = int.Parse(row["idUsuario"].ToString());
                    respuesta.respuestaErrorUsuario = false;
                }
            }

            return respuesta;
        }

        [HttpGet]
        [AllowAnonymous]
        public JsonResult ciudades(int idDepartamento)
        {
            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            DataTable dt = data.getCitybyDepatamento("SP_getCityByDepartament", idDepartamento);

            return Json(JsonConvert.SerializeObject(dt, Formatting.Indented), JsonRequestBehavior.AllowGet);
        }

        public class ElementJsonIntKey
        {
            public int Value { get; set; }
            public string Text { get; set; }
        }

        public class RespuestaUsuario
        {
            public int idUsuario { get; set; }
            public string Message { get; set; }
            public string usuario { get; set; }
            public bool respuestaErrorUsuario { get; set; }
        }


        public ActionResult MenuPropietario()
        {
            return View();
        }

        public ActionResult TablaOfertas()
        {
            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            DataTable dd = data.consultarCliente(1);
            if (dd.Rows.Count > 0)
                ViewBag.Rows = dd.Rows[0];

            ViewBag.Rows = data.getOffer("", 3).Rows;
            return View();
        }

        public ActionResult TablaVehiculos()
        {
            return View();
        }
        public ActionResult ContraOferta(string id)
        {
            
            ViewBag.codigo = id;
            @TempData["IdOferta"] = id;
            ViewBag.propietarioView = ConsultarInformacion();
            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            DataTable dt = data.ConsultarVehiculosPropietario(int.Parse(@TempData["IdUsuario"].ToString()));
            DataTable dtConductor = data.ConsultarConductores(int.Parse(@TempData["IdUsuario"].ToString()));
            DataTable dtEmpresa = data.ConsultarEmpresaTransporte();
            ViewBag.models = data.getOffer(id, 4).Rows;

            ViewBag.ListaVehiculos = ToSelectList(dt, "idVehiculo", "placa");
            ViewBag.ListaConductores = ToSelectList(dtConductor, "idConductor", "nombre");
            ViewBag.ListaEmpresas = ToSelectList(dtEmpresa, "idCliente", "nombreEmpresa");
            
            return View();
        }

        public ActionResult MostrarinformcaiconOferta()
        {
            return View();
        }

        public ActionResult PropietarioShowOffer(string id)
        {
            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            DataTable dd = data.consultarCliente(1);
            DataTable dt = data.getOffer(id, 4);


            DataTable ClientDs = data.getContracOffers(1, id);
            if (ClientDs.Rows.Count > 0)
                ViewBag.dt = dd.Rows;

            else
                ViewBag.dt = null;


            if (dd.Rows.Count > 0)
            {
                ViewBag.data = dd.Rows[0];
                ViewBag.idClientOffer = dt.Rows[0]["idClient"];
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
                else
                {
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

            return View();
        }



        public JsonResult ConsultarInfoVehiculo(int idVehiculo)
        {
            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            DataTable dt = data.ConsultarInfoVehiculos(idVehiculo);

            return Json(JsonConvert.SerializeObject(dt, Formatting.Indented), JsonRequestBehavior.AllowGet);
        }

        public JsonResult ConsultarConductor(int idConductor)
        {
            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            DataTable dt = data.ConsultarConductor(idConductor);

            return Json(JsonConvert.SerializeObject(dt, Formatting.Indented), JsonRequestBehavior.AllowGet);
        }






        public ActionResult VehivuloRegistro()
        {
            int IdUsuario = Convert.ToInt32(Session["IdUsuario"]);

            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            DataTable dd = data.DrowlistTipoVehiculo();
            ViewBag.Lista = ToSelectList2(dd, "idTipoDeVehiculo", "TipoDeVehiculo");

            return View();
        }


        [HttpPost]
        public ActionResult GuardarVehiculo(VehiculoViewModels models)
        {
            EnviarCorreos correoCreacion = new EnviarCorreos();

            if (models.TipoVehiculo != 14)
            {
                int IdUsuario = Convert.ToInt32(Session["IdUsuario"]);
                ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
                models.IdPropietario = IdUsuario;
                models.Estado = "0";
                string Rutasitio = Server.MapPath("~/");
                string date = DateTime.Now.ToString("dd-MM-yyyy");
                string hour = DateTime.Now.ToString("ss-mm-hh");
                string ruta = "Content/Files/" + models.IdPropietario + "_" + date + "_" + hour + "_";

                models.SoportePropiedad = Path.Combine(ruta + Path.GetFileName(models.SpPropiedad.FileName));
                models.SoporteMecanica = Path.Combine(ruta + Path.GetFileName(models.SpMecanica.FileName));
                models.SoporteSoat = Path.Combine(ruta + Path.GetFileName(models.SpMecanica.FileName));
                models.SoportetodoRiezgo = Path.Combine(ruta + Path.GetFileName(models.SpTodoResgo.FileName));

                models.SpPropiedad.SaveAs(Path.Combine(Rutasitio + models.SoportePropiedad));
                models.SpMecanica.SaveAs(Path.Combine(Rutasitio + models.SoporteMecanica));
                models.SpSoat.SaveAs(Path.Combine(Rutasitio + models.SoporteSoat));
                models.SpTodoResgo.SaveAs(Path.Combine(Rutasitio + models.SoportetodoRiezgo));
                if (models.TipoVehiculo < 5 || models.TipoVehiculo > 8)
                {
                    models.PlacaRemolque = "No remolque";
                }

                DataTable dt = data.GuardarVehiculo(models);
                string bodyCorreoAdmin = correoCreacion.ArmarCorreoPapelesVehiculo(models.Placa);
                correoCreacion.EnviarCorreo("cma010360@gmail.com", "Documentacion Vehicular", "Bidcargo@hotmail.com", bodyCorreoAdmin, "Bidcargo@hotmail.com", "Bidcargo@hotmail.com", "bidC#123", "");
                Session["message"] = "La carga de los documentos fue exitosa. Su vehiculo se encuentra en revision, contacte con el administrador, disculpe las molestias...";
                Session["type"] = "success";

                return RedirectToAction("VehivuloRegistro");

            }
            else
            {
                int IdUsuario = Convert.ToInt32(Session["IdUsuario"]);
                ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
                models.IdPropietario = IdUsuario;
                models.Estado = "0";
                string Rutasitio = Server.MapPath("~/");
                string date = DateTime.Now.ToString("dd-MM-yyyy");
                string hour = DateTime.Now.ToString("ss-mm-hh");
                string ruta = "Content/Files/" + models.IdPropietario + "_" + date + "_" + hour + "_";
                models.SoportePropiedad = Path.Combine(ruta + Path.GetFileName(models.SpPropiedad.FileName));
                string yrailer ="No apto para trailer";

                models.SpPropiedad.SaveAs(Path.Combine(Rutasitio + models.SoportePropiedad));
                models.SoporteMecanica = yrailer;
                models.SoporteSoat = yrailer;
                models.SoportetodoRiezgo = yrailer;

                DataTable dt = data.GuardarVehiculo(models);
                string bodyCorreoAdmin = correoCreacion.ArmarCorreoPapelesVehiculo(models.Placa);
                correoCreacion.EnviarCorreo("cma010360@gmail.com", "Documentacion Vehicular", "Bidcargo@hotmail.com", bodyCorreoAdmin, "Bidcargo@hotmail.com", "Bidcargo@hotmail.com", "bidC#123", "");
                Session["message"] = "La carga de los documentos fue exitosa. Su vehiculo se encuentra en revision, contacte con el administrador, disculpe las molestias...";
                Session["type"] = "success";

                return RedirectToAction("VehivuloRegistro");

            }


        }



        public SelectList ToSelectList2(DataTable table, string valueField, string textField)
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

        public ActionResult tablavehiculoPropietario()
        {
            int IdUsuario = Convert.ToInt32(Session["IdUsuario"]);
            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            DataTable dd = data.ReporteVehiculo2(IdUsuario);
            if (dd.Rows.Count > 0)
                ViewBag.data = dd.Rows[0];
            else
                dd = data.ReporteVehiculo2(IdUsuario);
            ViewBag.rows = dd.Rows;
            return View();

        }

        public ActionResult VisualizarVehiculo(int placa)
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





        public ActionResult GuardarContraOferta(ContraOfertaPropietario contraOferta)
        {
            EnviarCorreos correoCreacion = new EnviarCorreos();
            try
            {
                if (contraOferta.FileDocumento == null)
                {
                    string Rutasitio = Server.MapPath("~/");
                    string date = DateTime.Now.ToString("dd-MM-yyyy");
                    string hour = DateTime.Now.ToString("ss-mm-hh");
                    string ruta = "Content/Files/contraoferta_oferta_" + @TempData["IdOferta"].ToString() + "_" + date + "_" + hour;

                    contraOferta.PathDocumento = "No tiene Documento anexo";
                    

                    ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
                    DataTable dt = data.ObtenerIdOferta(@TempData["IdOferta"].ToString());
                    DataRow row = dt.Rows[0];

                    contraOferta.Estado = 0;
                    contraOferta.Fk_Usuario = Convert.ToInt32(Session["IdUsuario"]);
                    contraOferta.CodigoOferta = int.Parse(row["idClientOffer"].ToString());

                    DataTable dtContraOfer = data.CreacionContraOfertaPropietario(contraOferta);

                    Session["message"] = "El registro de la contra-oferta fue exitoso. ";
                    Session["type"] = "success";

                    string bodyCorreoAdmin = correoCreacion.sendMailContraOffer2(@TempData["IdOferta"].ToString(), "");
                    correoCreacion.EnviarCorreo("cma010360@gmail.com", "Contra-Oferta Realizada", "Bidcargo@hotmail.com", bodyCorreoAdmin, "Bidcargo@hotmail.com", "Bidcargo@hotmail.com", "bidC#123", "");

                    foreach (var item in contraOferta.Empresas)
                    {
                        DataTable ddd = data.consultarempresa(item);
                        string bodyCorreoAdmin2 = correoCreacion.sendMailContraOffer2(@TempData["IdOferta"].ToString(), "");
                        correoCreacion.EnviarCorreo(ddd.Rows[0]["email"].ToString(), "Contra-Oferta Realizada", "Bidcargo@hotmail.com", bodyCorreoAdmin2, "Bidcargo@hotmail.com", "Bidcargo@hotmail.com", "bidC#123", "");

                    }

                    return RedirectToAction("TablaOfertas");
                }
                else {
                    string Rutasitio = Server.MapPath("~/");
                    string date = DateTime.Now.ToString("dd-MM-yyyy");
                    string hour = DateTime.Now.ToString("ss-mm-hh");
                    string ruta = "Content/Files/contraoferta_oferta_" + @TempData["IdOferta"].ToString() + "_" + date + "_" + hour;

                    contraOferta.PathDocumento = Path.Combine(ruta + Path.GetFileName(contraOferta.FileDocumento.FileName));
                    contraOferta.FileDocumento.SaveAs(Path.Combine(Rutasitio + contraOferta.PathDocumento));

                    ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
                    DataTable dt = data.ObtenerIdOferta(@TempData["IdOferta"].ToString());
                    DataRow row = dt.Rows[0];

                    contraOferta.Estado = 0;
                    contraOferta.Fk_Usuario = Convert.ToInt32(Session["IdUsuario"]);
                    contraOferta.CodigoOferta = int.Parse(row["idClientOffer"].ToString());

                    DataTable dtContraOfer = data.CreacionContraOfertaPropietario(contraOferta);

                    Session["message"] = "El registro de la contra-oferta fue exitoso. ";
                    Session["type"] = "success";

                    string bodyCorreoAdmin = correoCreacion.sendMailContraOffer2(@TempData["IdOferta"].ToString(), "");
                    correoCreacion.EnviarCorreo("cma010360@gmail.com", "Contra-Oferta Realizada", "Bidcargo@hotmail.com", bodyCorreoAdmin, "Bidcargo@hotmail.com", "Bidcargo@hotmail.com", "bidC#123", "");

                    foreach (var item in contraOferta.Empresas)
                    {
                        DataTable ddd = data.consultarempresa(item);
                        string bodyCorreoAdmin2 = correoCreacion.sendMailContraOffer2(@TempData["IdOferta"].ToString(), "");
                        correoCreacion.EnviarCorreo(ddd.Rows[0]["email"].ToString(), "Contra-Oferta Realizada", "Bidcargo@hotmail.com", bodyCorreoAdmin2, "Bidcargo@hotmail.com", "Bidcargo@hotmail.com", "bidC#123", "");

                    }

                    return RedirectToAction("TablaOfertas");


                }
                
            }
            catch (Exception)
            {

                throw;
            }
        }


        public ActionResult PerfilPropietario()
        {
            int IdUsuario = int.Parse(Session["IdUsuario"].ToString());
            if (Session["IdUsuario"] == null)
            {
                return Redirect("~/Home/Login");
            }

            ConsultarPropietarioPerfil();

            return View();
        }

        private void ConsultarPropietarioPerfil()
        {
            int IdUsuario = int.Parse(Session["IdUsuario"].ToString());
            ConnectionDataBase.StoreProcediur connectionDataBase = new ConnectionDataBase.StoreProcediur();
            DataRow data = connectionDataBase.ConsultarUsuarioPropietarioPerfil(IdUsuario).Rows[0];
            ViewBag.propietarioSoportesView = data;
            
        }

        public FileResult DescargarArchivos(string ruta)
        {
            var nombrearchivo = Path.GetFileName(ruta);
            var archivo = Server.MapPath("~/Content/Files/" + nombrearchivo);
            return File(archivo, "application/pdf", Path.GetFileName(archivo));
        }
        public ActionResult TablaConductorPropietario() 
        {
            int IdUsuario = Convert.ToInt32(Session["IdUsuario"]);
            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            DataTable dd = data.ConductorPropietario(IdUsuario);
            if (dd.Rows.Count > 0)
                ViewBag.data = dd.Rows[0];
            else
                dd = data.ConductorPropietario(IdUsuario);
            ViewBag.rows = dd.Rows;
            return View();
        }

        public ActionResult ConsultarInformacionConducor2(int id, int tipo)
        {

            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            DataTable deta = data.ConsultarInformacionAdmin(id);
            DataRow dataRow = deta.Rows[0];
            CondutorvisualizacionComplera conductorView = new CondutorvisualizacionComplera();
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

            return View();
        }









        [HttpPost]
        public ActionResult ActualizarVehiculo(VehiculoViewModels models)
        {
            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
           
            string Rutasitio = Server.MapPath("~/");
            string date = DateTime.Now.ToString("dd-MM-yyyy");
            string hour = DateTime.Now.ToString("ss-mm-hh");
            string ruta = "Content/Files/" + models.IdPropietario + "_" + date + "_" + hour + "_";

            models.SoportePropiedad = Path.Combine(ruta + Path.GetFileName(models.SpPropiedad.FileName));
            models.SoporteMecanica = Path.Combine(ruta + Path.GetFileName(models.SpMecanica.FileName));
            models.SoporteSoat = Path.Combine(ruta + Path.GetFileName(models.SpMecanica.FileName));
            models.SoportetodoRiezgo = Path.Combine(ruta + Path.GetFileName(models.SpTodoResgo.FileName));

            models.SpPropiedad.SaveAs(Path.Combine(Rutasitio + models.SoportePropiedad));
            models.SpMecanica.SaveAs(Path.Combine(Rutasitio + models.SoporteMecanica));
            models.SpSoat.SaveAs(Path.Combine(Rutasitio + models.SoporteSoat));
            models.SpTodoResgo.SaveAs(Path.Combine(Rutasitio + models.SoportetodoRiezgo));
         

            DataTable dt = data.ActualizarVehiculo(models);

           
            Session["message"] = "informacion del vehiculo actualizada, disculpe las molestias...";
            Session["type"] = "success";

            

            return RedirectToAction("tablavehiculoPropietario");
        }


        public ActionResult TablaContraOfertasRealizadas()
        {
            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            int IdUsuario = int.Parse(Session["IdUsuario"].ToString());
            DataTable dataTable = data.ListaContraOfertaPropietarioPorUsuario(IdUsuario);
            List<ContraOferta> ListOfertaPropietario = new List<ContraOferta>();

            foreach (DataRow row in dataTable.Rows)
            {
                ContraOferta contraOferta = new ContraOferta();
                contraOferta.Sk_ContraOferta = int.Parse(row[0].ToString());
                contraOferta.CodeOffer = row[4].ToString();
                contraOferta.FechaFinal = row[2].ToString();
                contraOferta.Costo = double.Parse(row[3].ToString());
                contraOferta.Nombre = row[5].ToString();
                contraOferta.Apellido = row[6].ToString();
                contraOferta.fk_usuario = int.Parse(row[9].ToString());

                DataTable dataUsuario = data.ConsultarUsuarioPropietarioPerfil(contraOferta.fk_usuario);
                DataRow dataRow = dataUsuario.Rows[0];
                contraOferta.TipoPropietario = dataRow[1].ToString();
                ListOfertaPropietario.Add(contraOferta);
            }

            ViewBag.ListOfertaPropietario = ListOfertaPropietario;
            return View();
        }



        public ActionResult ViewContraOfertaRealizadaPropietario(string Sk_ContraOferta)
        {
            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            DataTable dataTable = data.ListaContraOffShearch(Sk_ContraOferta);
            Session["Sk_ContraOfertaPropiertario"] = Sk_ContraOferta;
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
                return RedirectToAction("Index");
            }

            return View();
        }

        [HttpPost]
        public void UpdateCostoContraoferta(string NuevoCosto){
            int Sk_ContraOfertaPropiertario = int.Parse(Session["Sk_ContraOfertaPropiertario"].ToString());
            decimal CostoNuevo = decimal.Parse(NuevoCosto);

            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            DataTable dataTable = data.UpdateCostoContraOferta(CostoNuevo, Sk_ContraOfertaPropiertario);

            DataRow row = dataTable.Rows[0];

            if (int.Parse(row["respuesta"].ToString()) == 1)
            {
                Session["message"] = "Costo de la contraoferta a sido modificado";
                Session["type"] = "success";
            }
            else
            {
                Session["message"] = "Costo de la contraoferta a sido modificado";
                Session["type"] = "warning";
            }

        }


    }
}