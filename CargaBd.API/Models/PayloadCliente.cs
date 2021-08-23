namespace CargaBd.API.Models
{
    //public class PayloadCliente
    //{
    //    public string tracking_id { get; set; }
    //    public string reference { get; set; }
    //    public string title { get; set; }
    //    public string address { get; set; }
    //    public string checkout_time { get; set; }
    //    public string status { get; set; }
    //    public string notes { get; set; }
    //    public string contact_name { get; set; }
    //    public string contact_phone { get; set; }
    //    public string contact_email { get; set; }
    //    public string checkout_comment { get; set; }
    //}
    public class PayloadCliente
    {
        public string Folio { get; set; }
        public string Origen { get; set; }
        public string Destino { get; set; }
        public string FechaRecepcion { get; set; }
        public string EstadoEnvio { get; set; }
        public string FechaEnvio { get; set; }
        public string FechaEntrega { get; set; }
        public string Observacion { get; set; }
        public string Seguimiento { get; set; }
        public string QuienRecibeNombre { get; set; }
        public string QuienRecibeRut { get; set; }
        public string Intentos { get; set; }
        public string FechaIntentos { get; set; }
        public string EtaIntentos { get; set; }
    }
}
