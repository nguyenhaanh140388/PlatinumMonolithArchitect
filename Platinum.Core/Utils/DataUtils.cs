#region License
/*
 **************************************************************
 *  Author: Rick Strahl 
 *          � West Wind Technologies, 2009
 *          http://www.west-wind.com/
 * 
 * Created: 09/12/2009
 *
 * Permission is hereby granted, free of charge, to any person
 * obtaining a copy of this software and associated documentation
 * files (the "Software"), to deal in the Software without
 * restriction, including without limitation the rights to use,
 * copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the
 * Software is furnished to do so, subject to the following
 * conditions:
 * 
 * The above copyright notice and this permission notice shall be
 * included in all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
 * OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
 * NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
 * HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
 * WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
 * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
 * OTHER DEALINGS IN THE SOFTWARE.
 **************************************************************  
*/
#endregion

using Microsoft.Data.SqlClient;
using Platinum.Core.Properties;
using System.Data;
using System.Reflection;
using System.Text;
using SystemDataCommon = System.Data.Common;

namespace Platinum.Core.Utils
{
    /// <summary>
    /// Utility library for common data operations.
    /// </summary>
    public static class DataUtils
    {
        private const BindingFlags MemberAccess =
            BindingFlags.Public | BindingFlags.NonPublic |
            BindingFlags.Static | BindingFlags.Instance | BindingFlags.IgnoreCase;

        //const BindingFlags MemberPublicInstanceAccess =
        //    BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase;

        #region Unique Ids and Random numbers
        /// <summary>
        /// Generates a unique Id as a string of up to 16 characters.
        /// Based on a GUID and the size takes that subset of a the
        /// Guid's 16 bytes to create a string id.
        /// 
        /// String Id contains numbers and lower case alpha chars 36 total.
        /// 
        /// Sizes: 6 gives roughly 99.97% uniqueness. 
        ///        8 gives less than 1 in a million doubles.
        ///        16 will give near full GUID strength uniqueness
        /// </summary>
        /// <param name="stringSize">Number of characters to generate between 8 and 16</param>
        /// <param name="additionalCharacters">Any additional characters you allow in the string. 
        /// You can add upper case letters and symbols which are not included in the default
        /// which includes only digits and lower case letters.
        /// </param>
        /// <returns></returns>        
        public static string GenerateUniqueId(int stringSize = 8, string additionalCharacters = null)
        {
            string chars = "abcdefghijkmnopqrstuvwxyz1234567890" + (additionalCharacters ?? string.Empty);
            StringBuilder result = new StringBuilder(stringSize);
            int count = 0;


            foreach (byte b in Guid.NewGuid().ToByteArray())
            {
                result.Append(chars[b % chars.Length]);
                count++;
                if (count >= stringSize)
                    break;
            }
            return result.ToString();
        }


        ///<summary>
        /// Generates a unique numeric ID. Generated off a GUID and
        /// returned as a 64 bit long value
        /// </summary>
        /// <returns></returns>
        public static long GenerateUniqueNumericId()
        {
            byte[] bytes = Guid.NewGuid().ToByteArray();
            return (long)BitConverter.ToUInt64(bytes, 0);
        }

        private static readonly Random rnd = new Random();

        /// <summary>
        /// Returns a random integer in a range of numbers
        /// a single seed value.
        /// </summary>
        /// <param name="min">The minimum value</param>
        /// <param name="max">The max value *including* this value (unlike Random.Next() which doesn't include it)</param>
        /// <returns></returns>
        public static int GetRandomNumber(int min, int max)
        {
            return rnd.Next(min, max + 1);
        }

        #endregion

        #region Byte Data

