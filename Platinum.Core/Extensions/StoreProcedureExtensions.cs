using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.Common;
using System.Reflection;
using Platinum.Core.Extensions;

namespace Platinum.Core.Extensions
{
    public static class StoreProcedureExtensions
    {
        public static DbCommand LoadStoredProc(
            this DbContext context, string storedProcName)
        {
            var cmd = context.Database.GetDbConnection().CreateCommand();
            cmd.CommandText = storedProcName;
            cmd.CommandType = CommandType.StoredProcedure;
            return cmd;
        }

        public static DbCommand WithSqlParam(
            this DbCommand cmd,
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

            throw new InvalidOperationException("Call LoadStoredProc before using this method");
        }

        public static async Task<IEnumerable<T>> MapToListAsync<T>(this DbDataReader dbDataReader, bool continueRead = false)
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


        public static IEnumerable<T> MapToList<T>(this DbDataReader dbDataReader, bool continueRead = false)
        {
            while (dbDataReader.Read())
            {
                T entity = (T)Activator.CreateInstance(typeof(T));

                for (int row = 0; row < dbDataReader.FieldCount; row++)
                {
                    Type type = typeof(T);
                    PropertyInfo prop = type.GetProperty(dbDataReader.GetName(row));
                    prop.SetValue(entity, Convert.IsDBNull(dbDataReader.GetValue(row)) ? null : dbDataReader.GetValue(row), null);
                }

                yield return entity;
            }

            if (continueRead)
                dbDataReader.NextResult();
        }

        public static async Task<DbDataReader> GetDataReaderAsync(this DbCommand command)
        {
            using (command)
            {
                if (command.Connection.State == ConnectionState.Closed)
                    await command.Connection.OpenAsync();
                try
                {
                    return await command.ExecuteReaderAsync();
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    // command.Connection.Close();
                }
            }
        }

        public static Dictionary<string, DbParameter> ToDictionary(this DbCommand command)
        {
            return command.Parameters
                        .Cast<DbParameter>()
                        .Where(p => p.SourceVersion == DataRowVersion.Current && !p.SourceColumnNullMapping)
                        .ToDictionary(p => p.ParameterName);
        }

        public static async Task<Tuple<DbDataReader, Dictionary<string, DbParameter>>> GetDataReaderAndParametersAsync(this DbCommand command)
        {
            using (command)
            {
                if (command.Connection.State == ConnectionState.Closed)
                    await command.Connection.OpenAsync();
                try
                {
                    var dataReader = await command.ExecuteReaderAsync();
                    var parameters = command.ToDictionary();

                    return new Tuple<DbDataReader, Dictionary<string, DbParameter>>(dataReader, parameters);
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    // command.Connection.Close();
                }
            }
        }

        public static async Task<T> GetScalarValueAsync<T>(this DbCommand command) where T : class
        {
            using (command)
            {
                if (command.Connection.State == ConnectionState.Closed)
                    await command.Connection.OpenAsync();
                try
                {
                    var result = await command.ExecuteScalarAsync();
                    return result.AsOrDefault<T>();
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    // command.Connection.Close();
                }
            }
        }

        public static async Task<Tuple<T, Dictionary<string, DbParameter>>> GetScalarValueAndParametersAsync<T>(this DbCommand command) where T : class
        {
            using (command)
            {
                if (command.Connection.State == ConnectionState.Closed)
                    await command.Connection.OpenAsync();
                try
                {
                    var result = await command.ExecuteScalarAsync();
                    var parameters = command.ToDictionary();

                    return new Tuple<T, Dictionary<string, DbParameter>>(result.AsOrDefault<T>(), parameters);
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    // command.Connection.Close();
                }
            }

        }
        public static async Task<int> GetNonQueryValueAsync(this DbCommand command)
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
                finally
                {
                    // command.Connection.Close();
                }
            }
        }

        public static async Task<Tuple<int, Dictionary<string, DbParameter>>> GetNonQueryValueAndParametersAsync(this DbCommand command)
        {
            using (command)
            {
                if (command.Connection.State == ConnectionState.Closed)
                    await command.Connection.OpenAsync();
                try
                {
                    var result = await command.ExecuteNonQueryAsync();
                    var parameters = command.ToDictionary();

                    return new Tuple<int, Dictionary<string, DbParameter>>(result, parameters);
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    // command.Connection.Close();
                }
            }
        }
    }
}
