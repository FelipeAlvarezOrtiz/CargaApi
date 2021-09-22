namespace CargaBd.API.Models
{
    public class Respuesta
    {
        public int CodigoRespuesta { get; set; }
        public string MensajeUsuario { get; set; }
        public string ResponseBody { get; set; }
        public int NumeroAtencion { get; set; }
    }
}
