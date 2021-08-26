namespace CargaBd.API.Models
{
    public class AppException
    {
        public AppException(int statusCode, string mensaje, string detalles = null)
        {
            StatusCode = statusCode;
            Mensaje = mensaje;
            Detalles = detalles;
        }
        public int StatusCode { get; set; }
        public string Mensaje { get; set; }
        public string Detalles { get; set; }
    }
}
