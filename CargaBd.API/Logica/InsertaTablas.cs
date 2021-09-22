using System;
using System.Collections.Generic;
using System.Data;
using CargaBd.API.Models;
using Microsoft.Data.SqlClient;
using Serilog;

namespace CargaBd.API.Logica
{
    public static class InsertaTablas
    {
        public static Respuesta InsertarPayloadTrack(SqlConnection connection,PeticionDto request)
        {
            var idCreado = 0;
            var commandInsertarPayloadTrack = new SqlCommand("InsertarCourrier")
            {
                CommandType = CommandType.StoredProcedure,
                Connection = connection,
                Parameters =
                {
                    new SqlParameter{
                        ParameterName = "@address",
                        SqlDbType = SqlDbType.NVarChar,
                        Direction = ParameterDirection.Input,
                        Value = request.address
                    },new SqlParameter{
                        ParameterName = "@tracking_id",
                        SqlDbType = SqlDbType.NVarChar,
                        Direction = ParameterDirection.Input,
                        Value = request.tracking_id
                    },new SqlParameter{
                        ParameterName = "@title",
                        SqlDbType = SqlDbType.NVarChar,
                        Direction = ParameterDirection.Input,
                        Value = request.title
                    },new SqlParameter{
                        ParameterName = "@load",
                        SqlDbType = SqlDbType.Int,
                        Direction = ParameterDirection.Input,
                        Value = request.load
                    },new SqlParameter{
                        ParameterName = "@load_2",
                        SqlDbType = SqlDbType.Int,
                        Direction = ParameterDirection.Input,
                        Value = request.load_2
                    },new SqlParameter{
                        ParameterName = "@load_3",
                        SqlDbType = SqlDbType.Int,
                        Direction = ParameterDirection.Input,
                        Value = request.load_3
                    },new SqlParameter{
                        ParameterName = "@contact_name",
                        SqlDbType = SqlDbType.NVarChar,
                        Direction = ParameterDirection.Input,
                        Value = request.contact_name
                    },new SqlParameter{
                        ParameterName = "@contact_email",
                        SqlDbType = SqlDbType.NVarChar,
                        Direction = ParameterDirection.Input,
                        Value = request.contact_email
                    },new SqlParameter{
                        ParameterName = "@contact_phone",
                        SqlDbType = SqlDbType.NVarChar,
                        Direction = ParameterDirection.Input,
                        Value = request.contact_phone
                    },new SqlParameter{
                        ParameterName = "@reference",
                        SqlDbType = SqlDbType.NVarChar,
                        Direction = ParameterDirection.Input,
                        Value = request.reference
                    },new SqlParameter{
                        ParameterName = "@notes",
                        SqlDbType = SqlDbType.NVarChar,
                        Direction = ParameterDirection.Input,
                        Value = request.notes
                    },new SqlParameter{
                        ParameterName = "@planned_date",
                        SqlDbType = SqlDbType.NVarChar,
                        Direction = ParameterDirection.Input,
                        Value = request.planned_date
                    },new SqlParameter{
                        ParameterName = "@lugar_retiro",
                        SqlDbType = SqlDbType.NVarChar,
                        Direction = ParameterDirection.Input,
                        Value = request.lugar_retiro
                    },new SqlParameter{
                        ParameterName = "@disponible_bodega",
                        SqlDbType = SqlDbType.NVarChar,
                        Direction = ParameterDirection.Input,
                        Value = request.disponible_bodega
                    },new SqlParameter{
                        ParameterName = "@usuario",
                        SqlDbType = SqlDbType.NVarChar,
                        Direction = ParameterDirection.Input,
                        Value = request.usuario
                    },new SqlParameter
                    {
                        ParameterName = "@idInsertado",
                        SqlDbType = SqlDbType.Int,
                        Direction = ParameterDirection.Output
                    }
                }
            };
            try
            {
                if (connection.State != ConnectionState.Open) 
                    connection.Open();
                commandInsertarPayloadTrack.CommandTimeout = 120000;
                commandInsertarPayloadTrack.ExecuteNonQuery();
                idCreado = (int)commandInsertarPayloadTrack.Parameters["@idInsertado"].Value;
                return new Respuesta()
                {
                    CodigoRespuesta = 200,
                    MensajeUsuario = "TODO OK",
                    ResponseBody = $"Su número de atención es {idCreado}",
                    NumeroAtencion = idCreado
                };
            }
            catch(Exception)
            {
                throw;
            }
        }

