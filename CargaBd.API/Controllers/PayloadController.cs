using System;
using System.Collections.Generic;
using System.Data;
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

        /// <summary>
        /// Inicia una carga desde una fecha de inicio hasta el dia de hoy, enviar la fecha actual para hacer una carga diaria
        /// </summary>
        /// <param name="FechaFin">Fecha en formato dd-MM-yyyy</param>
        /// <returns></returns>
        [HttpPost("CargaPipeline")]
        public async Task<IActionResult> CargarPipeline([FromBody]FechaDto Fecha)
        {
            if (!DateTime.TryParse(Fecha.FechaFin, out var TimeFixed))
                return BadRequest("El formato de la fecha no es compatible");
            if (DateTime.Compare(TimeFixed, DateTime.Today) > 0)
                return BadRequest("No se puede consultar al futuro ... todavía");
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Token", _config.GetValue<string>("Token"));
            //aqui va el While fecha sea menor a hoy
            var errores = 0;
            while (DateTime.Compare(TimeFixed, DateTime.Today) <= 0)
            {
                try
                {
                    var TimeFixedApi = TimeFixed.ToString("yyyy-MM-dd");
                    var result =
                        await httpClient.GetFromJsonAsync<List<PayloadDto>>(_config.GetValue<string>("Url") +
                                                                            TimeFixedApi);
                    if (result is null) return NotFound("No se han encontrado datos");
                    using (var connection = new SqlConnection(_config.GetConnectionString("conexion")))
                    {
                        foreach (var rustPayload in result)
                        {
                            #region Insertar Payload

                            var commandInsertPayload = new SqlCommand("InsertarPayload")
                            {
                                CommandType = CommandType.StoredProcedure,
                                Connection = connection,
                                Parameters =
                                {
                                    new SqlParameter
                                    {
                                        ParameterName = "@ID",
                                        SqlDbType = SqlDbType.Int,
                                        Direction = ParameterDirection.Input,
                                        Value = rustPayload.id,
                                    },
                                    new SqlParameter
                                    {
                                        ParameterName = "@ORDER",
                                        SqlDbType = SqlDbType.Int,
                                        Direction = ParameterDirection.Input,
                                        Value = rustPayload.order,
                                        IsNullable = true
                                    },
                                    new SqlParameter
                                    {
                                        ParameterName = "@TRACKING_ID",
                                        SqlDbType = SqlDbType.NVarChar,
                                        Direction = ParameterDirection.Input,
                                        Value = rustPayload.tracking_id,
                                        IsNullable = true
                                    },
                                    new SqlParameter
                                    {
                                        ParameterName = "@STATUS",
                                        SqlDbType = SqlDbType.NVarChar,
                                        Direction = ParameterDirection.Input,
                                        Value = rustPayload.status,
                                        IsNullable = true
                                    },
                                    new SqlParameter
                                    {
                                        ParameterName = "@TITLE",
                                        SqlDbType = SqlDbType.NVarChar,
                                        Direction = ParameterDirection.Input,
                                        Value = rustPayload.title,
                                        IsNullable = true
                                    },
                                    new SqlParameter
                                    {
                                        ParameterName = "@ADDRESS",
                                        SqlDbType = SqlDbType.NVarChar,
                                        Direction = ParameterDirection.Input,
                                        Value = rustPayload.address,
                                        IsNullable = true
                                    },
                                    new SqlParameter
                                    {
                                        ParameterName = "@LATITUDE",
                                        SqlDbType = SqlDbType.NVarChar,
                                        Direction = ParameterDirection.Input,
                                        Value = rustPayload.latitude,
                                        IsNullable = true
                                    },
                                    new SqlParameter
                                    {
                                        ParameterName = "@LONGITUDE",
                                        SqlDbType = SqlDbType.NVarChar,
                                        Direction = ParameterDirection.Input,
                                        Value = rustPayload.longitude,
                                        IsNullable = true
                                    },
                                    new SqlParameter
                                    {
                                        ParameterName = "@LOAD",
                                        SqlDbType = SqlDbType.Decimal,
                                        Direction = ParameterDirection.Input,
                                        Value = rustPayload.load,
                                        IsNullable = true
                                    },
                                    new SqlParameter
                                    {
                                        ParameterName = "@LOAD_2",
                                        SqlDbType = SqlDbType.Decimal,
                                        Direction = ParameterDirection.Input,
                                        Value = rustPayload.load_2,
                                        IsNullable = true
                                    },
                                    new SqlParameter
                                    {
                                        ParameterName = "@LOAD_3",
                                        SqlDbType = SqlDbType.Decimal,
                                        Direction = ParameterDirection.Input,
                                        Value = rustPayload.load_3,
                                        IsNullable = true
                                    },
                                    new SqlParameter
                                    {
                                        ParameterName = "@WINDOW_START",
                                        SqlDbType = SqlDbType.NVarChar,
                                        Direction = ParameterDirection.Input,
                                        Value = rustPayload.window_start,
                                        IsNullable = true
                                    },
                                    new SqlParameter
                                    {
                                        ParameterName = "@WINDOW_END",
                                        SqlDbType = SqlDbType.NVarChar,
                                        Direction = ParameterDirection.Input,
                                        Value = rustPayload.window_end,
                                        IsNullable = true
                                    },
                                    new SqlParameter
                                    {
                                        ParameterName = "@WINDOW_START_2",
                                        SqlDbType = SqlDbType.NVarChar,
                                        Direction = ParameterDirection.Input,
                                        Value = rustPayload.window_start_2,
                                        IsNullable = true
                                    },
                                    new SqlParameter
                                    {
                                        ParameterName = "@WINDOW_END_2",
                                        SqlDbType = SqlDbType.NVarChar,
                                        Direction = ParameterDirection.Input,
                                        Value = rustPayload.window_end_2,
                                        IsNullable = true
                                    },
                                    new SqlParameter
                                    {
                                        ParameterName = "@DURATION",
                                        SqlDbType = SqlDbType.NVarChar,
                                        Direction = ParameterDirection.Input,
                                        Value = rustPayload.duration,
                                        IsNullable = true
                                    },
                                    new SqlParameter
                                    {
                                        ParameterName = "@CONTACT_NAME",
                                        SqlDbType = SqlDbType.NVarChar,
                                        Direction = ParameterDirection.Input,
                                        Value = rustPayload.contact_name,
                                        IsNullable = true
                                    },
                                    new SqlParameter
                                    {
                                        ParameterName = "@CONTACT_PHONE",
                                        SqlDbType = SqlDbType.NVarChar,
                                        Direction = ParameterDirection.Input,
                                        Value = rustPayload.contact_phone,
                                        IsNullable = true
                                    },
                                    new SqlParameter
                                    {
                                        ParameterName = "@CONTACT_EMAIL",
                                        SqlDbType = SqlDbType.NVarChar,
                                        Direction = ParameterDirection.Input,
                                        Value = rustPayload.contact_email,
                                        IsNullable = true
                                    },
                                    new SqlParameter
                                    {
                                        ParameterName = "@REFERENCE",
                                        SqlDbType = SqlDbType.NVarChar,
                                        Direction = ParameterDirection.Input,
                                        Value = rustPayload.reference,
                                        IsNullable = true
                                    },
                                    new SqlParameter
                                    {
                                        ParameterName = "@NOTES",
                                        SqlDbType = SqlDbType.NVarChar,
                                        Direction = ParameterDirection.Input,
                                        Value = rustPayload.notes,
                                        IsNullable = true
                                    },
                                    new SqlParameter
                                    {
                                        ParameterName = "@PLANNED_DATE",
                                        SqlDbType = SqlDbType.NVarChar,
                                        Direction = ParameterDirection.Input,
                                        Value = rustPayload.planned_date,
                                        IsNullable = true
                                    },
                                    new SqlParameter
                                    {
                                        ParameterName = "@PROGRAMMED_DATE",
                                        SqlDbType = SqlDbType.NVarChar,
                                        Direction = ParameterDirection.Input,
                                        Value = rustPayload.programmed_date,
                                        IsNullable = true
                                    },
                                    new SqlParameter
                                    {
                                        ParameterName = "@ROUTE",
                                        SqlDbType = SqlDbType.NVarChar,
                                        Direction = ParameterDirection.Input,
                                        Value = rustPayload.route,
                                        IsNullable = true
                                    },
                                    new SqlParameter
                                    {
                                        ParameterName = "@ROUTE_ESTIMATED_TIME_START",
                                        SqlDbType = SqlDbType.NVarChar,
                                        Direction = ParameterDirection.Input,
                                        Value = rustPayload.route_estimated_time_start,
                                        IsNullable = true
                                    },
                                    new SqlParameter
                                    {
                                        ParameterName = "@ESTIMATED_TIME_ARRIVAL",
                                        SqlDbType = SqlDbType.NVarChar,
                                        Direction = ParameterDirection.Input,
                                        Value = rustPayload.estimated_time_arrival,
                                        IsNullable = true
                                    },
                                    new SqlParameter
                                    {
                                        ParameterName = "@ESTIMATED_TIME_DEPARTURE",
                                        SqlDbType = SqlDbType.NVarChar,
                                        Direction = ParameterDirection.Input,
                                        Value = rustPayload.estimated_time_departure,
                                        IsNullable = true
                                    },
                                    new SqlParameter
                                    {
                                        ParameterName = "@CHECKIN_TIME",
                                        SqlDbType = SqlDbType.NVarChar,
                                        Direction = ParameterDirection.Input,
                                        Value = rustPayload.checkin_time,
                                        IsNullable = true
                                    },
                                    new SqlParameter
                                    {
                                        ParameterName = "@CHECKOUT_TIME",
                                        SqlDbType = SqlDbType.NVarChar,
                                        Direction = ParameterDirection.Input,
                                        Value = rustPayload.checkout_time,
                                        IsNullable = true
                                    },
                                    new SqlParameter
                                    {
                                        ParameterName = "@CHECKOUT_LATITUDE",
                                        SqlDbType = SqlDbType.NVarChar,
                                        Direction = ParameterDirection.Input,
                                        Value = rustPayload.checkout_latitude,
                                        IsNullable = true
                                    },
                                    new SqlParameter
                                    {
                                        ParameterName = "@CHECKOUT_COMMENT",
                                        SqlDbType = SqlDbType.NVarChar,
                                        Direction = ParameterDirection.Input,
                                        Value = rustPayload.checkout_comment,
                                        IsNullable = true
                                    },
                                    new SqlParameter
                                    {
                                        ParameterName = "@CHECKOUT_LONGITUDE",
                                        SqlDbType = SqlDbType.NVarChar,
                                        Direction = ParameterDirection.Input,
                                        Value = rustPayload.checkout_longitude,
                                        IsNullable = true
                                    },
                                    new SqlParameter
                                    {
                                        ParameterName = "@CHECKOUT_OBSERVATION",
                                        SqlDbType = SqlDbType.NVarChar,
                                        Direction = ParameterDirection.Input,
                                        Value = rustPayload.checkout_observation,
                                        IsNullable = true
                                    },
                                    new SqlParameter
                                    {
                                        ParameterName = "@SIGNATURE",
                                        SqlDbType = SqlDbType.NVarChar,
                                        Direction = ParameterDirection.Input,
                                        Value = rustPayload.signature,
                                        IsNullable = true
                                    },
                                    new SqlParameter
                                    {
                                        ParameterName = "@CREATED",
                                        SqlDbType = SqlDbType.NVarChar,
                                        Direction = ParameterDirection.Input,
                                        Value = rustPayload.created,
                                        IsNullable = true
                                    },
                                    new SqlParameter
                                    {
                                        ParameterName = "@MODIFIED",
                                        SqlDbType = SqlDbType.NVarChar,
                                        Direction = ParameterDirection.Input,
                                        Value = rustPayload.modified,
                                        IsNullable = true
                                    },
                                    new SqlParameter
                                    {
                                        ParameterName = "@ETA_PREDICTED",
                                        SqlDbType = SqlDbType.NVarChar,
                                        Direction = ParameterDirection.Input,
                                        Value = rustPayload.eta_predicted,
                                        IsNullable = true
                                    },
                                    new SqlParameter
                                    {
                                        ParameterName = "@ETA_CURRENT",
                                        SqlDbType = SqlDbType.NVarChar,
                                        Direction = ParameterDirection.Input,
                                        Value = rustPayload.eta_current,
                                        IsNullable = true
                                    },
                                    new SqlParameter
                                    {
                                        ParameterName = "@DRIVER",
                                        SqlDbType = SqlDbType.Int,
                                        Direction = ParameterDirection.Input,
                                        Value = rustPayload.driver,
                                        IsNullable = true
                                    },
                                    new SqlParameter
                                    {
                                        ParameterName = "@VEHICLE",
                                        SqlDbType = SqlDbType.Int,
                                        Direction = ParameterDirection.Input,
                                        Value = rustPayload.vehicle,
                                        IsNullable = true
                                    },
                                    new SqlParameter
                                    {
                                        ParameterName = "@PRIORITY",
                                        SqlDbType = SqlDbType.Bit,
                                        Direction = ParameterDirection.Input,
                                        Value = rustPayload.priority,
                                        IsNullable = true
                                    },
                                    new SqlParameter
                                    {
                                        ParameterName = "@HAS_ALERT",
                                        SqlDbType = SqlDbType.Bit,
                                        Direction = ParameterDirection.Input,
                                        Value = rustPayload.has_alert,
                                        IsNullable = true
                                    },
                                    new SqlParameter
                                    {
                                        ParameterName = "@PRIORITY_LEVEL",
                                        SqlDbType = SqlDbType.Int,
                                        Direction = ParameterDirection.Input,
                                        Value = rustPayload.priority_level,
                                        IsNullable = true
                                    },
                                    new SqlParameter
                                    {
                                        ParameterName = "@EXTRA_FIELD_VALUES",
                                        SqlDbType = SqlDbType.NVarChar,
                                        Direction = ParameterDirection.Input,
                                        Value = rustPayload.extra_field_values,
                                        IsNullable = true
                                    },
                                    new SqlParameter
                                    {
                                        ParameterName = "@GEOCODE_ALERT",
                                        SqlDbType = SqlDbType.NVarChar,
                                        Direction = ParameterDirection.Input,
                                        Value = rustPayload.geocode_alert,
                                        IsNullable = true
                                    },
                                    new SqlParameter
                                    {
                                        ParameterName = "@VISIT_TYPE",
                                        SqlDbType = SqlDbType.NVarChar,
                                        Direction = ParameterDirection.Input,
                                        Value = rustPayload.visit_type,
                                        IsNullable = true
                                    },
                                    new SqlParameter
                                    {
                                        ParameterName = "@CURRENT_ETA",
                                        SqlDbType = SqlDbType.NVarChar,
                                        Direction = ParameterDirection.Input,
                                        Value = rustPayload.current_eta,
                                        IsNullable = true
                                    },
                                    new SqlParameter
                                    {
                                        ParameterName = "@FLEET",
                                        SqlDbType = SqlDbType.NVarChar,
                                        Direction = ParameterDirection.Input,
                                        Value = rustPayload.fleet,
                                        IsNullable = true
                                    },
                                }
                            };

                            

                            try
                            {
                                connection.Open();
                                commandInsertPayload.CommandTimeout = 120000;
                                commandInsertPayload.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {
                                errores++;
                                Console.Write(ex.Message);
                            }
                            finally
                            {
                                if (connection.State == ConnectionState.Open)
                                {
                                    connection.Close();
                                }
                            }
                            #endregion

                            #region Insertar Tags -- Por cada TAG en rustPayload se inserta

                            foreach (var tag in rustPayload.tags)
                            {
                                var commandInsertarTags = new SqlCommand("InsertarTags")
                                {
                                    CommandType = CommandType.StoredProcedure,
                                    Connection = connection,
                                    Parameters =
                                    {
                                        new SqlParameter
                                        {
                                            ParameterName = "@IdPayload",
                                            SqlDbType = SqlDbType.Int,
                                            Direction = ParameterDirection.Input,
                                            Value = rustPayload.id,
                                        },
                                        new SqlParameter
                                        {
                                            ParameterName = "@Tag",
                                            SqlDbType = SqlDbType.NVarChar,
                                            Direction = ParameterDirection.Input,
                                            Value = tag,
                                        }
                                    }
                                };
                                try
                                {
                                    connection.Open();
                                    commandInsertarTags.CommandTimeout = 120000;
                                    commandInsertarTags.ExecuteNonQuery();
                                }
                                catch (Exception ex)
                                {
                                    errores++;
                                    Console.Write(ex.Message);
                                }
                                finally
                                {
                                    if (connection.State == ConnectionState.Open)
                                    {
                                        connection.Close();
                                    }
                                }

                            }

                            ;

                            #endregion

                            #region Insertar Skills Required -- Por cada SR en rustPayload se inserta

                            foreach (var skillRequired in rustPayload.skills_required)
                            {
                                var commandInsertarSR = new SqlCommand("InsertarSkillsRequired")
                                {
                                    CommandType = CommandType.StoredProcedure,
                                    Connection = connection,
                                    Parameters =
                                    {
                                        new SqlParameter
                                        {
                                            ParameterName = "@IdPayload",
                                            SqlDbType = SqlDbType.Int,
                                            Direction = ParameterDirection.Input,
                                            Value = rustPayload.id,
                                        },
                                        new SqlParameter
                                        {
                                            ParameterName = "@NombreSR",
                                            SqlDbType = SqlDbType.NVarChar,
                                            Direction = ParameterDirection.Input,
                                            Value = skillRequired,
                                        }
                                    }
                                };
                                try
                                {
                                    connection.Open();
                                    commandInsertarSR.CommandTimeout = 120000;
                                    commandInsertarSR.ExecuteNonQuery();
                                }
                                catch (Exception ex)
                                {
                                    errores++;
                                    Console.Write(ex.Message);
                                }
                                finally
                                {
                                    if (connection.State == ConnectionState.Open)
                                    {
                                        connection.Close();
                                    }
                                }

                            }

                            ;

                            #endregion

                            #region Insertar Skills Optional -- Por cada SO en rustPayload se inserta

                            foreach (var skillOptional in rustPayload.skills_optional)
                            {
                                var commandInsertarSO = new SqlCommand("InsertarSkillsOptional")
                                {
                                    CommandType = CommandType.StoredProcedure,
                                    Connection = connection,
                                    Parameters =
                                    {
                                        new SqlParameter
                                        {
                                            ParameterName = "@IdPayload",
                                            SqlDbType = SqlDbType.Int,
                                            Direction = ParameterDirection.Input,
                                            Value = rustPayload.id,
                                        },
                                        new SqlParameter
                                        {
                                            ParameterName = "@NombreSO",
                                            SqlDbType = SqlDbType.NVarChar,
                                            Direction = ParameterDirection.Input,
                                            Value = skillOptional,
                                        }
                                    }
                                };
                                try
                                {
                                    connection.Open();
                                    commandInsertarSO.CommandTimeout = 120000;
                                    commandInsertarSO.ExecuteNonQuery();
                                }
                                catch (Exception ex)
                                {
                                    errores++;
                                    Console.Write(ex.Message);
                                }
                                finally
                                {
                                    if (connection.State == ConnectionState.Open)
                                    {
                                        connection.Close();
                                    }
                                }

                            }

                            ;

                            #endregion

                            #region Insertar Pictures -- Por cada picture en rustPyload se inserta

                            foreach (var picture in rustPayload.pictures)
                            {
                                var commandInsertarPicture = new SqlCommand("InsertarPicture")
                                {
                                    CommandType = CommandType.StoredProcedure,
                                    Connection = connection,
                                    Parameters =
                                    {
                                        new SqlParameter
                                        {
                                            ParameterName = "@IdPayload",
                                            SqlDbType = SqlDbType.Int,
                                            Direction = ParameterDirection.Input,
                                            Value = rustPayload.id,
                                        },
                                        new SqlParameter
                                        {
                                            ParameterName = "@UrlPicture",
                                            SqlDbType = SqlDbType.NVarChar,
                                            Direction = ParameterDirection.Input,
                                            Value = picture,
                                        }
                                    }
                                };
                                try
                                {
                                    connection.Open();
                                    commandInsertarPicture.CommandTimeout = 120000;
                                    commandInsertarPicture.ExecuteNonQuery();
                                }
                                catch (Exception ex)
                                {
                                    errores++;
                                    Console.Write(ex.Message);
                                }
                                finally
                                {
                                    if (connection.State == ConnectionState.Open)
                                    {
                                        connection.Close();
                                    }
                                }

                            }

                            ;

                            #endregion
                        }
                    }

                    TimeFixed = TimeFixed.AddDays(1);
                }
                catch(Exception errorException)
                {
                    Console.WriteLine("Ha ocurrido un error con "+errorException.Message);
                    TimeFixed = TimeFixed.AddDays(1);
                }
            }

            //fin while
            return errores == 0 ? Ok() : Ok("Se ha completado exitosamente, pero han ocurrido errores. Verificar log");
        }

        [HttpPost("NumeroOrden")]
        public async Task<IActionResult> ObtenerPayloadPorNumeroDeOrden([FromBody]NumeroOrdenDto request)
        {
            if (string.IsNullOrEmpty(request.NumeroOrden.ToString())) return BadRequest("El numero de orden no puede ir vácio.");
            if (string.IsNullOrEmpty(request.Usuario)) return BadRequest("El usuario no puede ir vacio.");

            await using (var connection = new SqlConnection(_config.GetConnectionString("conexion")))
            {
                var nameUser = string.Empty;
                //OBTENER USUARIO
                var commandObtenerUsuario = new SqlCommand("ObtenerUsuario")
                {
                    CommandType = CommandType.StoredProcedure,
                    Connection = connection,
                    Parameters =
                    {
                        new SqlParameter{
                            ParameterName = "@hash",
                            SqlDbType = SqlDbType.NVarChar,
                            Direction = ParameterDirection.Input,
                            Value = request.Usuario
                        }
                    }
                };
                try
                {
                    connection.Open();
                    var tablaResultUsuario = new DataTable();
                    commandObtenerUsuario.CommandTimeout = 120000;
                    var dataAdapterUser = new SqlDataAdapter(commandObtenerUsuario);
                    dataAdapterUser.Fill(tablaResultUsuario);
                    if (tablaResultUsuario.Rows.Count <= 0)
                        return NotFound("El usuario no existe o no se encuentra activo.");
                    nameUser = tablaResultUsuario.Rows[0]["NOMBRE_USUARIO"].ToString();
                }
                catch (Exception exception)
                {
                    Console.WriteLine($"Ha ocurrido un error al ejecutar el procedure del usuario con mensaje {exception.Message}");
                    return BadRequest("Ha ocurrido un error interno");
                }

                var commandObtenerData = new SqlCommand("ObtenerPayloadSegunTrack")
                {
                    CommandType = CommandType.StoredProcedure,
                    Connection = connection,
                    Parameters =
                    {
                        new SqlParameter{
                            ParameterName = "@trackId",
                            SqlDbType = SqlDbType.NVarChar,
                            Direction = ParameterDirection.Input,
                            Value = request.NumeroOrden.ToString()
                        },new SqlParameter{
                            ParameterName = "@usuario",
                            SqlDbType = SqlDbType.NVarChar,
                            Direction = ParameterDirection.Input,
                            Value = nameUser
                        }
                    }
                };
                try
                {
                    //connection.Open();
                    var tablaResult = new DataTable();
                    commandObtenerData.CommandTimeout = 120000;
                    var dataAdapter = new SqlDataAdapter(commandObtenerData);
                    dataAdapter.Fill(tablaResult);
                    if (tablaResult.Rows.Count <= 0)
                        return NotFound("No se han encontrado datos en relación a los parametros de búsqueda.");
                    var dtoRespuesta = new PayloadDto();
                    foreach (DataRow row in tablaResult.Rows)
                    {
                        var idPayload = int.Parse(row["ID"].ToString());
                        dtoRespuesta = new PayloadDto
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
                            has_alert = row["HAS_ALERT"].ToString().Equals("true"),
                            latitude = row["LATITUDE"].ToString(),
                            longitude = row["LONGITUDE"].ToString(),
                            modified = row["MODIFIED"].ToString(),
                            notes = row["NOTES"].ToString(),
                            planned_date = row["PLANNED_DATE"].ToString(),
                            priority = row["PRIORITY"].ToString().Equals("true"),
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
                        #region Cargar TAGS
                        var commandObtenerTags = new SqlCommand("ObtenerTagsSegunId")
                        {
                            CommandType = CommandType.StoredProcedure,
                            Connection = connection,
                            Parameters =
                            {
                                new SqlParameter{
                                    ParameterName = "@id",
                                    SqlDbType = SqlDbType.Int,
                                    Direction = ParameterDirection.Input,
                                    Value = idPayload
                                }
                            }
                        };
                        var dataAdapterTags = new SqlDataAdapter(commandObtenerTags);
                        var tablaTags = new DataTable();
                        dataAdapterTags.Fill(tablaTags);
                        dtoRespuesta.tags = new string[tablaTags.Rows.Count];
                        var iteradorTags = 0;
                        foreach (DataRow tags in tablaTags.Rows)
                        {
                            dtoRespuesta.tags[iteradorTags] = tags["NAMETAG"].ToString();
                            iteradorTags++;
                        }
                        #endregion
                        #region Cargar Pictures
                        var commandObtenerPictures = new SqlCommand("ObtenerPicturesSegunId")
                        {
                            CommandType = CommandType.StoredProcedure,
                            Connection = connection,
                            Parameters =
                            {
                                new SqlParameter{
                                    ParameterName = "@id",
                                    SqlDbType = SqlDbType.Int,
                                    Direction = ParameterDirection.Input,
                                    Value = idPayload
                                }
                            }
                        };
                        var dataAdapterPictures = new SqlDataAdapter(commandObtenerPictures);
                        var tablaPictures = new DataTable();
                        dataAdapterPictures.Fill(tablaPictures);
                        dtoRespuesta.pictures = new string[tablaPictures.Rows.Count];
                        var iteradorPictures = 0;
                        foreach (DataRow pictures in tablaPictures.Rows)
                        {
                            dtoRespuesta.pictures[iteradorPictures] = pictures["URLPICTURE"].ToString();
                            iteradorPictures++;
                        }
                        #endregion
                        #region Cargar SkillsRequired
                        var commandObtenerSR = new SqlCommand("ObtenerSkillsRequiredSegunId")
                        {
                            CommandType = CommandType.StoredProcedure,
                            Connection = connection,
                            Parameters =
                            {
                                new SqlParameter{
                                    ParameterName = "@id",
                                    SqlDbType = SqlDbType.Int,
                                    Direction = ParameterDirection.Input,
                                    Value = idPayload
                                }
                            }
                        };
                        var dataAdapterSR = new SqlDataAdapter(commandObtenerSR);
                        var tablaSR = new DataTable();
                        dataAdapterSR.Fill(tablaSR);
                        dtoRespuesta.skills_required = new int[tablaSR.Rows.Count];
                        var iteradorSR = 0;
                        foreach (DataRow skillsRequired in tablaSR.Rows)
                        {
                            dtoRespuesta.skills_required[iteradorSR] = int.Parse(skillsRequired["NAMESR"].ToString());
                            iteradorSR++;
                        }
                        #endregion
                        #region Cargar SkillsOptionals
                        var commandObtenerSO = new SqlCommand("ObtenerSkillsOptionalsSegunId")
                        {
                            CommandType = CommandType.StoredProcedure,
                            Connection = connection,
                            Parameters =
                            {
                                new SqlParameter{
                                    ParameterName = "@id",
                                    SqlDbType = SqlDbType.Int,
                                    Direction = ParameterDirection.Input,
                                    Value = idPayload
                                }
                            }
                        };
                        var dataAdapterSO = new SqlDataAdapter(commandObtenerSO);
                        var tablaSO = new DataTable();
                        dataAdapterSO.Fill(tablaSO);
                        dtoRespuesta.skills_optional = new int[tablaSR.Rows.Count];
                        var iteradorSO = 0;
                        foreach (DataRow skillsOptional in tablaSR.Rows)
                        {
                            dtoRespuesta.skills_optional[iteradorSO] = int.Parse(skillsOptional["NAMESO"].ToString());
                            iteradorSO++;
                        }
                        #endregion
                    }
                    return Ok(dtoRespuesta);
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                    return Problem(ex.Message);
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }
        }

        [HttpPost("BusquedaMasiva")]
        public async Task<IActionResult> ObtenerPayloadPorFecha([FromBody]BusquedaMasivaDto request)
        {
            if (string.IsNullOrEmpty(request.Usuario)) return BadRequest("El usuario no puede ir vacio.");
            if (string.IsNullOrEmpty(request.FechaDesde) || string.IsNullOrEmpty(request.FechaHasta)) return BadRequest("Los parametros no puede ir vacios.");
            if (!DateTime.TryParse(request.FechaDesde, out var TimeFixedDesde) || !DateTime.TryParse(request.FechaHasta, out var TimeFixedHasta))
                return BadRequest("El formato de la fecha no es compatible");
            if (DateTime.Compare(TimeFixedHasta, TimeFixedDesde) < 0)
                return BadRequest("Los rangos de fecha están cambiados. Limite 'hasta' es menor a la fecha 'desde'");
            var TimeFixedDesdeDb = TimeFixedDesde.ToString("yyyy-MM-dd");
            var TimeFixedHastaDb = TimeFixedHasta.ToString("yyyy-MM-dd");
            await using (var connection = new SqlConnection(_config.GetConnectionString("conexion")))
            {
                var nameUser = string.Empty;
                //OBTENER USUARIO
                var commandObtenerUsuario = new SqlCommand("ObtenerUsuario")
                {
                    CommandType = CommandType.StoredProcedure,
                    Connection = connection,
                    Parameters =
                    {
                        new SqlParameter{
                            ParameterName = "@hash",
                            SqlDbType = SqlDbType.NVarChar,
                            Direction = ParameterDirection.Input,
                            Value = request.Usuario
                        }
                    }
                };
                try
                {
                    connection.Open();
                    var tablaResultUsuario = new DataTable();
                    commandObtenerUsuario.CommandTimeout = 120000;
                    var dataAdapterUser = new SqlDataAdapter(commandObtenerUsuario);
                    dataAdapterUser.Fill(tablaResultUsuario);
                    if (tablaResultUsuario.Rows.Count <= 0)
                        return NotFound("El usuario no existe o no se encuentra activo.");
                    nameUser = tablaResultUsuario.Rows[0]["NOMBRE_USUARIO"].ToString();
                }
                catch (Exception exception)
                {
                    Console.WriteLine($"Ha ocurrido un error al ejecutar el procedure del usuario con mensaje {exception.Message}");
                    return BadRequest("Ha ocurrido un error interno");
                }

                var commandObtenerData = new SqlCommand("ObtenerPayloadEntreFechas")
                {
                    CommandType = CommandType.StoredProcedure,
                    Connection = connection,
                    Parameters =
                    {
                        new SqlParameter{
                            ParameterName = "@fechaDesde",
                            SqlDbType = SqlDbType.NVarChar,
                            Direction = ParameterDirection.Input,
                            Value = TimeFixedDesdeDb
                        },new SqlParameter{
                            ParameterName = "@fechaHasta",
                            SqlDbType = SqlDbType.NVarChar,
                            Direction = ParameterDirection.Input,
                            Value = TimeFixedHastaDb
                        },new SqlParameter{
                            ParameterName = "@usuario",
                            SqlDbType = SqlDbType.NVarChar,
                            Direction = ParameterDirection.Input,
                            Value = nameUser
                        }
                    }
                };
                var idActual = 0;
                try
                {
                    //connection.Open();
                    var tablaResult = new DataTable();
                    commandObtenerData.CommandTimeout = 120000;
                    var dataAdapter = new SqlDataAdapter(commandObtenerData);
                    dataAdapter.Fill(tablaResult);
                    if (tablaResult.Rows.Count <= 0)
                        return NotFound("No se han encontrado datos en relación a los parametros de búsqueda.");
                    var listaPayloads = new List<PayloadDto>();
                    foreach (DataRow row in tablaResult.Rows)
                    {
                        var dtoRespuesta = new PayloadDto();
                        var idPayload = int.Parse(row["ID"].ToString());
                        idActual = int.Parse(row["ID"].ToString());
                        if (idActual == 70475274)
                        {
                            Console.WriteLine("Error");
                        }
                        //var id = int.Parse(row["ID"].ToString() ?? string.Empty);
                        //int? order = int.TryParse(row["ORDER"].ToString(), out var orderParsed) ? orderParsed : null;
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
                        var has_alert = row["HAS_ALERT"].ToString().Equals("true");
                        var latitude = row["LATITUDE"].ToString();
                        var longitude = row["LONGITUDE"].ToString();
                        var modified = row["MODIFIED"].ToString();
                        var notes = row["NOTES"].ToString();
                        var planned_date = row["PLANNED_DATE"].ToString();
                        var priority = row["PRIORITY"].ToString().Equals("true");
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
                        var vehicle = int.Parse(row["VEHICLE"].ToString());
                        //var driver = int.Parse(row["DRIVER"].ToString() ?? string.Empty);
                        //var priority_level = int.Parse(row["PRIORITY_LEVEL"].ToString() ?? string.Empty);
                        var load = decimal.Parse(row["LOAD"].ToString());
                        var load_2 = decimal.Parse(row["LOAD_2"].ToString());
                        var load_3 = decimal.Parse(row["LOAD_3"].ToString());
                        dtoRespuesta = new PayloadDto
                        {
                            id = int.Parse(row["ID"].ToString() ?? null),
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
                            extra_field_values = row["EXTRA_FIELD_VALUES"].ToString(),
                            fleet = row["FLEET"].ToString(),
                            geocode_alert = row["GEOCODE_ALERT"].ToString(),
                            has_alert = row["HAS_ALERT"].ToString().Equals("true"),
                            latitude = row["LATITUDE"].ToString(),
                            longitude = row["LONGITUDE"].ToString(),
                            modified = row["MODIFIED"].ToString(),
                            notes = row["NOTES"].ToString(),
                            planned_date = row["PLANNED_DATE"].ToString(),
                            priority = row["PRIORITY"].ToString().Equals("true"),
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
                            driver = int.TryParse(row["DRIVER"].ToString(), out var driverParsed) ? driverParsed : null,
                            priority_level = int.TryParse(row["PRIORITY_LEVEL"].ToString(), out var plParsed) ? plParsed : null,
                            load = decimal.Parse(row["LOAD"].ToString()),
                            load_2 = decimal.Parse(row["LOAD_2"].ToString()),
                            load_3 = decimal.Parse(row["LOAD_3"].ToString()),
                        };
                        
                        #region Cargar TAGS
                        var commandObtenerTags = new SqlCommand("ObtenerTagsSegunId")
                        {
                            CommandType = CommandType.StoredProcedure,
                            Connection = connection,
                            Parameters =
                            {
                                new SqlParameter{
                                    ParameterName = "@id",
                                    SqlDbType = SqlDbType.Int,
                                    Direction = ParameterDirection.Input,
                                    Value = idPayload
                                }
                            }
                        };
                        var dataAdapterTags = new SqlDataAdapter(commandObtenerTags);
                        var tablaTags = new DataTable();
                        dataAdapterTags.Fill(tablaTags);
                        dtoRespuesta.tags = new string[tablaTags.Rows.Count];
                        var iteradorTags = 0;
                        foreach (DataRow tags in tablaTags.Rows)
                        {
                            dtoRespuesta.tags[iteradorTags] = tags["NAMETAG"].ToString();
                            iteradorTags++;
                        }
                        #endregion
                        
                        #region Cargar Pictures
                        var commandObtenerPictures = new SqlCommand("ObtenerPicturesSegunId")
                        {
                            CommandType = CommandType.StoredProcedure,
                            Connection = connection,
                            Parameters =
                            {
                                new SqlParameter{
                                    ParameterName = "@id",
                                    SqlDbType = SqlDbType.Int,
                                    Direction = ParameterDirection.Input,
                                    Value = idPayload
                                }
                            }
                        };
                        var dataAdapterPictures = new SqlDataAdapter(commandObtenerPictures);
                        var tablaPictures = new DataTable();
                        dataAdapterPictures.Fill(tablaPictures);
                        dtoRespuesta.pictures = new string[tablaPictures.Rows.Count];
                        var iteradorPictures = 0;
                        foreach (DataRow pictures in tablaPictures.Rows)
                        {
                            dtoRespuesta.pictures[iteradorPictures] = pictures["URLPICTURE"].ToString();
                            iteradorPictures++;
                        }
                        #endregion
                        
                        #region Cargar SkillsRequired
                        var commandObtenerSR = new SqlCommand("ObtenerSkillsRequiredSegunId")
                        {
                            CommandType = CommandType.StoredProcedure,
                            Connection = connection,
                            Parameters =
                            {
                                new SqlParameter{
                                    ParameterName = "@id",
                                    SqlDbType = SqlDbType.Int,
                                    Direction = ParameterDirection.Input,
                                    Value = idPayload
                                }
                            }
                        };
                        var dataAdapterSR = new SqlDataAdapter(commandObtenerSR);
                        var tablaSR = new DataTable();
                        dataAdapterSR.Fill(tablaSR);
                        dtoRespuesta.skills_required = new int[tablaSR.Rows.Count];
                        var iteradorSR = 0;
                        foreach (DataRow skillsRequired in tablaSR.Rows)
                        {
                            dtoRespuesta.skills_required[iteradorSR] = int.Parse(skillsRequired["NAMESR"].ToString());
                            iteradorSR++;
                        }
                        #endregion
                        
                        #region Cargar SkillsOptionals
                        var commandObtenerSO = new SqlCommand("ObtenerSkillsOptionalsSegunId")
                        {
                            CommandType = CommandType.StoredProcedure,
                            Connection = connection,
                            Parameters =
                            {
                                new SqlParameter{
                                    ParameterName = "@id",
                                    SqlDbType = SqlDbType.Int,
                                    Direction = ParameterDirection.Input,
                                    Value = idPayload
                                }
                            }
                        };
                        var dataAdapterSO = new SqlDataAdapter(commandObtenerSO);
                        var tablaSO = new DataTable();
                        dataAdapterSO.Fill(tablaSO);
                        dtoRespuesta.skills_optional = new int[tablaSR.Rows.Count];
                        var iteradorSO = 0;
                        foreach (DataRow skillsOptional in tablaSO.Rows)
                        {
                            dtoRespuesta.skills_optional[iteradorSO] = int.Parse(skillsOptional["NAMESO"].ToString());
                            iteradorSO++;
                        }
                        #endregion
                        
                        listaPayloads.Add(dtoRespuesta);
                    }
                    return (listaPayloads.Count > 0) ? Ok(listaPayloads) : NotFound("No existe data para los filtros ingresados");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(idActual);
                    return Problem(ex.Message);
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }
        }

        [HttpPost("Referencia")]
        public async Task<IActionResult> ObtenerPayloadPorReference([FromBody]ReferenciaDto request)
        {
            if (string.IsNullOrEmpty(request.Usuario)) return BadRequest("El usuario no puede ir vacio.");
            if (string.IsNullOrEmpty(request.Referencia)) return BadRequest("La referencia no puede ser nula");
            await using var connection = new SqlConnection(_config.GetConnectionString("conexion"));
            var nameUser = string.Empty;
            //OBTENER USUARIO
            var commandObtenerUsuario = new SqlCommand("ObtenerUsuario")
            {
                CommandType = CommandType.StoredProcedure,
                Connection = connection,
                Parameters =
                {
                    new SqlParameter{
                        ParameterName = "@hash",
                        SqlDbType = SqlDbType.NVarChar,
                        Direction = ParameterDirection.Input,
                        Value = request.Usuario
                    }
                }
            };
            try
            {
                connection.Open();
                var tablaResultUsuario = new DataTable();
                commandObtenerUsuario.CommandTimeout = 120000;
                var dataAdapterUser = new SqlDataAdapter(commandObtenerUsuario);
                dataAdapterUser.Fill(tablaResultUsuario);
                if (tablaResultUsuario.Rows.Count <= 0)
                    return NotFound("El usuario no existe o no se encuentra activo.");
                nameUser = tablaResultUsuario.Rows[0]["NOMBRE_USUARIO"].ToString();
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Ha ocurrido un error al ejecutar el procedure del usuario con mensaje {exception.Message}");
                return BadRequest("Ha ocurrido un error interno");
            }
            var commandObtenerData = new SqlCommand("ObtenerPayloadPorReferencia")
            {
                CommandType = CommandType.StoredProcedure,
                Connection = connection,
                Parameters =
                {
                    new SqlParameter{
                        ParameterName = "@reference",
                        SqlDbType = SqlDbType.NVarChar,
                        Direction = ParameterDirection.Input,
                        Value = request.Referencia
                    }, new SqlParameter{
                        ParameterName = "@usuario",
                        SqlDbType = SqlDbType.NVarChar,
                        Direction = ParameterDirection.Input,
                        Value = nameUser
                    }
                }
            };
            try
            {
                //connection.Open();
                var tablaResult = new DataTable();
                commandObtenerData.CommandTimeout = 120000;
                var dataAdapter = new SqlDataAdapter(commandObtenerData);
                dataAdapter.Fill(tablaResult);
                if (tablaResult.Rows.Count <= 0)
                    return NotFound("No se han encontrado datos en relación a los parametros de búsqueda.");
                var listaPayloads = new List<PayloadDto>();
                foreach (DataRow row in tablaResult.Rows)
                {
                    var dtoRespuesta = new PayloadDto();
                    var idPayload = int.Parse(row["ID"].ToString());

                    dtoRespuesta = new PayloadDto
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
                        has_alert = row["HAS_ALERT"].ToString().Equals("true"),
                        latitude = row["LATITUDE"].ToString(),
                        longitude = row["LONGITUDE"].ToString(),
                        modified = row["MODIFIED"].ToString(),
                        notes = row["NOTES"].ToString(),
                        planned_date = row["PLANNED_DATE"].ToString(),
                        priority = row["PRIORITY"].ToString().Equals("true"),
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

                    #region Cargar TAGS
                    var commandObtenerTags = new SqlCommand("ObtenerTagsSegunId")
                    {
                        CommandType = CommandType.StoredProcedure,
                        Connection = connection,
                        Parameters =
                        {
                            new SqlParameter{
                                ParameterName = "@id",
                                SqlDbType = SqlDbType.Int,
                                Direction = ParameterDirection.Input,
                                Value = idPayload
                            }
                        }
                    };
                    var dataAdapterTags = new SqlDataAdapter(commandObtenerTags);
                    var tablaTags = new DataTable();
                    dataAdapterTags.Fill(tablaTags);
                    dtoRespuesta.tags = new string[tablaTags.Rows.Count];
                    var iteradorTags = 0;
                    foreach (DataRow tags in tablaTags.Rows)
                    {
                        dtoRespuesta.tags[iteradorTags] = tags["NAMETAG"].ToString();
                        iteradorTags++;
                    }
                    #endregion

                    #region Cargar Pictures
                    var commandObtenerPictures = new SqlCommand("ObtenerPicturesSegunId")
                    {
                        CommandType = CommandType.StoredProcedure,
                        Connection = connection,
                        Parameters =
                        {
                            new SqlParameter{
                                ParameterName = "@id",
                                SqlDbType = SqlDbType.Int,
                                Direction = ParameterDirection.Input,
                                Value = idPayload
                            }
                        }
                    };
                    var dataAdapterPictures = new SqlDataAdapter(commandObtenerPictures);
                    var tablaPictures = new DataTable();
                    dataAdapterPictures.Fill(tablaPictures);
                    dtoRespuesta.pictures = new string[tablaPictures.Rows.Count];
                    var iteradorPictures = 0;
                    foreach (DataRow pictures in tablaPictures.Rows)
                    {
                        dtoRespuesta.pictures[iteradorPictures] = pictures["URLPICTURE"].ToString();
                        iteradorPictures++;
                    }
                    #endregion

                    #region Cargar SkillsRequired
                    var commandObtenerSR = new SqlCommand("ObtenerSkillsRequiredSegunId")
                    {
                        CommandType = CommandType.StoredProcedure,
                        Connection = connection,
                        Parameters =
                        {
                            new SqlParameter{
                                ParameterName = "@id",
                                SqlDbType = SqlDbType.Int,
                                Direction = ParameterDirection.Input,
                                Value = idPayload
                            }
                        }
                    };
                    var dataAdapterSR = new SqlDataAdapter(commandObtenerSR);
                    var tablaSR = new DataTable();
                    dataAdapterSR.Fill(tablaSR);
                    dtoRespuesta.skills_required = new int[tablaSR.Rows.Count];
                    var iteradorSR = 0;
                    foreach (DataRow skillsRequired in tablaSR.Rows)
                    {
                        dtoRespuesta.skills_required[iteradorSR] = int.Parse(skillsRequired["NAMESR"].ToString());
                        iteradorSR++;
                    }
                    #endregion

                    #region Cargar SkillsOptionals
                    var commandObtenerSO = new SqlCommand("ObtenerSkillsOptionalsSegunId")
                    {
                        CommandType = CommandType.StoredProcedure,
                        Connection = connection,
                        Parameters =
                        {
                            new SqlParameter{
                                ParameterName = "@id",
                                SqlDbType = SqlDbType.Int,
                                Direction = ParameterDirection.Input,
                                Value = idPayload
                            }
                        }
                    };
                    var dataAdapterSO = new SqlDataAdapter(commandObtenerSO);
                    var tablaSO = new DataTable();
                    dataAdapterSO.Fill(tablaSO);
                    dtoRespuesta.skills_optional = new int[tablaSR.Rows.Count];
                    var iteradorSO = 0;
                    foreach (DataRow skillsOptional in tablaSO.Rows)
                    {
                        dtoRespuesta.skills_optional[iteradorSO] = int.Parse(skillsOptional["NAMESO"].ToString());
                        iteradorSO++;
                    }
                    #endregion

                    listaPayloads.Add(dtoRespuesta);
                }
                return (listaPayloads.Count > 0) ? Ok(listaPayloads) : NotFound("No existe data para los filtros ingresados");
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        [HttpPost("Masivo")]
        public async Task<IActionResult> ObtenerPayloadPorFechaReferencia([FromBody]MasivaDto request)
        {
            if (string.IsNullOrEmpty(request.Usuario)) return BadRequest("El usuario no puede ir vacio.");
            if (string.IsNullOrEmpty(request.FechaDesde) && string.IsNullOrEmpty(request.FechaHasta) &&
                string.IsNullOrEmpty(request.Referencia))
                return BadRequest("Los parametros no pueden ir en vacio.");
            var TimeFixedDesdeDb = string.Empty;
            var TimeFixedHastaDb = string.Empty;
            if (!request.FechaDesde.Contains('-') && !request.FechaHasta.Contains('-'))
            {
                if (!DateTime.TryParse(request.FechaDesde, out var TimeFixedDesde) ||
                    !DateTime.TryParse(request.FechaHasta, out var TimeFixedHasta))
                    return BadRequest("El formato de la fecha no es compatible");
                if (DateTime.Compare(TimeFixedHasta, TimeFixedDesde) < 0)
                    return BadRequest("Los rangos de fecha están cambiados. Limite 'hasta' es menor a la fecha 'desde'");
                TimeFixedDesdeDb = TimeFixedDesde.ToString("yyyy-MM-dd");
                TimeFixedHastaDb = TimeFixedHasta.ToString("yyyy-MM-dd");
            }
            
            await using (var connection = new SqlConnection(_config.GetConnectionString("conexion")))
            {
                var nameUser = string.Empty;
                //OBTENER USUARIO
                var commandObtenerUsuario = new SqlCommand("ObtenerUsuario")
                {
                    CommandType = CommandType.StoredProcedure,
                    Connection = connection,
                    Parameters =
                    {
                        new SqlParameter{
                            ParameterName = "@hash",
                            SqlDbType = SqlDbType.NVarChar,
                            Direction = ParameterDirection.Input,
                            Value = request.Usuario
                        }
                    }
                };
                try
                {
                    connection.Open();
                    var tablaResultUsuario = new DataTable();
                    commandObtenerUsuario.CommandTimeout = 120000;
                    var dataAdapterUser = new SqlDataAdapter(commandObtenerUsuario);
                    dataAdapterUser.Fill(tablaResultUsuario);
                    if (tablaResultUsuario.Rows.Count <= 0)
                        return NotFound("El usuario no existe o no se encuentra activo.");
                    nameUser = tablaResultUsuario.Rows[0]["NOMBRE_USUARIO"].ToString();
                }
                catch (Exception exception)
                {
                    Console.WriteLine($"Ha ocurrido un error al ejecutar el procedure del usuario con mensaje {exception.Message}");
                    return BadRequest("Ha ocurrido un error interno");
                }
                var commandObtenerData = new SqlCommand("ObtenerPayloadEntreFechasYReferencia")
                {
                    CommandType = CommandType.StoredProcedure,
                    Connection = connection,
                    Parameters =
                    {
                        new SqlParameter{
                            ParameterName = "@fechaDesde",
                            SqlDbType = SqlDbType.NVarChar,
                            Direction = ParameterDirection.Input,
                            Value = TimeFixedDesdeDb
                        },new SqlParameter{
                            ParameterName = "@fechaHasta",
                            SqlDbType = SqlDbType.NVarChar,
                            Direction = ParameterDirection.Input,
                            Value = TimeFixedHastaDb
                        },new SqlParameter{
                            ParameterName = "@referencia",
                            SqlDbType = SqlDbType.NVarChar,
                            Direction = ParameterDirection.Input,
                            Value = TimeFixedHastaDb
                        },new SqlParameter{
                            ParameterName = "@usuario",
                            SqlDbType = SqlDbType.NVarChar,
                            Direction = ParameterDirection.Input,
                            Value = nameUser
                        }
                    }
                };
                try
                {
                    //connection.Open();
                    var tablaResult = new DataTable();
                    commandObtenerData.CommandTimeout = 120000;
                    var dataAdapter = new SqlDataAdapter(commandObtenerData);
                    dataAdapter.Fill(tablaResult);
                    if (tablaResult.Rows.Count <= 0)
                        return NotFound("No se han encontrado datos en relación a los parametros de búsqueda.");
                    var listaPayloads = new List<PayloadDto>();
                    foreach (DataRow row in tablaResult.Rows)
                    {
                        var dtoRespuesta = new PayloadDto();
                        var idPayload = int.Parse(row["ID"].ToString());

                        dtoRespuesta = new PayloadDto
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
                            has_alert = row["HAS_ALERT"].ToString().Equals("true"),
                            latitude = row["LATITUDE"].ToString(),
                            longitude = row["LONGITUDE"].ToString(),
                            modified = row["MODIFIED"].ToString(),
                            notes = row["NOTES"].ToString(),
                            planned_date = row["PLANNED_DATE"].ToString(),
                            priority = row["PRIORITY"].ToString().Equals("true"),
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

                        #region Cargar TAGS
                        var commandObtenerTags = new SqlCommand("ObtenerTagsSegunId")
                        {
                            CommandType = CommandType.StoredProcedure,
                            Connection = connection,
                            Parameters =
                            {
                                new SqlParameter{
                                    ParameterName = "@id",
                                    SqlDbType = SqlDbType.Int,
                                    Direction = ParameterDirection.Input,
                                    Value = idPayload
                                }
                            }
                        };
                        var dataAdapterTags = new SqlDataAdapter(commandObtenerTags);
                        var tablaTags = new DataTable();
                        dataAdapterTags.Fill(tablaTags);
                        dtoRespuesta.tags = new string[tablaTags.Rows.Count];
                        var iteradorTags = 0;
                        foreach (DataRow tags in tablaTags.Rows)
                        {
                            dtoRespuesta.tags[iteradorTags] = tags["NAMETAG"].ToString();
                            iteradorTags++;
                        }
                        #endregion

                        #region Cargar Pictures
                        var commandObtenerPictures = new SqlCommand("ObtenerPicturesSegunId")
                        {
                            CommandType = CommandType.StoredProcedure,
                            Connection = connection,
                            Parameters =
                            {
                                new SqlParameter{
                                    ParameterName = "@id",
                                    SqlDbType = SqlDbType.Int,
                                    Direction = ParameterDirection.Input,
                                    Value = idPayload
                                }
                            }
                        };
                        var dataAdapterPictures = new SqlDataAdapter(commandObtenerPictures);
                        var tablaPictures = new DataTable();
                        dataAdapterPictures.Fill(tablaPictures);
                        dtoRespuesta.pictures = new string[tablaPictures.Rows.Count];
                        var iteradorPictures = 0;
                        foreach (DataRow pictures in tablaPictures.Rows)
                        {
                            dtoRespuesta.pictures[iteradorPictures] = pictures["URLPICTURE"].ToString();
                            iteradorPictures++;
                        }
                        #endregion

                        #region Cargar SkillsRequired
                        var commandObtenerSR = new SqlCommand("ObtenerSkillsRequiredSegunId")
                        {
                            CommandType = CommandType.StoredProcedure,
                            Connection = connection,
                            Parameters =
                            {
                                new SqlParameter{
                                    ParameterName = "@id",
                                    SqlDbType = SqlDbType.Int,
                                    Direction = ParameterDirection.Input,
                                    Value = idPayload
                                }
                            }
                        };
                        var dataAdapterSR = new SqlDataAdapter(commandObtenerSR);
                        var tablaSR = new DataTable();
                        dataAdapterSR.Fill(tablaSR);
                        dtoRespuesta.skills_required = new int[tablaSR.Rows.Count];
                        var iteradorSR = 0;
                        foreach (DataRow skillsRequired in tablaSR.Rows)
                        {
                            dtoRespuesta.skills_required[iteradorSR] = int.Parse(skillsRequired["NAMESR"].ToString());
                            iteradorSR++;
                        }
                        #endregion

                        #region Cargar SkillsOptionals
                        var commandObtenerSO = new SqlCommand("ObtenerSkillsOptionalsSegunId")
                        {
                            CommandType = CommandType.StoredProcedure,
                            Connection = connection,
                            Parameters =
                            {
                                new SqlParameter{
                                    ParameterName = "@id",
                                    SqlDbType = SqlDbType.Int,
                                    Direction = ParameterDirection.Input,
                                    Value = idPayload
                                }
                            }
                        };
                        var dataAdapterSO = new SqlDataAdapter(commandObtenerSO);
                        var tablaSO = new DataTable();
                        dataAdapterSO.Fill(tablaSO);
                        dtoRespuesta.skills_optional = new int[tablaSR.Rows.Count];
                        var iteradorSO = 0;
                        foreach (DataRow skillsOptional in tablaSO.Rows)
                        {
                            dtoRespuesta.skills_optional[iteradorSO] = int.Parse(skillsOptional["NAMESO"].ToString());
                            iteradorSO++;
                        }
                        #endregion

                        listaPayloads.Add(dtoRespuesta);
                    }
                    return (listaPayloads.Count > 0) ? Ok(listaPayloads) : NotFound("No existe data para los filtros ingresados");
                }
                catch (Exception ex)
                {
                    return Problem(ex.Message);
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }

        }

    }
}