        /// <summary>
        /// Returns an index into a byte array to find sequence of
		/// of bytes.
		/// Note: You can use Span.IndexOf() where available instead.		
        /// </summary>
        /// <param name="buffer">byte array to be searched</param>
        /// <param name="bufferToFind">bytes to find</param>
        /// <returns></returns>
        public static int IndexOfByteArray(byte[] buffer, byte[] bufferToFind)
        {
            if (buffer.Length == 0 || bufferToFind.Length == 0)
                return -1;

            for (int i = 0; i < buffer.Length; i++)
            {
                if (buffer[i] == bufferToFind[0])
                {
                    bool innerMatch = true;
                    for (int j = 1; j < bufferToFind.Length; j++)
                    {
                        if (buffer[i + j] != bufferToFind[j])
                        {
                            innerMatch = false;
                            break;
                        }
                    }
                    if (innerMatch)
                        return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// Returns an index into a byte array to find a string in the byte array.
        /// Exact match using the encoding provided or UTF-8 by default.
        /// </summary>
        /// <param name="buffer">Source buffer to look for string</param>
        /// <param name="stringToFind">string to search for (case sensitive)</param>
        /// <param name="encoding">Optional encoding to use - defaults to UTF-8 if null</param>
        /// <returns></returns>
        public static int IndexOfByteArray(byte[] buffer, string stringToFind, Encoding encoding = null)
        {
            if (encoding == null)
                encoding = Encoding.UTF8;

            if (buffer.Length == 0 || string.IsNullOrEmpty(stringToFind))
                return -1;

            var bytes = encoding.GetBytes(stringToFind);

            return IndexOfByteArray(buffer, bytes);
        }

        /// <summary>
        /// Removes a sequence of bytes from a byte array
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="bytesToRemove"></param>
        /// <returns></returns>
        public static byte[] RemoveBytes(byte[] buffer, byte[] bytesToRemove)
        {
            if (buffer == null || buffer.Length == 0 ||
                bytesToRemove == null || bytesToRemove.Length == 0)
                return buffer;

            using (var ms = new MemoryStream())
            using (var bw = new BinaryWriter(ms))
            {
                var firstByte = bytesToRemove[0];

                for (int i = 0; i < buffer.Length; i++)
                {
                    var current = buffer[i];
                    if (current == firstByte && buffer.Length >= i + bytesToRemove.Length)
                    {
                        bool found = true;
                        for (int y = 1; y < bytesToRemove.Length; y++)
                        {
                            if (buffer[i + y] != bytesToRemove[y])
                            {
                                found = false;
                                break;
                            }
                        }

                        if (found)
                            i += bytesToRemove.Length - 1; // skip over
                        else
                            bw.Write(current);
                    }
                    else
                        bw.Write(current);

                }

                return ms.ToArray();
            }
        }

        #endregion

        #region Copying Objects and Data

        /// <summary>
        /// Copies the content of a data row to another. Runs through the target's fields
        /// and looks for fields of the same name in the source row. Structure must mathc
        /// or fields are skipped.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool CopyDataRow(DataRow source, DataRow target)
        {
            DataColumnCollection columns = target.Table.Columns;

            for (int x = 0; x < columns.Count; x++)
            {
                string fieldname = columns[x].ColumnName;

                try
                {
                    target[x] = source[fieldname];
                }
                catch {; }  // skip any errors
            }

            return true;
        }

        /// <summary>
        /// Populates an object passed in from values in
        /// a data row that's passed in.
        /// </summary>
        /// <param name="row">Data row with values to fill from</param>
        /// <param name="targetObject">Object to file values from data row</param>
        public static void CopyObjectFromDataRow(DataRow row, object targetObject, MemberInfo[] cachedMemberInfo = null)
        {
            if (cachedMemberInfo == null)
            {
                cachedMemberInfo = targetObject.GetType()
                    .FindMembers(MemberTypes.Field | MemberTypes.Property,
                        ReflectionUtils.MemberAccess, null, null);
            }
            foreach (MemberInfo Field in cachedMemberInfo)
            {
                string Name = Field.Name;
                if (!row.Table.Columns.Contains(Name))
                    continue;

                object value = row[Name];
                if (value == DBNull.Value)
                    value = null;

                if (Field.MemberType == MemberTypes.Field)
                {
                    ((FieldInfo)Field).SetValue(targetObject, value);
                }
                else if (Field.MemberType == MemberTypes.Property)
                {
                    ((PropertyInfo)Field).SetValue(targetObject, value, null);
                }
            }
        }

        /// <summary>
        /// Copies the content of an object to a DataRow with matching field names.
        /// Both properties and fields are copied. If a field copy fails due to a
        /// type mismatch copying continues but the method returns false
        /// </summary>
        /// <param name="row"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool CopyObjectToDataRow(DataRow row, object target)
        {
            bool result = true;

            MemberInfo[] miT = target.GetType().FindMembers(MemberTypes.Field | MemberTypes.Property, MemberAccess, null, null);
            foreach (MemberInfo Field in miT)
            {
                string name = Field.Name;
                if (!row.Table.Columns.Contains(name))
                    continue;

                try
                {
                    if (Field.MemberType == MemberTypes.Field)
                    {
                        row[name] = ((FieldInfo)Field).GetValue(target) ?? DBNull.Value;
                    }
                    else if (Field.MemberType == MemberTypes.Property)
                    {
                        row[name] = ((PropertyInfo)Field).GetValue(target, null) ?? DBNull.Value;
                    }
                }
                catch { result = false; }
            }

            return result;
        }

        /// <summary>
        /// Coverts a DataTable to a typed list of items
        /// </summary>
        /// <typeparam name="T">Type to </typeparam>
        /// <param name="dsTable"></param>
        /// <returns></returns>
        public static List<T> DataTableToTypedList<T>(DataTable dsTable) where T : class, new()
        {
            var objectList = new List<T>();

            MemberInfo[] cachedMemberInfo = null;
            foreach (DataRow dr in dsTable.Rows)
            {
                var obj = default(T); // Activator.CreateInstance<T>();				
                CopyObjectFromDataRow(dr, obj, cachedMemberInfo);
                objectList.Add(obj);
            }

            return objectList;
        }




        /// <summary>
        /// Copies the content of one object to another. The target object 'pulls' properties of the first. 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public static void CopyObjectData(object source, object target)
        {
            CopyObjectData(source, target, MemberAccess);
        }

        /// <summary>
        /// Copies the content of one object to another. The target object 'pulls' properties of the first. 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <param name="memberAccess"></param>
        public static void CopyObjectData(object source, object target, BindingFlags memberAccess)
        {
            CopyObjectData(source, target, null, memberAccess);
        }

        /// <summary>
        /// Copies the content of one object to another. The target object 'pulls' properties of the first. 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <param name="excludedProperties"></param>
        public static void CopyObjectData(object source, object target, string excludedProperties)
        {
            CopyObjectData(source, target, excludedProperties, MemberAccess);
        }

        /// <summary>
        /// Copies the data of one object to another. The target object 'pulls' properties of the first. 
        /// This any matching properties are written to the target.
        /// 
        /// The object copy is a shallow copy only. Any nested types will be copied as 
        /// whole values rather than individual property assignments (ie. via assignment)
        /// </summary>
        /// <param name="source">The source object to copy from</param>
        /// <param name="target">The object to copy to</param>
        /// <param name="excludedProperties">A comma delimited list of properties that should not be copied</param>
        /// <param name="memberAccess">Reflection binding access</param>
        public static void CopyObjectData(object source, object target, string excludedProperties = null, BindingFlags memberAccess = MemberAccess)
        {
            string[] excluded = null;
            if (!string.IsNullOrEmpty(excludedProperties))
                excluded = excludedProperties.Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            MemberInfo[] miT = target.GetType().GetMembers(memberAccess);
            foreach (MemberInfo Field in miT)
            {
                string name = Field.Name;

                // Skip over any property exceptions
                if (!string.IsNullOrEmpty(excludedProperties) &&
                    excluded.Contains(name))
                    continue;

                if (Field.MemberType == MemberTypes.Field)
                {
                    FieldInfo SourceField = source.GetType().GetField(name);
                    if (SourceField == null)
                        continue;

                    object SourceValue = SourceField.GetValue(source);
                    ((FieldInfo)Field).SetValue(target, SourceValue);
                }
                else if (Field.MemberType == MemberTypes.Property)
                {
                    PropertyInfo piTarget = Field as PropertyInfo;
                    PropertyInfo SourceField = source.GetType().GetProperty(name, memberAccess);
                    if (SourceField == null)
                        continue;

                    if (piTarget.CanWrite && SourceField.CanRead)
                    {
                        object SourceValue = SourceField.GetValue(source, null);
                        piTarget.SetValue(target, SourceValue, null);
                    }
                }
            }
        }
        #endregion

        #region DataTable and DataReader

        /// <summary>
        /// Coverts a DataTable to a typed list of items
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dsTable"></param>
        /// <returns></returns>
        public static List<T> DataTableToObjectList<T>(DataTable dsTable) where T : class, new()
        {
            var objectList = new List<T>();

            foreach (DataRow dr in dsTable.Rows)
            {
                var obj = Activator.CreateInstance<T>();
                CopyObjectFromDataRow(dr, obj);
                objectList.Add(obj);
            }

            return objectList;
        }


        /// <summary>
        /// Creates a list of a given type from all the rows in a DataReader.
        /// 
        /// Note this method uses Reflection so this isn't a high performance
        /// operation, but it can be useful for generic data reader to entity
        /// conversions on the fly and with anonymous types.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader">An open DataReader that's in position to read</param>
        /// <param name="propertiesToSkip">Optional - comma delimited list of fields that you don't want to update</param>
        /// <param name="piList">
        /// Optional - Cached PropertyInfo dictionary that holds property info data for this object.
        /// Can be used for caching hte PropertyInfo structure for multiple operations to speed up
        /// translation. If not passed automatically created.
        /// </param>
        /// <returns></returns>
        /// <remarks>DataReader is not closed by this method. Make sure you call reader.close() afterwards</remarks>
        public static List<T> DataReaderToObjectList<T>(IDataReader reader, string propertiesToSkip = null, Dictionary<string, PropertyInfo> piList = null)
            where T : new()
        {
            List<T> list = new List<T>();

            using (reader)
            {
                // Get a list of PropertyInfo objects we can cache for looping            
                if (piList == null)
                {
                    piList = new Dictionary<string, PropertyInfo>();
                    var props = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);
                    foreach (var prop in props)
                        piList.Add(prop.Name.ToLower(), prop);
                }

                while (reader.Read())
                {
                    T inst = new T();
                    DataReaderToObject(reader, inst, propertiesToSkip, piList);
                    list.Add(inst);
                }
            }

            return list;
        }

        /// <summary>
        /// Creates an IEnumerable of T from an open DataReader instance.
        ///
        /// Note this method uses Reflection so this isn't a high performance
        /// operation, but it can be useful for generic data reader to entity
        /// conversions on the fly and with anonymous types.
        /// </summary>
        /// <param name="reader">An open DataReader that's in position to read</param>
        /// <param name="propertiesToSkip">Optional - comma delimited list of fields that you don't want to update</param>
        /// <param name="piList">
        /// Optional - Cached PropertyInfo dictionary that holds property info data for this object.
        /// Can be used for caching hte PropertyInfo structure for multiple operations to speed up
        /// translation. If not passed automatically created.
        /// </param>
        /// <returns></returns>
        public static IEnumerable<T> DataReaderToIEnumerable<T>(IDataReader reader, string propertiesToSkip = null, Dictionary<string, PropertyInfo> piList = null)
            where T : new()
        {
            if (reader != null)
            {
                using (reader)
                {
                    // Get a list of PropertyInfo objects we can cache for looping            
                    if (piList == null)
                    {
                        piList = new Dictionary<string, PropertyInfo>();
                        var props = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);
                        foreach (var prop in props)
                            piList.Add(prop.Name.ToLower(), prop);
                    }

                    while (reader.Read())
                    {
                        T inst = new T();
                        DataReaderToObject(reader, inst, propertiesToSkip, piList);
                        yield return inst;
                    }
                }
            }
        }

        public static List<T> DataReaderToList<T>(IDataReader reader, string propertiesToSkip = null, Dictionary<string, PropertyInfo> piList = null)
          where T : new()
        {
            var list = new List<T>();

            if (reader != null)
            {
                using (reader)
                {
                    // Get a list of PropertyInfo objects we can cache for looping            
                    if (piList == null)
                    {
                        piList = new Dictionary<string, PropertyInfo>();
                        var props = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);
                        foreach (var prop in props)
                            piList.Add(prop.Name.ToLower(), prop);
                    }

                    while (reader.Read())
                    {
                        T inst = new T();
                        DataReaderToObject(reader, inst, propertiesToSkip, piList);
                        list.Add(inst);
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// Populates the properties of an object from a single DataReader row using
        /// Reflection by matching the DataReader fields to a public property on
        /// the object passed in. Unmatched properties are left unchanged.
        /// 
        /// You need to pass in a data reader located on the active row you want
        /// to serialize.
        /// 
        /// This routine works best for matching pure data entities and should
        /// be used only in low volume environments where retrieval speed is not
        /// critical due to its use of Reflection.
        /// </summary>
        /// <param name="reader">Instance of the DataReader to read data from. Should be located on the correct record (Read() should have been called on it before calling this method)</param>
        /// <param name="instance">Instance of the object to populate properties on</param>
        /// <param name="propertiesToSkip">Optional - A comma delimited list of object properties that should not be updated</param>
        /// <param name="piList">Optional - Cached PropertyInfo dictionary that holds property info data for this object</param>
        public static void DataReaderToObject(IDataReader reader, object instance,
                                              string propertiesToSkip = null,
                                              Dictionary<string, PropertyInfo> piList = null)
        {
            if (reader.IsClosed)
                throw new InvalidOperationException(Resources.DataReaderPassedToDataReaderToObjectCannot);

            if (string.IsNullOrEmpty(propertiesToSkip))
                propertiesToSkip = string.Empty;
            else
                propertiesToSkip = "," + propertiesToSkip + ",";

            propertiesToSkip = propertiesToSkip.ToLower();

            // create a dictionary of properties to look up
            // we can pass this in so we can cache the list once 
            // for a list operation 
            if (piList == null || piList.Count < 1)
            {
                if (piList == null)
                    piList = new Dictionary<string, PropertyInfo>();

                var props = instance.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
                foreach (var prop in props)
                    piList.Add(prop.Name.ToLower(), prop);
            }

            for (int index = 0; index < reader.FieldCount; index++)
            {
                string name = reader.GetName(index).ToLower();
                if (piList.ContainsKey(name))
                {
                    var prop = piList[name];

                    // skip fields in skip list
                    if (!string.IsNullOrEmpty(propertiesToSkip) && propertiesToSkip.Contains("," + name + ","))
                        continue;

                    // find writable properties and assign
                    if (prop != null && prop.CanWrite)
                    {
                        var val = reader.GetValue(index);

                        if (val == DBNull.Value)
                            val = null;
                        // deal with data drivers return bit values as int64 or in
                        else if (prop.PropertyType == typeof(bool) && (val is long || val is int))
                            val = Convert.ToInt64(val) == 1;
                        // int conversions when the value is not different type of number
                        else if (prop.PropertyType == typeof(int) && (val is long || val is decimal))
                            val = Convert.ToInt32(val);

                        prop.SetValue(instance, val, null);
                    }
                }
            }

            return;
        }


        /// <summary>
        /// The default SQL date used by InitializeDataRowWithBlanks. Considered a blank date instead of null.
        /// </summary>
        public static DateTime MinimumSqlDate = DateTime.Parse("01/01/1900");

        /// <summary>
        /// Initializes a Datarow containing NULL values with 'empty' values instead.
        /// Empty values are:
        /// String - ""
        /// all number types - 0 or 0.00
        /// DateTime - Value of MinimumSqlData (1/1/1900 by default);
        /// Boolean - false
        /// Binary values and timestamps are left alone
        /// </summary>
        /// <param name="row">DataRow to be initialized</param>
        public static void InitializeDataRowWithBlanks(DataRow row)
        {
            DataColumnCollection loColumns = row.Table.Columns;

            for (int x = 0; x < loColumns.Count; x++)
            {
                if (!row.IsNull(x))
                    continue;

                string lcRowType = loColumns[x].DataType.Name;

                if (lcRowType == "String")
                    row[x] = string.Empty;
                else if (lcRowType.StartsWith("Int"))
                    row[x] = 0;
                else if (lcRowType == "Byte")
                    row[x] = 0;
                else if (lcRowType == "Decimal")
                    row[x] = 0.00M;
                else if (lcRowType == "Double")
                    row[x] = 0.00;
                else if (lcRowType == "Boolean")
                    row[x] = false;
                else if (lcRowType == "DateTime")
                    row[x] = MinimumSqlDate;

                // Everything else isn't handled explicitly and left alone
                // Byte[] most specifically

            }
        }

        #endregion

        #region Provider Factories

        /// <summary>
        /// Loads a SQL Provider factory based on the DbFactory type name and assembly.       
        /// </summary>
        /// <param name="dbProviderFactoryTypename">Type name of the DbProviderFactory</param>
        /// <param name="assemblyName">Short assembly name of the provider factory. Note: Host project needs to have a reference to this assembly</param>
        /// <returns></returns>
        public static SystemDataCommon.DbProviderFactory GetDbProviderFactory(string dbProviderFactoryTypename, string assemblyName)
        {
            var instance = ReflectionUtils.GetStaticProperty(dbProviderFactoryTypename, "Instance");
            if (instance == null)
            {
                var a = ReflectionUtils.LoadAssembly(assemblyName);
                if (a != null)
                    instance = ReflectionUtils.GetStaticProperty(dbProviderFactoryTypename, "Instance");
            }

            if (instance == null)
                throw new InvalidOperationException(string.Format(Resources.UnableToRetrieveDbProviderFactoryForm, dbProviderFactoryTypename));

            return instance as SystemDataCommon.DbProviderFactory;
        }

        /// <summary>
        /// This method loads various providers dynamically similar to the 
        /// way that DbProviderFactories.GetFactory() works except that
        /// this API is not available on .NET Standard 2.0
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static SystemDataCommon.DbProviderFactory GetDbProviderFactory(DataAccessProviderTypes type)
        {
            if (type == DataAccessProviderTypes.SqlServer)
                return SqlClientFactory.Instance; // this library has a ref to SqlClient so this works

            if (type == DataAccessProviderTypes.SqLite)
                return GetDbProviderFactory("System.Data.SQLite.SQLiteFactory", "System.Data.SQLite");
            if (type == DataAccessProviderTypes.MySql)
                return GetDbProviderFactory("MySql.Data.MySqlClient.MySqlClientFactory", "MySql.Data");
            if (type == DataAccessProviderTypes.PostgreSql)
                return GetDbProviderFactory("Npgsql.NpgsqlFactory", "Npgsql");
#if NETFULL
            if (type == DataAccessProviderTypes.OleDb)
                return System.Data.OleDb.OleDbFactory.Instance;
            if (type == DataAccessProviderTypes.SqlServerCompact)
                return DbProviderFactories.GetFactory("System.Data.SqlServerCe.4.0");                
#endif

            throw new NotSupportedException(string.Format(Resources.UnsupportedProviderFactory, type.ToString()));
        }



        /// <summary>
        /// Returns a provider factory using the old Provider Model names from full framework .NET.
        /// Simply calls DbProviderFactories.
        /// </summary>
        /// <param name="providerName"></param>
        /// <returns></returns>
        public static SystemDataCommon.DbProviderFactory GetDbProviderFactory(string providerName)
        {
#if NETFULL
            return DbProviderFactories.GetFactory(providerName);
#else
            var lowerProvider = providerName.ToLower();

            if (lowerProvider == "system.data.sqlclient")
                return GetDbProviderFactory(DataAccessProviderTypes.SqlServer);
            if (lowerProvider == "system.data.sqlite" || lowerProvider == "microsoft.data.sqlite")
                return GetDbProviderFactory(DataAccessProviderTypes.SqLite);
            if (lowerProvider == "mysql.data.mysqlclient" || lowerProvider == "mysql.data")
                return GetDbProviderFactory(DataAccessProviderTypes.MySql);
            if (lowerProvider == "npgsql")
                return GetDbProviderFactory(DataAccessProviderTypes.PostgreSql);

            throw new NotSupportedException(string.Format(Resources.UnsupportedProviderFactory, providerName));
#endif
        }

        #endregion


        //#region Type Conversions
        ///// <summary>
        ///// Maps a SqlSystemData.DbType to a .NET type
        ///// </summary>
        ///// <param name="sqlType"></param>
        ///// <returns></returns>
        //public static Type SqlTypeToDotNetType(SqlSystemData.DbType sqlType)
        //{
        //    if (sqlType ==System.Data.SqlSystemData.DbType.NText || sqlType == SqlSystemData.DbType.Text ||
        //        sqlType == SqlSystemData.DbType.VarChar || sqlType == SqlSystemData.DbType.NVarChar)
        //        return typeof(string);
        //    else if (sqlType == SqlSystemData.DbType.Int)
        //        return typeof(Int32);
        //    else if (sqlType == SqlSystemData.DbType.Decimal || sqlType == SqlSystemData.DbType.Money)
        //        return typeof(decimal);
        //    else if (sqlType == SqlSystemData.DbType.Bit)
        //        return typeof(Boolean);
        //    else if (sqlType == SqlSystemData.DbType.DateTime || sqlType == SqlSystemData.DbType.DateTime2)
        //        return typeof(DateTime);
        //    else if (sqlType == SqlSystemData.DbType.Char || sqlType == SqlSystemData.DbType.NChar)
        //        return typeof(char);
        //    else if (sqlType == SqlSystemData.DbType.Float)
        //        return typeof(Single);
        //    else if (sqlType == SqlSystemData.DbType.Real)
        //        return typeof(Double);
        //    else if (sqlType == SqlSystemData.DbType.BigInt)
        //        return typeof(Int64);
        //    else if (sqlType == SqlSystemData.DbType.Image)
        //        return typeof(byte[]);
        //    else if (sqlType == SqlSystemData.DbType.SmallInt)
        //        return typeof(byte);

        //    throw new InvalidCastException("Unable to convert " + sqlType.ToString() + " to .NET type.");
        //}

        ///// <summary>
        ///// Maps a SystemData.DbType to a .NET native type
        ///// </summary>
        ///// <param name="sqlType"></param>
        ///// <returns></returns>
        //public static Type SystemData.DbTypeToDotNetType(SystemData.DbType sqlType)
        //{
        //    if (sqlType == SystemData.DbType.String || sqlType == SystemData.DbType.StringFixedLength || sqlType == SystemData.DbType.AnsiString)
        //        return typeof(string);
        //    else if (sqlType == SystemData.DbType.Int16 || sqlType == SystemData.DbType.Int32)
        //        return typeof(Int32);
        //    else if (sqlType == SystemData.DbType.Int64)
        //        return typeof(Int64);
        //    else if (sqlType == SystemData.DbType.Decimal || sqlType == SystemData.DbType.Currency)
        //        return typeof(decimal);
        //    else if (sqlType == SystemData.DbType.Boolean)
        //        return typeof(Boolean);
        //    else if (sqlType == SystemData.DbType.DateTime || sqlType == SystemData.DbType.DateTime2 || sqlType == SystemData.DbType.Date)
        //        return typeof(DateTime);
        //    else if (sqlType == SystemData.DbType.Single)
        //        return typeof(Single);
        //    else if (sqlType == SystemData.DbType.Double)
        //        return typeof(Double);
        //    else if (sqlType == SystemData.DbType.Binary)
        //        return typeof(byte[]);
        //    else if (sqlType == SystemData.DbType.SByte || sqlType == SystemData.DbType.Byte)
        //        return typeof(byte);
        //    else if (sqlType == SystemData.DbType.Guid)
        //        return typeof(Guid);
        //    else if (sqlType == SystemData.DbType.Binary)
        //        return typeof(byte[]);

        //    throw new InvalidCastException("Unable to convert " + sqlType.ToString() + " to .NET type.");
        //}

        ///// <summary>
        ///// Converts a .NET type into a SystemData.DbType value
        ///// </summary>
        ///// <param name="type"></param>
        ///// <returns></returns>
        //public static SystemData.DbType DotNetTypeToSystemData.DbType(Type type)
        //{
        //    if (type == typeof(string))
        //        return SystemData.DbType.String;
        //    else if (type == typeof(Int32))
        //        return SystemData.DbType.Int32;
        //    else if (type == typeof(Int16))
        //        return SystemData.DbType.Int16;
        //    else if (type == typeof(Int64))
        //        return SystemData.DbType.Int64;
        //    else if (type == typeof(Guid))
        //        return SystemData.DbType.Guid;
        //    else if (type == typeof(decimal))
        //        return SystemData.DbType.Decimal;
        //    else if (type == typeof(double) || type == typeof(float))
        //        return SystemData.DbType.Double;
        //    else if (type == typeof(Single))
        //        return SystemData.DbType.Single;
        //    else if (type == typeof(bool) || type == typeof(Boolean))
        //        return SystemData.DbType.Boolean;
        //    else if (type == typeof(DateTime))
        //        return SystemData.DbType.DateTime;
        //    else if (type == typeof(DateTimeOffset))
        //        return SystemData.DbType.DateTimeOffset;
        //    else if (type == typeof(byte))
        //        return SystemData.DbType.Byte;
        //    else if (type == typeof(byte[]))
        //        return SystemData.DbType.Binary;

        //    throw new InvalidCastException(string.Format("Unable to cast {0} to a SystemData.DbType", type.Name));
        //}

        ///// <summary>
        ///// Converts a .NET type into a SqlSystemData.DbType.
        ///// </summary>
        ///// <param name="type"></param>
        ///// <returns></returns>
        //public static DbType.SqlSystemData.DbType DotNetTypeToSqlType(Type type)
        //{
        //    if (type == typeof(string))
        //        return DbType.DbType.NVarChar;
        //    else if (type == typeof(Int32))
        //        return SqlSystemData.DbType.Int;
        //    else if (type == typeof(Int16))
        //        return SqlSystemData.DbType.SmallInt;
        //    else if (type == typeof(Int64))
        //        return SqlSystemData.DbType.BigInt;
        //    else if (type == typeof(Guid))
        //        return SqlSystemData.DbType.UniqueIdentifier;
        //    else if (type == typeof(decimal))
        //        return SqlSystemData.DbType.Decimal;
        //    else if (type == typeof(double) || type == typeof(float))
        //        return SqlSystemData.DbType.Float;
        //    else if (type == typeof(Single))
        //        return SqlSystemData.DbType.Float;
        //    else if (type == typeof(bool) || type == typeof(Boolean))
        //        return SqlSystemData.DbType.Bit;
        //    else if (type == typeof(DateTime))
        //        return SqlSystemData.DbType.DateTime;
        //    else if (type == typeof(DateTimeOffset))
        //        return SqlSystemData.DbType.DateTimeOffset;
        //    else if (type == typeof(byte))
        //        return SqlSystemData.DbType.SmallInt;
        //    else if (type == typeof(byte[]))
        //        return SqlSystemData.DbType.Image;

        //    throw new InvalidCastException(string.Format("Unable to cast {0} to a SystemData.DbType", type.Name));
        //}

        //#endregion

        #region Minimal Sql Data Access Function

        /// <summary>
        /// Creates a Command object and opens a connection
        /// </summary>
        /// <param name="ConnectionString"></param>
        /// <param name="Sql"></param>
        /// <returns></returns>
        public static SqlCommand GetSqlCommand(string ConnectionString, string Sql, params SqlParameter[] Parameters)
        {
            SqlCommand Command = new SqlCommand
            {
                CommandText = Sql
            };

            try
            {
#if NETFULL
				if (!ConnectionString.Contains(';'))
                    ConnectionString =  ConfigurationManager.ConnectionStrings[ConnectionString].ConnectionString;
#endif

                Command.Connection = new SqlConnection(ConnectionString);
                Command.Connection.Open();
            }
            catch
            {
                return null;
            }


            if (Parameters != null)
            {
                foreach (SqlParameter Parm in Parameters)
                {
                    Command.Parameters.Add(Parm);
                }
            }

            return Command;
        }

        /// <summary>
        /// Returns a SqlDataReader object from a SQL string.
        /// 
        /// Please ensure you close the Reader object
        /// </summary>
        /// <param name="ConnectionString"></param>
        /// <param name="Sql"></param>
        /// <param name="Parameters"></param>
        /// <returns></returns>
        public static SqlDataReader GetSqlDataReader(string ConnectionString, string Sql, params SqlParameter[] Parameters)
        {
            SqlCommand Command = GetSqlCommand(ConnectionString, Sql, Parameters);
            if (Command == null)
                return null;

            SqlDataReader Reader;
            try
            {
                Reader = Command.ExecuteReader();
            }
            catch
            {
                CloseConnection(Command);
                return null;
            }

            return Reader;
        }

        /// <summary>
        /// Returns a DataTable from a Sql Command string passed in.
        /// </summary>
        /// <param name="Tablename"></param>
        /// <param name="ConnectionString"></param>
        /// <param name="Sql"></param>
        /// <param name="Parameters"></param>
        /// <returns></returns>
        public static DataTable GetDataTable(string Tablename, string ConnectionString, string Sql, params SqlParameter[] Parameters)
        {
            SqlCommand Command = GetSqlCommand(ConnectionString, Sql, Parameters);
            if (Command == null)
                return null;

            SqlDataAdapter Adapter = new SqlDataAdapter(Command);

            DataTable dt = new DataTable(Tablename);

            try
            {
                Adapter.Fill(dt);
            }
            catch
            {
                return null;
            }
            finally
            {
                CloseConnection(Command);
            }

            return dt;
        }


        /// <summary>
        /// Closes a connection
        /// </summary>
        /// <param name="Command"></param>
        public static void CloseConnection(SqlCommand Command)
        {
            if (Command.Connection != null &&
                Command.Connection.State == ConnectionState.Open)
                Command.Connection.Close();
        }
        #endregion

    }

    public enum DataAccessProviderTypes
    {
        SqlServer,
        SqLite,
        MySql,
        PostgreSql,

#if NETFULL
        OleDb,
        SqlServerCompact
#endif
    }
}
