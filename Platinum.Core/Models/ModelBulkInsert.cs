using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Platinum.Core.Abstractions.Identitys;
using Platinum.Core.Attributes;
using Platinum.Core.Extensions;
using System.Data;

namespace Platinum.Core.Models
{
    public class ModelBulkOperation<T> where T : ModelBase
    {
        private IApplicationUserManager userManager;

        private ReflectionCache<T> reflectionCache;
        private SqlTransaction sqlTransaction;
        private SqlConnection sqlConnection;
        string schemaName = "dbo";
        string _mergedPrimaryKeys = "Id";

        public event SqlRowsCopiedHandler SqlRowsCopiedEvent;
        public delegate void SqlRowsCopiedHandler(object sender, SqlRowsCopiedEventArgs e);
        public long TotalCopiedRows { get; set; }
        public string MergedPrimaryKeys { get => _mergedPrimaryKeys; set => _mergedPrimaryKeys = value; }
        public Dictionary<string, string> UpdateColumnMappings { get; set; }

        public ModelBulkOperation(DatabaseFacade database, IApplicationUserManager userManager = null)
        {
            sqlConnection = database.GetDbConnection() as SqlConnection;
            sqlTransaction = database.CurrentTransaction?.GetDbTransaction() as SqlTransaction;
            this.userManager = userManager;
        }

        public async Task Insert(IEnumerable<T> items,
            int batchSize = 200)
        {
            await WithBulk(items, 200, async (a, b, c) =>
              {
                  await b.WriteToServerAsync(c);
                  b.Close();
              });
        }

        public async Task Merge(IEnumerable<T> data
            , int batchSize = 200
            , bool isInsert = true
            , bool isUpdate = true
            , bool isDelete = false
            , bool isOutput = false)
        {
            await WithBulk(data, 200, async (a, b, c) =>
            {
                await b.WriteToServerAsync(c);
                b.Close();

                string sourceTable = b.DestinationTableName;
                string targetTableName = string.Format("[{0}].[{1}]", schemaName, sourceTable.Remove(0, 1));
                string sourceTableName = string.Format("[{0}].[{1}]", schemaName, sourceTable);
                List<string> insertColumns = null;
                List<string> updateColumns = null;
                List<string> outputColumns = null;

                Type modelType = typeof(T);
                var bulkTableAttr = modelType.GetCustomAttributes(typeof(BulkTableAttribute), true);
                if (bulkTableAttr[0] is BulkTableAttribute bulkTableAttribute)
                {
                    insertColumns = bulkTableAttribute.InsertColumns.Split(",", StringSplitOptions.RemoveEmptyEntries).ToList();
                    updateColumns = bulkTableAttribute.UpdateColumns.Split(",", StringSplitOptions.RemoveEmptyEntries).ToList();
                    outputColumns = bulkTableAttribute.OutputColumns.Split(",", StringSplitOptions.RemoveEmptyEntries).ToList();
                }

                // List<string> insertColumns = new List<string> { "Id", "Name", "IsDeleted" };
                // List<string> updateColumns = new List<string> { "Name"};

                DynamicMergeStatementBuilder builder = new DynamicMergeStatementBuilder();
                Dictionary<string, string> updateColumnMappings = updateColumns.ToDictionary(e => e, e => e);
                // updateColumnMappings.Add("ModifiedDate", "GETUTCDATE()");

                if (UpdateColumnMappings != null)
                {
                    foreach (var d1Item in UpdateColumnMappings)
                        updateColumnMappings.Add(d1Item.Key, d1Item.Value);
                }

                //List<string> outputColumns = new List<string> { "Id", "Name", "ModifiedDate" };

                builder.WithTables(sourceTableName, targetTableName)
                       .MatchOn(_mergedPrimaryKeys.Split(",".ToCharArray()).ToList())
                       .AddInsertStatement(insertColumns, isInsert)
                       .AddUpdateStatement(updateColumnMappings, isOutput, UpdateColumnMappings.Keys.ToList())
                       .AddDeleteStatement(isUpdate)
                       .AddOutputStatement(outputColumns, isOutput);

                string mergeStatement = builder.Build();

                using (SqlCommand command = sqlConnection.LoadSqlCommand(mergeStatement, CommandType.Text, sqlTransaction))
                {
                    command.Connection = sqlConnection;
                    //Creating temp table on database
                    var result = await command.ExecuteReaderAsync();

                    var output = await result.MapToListAsync<MergingOutputModel>(false);
                }
            });
        }

