namespace CargaBd.API.Models
{
    public class FechaDto
    {
        public string FechaFin { get; set; }
    }

    public class NumeroOrdenDto
    {
        public int NumeroOrden { get; set; }
        public string Usuario { get; set; }
    }

    public class BusquedaMasivaDto
    {
        public string FechaDesde { get; set; }
        public string FechaHasta { get; set; }
        public string Usuario { get; set; }
    }

    public class ReferenciaDto
    {
        public string Referencia { get; set; }
        public string Usuario { get; set; }
    }

    public class MasivaDto
    {
        public string FechaDesde { get; set; }
        public string FechaHasta { get; set; }
        public string Referencia { get; set; }
        public string Usuario { get; set; }
    }
}
