namespace CargaBd.API.Models
{
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
        public string EstadoIntentos { get; set; }
        public string PesoPaquete { get; set; }
        public string Precio { get; set; }
        public string TipoCobro { get; set; }
    }
}
