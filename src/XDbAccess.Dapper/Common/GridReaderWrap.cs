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
    /// <summary>
    /// GridReader对象封装
    /// </summary>
    public class GridReaderWrap : IDisposable
    {
        private IDbConnection _Conn;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="conn"></param>
        public GridReaderWrap(SqlMapper.GridReader reader, IDbConnection conn)
        {
            GridReader = reader;
            _Conn = conn;
        }

        /// <summary>
        /// 
        /// </summary>
        public SqlMapper.GridReader GridReader { get; }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            GridReader.Dispose();
            _Conn.Dispose();
        }
    }
}
