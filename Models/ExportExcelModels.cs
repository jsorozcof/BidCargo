using System;

namespace BidCargo_.Models
{
    public class ExportOfferModels
    {
        //Oferta
        public int ID { get; set; }
        public string CODIGO { get; set; }
        public string EMPRESA { get; set; }
        public int VALOR_OFERTA { get; set; }
        public string TRAYECTO { get; set; }
        public string FACTORING { get; set; }
        public string FECHA_OFERTA { get; set; }
        public string ESTADO { get; set; }
    }

    public class ExportCompanyModels
    {
        //Empresa
        public int ID { get; set; }
        public string RAZON_SOCIAL { get; set; }
        public string NIT { get; set; }
        public string CONTACTO { get; set; }
        public string TIPO_EMPRESA_PRINCIPAL { get; set; }
        public string ESTADO { get; set; }
    }
}