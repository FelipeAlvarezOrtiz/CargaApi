using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CargaBd.API.Logica;
using CargaBd.API.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Serilog;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Net.Mail;
using System.Net;
using Rectangle = iTextSharp.text.Rectangle;

namespace CargaBd.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayloadController : ControllerBase
    {
        private readonly IConfiguration _config;

        public PayloadController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost("CargaPipeline")]
        public async Task<IActionResult> CargarPipeline([FromBody] FechaDto Fecha, CancellationToken cancellationToken)
        {
            await using var connectionValida = new SqlConnection(_config.GetConnectionString("conexion"));
            var nombreUsuario = string.Empty;
            var esAdmin = false;
            var resultValidacion = FiltroConsulta.ValidaTipoUsuario(connectionValida, Fecha.Usuario);
            nombreUsuario = resultValidacion.Item1;
            esAdmin = resultValidacion.Item2;

            if (!esAdmin)
            {
                return BadRequest("El usuario no tiene los permisos necesarios para realizar la carga.");
            }

            if (!DateTime.TryParse(Fecha.FechaFin, out var TimeFixed))
            {
                return BadRequest("El formato de la fecha no es compatible, se espera formato dd-MM-yyyy");
            }

            switch (DateTime.Compare(TimeFixed, DateTime.Today))
            {
                case > 0:
                    return BadRequest("No se puede consultar al futuro ... todavía");
                case 0:
                    TimeFixed = DateTime.Today.AddDays(-1);
                    break;
            }

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", _config.GetValue<string>("Token"));
            //aqui va el While fecha sea menor a hoy
            var errores = 0;
            while (DateTime.Compare(TimeFixed, DateTime.Today) < 0)
            {
                try
                {
                    var TimeFixedApi = TimeFixed.ToString("yyyy-MM-dd");
                    Log.Information($"SE HA COMENZADO LA CARGA DESDE FECHA {TimeFixedApi}");
                    var result =
                        await httpClient.GetFromJsonAsync<List<PayloadDto>>(_config.GetValue<string>("Url") +
                                                                            TimeFixedApi);
                    if (result is null) return NotFound("No se han encontrado datos");
                    await using (var connection = new SqlConnection(_config.GetConnectionString("conexion")))
                    {
                        foreach (var rustPayload in result)
                        {
                            var idInsertado = 0;

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
                                        Value = string.Empty,
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
                                    new SqlParameter
                                    {
                                        ParameterName = "@idInsertado",
                                        SqlDbType = SqlDbType.Int,
                                        Direction = ParameterDirection.Output
                                    }
                                }
                            };

                            try
                            {
                                connection.Open();
                                commandInsertPayload.CommandTimeout = 120000;
                                commandInsertPayload.ExecuteNonQuery();

                                var returnValue = (int)commandInsertPayload.Parameters["@idInsertado"].Value;

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
                                            Value = returnValue,
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
                                        commandInsertarTags.CommandTimeout = 120000;
                                        commandInsertarTags.ExecuteNonQuery();
                                    }
                                    catch (Exception ex)
                                    {
                                        errores++;
                                        Log.Error(ex, $"HA OCURRIDO UN ERROR EN LA CARGA DE TAGS");
                                        Console.Write(ex.Message);
                                    }
                                };
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
                                                Value = returnValue,
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
                                        commandInsertarSR.CommandTimeout = 120000;
                                        commandInsertarSR.ExecuteNonQuery();
                                    }
                                    catch (Exception ex)
                                    {
                                        Log.Error(ex, $"HA OCURRIDO UN ERROR EN LA CARGA DE SKILLS REQUIRED");
                                        errores++;
                                        Console.Write(ex.Message);
                                    }
                                };
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
                                            Value = returnValue,
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
                                        commandInsertarSO.CommandTimeout = 120000;
                                        commandInsertarSO.ExecuteNonQuery();
                                    }
                                    catch (Exception ex)
                                    {
                                        errores++;
                                        Log.Error(ex, $"HA OCURRIDO UN ERROR EN LA CARGA DE SKILLS OPTIONAL");
                                        Console.Write(ex.Message);
                                    }
                                };
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
                                            Value = returnValue,
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
                                        commandInsertarPicture.CommandTimeout = 120000;
                                        commandInsertarPicture.ExecuteNonQuery();
                                    }
                                    catch (Exception ex)
                                    {
                                        errores++;
                                        Log.Error(ex, $"HA OCURRIDO UN ERROR EN LA CARGA DE PICTURES");
                                        Console.Write(ex.Message);
                                    }
                                };

                                #endregion

                                #region Insertar EXTRA FIELDS --

                                rustPayload.extra_field_values ??= new ExtraFieldHelper()
                                {
                                    sep360_nintento = string.Empty,
                                    sep360_nombrerecibe = string.Empty,
                                    sep360_nintentof = string.Empty,
                                    sep360_rutrecibe = string.Empty
                                };

                                var commandInsertarEF = new SqlCommand("InsertarExtraFields")
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
                                            Value = returnValue,
                                        },
                                        new SqlParameter
                                        {
                                            ParameterName = "@intentoF",
                                            SqlDbType = SqlDbType.NVarChar,
                                            Direction = ParameterDirection.Input,
                                            Value = rustPayload.extra_field_values.sep360_nintentof,
                                        },
                                        new SqlParameter
                                        {
                                            ParameterName = "@nombreRecibe",
                                            SqlDbType = SqlDbType.NVarChar,
                                            Direction = ParameterDirection.Input,
                                            Value = rustPayload.extra_field_values.sep360_nombrerecibe,
                                        },
                                        new SqlParameter
                                        {
                                            ParameterName = "@rutRecibe",
                                            SqlDbType = SqlDbType.NVarChar,
                                            Direction = ParameterDirection.Input,
                                            Value = rustPayload.extra_field_values.sep360_rutrecibe,
                                        },
                                        new SqlParameter
                                        {
                                            ParameterName = "@nIntento",
                                            SqlDbType = SqlDbType.NVarChar,
                                            Direction = ParameterDirection.Input,
                                            Value = rustPayload.extra_field_values.sep360_nintento,
                                        }
                                    }
                                };
                                try
                                {
                                    commandInsertarEF.CommandTimeout = 120000;
                                    commandInsertarEF.ExecuteNonQuery();
                                }
                                catch (Exception ex)
                                {
                                    errores++;
                                    Log.Error(ex, $"HA OCURRIDO UN ERROR EN LA CARGA DE EXTRA FIELDS");
                                    Console.Write(ex.Message);
                                }

                                #endregion
                            }
                            catch (Exception ex)
                            {
                                Log.Error(ex, $"HA OCURRIDO UN ERROR EN LA CARGA DE DATOS AL INSERTAR PAYLOAD");
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
                        }
                    }
                    Log.Information($"HA FINALIZADO LA CARGA DEL DÍA.");
                    TimeFixed = TimeFixed.AddDays(1);
                }
                catch (Exception errorException)
                {
                    Console.WriteLine("Ha ocurrido un error con " + errorException.Message);
                    Log.Error(errorException, $"HA OCURRIDO UN ERROR EN LA CARGA DE DATOS");
                    TimeFixed = TimeFixed.AddDays(1);
                }
            }
            //fin while
            return errores == 0 ? Ok() : Ok("Se ha completado exitosamente, pero han ocurrido errores. Verificar log");
        }
        
        [HttpPost("Celmedia/Masivo")]
        public async Task<ActionResult<List<PayloadCliente>>> ObtenerPayloadCelmedia([FromBody] MasivaDto request)
        {
            if (string.IsNullOrEmpty(request.Usuario))
            {
                return BadRequest("El usuario no puede ir en vacio.");
            }

            await using var connection = new SqlConnection(_config.GetConnectionString("conexion"));
            var nombreUsuario = string.Empty;
            var esAdmin = false;
            var resultValidacion = FiltroConsulta.ValidaTipoUsuario(connection, request.Usuario);
            nombreUsuario = resultValidacion.Item1;
            esAdmin = resultValidacion.Item2;
            DataTable tablaResult = new();

            if (!string.IsNullOrEmpty(request.Id))
            {
                if (!string.IsNullOrEmpty(request.FechaDesde) || !string.IsNullOrEmpty(request.FechaHasta) || !string.IsNullOrEmpty(request.Referencia))
                    return BadRequest("NO PUEDES CONSULTAR POR FECHAS SI BUSCAS POR ID");
                tablaResult = esAdmin
                    ? FiltroConsulta.ConsultaAdminPorId(request.Id, connection)
                    : FiltroConsulta.ConsultaClientePorId(request.Id, nombreUsuario, connection);
            }
            else if (!string.IsNullOrEmpty(request.FechaDesde) && !string.IsNullOrEmpty(request.FechaHasta))
            {
                if (!DateTime.TryParse(request.FechaDesde, out var TimeFixedDesde) || !DateTime.TryParse(request.FechaHasta, out var TimeFixedHasta))
                    return BadRequest("El formato de la fecha no es compatible");
                if (DateTime.Compare(TimeFixedHasta, TimeFixedDesde) < 0)
                    return BadRequest("Los rangos de fecha están cambiados. Limite 'hasta' es menor a la fecha 'desde'");
                if (DateTime.Compare(TimeFixedDesde.AddDays(30), TimeFixedHasta) <= 0)
                    return BadRequest("Las fechas no pueden llevar por más de 30 días");

                if (!string.IsNullOrEmpty(request.Referencia))
                {
                    tablaResult = esAdmin
                        ? FiltroConsulta.ConsultaAdminFechasReferencia(TimeFixedDesde.ToShortDateString(),
                            TimeFixedHasta.ToShortDateString(), request.Referencia, connection)
                        : FiltroConsulta.ConsultaClienteFechasReferencia(TimeFixedDesde.ToShortDateString(),
                            TimeFixedHasta.ToShortDateString(), request.Referencia, nombreUsuario, connection);
                }
                else
                {
                    tablaResult = esAdmin
                        ? FiltroConsulta.ConsultaAdminPorFechas(TimeFixedDesde.ToShortDateString(),
                            TimeFixedHasta.ToShortDateString(), connection)
                        : FiltroConsulta.ConsultaClienteEntreFechas(TimeFixedDesde.ToShortDateString(),
                            TimeFixedHasta.ToShortDateString(), nombreUsuario, connection);
                }
            }
            var listaPayloads = new List<PayloadCliente>(tablaResult.Rows.Count);

            try
            {
                foreach (DataRow row in tablaResult.Rows)
                {
                    var dtoRespuestaCliente = new PayloadCliente();
                    var idPayload = int.Parse(row["SECUENCIA"].ToString());
                    dtoRespuestaCliente = CreaObjetos.CrearObjetoCliente(row);

                    var commandObtenerEF = new SqlCommand("ObtenerExtraSegunId")
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
                    var dataAdapterEF = new SqlDataAdapter(commandObtenerEF);
                    var tablaEF = new DataTable();
                    dataAdapterEF.Fill(tablaEF);
                    if (tablaEF.Rows.Count > 0)
                    {
                        var extraHelperClient = new ExtraFieldHelper
                        {
                            sep360_nintento = tablaEF.Rows[0]["NINTENTO"].ToString() ?? string.Empty,
                            sep360_nombrerecibe = tablaEF.Rows[0]["NOMBRERECIBE"].ToString() ?? string.Empty,
                            sep360_nintentof = tablaEF.Rows[0]["INTENTOF"].ToString() ?? string.Empty,
                            sep360_rutrecibe = tablaEF.Rows[0]["RUTRECIBE"].ToString() ?? string.Empty,
                        };
                        dtoRespuestaCliente.QuienRecibeNombre = extraHelperClient.sep360_nombrerecibe;
                        dtoRespuestaCliente.QuienRecibeRut = extraHelperClient.sep360_rutrecibe;
                        dtoRespuestaCliente.Intentos = extraHelperClient.sep360_nintento;
                        //dtoRespuestaCliente.FechaIntentos = extraHelperClient.sep360_nintentof;
                    }
                    listaPayloads.Add(dtoRespuestaCliente);
                }

                return (listaPayloads.Count > 0)
                    ? Ok(listaPayloads)
                    : NotFound("No existe data para los filtros ingresados");
            }
            catch (Exception error)
            {
                Log.Error(error, error.Message);
                throw;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        [HttpPost("Pickup/Masivo")]
        public async Task<ActionResult<List<PayloadRespuesta>>> ObtenerPayloadPickup([FromBody] MasivaDto request)
        {
            if (string.IsNullOrEmpty(request.Usuario))
            {
                return BadRequest("El usuario no puede ir en vacio.");
            }

            await using var connection = new SqlConnection(_config.GetConnectionString("conexion"));
            var nombreUsuario = string.Empty;
            var esAdmin = false;
            var resultValidacion = FiltroConsulta.ValidaTipoUsuario(connection, request.Usuario);
            nombreUsuario = resultValidacion.Item1;
            esAdmin = resultValidacion.Item2;
            DataTable tablaResult = new();

            if (!string.IsNullOrEmpty(request.Id))
            {
                if (!string.IsNullOrEmpty(request.FechaDesde) || !string.IsNullOrEmpty(request.FechaHasta) || !string.IsNullOrEmpty(request.Referencia))
                    return BadRequest("NO PUEDES CONSULTAR POR FECHAS SI BUSCAS POR ID");
                tablaResult = esAdmin
                    ? FiltroConsulta.ConsultaAdminPorId(request.Id, connection)
                    : FiltroConsulta.ConsultaClientePorId(request.Id, nombreUsuario, connection);
            }
            else if (!string.IsNullOrEmpty(request.FechaDesde) && !string.IsNullOrEmpty(request.FechaHasta))
            {
                if (!DateTime.TryParse(request.FechaDesde, out var TimeFixedDesde) || !DateTime.TryParse(request.FechaHasta, out var TimeFixedHasta))
                    return BadRequest("El formato de la fecha no es compatible");
                if (DateTime.Compare(TimeFixedHasta, TimeFixedDesde) < 0)
                    return BadRequest("Los rangos de fecha están cambiados. Limite 'hasta' es menor a la fecha 'desde'");
                if (DateTime.Compare(TimeFixedDesde.AddDays(30), TimeFixedHasta) <= 0)
                    return BadRequest("Las fechas no pueden llevar por más de 30 días");

                if (!string.IsNullOrEmpty(request.Referencia))
                {
                    //preguntar por referencia y fechas
                    tablaResult = esAdmin
                        ? FiltroConsulta.ConsultaAdminFechasReferencia(TimeFixedDesde.ToShortDateString(),
                            TimeFixedHasta.ToShortDateString(), request.Referencia, connection)
                        : FiltroConsulta.ConsultaClienteFechasReferencia(TimeFixedDesde.ToShortDateString(),
                            TimeFixedHasta.ToShortDateString(), request.Referencia, nombreUsuario, connection);
                }
                else
                {
                    tablaResult = esAdmin
                        ? FiltroConsulta.ConsultaAdminPorFechas(TimeFixedDesde.ToShortDateString(),
                            TimeFixedHasta.ToShortDateString(), connection)
                        : FiltroConsulta.ConsultaClienteEntreFechas(TimeFixedDesde.ToShortDateString(),
                            TimeFixedHasta.ToShortDateString(), nombreUsuario, connection);
                }
            }

            var listaPayloads = new List<PayloadRespuesta>(tablaResult.Rows.Count);
            try
            {
                foreach (DataRow row in tablaResult.Rows)
                {
                    var dtoRespuesta = new PayloadRespuesta();
                    var idPayload = int.Parse(row["SECUENCIA"].ToString());
                    dtoRespuesta = CreaObjetos.CreaObjetoAdmin(row);

                    #region Cargar TAGS

                    var commandObtenerTags = new SqlCommand("ObtenerTagsSegunId")
                    {
                        CommandType = CommandType.StoredProcedure,
                        Connection = connection,
                        Parameters =
                        {
                            new SqlParameter
                            {
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
                            new SqlParameter
                            {
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
                            new SqlParameter
                            {
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
                            new SqlParameter
                            {
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

                    #region Cargar Utility Extra fields

                    var commandObtenerEF = new SqlCommand("ObtenerExtraSegunId")
                    {
                        CommandType = CommandType.StoredProcedure,
                        Connection = connection,
                        Parameters =
                        {
                            new SqlParameter
                            {
                                ParameterName = "@id",
                                SqlDbType = SqlDbType.Int,
                                Direction = ParameterDirection.Input,
                                Value = idPayload
                            }
                        }
                    };
                    var dataAdapterEF = new SqlDataAdapter(commandObtenerEF);
                    var tablaEF = new DataTable();
                    dataAdapterEF.Fill(tablaEF);
                    var extraHelperClient = new ExtraFieldHelper();
                    if (tablaEF.Rows.Count > 0)
                    {
                        extraHelperClient.sep360_nintento = tablaEF.Rows[0]["NINTENTO"].ToString() ?? string.Empty;
                        extraHelperClient.sep360_nombrerecibe =
                            tablaEF.Rows[0]["NOMBRERECIBE"].ToString() ?? string.Empty;
                        extraHelperClient.sep360_nintentof = tablaEF.Rows[0]["INTENTOF"].ToString() ?? string.Empty;
                        extraHelperClient.sep360_rutrecibe = tablaEF.Rows[0]["RUTRECIBE"].ToString() ?? string.Empty;
                    }

                    #endregion

                    dtoRespuesta.extra_field_values = extraHelperClient;
                    listaPayloads.Add(dtoRespuesta);
                }

                return (listaPayloads.Count > 0)
                    ? Ok(listaPayloads)
                    : NotFound("No existe data para los filtros ingresados");
            }
            catch (Exception error)
            {
                Log.Error(error, error.Message);
                throw;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }

        }
        [HttpPost("Pickup/Etiqueta")]
        public async Task<IActionResult> CargarEtiqueta([FromBody] Etiqueta_array request)
        {
            List<Etiqueta> _myArray;
            _myArray = request.Etiquetas;
            string email = request.email_envio;
            //creacion de etiqueta libreria barcode
            //string base64String = Convert.ToBase64String(bitmap1);
            //string MyValue = "11111111";
            /*Bitmap BarcodeBmp = IronBarCode.BarcodeWriter.CreateBarcode(MyValue, BarcodeEncoding.Code128).ResizeTo(300, 200).SetMargins(100).ToBitmap();
            System.IO.MemoryStream ms_ = new MemoryStream();
            BarcodeBmp.Save(ms_, ImageFormat.Jpeg);
            byte[] byteImage = ms_.ToArray();
            var SigBase64 = Convert.ToBase64String(byteImage);
            byte[] imageBytes = Convert.FromBase64String(SigBase64);*/

            //iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(imageBytes);
            System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();

            Document document = new Document(PageSize.A5);
            PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
            document.Open();

            document.AddTitle("Etiqueta PDF");

            foreach (var v in _myArray)
            {
                Console.WriteLine("Element = {0}", v.Cliente);

                iTextSharp.text.Font _standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.NORMAL, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);


                PdfPTable tblPrueba = new PdfPTable(2);
                tblPrueba.WidthPercentage = 50;

                iTextSharp.text.Image tif = iTextSharp.text.Image.GetInstance("\\images\\logopdf.JPG");
                tif.ScalePercent(24f);

                document.Add(tif);
                document.Add(new Paragraph(v.Compania));

                tblPrueba.AddCell(new PdfPCell { Colspan = 1 });
                PdfPCell cell = new PdfPCell { Colspan = 1, HorizontalAlignment = 1 };
                tblPrueba.DefaultCell.Border = Rectangle.NO_BORDER;
                tblPrueba.AddCell(cell);
                tblPrueba.AddCell("Cliente");
                tblPrueba.AddCell(v.Cliente);
                tblPrueba.AddCell("Dirección");
                tblPrueba.AddCell(v.Direccion);
                tblPrueba.AddCell("Número");
                tblPrueba.AddCell(v.Numero);
                tblPrueba.AddCell("Referencia");
                tblPrueba.AddCell(v.Referencia);
                tblPrueba.AddCell("Móvil");
                tblPrueba.AddCell(v.movil);
                tblPrueba.AddCell("Región");
                tblPrueba.AddCell(v.region);
                tblPrueba.AddCell("Comuna");
                tblPrueba.AddCell(v.Comuna);
                tblPrueba.AddCell("Orden");
                tblPrueba.AddCell(v.Orden);
                tblPrueba.AddCell("Bulto");
                tblPrueba.AddCell(v.Bulto);

                document.Add(tblPrueba);


                document.Add(new Paragraph(20,"Recibe"));
                Chunk linea = new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(1f, 40f, BaseColor.BLACK, Element.ALIGN_CENTER,10f));
                document.Add(linea);
                document.Add(new Paragraph(20, "RUT"));
                Chunk linea2 = new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(1f, 40f, BaseColor.BLACK, Element.ALIGN_CENTER, 10f));
                document.Add(linea2);
                document.NewPage();
            }


            byte[] bytes = memoryStream.ToArray();
            //memoryStream.Position = 0;
            String base64 = Convert.ToBase64String(bytes);
            writer.CloseStream = false;
            document.Close();
            DateTime localDate = DateTime.Now;
            memoryStream.Position = 0;
            try {

                MailMessage mm = new MailMessage("ycastillo@prosys.cl", email);
                mm.Subject = "subject";
                mm.IsBodyHtml = true;
                mm.Body = "<h3>Estimado se adjunta en correo pdf de etiquetas.</h3>";
                mm.Attachments.Add(new Attachment(memoryStream, "Etiquetas" + localDate + ".pdf"));


                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.UseDefaultCredentials = false;
                smtp.EnableSsl = true;
                smtp.Credentials = new NetworkCredential(_config.GetValue<string>("configuracion_correo:usuario"), _config.GetValue<string>("configuracion_correo:contrasena"));


                smtp.Send(mm);

                document.Dispose();
                return Ok("Enviado exitoso");
            }
            catch(Exception ex)
            {
                return Ok("Error en el envio : "+ ex);
            }



        }


    

        [HttpPost("Pickup/MasivoExportCsv")]
        public async Task<ActionResult<List<PayloadRespuesta>>> MasivoExportCsv([FromBody] MasivaDto request)
        {
            if (string.IsNullOrEmpty(request.Usuario))
            {
                return BadRequest("El usuario no puede ir en vacio.");
            }

            await using var connection = new SqlConnection(_config.GetConnectionString("conexion"));
            var nombreUsuario = string.Empty;
            var esAdmin = false;
            var resultValidacion = FiltroConsulta.ValidaTipoUsuario(connection, request.Usuario);
            nombreUsuario = resultValidacion.Item1;
            esAdmin = resultValidacion.Item2;
            DataTable tablaResult = new();

            if (!string.IsNullOrEmpty(request.Id))
            {
                if (!string.IsNullOrEmpty(request.FechaDesde) || !string.IsNullOrEmpty(request.FechaHasta) || !string.IsNullOrEmpty(request.Referencia))
                {
                    return BadRequest("NO PUEDES CONSULTAR POR FECHAS SI BUSCAS POR ID");
                }

                tablaResult = esAdmin
                    ? FiltroConsulta.ConsultaAdminPorId(request.Id, connection)
                    : FiltroConsulta.ConsultaClientePorId(request.Id, nombreUsuario, connection);
            }
            else if (!string.IsNullOrEmpty(request.FechaDesde) && !string.IsNullOrEmpty(request.FechaHasta))
            {
                if (!DateTime.TryParse(request.FechaDesde, out var TimeFixedDesde) || !DateTime.TryParse(request.FechaHasta, out var TimeFixedHasta))
                    return BadRequest("El formato de la fecha no es compatible");
                if (DateTime.Compare(TimeFixedHasta, TimeFixedDesde) < 0)
                    return BadRequest("Los rangos de fecha están cambiados. Limite 'hasta' es menor a la fecha 'desde'");
                if (DateTime.Compare(TimeFixedDesde.AddDays(30), TimeFixedHasta) <= 0)
                    return BadRequest("Las fechas no pueden llevar por más de 30 días");

                if (!string.IsNullOrEmpty(request.Referencia))
                {
                    //preguntar por referencia y fechas
                    tablaResult = esAdmin
                        ? FiltroConsulta.ConsultaAdminFechasReferencia(TimeFixedDesde.ToShortDateString(),
                            TimeFixedHasta.ToShortDateString(), request.Referencia, connection)
                        : FiltroConsulta.ConsultaClienteFechasReferencia(TimeFixedDesde.ToShortDateString(),
                            TimeFixedHasta.ToShortDateString(), request.Referencia, nombreUsuario, connection);
                }
                else
                {
                    tablaResult = esAdmin
                        ? FiltroConsulta.ConsultaAdminPorFechas(TimeFixedDesde.ToShortDateString(),
                            TimeFixedHasta.ToShortDateString(), connection)
                        : FiltroConsulta.ConsultaClienteEntreFechas(TimeFixedDesde.ToShortDateString(),
                            TimeFixedHasta.ToShortDateString(), nombreUsuario, connection);
                }
            }

            var listaPayloads = new List<PayloadRespuesta>(tablaResult.Rows.Count);
            try
            {
                foreach (DataRow row in tablaResult.Rows)
                {
                    var dtoRespuesta = new PayloadRespuesta();
                    var idPayload = int.Parse(row["SECUENCIA"].ToString());
                    dtoRespuesta = CreaObjetos.CreaObjetoAdmin(row);

                    #region Cargar TAGS

                    var commandObtenerTags = new SqlCommand("ObtenerTagsSegunId")
                    {
                        CommandType = CommandType.StoredProcedure,
                        Connection = connection,
                        Parameters =
                        {
                            new SqlParameter
                            {
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
                            new SqlParameter
                            {
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
                            new SqlParameter
                            {
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
                            new SqlParameter
                            {
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

                    #region Cargar Utility Extra fields

                    var commandObtenerEF = new SqlCommand("ObtenerExtraSegunId")
                    {
                        CommandType = CommandType.StoredProcedure,
                        Connection = connection,
                        Parameters =
                        {
                            new SqlParameter
                            {
                                ParameterName = "@id",
                                SqlDbType = SqlDbType.Int,
                                Direction = ParameterDirection.Input,
                                Value = idPayload
                            }
                        }
                    };
                    var dataAdapterEF = new SqlDataAdapter(commandObtenerEF);
                    var tablaEF = new DataTable();
                    dataAdapterEF.Fill(tablaEF);
                    var extraHelperClient = new ExtraFieldHelper();
                    if (tablaEF.Rows.Count > 0)
                    {
                        extraHelperClient.sep360_nintento = tablaEF.Rows[0]["NINTENTO"].ToString() ?? string.Empty;
                        extraHelperClient.sep360_nombrerecibe =
                            tablaEF.Rows[0]["NOMBRERECIBE"].ToString() ?? string.Empty;
                        extraHelperClient.sep360_nintentof = tablaEF.Rows[0]["INTENTOF"].ToString() ?? string.Empty;
                        extraHelperClient.sep360_rutrecibe = tablaEF.Rows[0]["RUTRECIBE"].ToString() ?? string.Empty;
                    }

                    #endregion

                    dtoRespuesta.extra_field_values = extraHelperClient;
                    listaPayloads.Add(dtoRespuesta);
                }

                var cc = new CsvConfiguration(new System.Globalization.CultureInfo("en-US"));
                using (var ms = new MemoryStream())
                {
                    using (var sw = new StreamWriter(stream: ms, encoding: new UTF8Encoding(true)))
                    {
                        using (var cw = new CsvWriter(sw, cc))
                        {
                            cw.WriteRecords(listaPayloads);
                        }// The stream gets flushed here.
                        return File(ms.ToArray(), "text/csv", $"export_{DateTime.UtcNow.Ticks}.csv");
                    }
                }
               


                //return (listaPayloads.Count > 0)
                //    ? Ok(listaPayloads)
                //    : NotFound("No existe data para los filtros ingresados");
            }
            catch (Exception error)
            {
                Log.Error(error, error.Message);
                throw;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }

        }

        [HttpPost("CourrierCliente")]
        public async Task<ActionResult<Respuesta>> InsertaDatos(PeticionDto request){
            if (string.IsNullOrEmpty(request.usuario))
            {
                return BadRequest(new Respuesta()
                {
                    CodigoRespuesta = 404,
                    MensajeUsuario = "Se debe especificar un usuario",
                    ResponseBody = "error"
                });
            }
            await using var connection = new SqlConnection(_config.GetConnectionString("conexion"));
            var nombreUsuario = string.Empty;
            var esAdmin = false;
            var resultValidacion = FiltroConsulta.ValidaTipoUsuario(connection, request.usuario);
            nombreUsuario = resultValidacion.Item1;
            request.usuario = nombreUsuario;
            return InsertaTablas.InsertarPayloadTrack(connection,request);
        }
    }
}
