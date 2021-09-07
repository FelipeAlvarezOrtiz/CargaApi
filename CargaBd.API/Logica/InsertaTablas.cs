using System.Data;
using CargaBd.API.Models;
using Microsoft.Data.SqlClient;

namespace CargaBd.API.Logica
{
    public static class InsertaTablas
    {
        public static bool InsertarPayloadTrack(SqlConnection connection,PeticionDto request)
        {
            var commandInsertarPayloadTrack = new SqlCommand("ObtenerPayloadEntreFechasYRefAdmin")
            {
                CommandType = CommandType.StoredProcedure,
                Connection = connection,
                Parameters =
                {
                    new SqlParameter{
                        ParameterName = "@fechaDesde",
                        SqlDbType = SqlDbType.NVarChar,
                        Direction = ParameterDirection.Input,
                        Value = request.address
                    }
                }
            };
            return false;
        }
    }
}
