using BidCargo_.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;

namespace BidCargo_.Controllers
{
    [Authorize(Roles = "2")]
    public class ConductorController : Controller
    {
        // GET: Conductor
        public ActionResult PreRegistro()
        {
            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            DataTable dt = data.ObtenerData("SP_departamentos");

            ViewBag.ListaDepartamentos = ToSelectList(dt, "idDepartamentos", "departamentos");

            dt = data.ObtenerData("SP_ConsultarTipoDocumento");
            ViewBag.ListTipoDocumento = ToSelectList(dt, "sk_tipoDocumento", "nombre");

            return View();
        }

        [HttpGet]
        public JsonResult ConsultarCiudades(int idDepartamento)
        {
            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            DataTable dt = data.getCitybyDepatamento("SP_getCityByDepartament", idDepartamento);
            //ViewBag.ListaCiudades = ToSelectList(dt, "idCiudad", "Ciudad");
            return Json(JsonConvert.SerializeObject(dt, Formatting.Indented), JsonRequestBehavior.AllowGet);
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

        public ActionResult GuardarPreRegistroConductor(ConductorInputModel conductor)
        {
            
            EnviarCorreos correoCreacion = new EnviarCorreos();
            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            CredencialesDeAcceso acceso = new CredencialesDeAcceso();

            int IdUsuario = Convert.ToInt32(Session["IdUsuario"]);

            DataTable dtConductor = data.CrearConductor(conductor, IdUsuario, IdUsuario);

            DataRow rowConductor = dtConductor.Rows[0];

            if (rowConductor["respuesta"].ToString() != "0")
            {
                Session["idConductor"] = rowConductor["respuesta"];
                string bodyCorreoAdmin = correoCreacion.correoAdministrativos(conductor.nombre, conductor.numeroLicencia, "C");
                correoCreacion.EnviarCorreo("cma010360@gmail.com", "Cargue Documentos Conductor BidCargo - ", "Bidcargo@hotmail.com", bodyCorreoAdmin, "Bidcargo@hotmail.com", "Bidcargo@hotmail.com", "bidC#123", "");


                return GuardarArchivos(conductor);


                
            }
            else
            {
                Session["type"] = "error";
                Session["message"] = "El usuario con el Email " + conductor.correoContacto + " ya se encuentra registrado en el sistema";

                return RedirectToAction("PreRegistro");
            }
        }

        //public ActionResult 


        public ActionResult CargueDocumento()
        {
            
            ViewBag.conductorView = ConsultarInfoConductor();

            return View();
        }

        public ConductorViewModel ConsultarInfoConductor()
        {
            int IdConductor = Convert.ToInt32(Session["idConductor"]);
            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            DataTable dt = data.ConsultarUsuarioConductor(IdConductor);
            DataRow dataRow = dt.Rows[0];

            ConductorViewModel conductorView = new ConductorViewModel();

            conductorView.tipoDocumento = Convert.ToInt32(dataRow["tipoDocumento"]);
            conductorView.nombre = dataRow["nombres"].ToString();
            conductorView.apellido = dataRow["apellidos"].ToString();
            conductorView.telefonoMovil = dataRow["telefono"].ToString();
            conductorView.correo = dataRow["correo"].ToString();
            conductorView.numeroDocumento = dataRow["cedula"].ToString();
            conductorView.idConductor = Convert.ToInt32(dataRow["idConductor"]);

            @TempData["correo"] = dataRow["correo"].ToString();
            TempData["nombre"] = dataRow["nombres"].ToString() + " " + dataRow["apellidos"].ToString();

            return conductorView;
        }

        public ActionResult GuardarArchivos(ConductorInputModel conductorFileInput)
        {
            string rutaSitio = Server.MapPath("~/");
            string date = DateTime.Now.ToString("dd-MM-yyyy");
            string hour = DateTime.Now.ToString("ss-mm-hh");
            string ruta = "Content/Files/" + Session["idConductor"].ToString() + "_" + date + "_" + hour + "_";

            ConductorPathInputModel conductorPath = new ConductorPathInputModel();

            conductorPath.pathCedula = Path.Combine(ruta + Path.GetFileName(conductorFileInput.cedula.FileName));
            conductorPath.pathLicencia = Path.Combine(ruta + Path.GetFileName(conductorFileInput.licencia.FileName));
            conductorPath.pathSeguridadSoc = Path.Combine(ruta + Path.GetFileName(conductorFileInput.seguridadSocial.FileName));
            conductorPath.pathCursos = Path.Combine(ruta + Path.GetFileName(conductorFileInput.cursos.FileName));
            conductorPath.idConductor = int.Parse(conductorFileInput.numeroLicencia); //int.Parse(Session["idConductor"].ToString());

            //if (!ModelState.IsValid)
            //{
            //    return View("CargueDocumento", conductorFileInput);
            //}

            conductorFileInput.cedula.SaveAs(Path.Combine(rutaSitio + conductorPath.pathCedula));
            conductorFileInput.licencia.SaveAs(Path.Combine(rutaSitio + conductorPath.pathLicencia));
            conductorFileInput.seguridadSocial.SaveAs(Path.Combine(rutaSitio + conductorPath.pathSeguridadSoc));
            conductorFileInput.cursos.SaveAs(Path.Combine(rutaSitio + conductorPath.pathCursos));

            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            DataTable dt = data.RegistrarSoportesConductor(conductorPath);
            //DataRow dataRow = dt.Rows[0];

            Session["message"] = "La carga de los documentos del conductor fue exitosa. ";
            Session["type"] = "success";

            EnviarCorreos correoCreacion = new EnviarCorreos();

            string bodyCorreoAdmin = correoCreacion.ArmarCorreoPapelesConductor(conductorFileInput.nombre , conductorFileInput.numeroLicencia);
            correoCreacion.EnviarCorreo("cma010360@gmail.com", "Documentacion Conductor", "Bidcargo@hotmail.com", bodyCorreoAdmin, "Bidcargo@hotmail.com", "Bidcargo@hotmail.com", "bidC#123", "");

            return RedirectToAction("PreRegistro");
        }

        public ActionResult MenuConductor()
        {
            return View();
        }

        public ActionResult VerPerfil()
        {
            ViewBag.conductorView = ConsultarInfoConductor();

            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            DataTable deta = data.ConsultarInformacionAdmin(ViewBag.conductorView.idConductor);
            DataRow dataRow = deta.Rows[0];
            ConductorPathInputModel conductorViewPath = new ConductorPathInputModel();

            conductorViewPath.pathCedula = dataRow["cedulaciudadania"].ToString();
            conductorViewPath.pathLicencia = dataRow["licenciacoducion"].ToString();
            conductorViewPath.pathSeguridadSoc = dataRow["planillapagos"].ToString();
            conductorViewPath.pathCursos = dataRow["cursos"].ToString();

            ViewBag.conductorViewPath = conductorViewPath;

            return View();
        }

        public FileResult DescargarArchivos(string ruta)
        {
            var nombrearchivo = Path.GetFileName(ruta);
            var archivo = Server.MapPath("~/Content/Files/" + nombrearchivo);
            return File(archivo, "application/pdf", Path.GetFileName(archivo));
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

        public ActionResult ConductorShowOffer(string id)
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

    }
}