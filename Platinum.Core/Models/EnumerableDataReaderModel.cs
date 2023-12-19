using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Platinum.Core.Models
{
    static class DataReaderExtensions
    {
        public static IDataReader AsDataReader<TSource>(this IEnumerable<TSource> source, int fieldCount, Func<TSource, int, object> getValue)
        {
            return EnumerableDataReader.Create(source, fieldCount, getValue);
        }
    }

    internal static class EnumerableDataReader
    {
        public static IDataReader Create<TSource>(IEnumerable<TSource> source, int fieldCount, Func<TSource, int, object> getValue)
        {
            return new EnumerableDataReaderModel<TSource>(source.GetEnumerator(), fieldCount, getValue);
        }
    }

    internal class EnumerableDataReaderModel<TSource> : IDataReader
    {
        private readonly IEnumerator<TSource> _source;
        private readonly int _fieldCount;
        private readonly Func<TSource, int, object> _getValue;

        internal EnumerableDataReaderModel(IEnumerator<TSource> source, int fieldCount, Func<TSource, int, object> getValue)
        {
            _source = source;
            _getValue = getValue;
            _fieldCount = fieldCount;
        }

        public void Dispose()
        {
            _source.Dispose();
        }

        public object GetValue(int i)
        {
            return _getValue(_source.Current, i);
        }

        public int FieldCount
        {
            get { return _fieldCount; }
        }

        public bool Read()
        {
            return _source.MoveNext();
        }

        // all other members throw NotImplementedException
        public int Depth => throw new NotImplementedException();

        public bool IsClosed => throw new NotImplementedException();

        public int RecordsAffected => throw new NotImplementedException();

        public object this[string name] => throw new NotImplementedException();

        public object this[int i] => throw new NotImplementedException();
        public bool GetBoolean(int i) => throw new NotImplementedException();

        public void Close()
        {
            throw new NotImplementedException();
        }

        public DataTable GetSchemaTable()
        {
            throw new NotImplementedException();
        }

        public bool NextResult()
        {
            throw new NotImplementedException();
        }

        public byte GetByte(int i)
        {
            throw new NotImplementedException();
        }

        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        public char GetChar(int i)
        {
            throw new NotImplementedException();
        }

        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        public IDataReader GetData(int i)
        {
            throw new NotImplementedException();
        }

        public string GetDataTypeName(int i)
        {
            throw new NotImplementedException();
        }

        public DateTime GetDateTime(int i)
        {
            throw new NotImplementedException();
        }

        public decimal GetDecimal(int i)
        {
            throw new NotImplementedException();
        }

        public double GetDouble(int i)
        {
            throw new NotImplementedException();
        }

        public Type GetFieldType(int i)
        {
            throw new NotImplementedException();
        }

        public float GetFloat(int i)
        {
            throw new NotImplementedException();
        }

        public Guid GetGuid(int i)
        {
            throw new NotImplementedException();
        }

        public short GetInt16(int i)
        {
            throw new NotImplementedException();
        }

        public int GetInt32(int i)
        {
            throw new NotImplementedException();
        }

        public long GetInt64(int i)
        {
            throw new NotImplementedException();
        }

        public string GetName(int i)
        {
            throw new NotImplementedException();
        }

        public int GetOrdinal(string name)
        {
            throw new NotImplementedException();
        }

        public string GetString(int i)
        {
            throw new NotImplementedException();
        }

        public int GetValues(object[] values)
        {
            throw new NotImplementedException();
        }

        public bool IsDBNull(int i)
        {
            throw new NotImplementedException();
        }
    }
}
