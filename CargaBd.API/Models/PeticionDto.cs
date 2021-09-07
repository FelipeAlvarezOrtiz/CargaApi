namespace CargaBd.API.Models
{
    public class PeticionDto
    {
        public string tracking_id { get; set; }
        public string title { get; set; }
        public string address { get; set; }
        public int load { get; set; }
        public int load_2 { get; set; }
        public int load_3 { get; set; }
        public string contact_name { get; set; }
        public string contact_phone { get; set; }
        public string contact_email { get; set; }
        public string reference { get; set; }
        public string notes { get; set; }
        public string planned_date { get; set; }
        public string lugar_retiro { get; set; }
        public string disponible_bodega { get; set; }
        public string usuario { get; set; }
    }
}
