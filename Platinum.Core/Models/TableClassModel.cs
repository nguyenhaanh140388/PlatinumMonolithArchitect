using Platinum.Core.Attributes;
using System.Reflection;
using System.Text;

namespace Platinum.Core.Models
{
    public class TableClassModel
    {
        private List<KeyValuePair<string, PropertyInfo>> _fieldInfo = new List<KeyValuePair<string, PropertyInfo>>();
        private string _className = string.Empty;

        private Dictionary<Type, string> dataMapper
        {
            get
            {
                // Add the rest of your CLR Types to SQL Types mapping here
                Dictionary<Type, string> dataMapper = new Dictionary<Type, string>();
                dataMapper.Add(typeof(int), "BIGINT");
                dataMapper.Add(typeof(string), "NVARCHAR(500)");
                dataMapper.Add(typeof(bool), "BIT");
                dataMapper.Add(typeof(DateTime), "DATETIME");
                dataMapper.Add(typeof(DateTime?), "DATETIME");
                dataMapper.Add(typeof(float), "FLOAT");
                dataMapper.Add(typeof(decimal), "DECIMAL(18,0)");
                dataMapper.Add(typeof(Guid), "UNIQUEIDENTIFIER");
                dataMapper.Add(typeof(Guid?), "UNIQUEIDENTIFIER");

                return dataMapper;
            }
        }

        public List<KeyValuePair<string, PropertyInfo>> Fields
        {
            get { return _fieldInfo; }
            set { _fieldInfo = value; }
        }

        public string ClassName
        {
            get { return _className; }
            set { _className = value; }
        }

        public TableClassModel(Type t)
        {
            _className = t.Name;

            foreach (PropertyInfo p in t.GetProperties())
            {
                var field = new KeyValuePair<string, PropertyInfo>(p.Name, p);

                Fields.Add(field);
            }
        }

        public string CreateTableScript(string tableName = "")
        {
            StringBuilder script = new StringBuilder();

            script.AppendLine("CREATE TABLE " + (string.IsNullOrEmpty(tableName) ? ClassName : tableName));
            script.AppendLine("(");
            //script.AppendLine("\t ID BIGINT,");
            for (int i = 0; i < Fields.Count; i++)
            {
                KeyValuePair<string, PropertyInfo> field = Fields[i];

                if (dataMapper.ContainsKey(field.Value.PropertyType))
                {
                    script.Append("\t " + field.Key + " " + dataMapper[field.Value.PropertyType]);
                }
                else
                {
                    // Complex Type? 
                    script.Append("\t " + field.Key + " BIGINT");
                }

                if (i != Fields.Count - 1)
                {
                    script.Append(",");
                }

                script.Append(Environment.NewLine);
            }

            script.AppendLine(")");

            return script.ToString();
        }

        public string CreateUpdateScript(string tableName = "")
        {
            StringBuilder script = new StringBuilder();
            List<string> matchKeys = new List<string>();

            var updatedTableName = string.IsNullOrEmpty(tableName) ? ClassName : tableName;

            script.AppendLine("UPDATE T ");
            script.AppendLine("SET ");

            for (int i = 0; i < Fields.Count; i++)
            {
                KeyValuePair<string, PropertyInfo> field = Fields[i];

                //if (dataMapper.ContainsKey(field.Value))
                //{
                //    script.Append("\t " + field.Key + " " + dataMapper[field.Value]);
                //}
                //else
                //{
                //    // Complex Type? 
                //    script.Append("\t " + field.Key + " BIGINT");
                //}

                //if (i != this.Fields.Count - 1)
                //{
                //    script.Append(",");
                //}

                var attribute = field.Value.GetCustomAttribute(typeof(MatchedColumnAttribute), true);
                if (attribute != null)
                {
                    matchKeys.Add(field.Key);
                }
            }

            script.AppendLine("FROM ");
            script.AppendLine(updatedTableName + " T ");
            script.AppendLine("INNER JOIN ");
            script.AppendLine(string.Format("#{0} temp ", updatedTableName));

            // compare key
            script.AppendLine(string.Format("ON T.{0} = temp.{0} ", updatedTableName));

            //script.AppendLine(")");

            return script.ToString();
        }
        // UPDATE T SET Name=Temp.Name, Address=Temp.Address FROM tblTmpPerson T INNER JOIN #tblTmpPerson Temp ON T.ID = Temp.ID; DROP TABLE #tblTmpPerson;
    }
}
