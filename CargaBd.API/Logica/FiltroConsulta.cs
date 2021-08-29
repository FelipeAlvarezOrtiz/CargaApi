using System;
using System.Data;
using Microsoft.Data.SqlClient;
using Serilog;

namespace CargaBd.API.Logica
{
    public static class FiltroConsulta
    {
        public static DataTable ConsultaAdminFechasReferencia(string fechaDesde,string fechaHasta,string referencia, SqlConnection connection)
        {
            var tablaResultUsuario = new DataTable();
            var commandObtenerUsuario = new SqlCommand("ObtenerPayloadEntreFechasYRefAdmin")
            {
                CommandType = CommandType.StoredProcedure,
                Connection = connection,
                Parameters =
                {
                    new SqlParameter{
                        ParameterName = "@fechaDesde",
                        SqlDbType = SqlDbType.NVarChar,
                        Direction = ParameterDirection.Input,
                        Value = fechaDesde
                    },
                    new SqlParameter{
                        ParameterName = "@fechaHasta",
                        SqlDbType = SqlDbType.NVarChar,
                        Direction = ParameterDirection.Input,
                        Value = fechaHasta
                    },
                    new SqlParameter{
                        ParameterName = "@referencia",
                        SqlDbType = SqlDbType.NVarChar,
                        Direction = ParameterDirection.Input,
                        Value = referencia
                    },
                }
            };
            try
            {
                connection.Open();
                commandObtenerUsuario.CommandTimeout = 120000;
                var dataAdapterUser = new SqlDataAdapter(commandObtenerUsuario);
                dataAdapterUser.Fill(tablaResultUsuario);
                return tablaResultUsuario;
            }
            catch (Exception exception)
            {
                Console.WriteLine(
                    $"Ha ocurrido un error al ejecutar el procedure del usuario con mensaje {exception.Message}");
                Log.Error(exception, "HA OCURRIDO UN ERROR AL RECUPERAR AL USUARIO EN BUSQUEDA MASIVA CON REFERENCIAS");
                throw;
            }
        }

        public static DataTable ConsultaAdminPorFechas(string fechaDesde, string fechaHasta, SqlConnection connection)
        {
            var tablaResultUsuario = new DataTable();
            var commandObtenerUsuario = new SqlCommand("ObtenerPayloadEntreFechasAdmin")
            {
                CommandType = CommandType.StoredProcedure,
                Connection = connection,
                Parameters =
                {
                    new SqlParameter{
                        ParameterName = "@fechaDesde",
                        SqlDbType = SqlDbType.NVarChar,
                        Direction = ParameterDirection.Input,
                        Value = fechaDesde
                    },
                    new SqlParameter{
                        ParameterName = "@fechaHasta",
                        SqlDbType = SqlDbType.NVarChar,
                        Direction = ParameterDirection.Input,
                        Value = fechaHasta
                    }
                }
            };
            try
            {
                connection.Open();
                commandObtenerUsuario.CommandTimeout = 120000;
                var dataAdapterUser = new SqlDataAdapter(commandObtenerUsuario);
                dataAdapterUser.Fill(tablaResultUsuario);
                return tablaResultUsuario;
            }
            catch (Exception exception)
            {
                Console.WriteLine(
                    $"Ha ocurrido un error al ejecutar el procedure del usuario con mensaje {exception.Message}");
                Log.Error(exception, "HA OCURRIDO UN ERROR AL RECUPERAR AL USUARIO EN BUSQUEDA MASIVA CON REFERENCIAS");
                throw;
            }
        }

        public static DataTable ConsultaAdminPorId(string id, SqlConnection connection)
        {
            var tablaResultUsuario = new DataTable();
            var commandObtenerUsuario = new SqlCommand("ObtenerPayloadSegunTrackAdmin")
            {
                CommandType = CommandType.StoredProcedure,
                Connection = connection,
                Parameters =
                {
                    new SqlParameter{
                        ParameterName = "@trackId",
                        SqlDbType = SqlDbType.NVarChar,
                        Direction = ParameterDirection.Input,
                        Value = id
                    }
                }
            };
            try
            {
                connection.Open();
                commandObtenerUsuario.CommandTimeout = 120000;
                var dataAdapterUser = new SqlDataAdapter(commandObtenerUsuario);
                dataAdapterUser.Fill(tablaResultUsuario);
                return tablaResultUsuario;
            }
            catch (Exception exception)
            {
                Console.WriteLine(
                    $"Ha ocurrido un error al ejecutar el procedure del -ObtenerPayloadSegunTrackAdmin- con mensaje {exception.Message}");
                Log.Error(exception, "Ha ocurrido un error al ejecutar el procedure del -ObtenerPayloadSegunTrackAdmin-");
                throw;
            }
        }

