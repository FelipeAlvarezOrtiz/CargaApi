using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CargaBd.API.Models
{
    public class Etiqueta
    {
        public string Compania { get; set; }
        public string Cliente { get; set; }
        public string Direccion { get; set; }
        public string Numero { get; set; }
        public string Referencia { get; set; }
        public string movil { get; set; }
        public string region { get; set; }
        public string Comuna { get; set; }
        public string Orden { get; set; }
        public string Bulto { get; set; }
        
    }
    public class Etiqueta_array
    {
        public string email_envio { get; set; }
        public List<Etiqueta> Etiquetas { get; set; }
    }

}