        public static List<int> ObtenerIdsDeFiltro(SqlConnection connection, string fechaDesde, string fechaHasta)
        {
            try
            {
                var commandInsertarPayloadTrack = new SqlCommand("ObtenerPayloadEntreFechasYRefAdmin")
                {
                    CommandType = CommandType.StoredProcedure,
                    Connection = connection,
                    Parameters =
                    {
                        new SqlParameter
                        {
                            ParameterName = "@adress",
                            SqlDbType = SqlDbType.NVarChar,
                            Direction = ParameterDirection.Input,
                            Value = fechaDesde
                        },
                        new SqlParameter
                        {
                            ParameterName = "@trackingId",
                            SqlDbType = SqlDbType.NVarChar,
                            Direction = ParameterDirection.Input,
                            Value = fechaHasta
                        }
                    }
                };
                connection.Open();
                commandInsertarPayloadTrack.CommandTimeout = 120000;
                commandInsertarPayloadTrack.ExecuteNonQuery();
                
            }
            catch (Exception)
            {
                throw;
            }

            return new List<int>();
        }

        public static Tuple<bool,DataTable> ObtenerPayloadFiltro(SqlConnection connection, int idPayload,ref Dictionary<string,string> datosAgregados)
        {
            var tablaPayload = new DataTable();
            try
            {
                if(connection.State == ConnectionState.Closed)
                    connection.Open();
                var commandQuery = new SqlCommand("ObtenerPayloadPorId")
                {
                    Connection = connection,
                    CommandType = CommandType.StoredProcedure,
                    CommandTimeout = 10000,
                    Parameters =
                    {
                        new SqlParameter()
                        {
                            Direction = ParameterDirection.Input,
                            DbType = DbType.Int32,
                            ParameterName = "@idPayload",
                            Value = idPayload
                        }
                    }
                };
                var dataAdapterUser = new SqlDataAdapter(commandQuery);
                dataAdapterUser.Fill(tablaPayload);
                var referencia = tablaPayload.Rows[0]["REFERENCE"].ToString();
                if (datosAgregados.ContainsKey(referencia))
                {
                    return Tuple.Create<bool,DataTable>(false, null);
                }
                var usuario = tablaPayload.Rows[0]["TITLE"].ToString();
                var resultadoFinal = ObtenerReferenciaMasActual(connection, referencia, usuario);
                datosAgregados.Add(referencia,usuario);
                //obtenemos la referencia y vamos a buscar por referencia
                return Tuple.Create<bool,DataTable>(true,resultadoFinal);
            }
            catch (Exception error)
            {
                Log.Error(error,"HA OCURRIDO UN ERROR AL OBTENER PAYLOAD CON MENSAJE {0}", error.Message);
                throw;
            }
        }

        public static DataTable ObtenerReferenciaMasActual(SqlConnection connection, string referencia,string usuario)
        {
            var tablaPayload = new DataTable();
            try
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                var commandQuery = new SqlCommand("ObtenerTopMaxReferencia")
                {
                    Connection = connection,
                    CommandType = CommandType.StoredProcedure,
                    CommandTimeout = 10000,
                    Parameters =
                    {
                        new SqlParameter()
                        {
                            Direction = ParameterDirection.Input,
                            DbType = DbType.String,
                            ParameterName = "@referencia",
                            Value = referencia
                        },new SqlParameter()
                        {
                            Direction = ParameterDirection.Input,
                            DbType = DbType.String,
                            ParameterName = "@usuario",
                            Value = usuario
                        }
                    }
                };
                var dataAdapterUser = new SqlDataAdapter(commandQuery);
                dataAdapterUser.Fill(tablaPayload);
                //obtenemos la referencia y vamos a buscar por referencia
                return tablaPayload;
            }
            catch (Exception error)
            {
                Log.Error(error, "HA OCURRIDO UN ERROR AL OBTENER PAYLOAD CON MENSAJE {0}", error.Message);
                throw;
            }
        }
    }
}