        public static DataTable ConsultaClienteFechasReferencia(string fechaDesde, string fechaHasta, string referencia,string usuario, SqlConnection connection)
        {
            var tablaResultUsuario = new DataTable();
            var commandObtenerUsuario = new SqlCommand("ObtenerPayloadEntreFechasYReferencia")
            {
                CommandType = CommandType.StoredProcedure,
                Connection = connection,
                Parameters =
                {
                    new SqlParameter{
                        ParameterName = "@fechaDesde",
                        SqlDbType = SqlDbType.NVarChar,
                        Direction = ParameterDirection.Input,
                        Value = fechaDesde
                    },
                    new SqlParameter{
                        ParameterName = "@fechaHasta",
                        SqlDbType = SqlDbType.NVarChar,
                        Direction = ParameterDirection.Input,
                        Value = fechaHasta
                    },
                    new SqlParameter{
                        ParameterName = "@referencia",
                        SqlDbType = SqlDbType.NVarChar,
                        Direction = ParameterDirection.Input,
                        Value = referencia
                    },
                    new SqlParameter{
                        ParameterName = "@usuario",
                        SqlDbType = SqlDbType.NVarChar,
                        Direction = ParameterDirection.Input,
                        Value = usuario
                    },
                }
            };
            try
            {
                connection.Open();
                commandObtenerUsuario.CommandTimeout = 120000;
                var dataAdapterUser = new SqlDataAdapter(commandObtenerUsuario);
                dataAdapterUser.Fill(tablaResultUsuario);
                return tablaResultUsuario;
            }
            catch (Exception exception)
            {
                Console.WriteLine(
                    $"Ha ocurrido un error al ejecutar el procedure del usuario con mensaje {exception.Message}");
                Log.Error(exception, "HA OCURRIDO UN ERROR AL RECUPERAR AL USUARIO EN BUSQUEDA MASIVA CON REFERENCIAS");
                throw;
            }
        }

        public static DataTable ConsultaClientePorId(string id,string usuario, SqlConnection connection)
        {
            var tablaResultUsuario = new DataTable();
            var commandObtenerUsuario = new SqlCommand("ObtenerPayloadSegunTrack")
            {
                CommandType = CommandType.StoredProcedure,
                Connection = connection,
                Parameters =
                {
                    new SqlParameter{
                        ParameterName = "@trackId",
                        SqlDbType = SqlDbType.NVarChar,
                        Direction = ParameterDirection.Input,
                        Value = id
                    },
                    new SqlParameter{
                        ParameterName = "@usuario",
                        SqlDbType = SqlDbType.NVarChar,
                        Direction = ParameterDirection.Input,
                        Value = usuario
                    },
                }
            };
            try
            {
                connection.Open();
                commandObtenerUsuario.CommandTimeout = 120000;
                var dataAdapterUser = new SqlDataAdapter(commandObtenerUsuario);
                dataAdapterUser.Fill(tablaResultUsuario);
                return tablaResultUsuario;
            }
            catch (Exception exception)
            {
                Console.WriteLine(
                    $"Ha ocurrido un error al ejecutar el procedure -ObtenerPayloadSegunTrack- con mensaje {exception.Message}");
                Log.Error(exception, "Ha ocurrido un error al ejecutar el procedure -ObtenerPayloadSegunTrack-");
                throw;
            }
        }

        public static DataTable ConsultaClienteEntreFechas(string fechaDesde, string fechaHasta,string usuario, SqlConnection connection)
        {
            var tablaResultUsuario = new DataTable();
            var commandObtenerUsuario = new SqlCommand("ObtenerPayloadSegunTrack")
            {
                CommandType = CommandType.StoredProcedure,
                Connection = connection,
                Parameters =
                {
                    new SqlParameter{
                        ParameterName = "@fechaDesde",
                        SqlDbType = SqlDbType.NVarChar,
                        Direction = ParameterDirection.Input,
                        Value = fechaDesde
                    },
                    new SqlParameter{
                        ParameterName = "@fechaHasta",
                        SqlDbType = SqlDbType.NVarChar,
                        Direction = ParameterDirection.Input,
                        Value = fechaHasta
                    },
                    new SqlParameter{
                        ParameterName = "@usuario",
                        SqlDbType = SqlDbType.NVarChar,
                        Direction = ParameterDirection.Input,
                        Value = usuario
                    },
                }
            };
            try
            {
                connection.Open();
                commandObtenerUsuario.CommandTimeout = 120000;
                var dataAdapterUser = new SqlDataAdapter(commandObtenerUsuario);
                dataAdapterUser.Fill(tablaResultUsuario);
                return tablaResultUsuario;
            }
            catch (Exception exception)
            {
                Console.WriteLine(
                    $"Ha ocurrido un error al ejecutar el procedure -ObtenerPayloadSegunTrack- con mensaje {exception.Message}");
                Log.Error(exception, "Ha ocurrido un error al ejecutar el procedure -ObtenerPayloadSegunTrack-");
                throw;
            }
        }

        public static Tuple<string, bool> ValidaTipoUsuario(SqlConnection connection,string hashUsuario)
        {
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
                        Value = hashUsuario
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
                    throw new Exception("El usuario no existe o no se encuentra activo.");
                return new Tuple<string, bool>(tablaResultUsuario.Rows[0]["NOMBRE_USUARIO"].ToString(),
                    tablaResultUsuario.Rows[0]["ESADMIN"].ToString().Equals("True"));
            }
            catch (Exception error)
            {
                Log.Error(error, "Ha ocurrido un error al ejecutar el procedure ObtenerUsuario");
                throw;
            }
        }
    }
}
