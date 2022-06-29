using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BidCargo_.Models
{
    public class VehiculoViewModels
    {
        [Required]
        public string Placa { get; set; }
        [Required]
        public int TipoVehiculo { get; set; }
        [Required]
        public string Marca { get; set; }
        [Required]
        public string Modelo { get; set; }
        [Required]
        public string Linea { get; set; }
        [Required]
        public string Operador { get; set; }
        [Required]
        public string Usuario { get; set; }
        [Required]
        public string Contraseña { get; set; }
        [Required]
        public string PlacaRemolque { get; set; }


        public string SoportePropiedad { get; set; }
        public string SoporteSoat { get; set; }
        public string SoporteMecanica { get; set; }
        public string SoportetodoRiezgo { get; set; }

       
        public int idVehiculo { get; set; }
        public string Estado { get; set; }
        public int IdPropietario { get; set; }

            public HttpPostedFileBase SpPropiedad { get; set; }
            public HttpPostedFileBase SpSoat { get; set; }
            public HttpPostedFileBase SpMecanica { get; set; }
            public HttpPostedFileBase SpTodoResgo { get; set; }



    }
}