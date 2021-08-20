using System;
using System.Data;
using CargaBd.API.Models;
using Serilog;

namespace CargaBd.API.Logica 
{
    public static class CreaObjetos
    {
        public static PayloadDto CreaObjetoAdmin(DataRow row)
        {
            try
            {
                return new PayloadDto
                {
                    id = int.Parse(row["ID"].ToString() ?? string.Empty),
                    order = int.Parse(row["ORDER"].ToString() ?? string.Empty),
                    tracking_id = row["TRACKING_ID"].ToString(),
                    status = row["STATUS"].ToString(),
                    title = row["TITLE"].ToString(),
                    address = row["ADDRESS"].ToString(),
                    checkin_time = row["CHECKIN_TIME"].ToString(),
                    checkout_comment = row["CHECKOUT_COMMENT"].ToString(),
                    checkout_latitude = row["CHECKOUT_LATITUDE"].ToString(),
                    checkout_longitude = row["CHECKOUT_LONGITUDE"].ToString(),
                    checkout_observation = row["CHECKOUT_OBSERVATION"].ToString(),
                    checkout_time = row["CHECKOUT_TIME"].ToString(),
                    contact_email = row["CONTACT_EMAIL"].ToString(),
                    contact_name = row["CONTACT_NAME"].ToString(),
                    contact_phone = row["CONTACT_PHONE"].ToString(),
                    created = row["CREATED"].ToString(),
                    current_eta = row["CURRENT_ETA"].ToString(),
                    duration = row["DURATION"].ToString(),
                    estimated_time_arrival = row["ESTIMATED_TIME_ARRIVAL"].ToString(),
                    estimated_time_departure = row["ESTIMATED_TIME_DEPARTURE"].ToString(),
                    eta_current = row["ETA_CURRENT"].ToString(),
                    eta_predicted = row["ETA_PREDICTED"].ToString(),
                    extra_field_values = row["EXTRA_FIELD_VALUES"].ToString(),
                    fleet = row["FLEET"].ToString(),
                    geocode_alert = row["GEOCODE_ALERT"].ToString(),
                    has_alert = row["HAS_ALERT"].ToString().Equals("True"),
                    latitude = row["LATITUDE"].ToString(),
                    longitude = row["LONGITUDE"].ToString(),
                    modified = row["MODIFIED"].ToString(),
                    notes = row["NOTES"].ToString(),
                    planned_date = row["PLANNED_DATE"].ToString(),
                    priority = row["PRIORITY"].ToString().Equals("True"),
                    programmed_date = row["PROGRAMMED_DATE"].ToString(),
                    window_start_2 = row["WINDOW_START_2"].ToString(),
                    window_end = row["WINDOW_END"].ToString(),
                    window_start = row["WINDOW_START"].ToString(),
                    window_end_2 = row["WINDOW_END_2"].ToString(),
                    visit_type = row["VISIT_TYPE"].ToString(),
                    signature = row["SIGNATURE"].ToString(),
                    route_estimated_time_start = row["ROUTE_ESTIMATED_TIME_START"].ToString(),
                    route = row["ROUTE"].ToString(),
                    reference = row["REFERENCE"].ToString(),
                    vehicle = int.Parse(row["VEHICLE"].ToString()),
                    driver = int.Parse(row["DRIVER"].ToString() ?? string.Empty),
                    priority_level = int.Parse(row["PRIORITY_LEVEL"].ToString() ?? string.Empty),
                    load = decimal.Parse(row["LOAD"].ToString()),
                    load_2 = decimal.Parse(row["LOAD_2"].ToString()),
                    load_3 = decimal.Parse(row["LOAD_3"].ToString()),
                };
            }
            catch (Exception exception)
            {
                Log.Error(exception,"HA OCURRIDO UN ERROR AL CREAR UN OBJETO PARA EL ADMIN");
                return null;
            }
        }

        public static PayloadCliente CrearObjetoCliente(DataRow row)
        {
            try
            {
                return new PayloadCliente()
                {
                    tracking_id = row["TRACKING_ID"].ToString(),
                    status = row["STATUS"].ToString(),
                    title = row["TITLE"].ToString(),
                    address = row["ADDRESS"].ToString(),
                    checkout_comment = row["CHECKOUT_COMMENT"].ToString(),
                    checkout_time = row["CHECKOUT_TIME"].ToString(),
                    contact_email = row["CONTACT_EMAIL"].ToString(),
                    contact_name = row["CONTACT_NAME"].ToString(),
                    contact_phone = row["CONTACT_PHONE"].ToString(),
                    notes = row["NOTES"].ToString(),
                    reference = row["REFERENCE"].ToString()
                };
            }
            catch (Exception exception)
            {
                Log.Error(exception, "HA OCURRIDO UN ERROR AL CREAR UN OBJETO PARA EL CLIENTE");
                return null;
            }
        }
    }
}
