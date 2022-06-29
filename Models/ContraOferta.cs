using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BidCargo_.Models
{
    public class ContraOferta
    {
        public int Sk_ContraOferta { get; set; }
        public string CodeOffer { get; set; }
        public string FechaFinal { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Costo { get; set; }
        public string TipoPropietario { get; set; }
        public int fk_usuario { get; set; }

    }
}