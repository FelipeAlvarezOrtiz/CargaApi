using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
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
            if (!DateTime.TryParse(FechaFin, out var TimeFixed))
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

                            #endregion

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
                }
            }

            //fin while
            return errores == 0 ? Ok() : Ok("Se ha completado exitosamente, pero han ocurrido errores. Verificar log");
        }

        [HttpGet("{numeroOrden}")]
        public async Task<IActionResult> ObtenerPorNumeroDeOrden(string numeroOrden)
        {
            if (string.IsNullOrEmpty(numeroOrden)) return BadRequest();
            using (var connection = new SqlConnection(_config.GetConnectionString("conexion")))
            {
                var commandObtenerData = new SqlCommand("InsertarTags")
                {
                    CommandType = CommandType.StoredProcedure,
                    Connection = connection,
                    Parameters =
                    {
                        new SqlParameter{
                            ParameterName = "@numeroOrden",
                            SqlDbType = SqlDbType.NVarChar,
                            Direction = ParameterDirection.Input,
                            Value = numeroOrden
                        }
                    }
                };
                try
                {
                    connection.Open();
                    var tablaResult = new DataTable();
                    commandObtenerData.CommandTimeout = 120000;
                    commandObtenerData.ExecuteNonQuery();
                    
                }
                catch (Exception ex)
                {
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
            return NotFound();
        }
    }
}
