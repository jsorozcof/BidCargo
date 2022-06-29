using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BidCargo_.Models
{
    public class UsuarioInputModel
    {
        public string nombreUsusario { get; set; }
        public string correoContacto { get; set; }
        public string telefonoMovil { get; set; }
        public Byte[] contraseña { get; set; }
        public Byte[] KEY { get; set; }
        public Byte[] IV { get; set; }
        public int rol { get; set; }
        public int idEstadoCliente { get; set; }
    }

    public class UsuarioViewModel: UsuarioInputModel
    {
        public int idUsuario { get; set; }
    }
}