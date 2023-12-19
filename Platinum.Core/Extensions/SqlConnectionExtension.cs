using System.Data;
using System.Data.Common;
using System.Reflection;
using Microsoft.Data.SqlClient;
using Platinum.Core.Extensions;

namespace Platinum.Core.Extensions
{
    public static class SqlConnectionExtension
    {
        public static SqlCommand LoadSqlCommand(
            this SqlConnection sqlConnection,
            string commandTextOrProc,
            CommandType commandType,
            SqlTransaction sqlTransaction = null)
        {
            var cmd = sqlConnection.CreateCommand();
            cmd.CommandText = commandTextOrProc;
            cmd.CommandType = commandType;
            cmd.CommandTimeout = 300;
            cmd.Transaction = sqlTransaction;
            return cmd;
        }

        public static SqlCommand WithSqlParam(
          this SqlCommand cmd,
          string paramName,
          object paramValue,
          ParameterDirection parameterDirection = ParameterDirection.Input,
          DbType dbType = DbType.String
          )
        {
            if (!string.IsNullOrEmpty(cmd.CommandText))
            {
                var param = cmd.CreateParameter();
                param.ParameterName = paramName;
                param.Direction = parameterDirection;
                param.Value = paramValue;
                param.DbType = dbType;
                cmd.Parameters.Add(param);
                return cmd;
            }

            throw new InvalidOperationException("Call LoadSqlCommand before using this method");
        }

        public static Dictionary<string, SqlParameter> ToDictionary(this SqlCommand command)
        {
            return command.Parameters
                        .Cast<SqlParameter>()
                        .Where(p => p.SourceVersion == DataRowVersion.Current && !p.SourceColumnNullMapping)
                        .ToDictionary(p => p.ParameterName);
        }

        public static async Task<IEnumerable<T>> MapToListAsync<T>(this SqlDataReader dbDataReader, bool continueRead = false)
        {
            List<T> listObjects = new List<T>();

            while (await dbDataReader.ReadAsync())
            {
                T entity = (T)Activator.CreateInstance(typeof(T));


                for (int row = 0; row < dbDataReader.FieldCount; row++)
                {
                    Type type = typeof(T);
                    PropertyInfo prop = type.GetProperty(dbDataReader.GetName(row));
                    prop.SetValue(entity, Convert.IsDBNull(dbDataReader.GetValue(row)) ? null : dbDataReader.GetValue(row), null);
                }

                listObjects.Add(entity);
            }

            if (continueRead)
                await dbDataReader.NextResultAsync();

            return listObjects;
        }

        public static async Task<Tuple<DbDataReader, Dictionary<string, SqlParameter>>> GetDataReaderAndParametersAsync(this SqlCommand command)
        {
            using (command)
            {
                if (command.Connection.State == ConnectionState.Closed)
                    await command.Connection.OpenAsync();
                try
                {
                    var dataReader = await command.ExecuteReaderAsync();
                    var parameters = command.ToDictionary();

                    return new Tuple<DbDataReader, Dictionary<string, SqlParameter>>(dataReader, parameters);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public static async Task<int> GetNonQueryValueAsync(this SqlCommand command)
        {
            using (command)
            {
                if (command.Connection.State == ConnectionState.Closed)
                    await command.Connection.OpenAsync();
                try
                {
                    return await command.ExecuteNonQueryAsync();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public static async Task<Tuple<int, Dictionary<string, SqlParameter>>> GetNonQueryValueAndParametersAsync(this SqlCommand command)
        {
            using (command)
            {
                if (command.Connection.State == ConnectionState.Closed)
                    await command.Connection.OpenAsync();
                try
                {
                    var result = await command.ExecuteNonQueryAsync();
                    var parameters = command.ToDictionary();

                    return new Tuple<int, Dictionary<string, SqlParameter>>(result, parameters);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
    }
}
