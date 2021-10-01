using System;
using System.Data;
using CargaBd.API.Models;
using Serilog;

namespace CargaBd.API.Logica 
{
    public static class CreaObjetos
    {
        public static PayloadRespuesta CreaObjetoAdmin(DataRow row)
        {
            var id = int.Parse(row["ID"].ToString() ?? string.Empty);
            try
            {
                return new PayloadRespuesta
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
                    reference = row["REFERENCE"].ToString().Replace("( prioridad )",string.Empty).Replace("(prioridad)",string.Empty),
                    vehicle = int.Parse(row["VEHICLE"].ToString()),
                    driver = int.TryParse(row["ORDER"].ToString(), out var driverParsed) ? driverParsed : null,
                    priority_level = int.TryParse(row["ORDER"].ToString(), out var plParsed) ? plParsed : null,
                    load = decimal.Parse(row["LOAD"].ToString()),
                    load_2 = decimal.Parse(row["LOAD_2"].ToString()),
                    load_3 = decimal.Parse(row["LOAD_3"].ToString()),
                    PesoPaquete = row["PESO_PAQUETE"].ToString(),
                    Precio = row["PRECIO"].ToString(),
                    TipoCobro = row["TIPO_COBRO"].ToString()
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
                var folio = row["REFERENCE"].ToString().Replace("( prioridad )", string.Empty).Replace("(prioridad)", string.Empty);
                var origen = "Santiago";
                var destino = row["ADDRESS"].ToString();
                var fechaRecepcion = row["PLANNED_DATE"].ToString() + " " + row["estimated_time_departure"].ToString();
                var estadoEnvio = row["STATUS"].ToString();
                var fechaEnvio = row["CHECKIN_TIME"].ToString();
                //var fechaEntrega = row["CHECKOUT_TIME"].ToString();
                var fechaEntrega = DateTime.TryParse(row["CHECKOUT_TIME"].ToString(),out var entregaParse) ? entregaParse.ToString("yyyy-MM-dd HH:mm:ss") : string.Empty;
                var observacion = row["CHECKOUT_COMMENT"].ToString();
                var seguimiento = row["ID"].ToString();
                var arrayComment = row["CHECKOUT_COMMENT"].ToString()?.Split("/");
                var quienRecibeNombre = string.Empty;
                var quienRecibeRut = string.Empty;
                var intentos = string.Empty;
                //var fechaIntentos = string.Empty;
                //var fechaIntentos = row["CHECKOUT_TIME"].ToString();
                var fechaIntentos = DateTime.TryParse(row["CHECKOUT_TIME"].ToString(), out var fechaIntentosParse) ? fechaIntentosParse.ToString("yyyy-MM-dd HH:mm:ss") : string.Empty;
                var etaIntentos = row["STATUS"].ToString();
                var pesoPaquete = row["PESO_PAQUETE"].ToString();
                var precio = row["PRECIO"].ToString();
                var tipoCobro = row["TIPO_COBRO"].ToString();
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
                    EstadoIntentos = etaIntentos,
                    PesoPaquete = pesoPaquete,
                    Precio = precio,
                    TipoCobro = tipoCobro
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
