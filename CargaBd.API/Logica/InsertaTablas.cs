using System;
using System.Collections.Generic;
using System.Data;
using CargaBd.API.Models;
using Microsoft.Data.SqlClient;

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
                    ResponseBody = $"Su número de atención es {idCreado}"
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

        public static PayloadCliente ObtenerPayloadFiltro(SqlConnection connection, int idPayload)
        {
            try
            {
                if(connection.State == ConnectionState.Closed)
                    connection.Open();
                var commandQuery = new SqlCommand()
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
                            ParameterName = "@idPayload"
                        }
                    }
                };
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
