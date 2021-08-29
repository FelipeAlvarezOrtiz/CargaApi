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
            var id = int.Parse(row["ID"].ToString() ?? string.Empty);
            try
            {
                if (id == 70711018)
                {
                    Console.WriteLine("Error");
                }
                //var order = int.Parse(row["ORDER"].ToString() ?? string.Empty);
                var tracking_id = row["TRACKING_ID"].ToString();
                var status = row["STATUS"].ToString();
                var title = row["TITLE"].ToString();
                var address = row["ADDRESS"].ToString();
                var checkin_time = row["CHECKIN_TIME"].ToString();
                var checkout_comment = row["CHECKOUT_COMMENT"].ToString();
                var checkout_latitude = row["CHECKOUT_LATITUDE"].ToString();
                var checkout_longitude = row["CHECKOUT_LONGITUDE"].ToString();
                var checkout_observation = row["CHECKOUT_OBSERVATION"].ToString();
                var checkout_time = row["CHECKOUT_TIME"].ToString();
                var contact_email = row["CONTACT_EMAIL"].ToString();
                var contact_name = row["CONTACT_NAME"].ToString();
                var contact_phone = row["CONTACT_PHONE"].ToString();
                var created = row["CREATED"].ToString();
                var current_eta = row["CURRENT_ETA"].ToString();
                var duration = row["DURATION"].ToString();
                var estimated_time_arrival = row["ESTIMATED_TIME_ARRIVAL"].ToString();
                var estimated_time_departure = row["ESTIMATED_TIME_DEPARTURE"].ToString();
                var eta_current = row["ETA_CURRENT"].ToString();
                var eta_predicted = row["ETA_PREDICTED"].ToString();
                var extra_field_values = row["EXTRA_FIELD_VALUES"].ToString();
                var fleet = row["FLEET"].ToString();
                var geocode_alert = row["GEOCODE_ALERT"].ToString();
                var has_alert = row["HAS_ALERT"].ToString().Equals("True");
                var latitude = row["LATITUDE"].ToString();
                var longitude = row["LONGITUDE"].ToString();
                var modified = row["MODIFIED"].ToString();
                var notes = row["NOTES"].ToString();
                var planned_date = row["PLANNED_DATE"].ToString();
                var priority = row["PRIORITY"].ToString().Equals("True");
                var programmed_date = row["PROGRAMMED_DATE"].ToString();
                var window_start_2 = row["WINDOW_START_2"].ToString();
                var window_end = row["WINDOW_END"].ToString();
                var window_start = row["WINDOW_START"].ToString();
                var window_end_2 = row["WINDOW_END_2"].ToString();
                var visit_type = row["VISIT_TYPE"].ToString();
                var signature = row["SIGNATURE"].ToString();
                var route_estimated_time_start = row["ROUTE_ESTIMATED_TIME_START"].ToString();
                var route = row["ROUTE"].ToString();
                var reference = row["REFERENCE"].ToString();
                //var vehicle = int.Parse(row["VEHICLE"].ToString());
                //var driver = int.Parse(row["DRIVER"].ToString() ?? string.Empty);
                //var priority_level = int.Parse(row["PRIORITY_LEVEL"].ToString() ?? string.Empty);
                var load = decimal.Parse(row["LOAD"].ToString());
                var load_2 = decimal.Parse(row["LOAD_2"].ToString());
                var load_3 = decimal.Parse(row["LOAD_3"].ToString());
                return new PayloadDto
                {
                    id = int.Parse(row["ID"].ToString() ?? string.Empty),
                    order = int.TryParse(row["ORDER"].ToString(), out var orderParsed) ? orderParsed : null,
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
                    driver = int.TryParse(row["ORDER"].ToString(), out var driverParsed) ? driverParsed : null,
                    priority_level = int.TryParse(row["ORDER"].ToString(), out var plParsed) ? plParsed : null,
                    load = decimal.Parse(row["LOAD"].ToString()),
                    load_2 = decimal.Parse(row["LOAD_2"].ToString()),
                    load_3 = decimal.Parse(row["LOAD_3"].ToString()),
                };
            }
            catch (Exception exception)
            {
                Log.Error(exception,"HA OCURRIDO UN ERROR AL CREAR UN OBJETO PARA EL ADMIN CON ID "+ id);
                throw;
            }
        }

        public static PayloadCliente CrearObjetoCliente(DataRow row)
        {
            try
            {
                var folio = row["REFERENCE"].ToString();
                var origen = "Santiago";
                var destino = row["ADDRESS"].ToString();
                var fechaRecepcion = row["CHECKOUT_TIME"].ToString();
                var estadoEnvio = row["STATUS"].ToString();
                var fechaEnvio = row["PLANNED_DATE"].ToString();
                var fechaEntrega = row["CHECKOUT_TIME"].ToString();
                var observacion = row["CHECKOUT_COMMENT"].ToString();
                var seguimiento = row["ID"].ToString();
                var arrayComment = row["CHECKOUT_COMMENT"].ToString()?.Split("/");
                var quienRecibeNombre = string.Empty;
                var quienRecibeRut = string.Empty;
                //switch (arrayComment.Length)
                //{
                //    case 0:
                //        break;
                //    case 1:
                //        break;
                //    case 2:
                //        quienRecibeNombre = arrayComment[1];
                //        break;
                //    case 3:
                //        quienRecibeNombre = arrayComment[1];
                //        quienRecibeRut = arrayComment[2];
                //        break;
                //}
                var intentos = string.Empty;
                var fechaIntentos = string.Empty;
                var etaIntentos = string.Empty;
                //var arrayNotes = row["NOTES"].ToString()?.Split("/");
                //switch (arrayNotes.Length)
                //{
                //    case 0:
                //        break;
                //    case 1:
                //        break;
                //    case 2:
                //        intentos = arrayNotes[1];
                //        break;
                //    case 3:
                //        intentos = arrayNotes[1];
                //        fechaIntentos = arrayNotes[2];
                //        break;
                //    case 4:
                //        intentos = arrayNotes[1];
                //        fechaIntentos = arrayNotes[2];
                //        etaIntentos = arrayNotes[3];
                //        break;
                //}
                return new PayloadCliente()
                {
                    Folio = folio,
                    Origen = origen,
                    Destino = destino,
                    FechaRecepcion = fechaRecepcion,
                    EstadoEnvio = estadoEnvio,
                    FechaEnvio = fechaEnvio,
                    FechaEntrega = fechaEntrega,
                    Observacion = observacion,
                    Seguimiento = seguimiento,
                    QuienRecibeNombre = quienRecibeNombre,
                    QuienRecibeRut = quienRecibeRut,
                    Intentos = intentos,
                    FechaIntentos = fechaIntentos,
                    EtaIntentos = etaIntentos
                };
            }
            catch (Exception exception)
            {
                Log.Error(exception, "HA OCURRIDO UN ERROR AL CREAR UN OBJETO PARA EL CLIENTE");
                throw;
            }
        }
    }
}
