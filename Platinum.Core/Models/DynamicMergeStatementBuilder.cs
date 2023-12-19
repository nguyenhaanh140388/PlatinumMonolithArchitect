using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Platinum.Core.Models
{
    public class DynamicMergeStatementBuilder
    {
        private string SourceStatement { get; set; }

        private string TargetStatement { get; set; }

        private string MatchStatement { get; set; }

        private string InsertStatement { get; set; }

        private string UpdateStatement { get; set; }

        private string DeleteStatement { get; set; }

        private string OutputStatement { get; set; }

        public static string CreateTempTableStatement(string tempTableName, Dictionary<string, string> columnWithDataTypes)
        {
            if (string.IsNullOrWhiteSpace(tempTableName))
            {
                throw new ArgumentNullException("Specified value for argument tempTableName is empty");
            }

            ValidateArgument(columnWithDataTypes);

            StringBuilder builder = new StringBuilder();
            string concatenatedCols = string.Join(",", columnWithDataTypes.Select(c => string.Format("[{0}] {1}", c.Key, GetSqlTypeFromFriendlyName(c.Value))));
            builder.AppendFormat("CREATE TABLE {0} ({1})", tempTableName, concatenatedCols);
            return builder.ToString();
        }

        public DynamicMergeStatementBuilder WithTables(string sourceTable, string targetTable)
        {
            SourceStatement = string.Format("MERGE INTO {0} as t ", targetTable);
            TargetStatement = string.Format("USING {0} as s ", sourceTable);
            return this;
        }

        public DynamicMergeStatementBuilder MatchOn(List<string> columns)
        {
            return MatchOn(columns.ToDictionary(e => e, e => e));
        }

        public DynamicMergeStatementBuilder MatchOn(Dictionary<string, string> sourceTargetMapping)
        {
            ValidateArgument(sourceTargetMapping);

            StringBuilder builder = new StringBuilder();
            builder.AppendLine("ON");
            foreach (var sourceColumn in sourceTargetMapping.Keys)
            {
                builder.AppendFormat("t.[{0}] = s.[{1}] ", sourceColumn, sourceTargetMapping[sourceColumn]);
                builder.Append("AND ");
            }

            builder.Remove(builder.Length - 4, 4);
            MatchStatement = builder.ToString();
            return this;
        }

        public DynamicMergeStatementBuilder AddInsertStatement(List<string> columns, bool hasInsert)
        {
            return hasInsert ? AddInsertStatement(columns.ToDictionary(e => e, e => e)) : this;
        }

        public DynamicMergeStatementBuilder AddInsertStatement(Dictionary<string, string> sourceTargetMapping)
        {
            ValidateArgument(sourceTargetMapping);

            StringBuilder builder = new StringBuilder();
            builder.AppendLine("WHEN NOT MATCHED THEN INSERT");
            builder.AppendFormat("({0}) ", string.Join(",", sourceTargetMapping.Values.Select(c => string.Format("[{0}]", c)).ToArray()));
            builder.Append(string.Format("VALUES ("));
            foreach (var sourceColumn in sourceTargetMapping.Keys)
            {
                builder.AppendFormat("s.[{0}]", sourceColumn);
                builder.Append(",");
            }

            builder.Remove(builder.Length - 1, 1);
            builder.Append(")");
            InsertStatement = builder.ToString();
            return this;
        }

        public DynamicMergeStatementBuilder AddUpdateStatement(List<string> columns, bool hasUpdate)
        {
            return hasUpdate ? AddUpdateStatement(sourceTargetMapping: columns.ToDictionary(e => e, e => e)
                , hasUpdate: hasUpdate) : this;
        }

        public DynamicMergeStatementBuilder AddUpdateStatement(Dictionary<string, string> sourceTargetMapping
            , bool hasUpdate
            , List<string> targetColumnsWithStaticValues = null)
        {
            if (!hasUpdate)
                return this;

            ValidateArgument(sourceTargetMapping);

            StringBuilder builder = new StringBuilder();
            builder.AppendLine("WHEN MATCHED THEN UPDATE SET");
            foreach (var sourceColumn in sourceTargetMapping.Keys)
            {
                if (targetColumnsWithStaticValues != null && targetColumnsWithStaticValues.Contains(sourceColumn))
                {
                    builder.AppendFormat("t.[{0}] = {1} ", sourceColumn, sourceTargetMapping[sourceColumn]);
                }
                else
                {
                    builder.AppendFormat("t.[{0}] = s.[{1}] ", sourceTargetMapping[sourceColumn], sourceColumn);
                }

                builder.Append(",");
            }

            builder.Remove(builder.Length - 1, 1);
            UpdateStatement = builder.ToString();
            return this;
        }

        public DynamicMergeStatementBuilder AddDeleteStatement(bool hasDelete)
        {
            DeleteStatement = hasDelete ? "WHEN NOT MATCHED BY SOURCE THEN DELETE" : string.Empty;
            return this;
        }

        public DynamicMergeStatementBuilder AddOutputStatement(List<string> columns, bool hasOutput)
        {
            if (!hasOutput) return this;

            StringBuilder builder = new StringBuilder();
            builder.AppendLine("OUTPUT $action as Action,");

            foreach (var sourceColumn in columns)
            {
                builder.AppendFormat("DELETED.{0} AS Target{0}", sourceColumn);
                builder.Append(",");
            }

            foreach (var sourceColumn in columns)
            {
                builder.AppendFormat("INSERTED.{0} AS Source{0}", sourceColumn);
                builder.Append(",");
            }

            builder.Remove(builder.Length - 1, 1);

            OutputStatement = builder.ToString();
            return this;
        }

        public string Build()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine(SourceStatement);
            builder.AppendLine(TargetStatement);
            builder.AppendLine(MatchStatement);
            builder.AppendLine(InsertStatement);
            builder.AppendLine(UpdateStatement);
            builder.AppendLine(DeleteStatement);
            builder.AppendLine(OutputStatement);
            builder.Append(";");

            return builder.ToString();
        }

        private static void ValidateArgument(Dictionary<string, string> sourceTargetMapping)
        {
            if (sourceTargetMapping == null || sourceTargetMapping.Count == 0)
            {
                throw new ArgumentNullException("Number of columns must be greater than zero");
            }
        }

        private static string GetSqlTypeFromFriendlyName(string typeName)
        {
            if (typeName == null)
            {
                throw new ArgumentNullException("typeName");
            }

            int minimumPrecision = 10;

            if (typeName.Contains("("))
            {
                minimumPrecision = int.Parse(typeName.Substring(typeName.IndexOf("(") + 1, typeName.IndexOf(")") - typeName.IndexOf("(") - 1));
                typeName = typeName.Remove(typeName.IndexOf("("), 3);
            }

            typeName = typeName.ToLower();

            string parsedTypeName = null;
            switch (typeName)
            {
                case "bool":
                case "boolean":
                    parsedTypeName = "[bit]";
                    break;
                case "datetime":
                    parsedTypeName = "[datetime]";
                    break;
                case "float":
                    parsedTypeName = string.Format("[decimal](18,{0})", minimumPrecision);
                    break;
                case "int32":
                case "int":
                    parsedTypeName = "[int]";
                    break;
                case "string":
                    parsedTypeName = "[nvarchar](MAX)";
                    break;
            }

            return parsedTypeName;
        }
    }
}
