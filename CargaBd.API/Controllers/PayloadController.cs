using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CargaBd.API.Context;
using CargaBd.API.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace CargaBd.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayloadController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;

        public PayloadController(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpGet]
        public async Task<IActionResult> CargarPipeline(string FechaFin)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Token", _config.GetValue<string>("Token"));

            var result =
                await httpClient.GetFromJsonAsync<List<PayloadDto>>(
                    "https://api.simpliroute.com/v1/routes/visits/?planned_date=2021-08-09");
            if (result is null) return NotFound("No se han encontrado datos");

            using (var connection = new SqlConnection(_config.GetConnectionString("conexion")))
            {
                foreach (var rustPayload in result)
                {
                    var sqlQuery = @"INSERT INTO Payload(Id,Address,CheckinTime,CheckoutCommet,CheckoutLatitude) 
                                        VALUES(
                                        " + rustPayload.id + "," +
                                   " '" + rustPayload.address + "'," +
                                   " '" + rustPayload.checkin_time + "'," +
                                   " '" + rustPayload.checkout_comment + "'," +
                                   " '" + rustPayload.checkout_latitude + "'" +
                                   //" "+ rustPayload.checkout_longitude + "" +
                                   //" "+ rustPayload.checkout_observation + "" +
                                   //" "+ rustPayload.checkout_time + "" +
                                   //" "+ rustPayload.contact_email + "" +
                                   //" "+ rustPayload.contact_name + "" +
                                   //" "+ rustPayload.contact_phone + "" +
                                   //" "+ rustPayload.created + "" +
                                   //" "+ rustPayload.current_eta + "" +
                                   //" "+ rustPayload.driver + "" +
                                   //" "+ rustPayload.duration + "" +
                                   //" "+ rustPayload.estimated_time_arrival + "" +
                                   //" "+ rustPayload.estimated_time_departure + "" +
                                   //" "+ rustPayload.eta_current + "" +
                                   //" "+ rustPayload.eta_predicted + "" +
                                   //" "+ rustPayload.extra_field_values + "" +
                                   //" "+ rustPayload.fleet + "" +
                                   //" "+ rustPayload.geocode_alert + "" +
                                   //" "+ rustPayload.has_alert + "" +
                                   //" "+ rustPayload.latitude + "" +
                                   //" "+ rustPayload.load + "" +
                                   //" "+ rustPayload.load_2 + "" +
                                   //" "+ rustPayload.load_3 + "" +
                                   //" "+ rustPayload.longitude + "" +
                                   //" "+ rustPayload.modified + "" +
                                   //" "+ rustPayload.notes + "" +
                                   //" "+ rustPayload.order + "" +
                                   //" "+ rustPayload.planned_date + "" +
                                   //" "+ rustPayload.priority + "" +
                                   //" "+ rustPayload.priority_level + "" +
                                   //" "+ rustPayload.programmed_date + "" +
                                   //" "+ rustPayload.route + "" +
                                   //" "+ rustPayload.route_estimated_time_start + "" +
                                   //" "+ rustPayload.signature + "" +
                                   //" "+ rustPayload.status + "" +
                                   //" "+ rustPayload.title + "" +
                                   //" "+ rustPayload.tracking_id + "" +
                                   //" "+ rustPayload.vehicle + "" +
                                   //" "+ rustPayload.visit_type + "" +
                                   //" "+ rustPayload.window_end + "" +
                                   //" "+ rustPayload.window_end_2 + "" +
                                   //" "+ rustPayload.window_start + "" +
                                   //" "+ rustPayload.window_end_2 + "" +
                                   //" "+ rustPayload.window_start_2 + "" +
                                   ")";
                    using (var querySaveStaff = new SqlCommand(sqlQuery))
                    {
                        querySaveStaff.Connection = connection;
                        connection.Open();
                        querySaveStaff.ExecuteNonQuery();
                    };
                }
            }
            return Ok();
        }
        //foreach (var rustPayload in result)
        //{
        //    var listaPictures = rustPayload.pictures.Select(urlPicture => new Picture {UrlPicture = urlPicture}).ToList();
        //    //Validamos Pictures
        //    var listaTags = rustPayload.tags.Select(tag => new Tag {NombreTag = tag}).ToList();
        //    var listaSkillsRequired = rustPayload.skills_required
        //        .Select(skillsReq => new SkillRequired {NombreSkillRequired = skillsReq}).ToList();
        //    var listaSkillsOptional = rustPayload.skills_optional
        //        .Select(skillsOptional => new SkillOptional {NombreSkillOptional = skillsOptional}).ToList();

        //if (rustPayload.id != null)
        //    await _context.Payload.AddAsync(new Payload()
        //    {
        //        Id = (int) rustPayload.id,
        //        Pictures = listaPictures,
        //        Address = rustPayload.address,
        //        CheckinTime = rustPayload.checkin_time,
        //        CheckoutCommet = rustPayload.checkout_comment,
        //        CheckoutLatitude = rustPayload.checkout_latitude,
        //        CheckoutLongitude = rustPayload.checkout_longitude,
        //        CheckoutObservation = rustPayload.checkout_observation,
        //        CheckoutTime = rustPayload.checkout_time,
        //        ContactEmail = rustPayload.contact_email,
        //        ContactName = rustPayload.contact_name,
        //        ContactPhone = rustPayload.contact_phone,
        //        Created = rustPayload.created,
        //        CurrentEta = rustPayload.current_eta,
        //        Driver = (int) rustPayload.driver,
        //        Duration = rustPayload.duration,
        //        EstimatedTimeArrival = rustPayload.estimated_time_arrival,
        //        EstimatedTimeDeparture = rustPayload.estimated_time_departure,
        //        EtaCurrent = rustPayload.eta_current,
        //        EtaPredicted = rustPayload.eta_predicted,
        //        ExtraFieldValues = rustPayload.extra_field_values,
        //        Fleet = rustPayload.fleet,
        //        GeocodeAlert = rustPayload.geocode_alert,
        //        Has_alert = rustPayload.has_alert,
        //        Latitude = rustPayload.latitude,
        //        Load = rustPayload.load,
        //        Load2 = rustPayload.load_2,
        //        Load3 = rustPayload.load_3,
        //        Longitude = rustPayload.longitude,
        //        Modified = rustPayload.modified,
        //        Notes = rustPayload.notes,
        //        Order = rustPayload.order,
        //        PlannedDate = rustPayload.planned_date,
        //        Priority = rustPayload.priority,
        //        PriorityLevel = rustPayload.priority_level,
        //        ProgrammedDate = rustPayload.programmed_date,
        //        Reference = rustPayload.reference,
        //        Route = rustPayload.route,
        //        RouteEstimatedTimeStart = rustPayload.route_estimated_time_start,
        //        Signature = rustPayload.signature,
        //        SkillsOptional = listaSkillsOptional,
        //        SkillsRequired = listaSkillsRequired,
        //        Status = rustPayload.status,
        //        Tags = listaTags,
        //        Title = rustPayload.title,
        //        TrackingId = rustPayload.tracking_id,
        //        Vehicle = rustPayload.vehicle,
        //        VisitType = rustPayload.visit_type,
        //        WindowEnd = rustPayload.window_end,
        //        WindowEnd2 = rustPayload.window_end_2,
        //        WindowStart = rustPayload.window_start,
        //        WindowStart2 = rustPayload.window_start_2
        //    });
        //return await _context.SaveChangesAsync() > 0 ? Ok() : BadRequest();
        //    }


    }
}
