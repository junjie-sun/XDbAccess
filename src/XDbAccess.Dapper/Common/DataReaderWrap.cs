// Copyright (c) junjie sun. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace XDbAccess.Dapper
{
    public class DataReaderWrap : IDataReader
    {
        private IDataReader _Reader;

        private IDbConnection _Conn;

        public DataReaderWrap(IDataReader reader, IDbConnection conn)
        {
            _Reader = reader;
            _Conn = conn;
        }

        public object this[string name]
        {
            get
            {
                return _Reader[name];
            }
        }

        public object this[int i]
        {
            get
            {
                return _Reader[i];
            }
        }

        public int Depth
        {
            get
            {
                return _Reader.Depth;
            }
        }

        public int FieldCount
        {
            get
            {
                return _Reader.FieldCount;
            }
        }

        public bool IsClosed
        {
            get
            {
                return _Reader.IsClosed;
            }
        }

        public int RecordsAffected
        {
            get
            {
                return _Reader.RecordsAffected;
            }
        }

        public void Close()
        {
            _Reader.Close();
            _Conn.Close();
        }

        public void Dispose()
        {
            _Reader.Dispose();
            _Conn.Dispose();
        }

        public bool GetBoolean(int i)
        {
            return _Reader.GetBoolean(i);
        }

        public byte GetByte(int i)
        {
            return _Reader.GetByte(i);
        }

        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            return _Reader.GetBytes(i, fieldOffset, buffer, bufferoffset, length);
        }

        public char GetChar(int i)
        {
            return _Reader.GetChar(i);
        }

        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            return _Reader.GetChars(i, fieldoffset, buffer, bufferoffset, length);
        }

        public IDataReader GetData(int i)
        {
            return _Reader.GetData(i);
        }

        public string GetDataTypeName(int i)
        {
            return _Reader.GetDataTypeName(i);
        }

        public DateTime GetDateTime(int i)
        {
            return _Reader.GetDateTime(i);
        }

        public decimal GetDecimal(int i)
        {
            return _Reader.GetDecimal(i);
        }

        public double GetDouble(int i)
        {
            return _Reader.GetDouble(i);
        }

        public Type GetFieldType(int i)
        {
            return _Reader.GetFieldType(i);
        }

        public float GetFloat(int i)
        {
            return _Reader.GetFloat(i);
        }

        public Guid GetGuid(int i)
        {
            return _Reader.GetGuid(i);
        }

        public short GetInt16(int i)
        {
            return _Reader.GetInt16(i);
        }

        public int GetInt32(int i)
        {
            return _Reader.GetInt32(i);
        }

        public long GetInt64(int i)
        {
            return _Reader.GetInt64(i);
        }

        public string GetName(int i)
        {
            return _Reader.GetName(i);
        }

        public int GetOrdinal(string name)
        {
            return _Reader.GetOrdinal(name);
        }

        public DataTable GetSchemaTable()
        {
            return _Reader.GetSchemaTable();
        }

        public string GetString(int i)
        {
            return _Reader.GetString(i);
        }

        public object GetValue(int i)
        {
            return _Reader.GetValue(i);
        }

        public int GetValues(object[] values)
        {
            return _Reader.GetValues(values);
        }

        public bool IsDBNull(int i)
        {
            return _Reader.IsDBNull(i);
        }

        public bool NextResult()
        {
            return _Reader.NextResult();
        }

        public bool Read()
        {
            return _Reader.Read();
        }
    }
}
