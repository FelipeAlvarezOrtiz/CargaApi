namespace CargaBd.API.Models
{
    public class PayloadDto
    {
            public int? id { get; set; }
            public int? order { get; set; }
            public string tracking_id { get; set; }
            public string status { get; set; }
            public string title { get; set; }
            public string address { get; set; }
            public string latitude { get; set; }
            public string longitude { get; set; }
            public decimal load { get; set; }
            public decimal load_2 { get; set; }
            public decimal load_3 { get; set; }
            public string window_start { get; set; }
            public string window_end { get; set; }
            public string window_start_2 { get; set; }
            public string window_end_2 { get; set; }
            public string duration { get; set; }
            public string contact_name { get; set; }
            public string contact_phone { get; set; }
            public string contact_email { get; set; }
            public string reference { get; set; }
            public string notes { get; set; }
            public int[] skills_required { get; set; }
            public int[] skills_optional { get; set; }
            public string[] tags { get; set; }
            public string planned_date { get; set; }
            public string programmed_date { get; set; }
            public string route { get; set; }
            public string route_estimated_time_start { get; set; }
            public string estimated_time_arrival { get; set; }
            public string estimated_time_departure { get; set; }
            public string checkin_time { get; set; }
            public string checkout_time { get; set; }
            public string checkout_latitude { get; set; }
            public string checkout_longitude { get; set; }
            public string checkout_comment { get; set; }
            public string checkout_observation { get; set; }
            public string signature { get; set; }
            public string[] pictures { get; set; }
            public string created { get; set; }
            public string modified { get; set; }
            public string eta_predicted { get; set; }
            public string eta_current { get; set; }
            public int? driver { get; set; }
            public int vehicle { get; set; }
            public bool priority { get; set; }
            public bool has_alert { get; set; }
            public int? priority_level { get; set; }
            public object extra_field_values { get; set; }
            public string geocode_alert { get; set; }
            public string visit_type { get; set; }
            public string current_eta { get; set; }
            public string fleet { get; set; }
    }
}