        private async Task WithBulk(IEnumerable<T> items,
            int batchSize,
            Func<SqlConnection, SqlBulkCopy, DataTable, Task> actionBulk)
        {
            Type modelType = typeof(T);
            string tableName = string.Empty;
            bool isTempTable = false;

            var bulkTableAttr = modelType.GetCustomAttributes(typeof(BulkTableAttribute), true);

            if (bulkTableAttr[0] is BulkTableAttribute bulkTableAttribute)
            {
                tableName = bulkTableAttribute.Name;
                isTempTable = bulkTableAttribute.IsTempTable;
            }

            if (string.IsNullOrEmpty(tableName))
            {
                throw new NullReferenceException(nameof(tableName));
            }

            DataTable dataTable;

            if (reflectionCache == null)
            {
                reflectionCache = new ReflectionCache<T>();
            }

            dataTable = reflectionCache.ModelTypeTable.Clone();
            dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["Id"] };

            using (dataTable)
            {
                SqlBulkCopy sqlBulkCopy;

                if (sqlConnection.State == ConnectionState.Closed)
                {
                    await sqlConnection.OpenAsync();
                }

                if (sqlTransaction != null)
                {
                    sqlBulkCopy = new SqlBulkCopy
                        (
                            sqlConnection,
                            SqlBulkCopyOptions.Default,
                            sqlTransaction
                        );
                }
                else
                {
                    sqlBulkCopy = new SqlBulkCopy(sqlConnection);
                }

                using (sqlBulkCopy)
                {
                    sqlBulkCopy.DestinationTableName = !isTempTable ? tableName : $"#{tableName}";
                    sqlBulkCopy.BatchSize = batchSize;
                    sqlBulkCopy.BulkCopyTimeout = 1800;
                    sqlBulkCopy.NotifyAfter = dataTable.Rows.Count;

                    sqlBulkCopy.SqlRowsCopied += (s, e) =>
                    {
                        TotalCopiedRows = e.RowsCopied;
                        SqlRowsCopiedEvent?.Invoke(s, e);
                    };

                    foreach (DataColumn colunm in dataTable.Columns)
                    {
                        sqlBulkCopy.ColumnMappings.Add(colunm.ColumnName, colunm.ColumnName);
                    }

                    var rows = items.Select(r =>
                    {
                        if (r is ModelBase modelBase)
                        {
                            modelBase.IsDeleted = false;
                            modelBase.ModifiedDate = DateTime.Now;
                            modelBase.CreatedDate = DateTime.Now;
                            modelBase.CreatedBy = userManager?.CurrentUserId;
                            modelBase.ModifiedBy = userManager?.CurrentUserId;
                        }

                        var row = dataTable.NewRow();
                        Array.ForEach(reflectionCache.ModelTypeProperties.Keys.ToArray(), (p) =>
                        {
                            var getter = reflectionCache.ModelTypeProperties[p];
                            row[p.Name] = getter(r) ?? DBNull.Value;
                        });

                        return row;
                    });

                    foreach (var row in rows)
                    {
                        dataTable.Rows.Add(row);
                    }

                    dataTable.AcceptChanges();

                    if (dataTable.Rows.Count > 0)
                    {

                        if (isTempTable)
                        {
                            var tableClass = new TableClassModel(typeof(T));
                            string createTableQuery = tableClass.CreateTableScript(sqlBulkCopy.DestinationTableName);
                            using (SqlCommand command = sqlConnection.LoadSqlCommand(createTableQuery, CommandType.Text, sqlTransaction))
                            {

                                command.Connection = sqlConnection;

                                //Creating temp table on database
                                var result = await command.ExecuteNonQueryAsync();
                            }
                        }

                        await actionBulk(sqlConnection, sqlBulkCopy, dataTable);
                    }
                }
            }
        }

        public async void Update(IEnumerable<T> items,
        string tableName,
        int batchSize = 200)
        {
            await WithBulk(items, 200, async (a, b, c) =>
            {
                var tableClass = new TableClassModel(typeof(T));

                string createTableQuery = tableClass.CreateTableScript(string.Format("#{0}", tableName));

                // "CREATE TABLE #tblTmpPerson([ID] [int] NOT NULL, [Name][varchar](50) NULL, [Address] [varchar] (50) NULL);";
                using (SqlCommand command = a.LoadSqlCommand(createTableQuery, CommandType.Text))
                {
                    try
                    {
                        //Creating temp table on database
                        command.ExecuteNonQuery();

                        await b.WriteToServerAsync(c);
                        b.Close();

                        // Updating destination table, and dropping temp table
                        command.CommandTimeout = 300;
                        command.CommandText = tableClass.CreateUpdateScript(tableName);
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        // Handle exception properly
                    }
                    finally
                    {
                        command.Dispose();
                    }
                }
            });
        }
    }
}