namespace CargaBd.API.Models
{
    public class PayloadCliente
    {
        public string tracking_id { get; set; }
        public string reference { get; set; }
        public string title { get; set; }
        public string address { get; set; }
        public string checkout_time { get; set; }
        public string status { get; set; }
        public string notes { get; set; }
        public string contact_name { get; set; }
        public string contact_phone { get; set; }
        public string contact_email { get; set; }
        public string checkout_comment { get; set; }
    }
}
