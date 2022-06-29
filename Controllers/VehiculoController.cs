using BidCargo_.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BidCargo_.Controllers
{
    public class VehiculoController : Controller
    {


        public ActionResult VehivuloRegistro()
        {
            int IdUsuario = Convert.ToInt32(Session["IdUsuario"]);

            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
            DataTable dd = data.DrowlistTipoVehiculo();
            ViewBag.Lista = ToSelectList(dd, "idTipoDeVehiculo", "TipoDeVehiculo");
           
            return View();
        }


        [HttpPost]
        public ActionResult GuardarVehiculo(VehiculoViewModels models)
        {


            ConnectionDataBase.StoreProcediur data = new ConnectionDataBase.StoreProcediur();
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

          
            DataTable dt = data.GuardarVehiculo(models);
            Session["message"] = "La carga de los documentos fue exitosa. Su vehiculo se encuentra en revision, contacte con el administrador, disculpe las molestias...";
            Session["type"] = "success";
           
            Session.Remove("IdUsuario");

            return RedirectToAction("VehivuloRegistro");
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

        public ActionResult Prueba() 
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


    


      








    }
}