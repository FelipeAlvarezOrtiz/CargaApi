namespace CargaBd.API.Models
{
    public class FechaDto
    {
        public string FechaFin { get; set; }
    }

    public class NumeroOrdenDto
    {
        public string NumeroOrden { get; set; }
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
        public string Id { get; set; }
        public string Usuario { get; set; }
    }

    public class ExtraFieldHelper
    {
        public string sep360_nintentof { get; set; }
        public string sep360_nombrerecibe { get; set; }
        public string sep360_rutrecibe { get; set; }
        public string sep360_nintento { get; set; }
    }
}
