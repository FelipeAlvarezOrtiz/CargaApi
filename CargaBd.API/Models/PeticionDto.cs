using System.ComponentModel.DataAnnotations;

namespace CargaBd.API.Models
{
    public class PeticionDto
    {
        [Required(ErrorMessage = @"El dato {0} es requerido")]
        public string tracking_id { get; set; }
        [Required(ErrorMessage = @"El dato {0} es requerido")]
        public string title { get; set; }
        [Required(ErrorMessage = @"El dato {0} es requerido")]
        public string address { get; set; }
        [Required(ErrorMessage = @"El dato {0} es requerido")]
        public int load { get; set; }
        [Required(ErrorMessage = @"El dato {0} es requerido")]
        public int load_2 { get; set; }
        [Required(ErrorMessage = @"El dato {0} es requerido")]
        public int load_3 { get; set; }
        [Required(ErrorMessage = @"El dato {0} es requerido")]
        public string contact_name { get; set; }
        [Required(ErrorMessage = @"El dato {0} es requerido")]
        public string contact_phone { get; set; }
        [Required(ErrorMessage = @"El dato {0} es requerido")]
        public string contact_email { get; set; }
        [Required(ErrorMessage = @"El dato {0} es requerido")]
        public string reference { get; set; }
        [Required(ErrorMessage = @"El dato {0} es requerido")]
        public string notes { get; set; }
        [Required(ErrorMessage = @"El dato {0} es requerido")]
        public string planned_date { get; set; }
        [Required(ErrorMessage = @"El dato {0} es requerido")]
        public string lugar_retiro { get; set; }
        [Required(ErrorMessage = @"El dato {0} es requerido")]
        public string disponible_bodega { get; set; }
        [Required(ErrorMessage = @"El dato {0} es requerido")]
        public string usuario { get; set; }
    }
}
