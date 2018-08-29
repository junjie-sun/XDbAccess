// Copyright (c) junjie sun. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace XDbAccess.Dapper
{
    public class GridReaderWrap : IDisposable
    {
        private IDbConnection _Conn;

        public GridReaderWrap(SqlMapper.GridReader reader, IDbConnection conn)
        {
            GridReader = reader;
            _Conn = conn;
        }

        public SqlMapper.GridReader GridReader { get; }

        public void Dispose()
        {
            GridReader.Dispose();
            _Conn.Dispose();
        }
    }
}
