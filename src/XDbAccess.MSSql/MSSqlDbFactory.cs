// Copyright (c) junjie sun. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using XDbAccess.AutoTrans;

namespace XDbAccess.MSSql
{
    /// <summary>
    /// MSSQL工厂
    /// </summary>
    public class MSSqlDbFactory : IDbFactory
    {
        private string _ConnectionString;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionString"></param>
        public MSSqlDbFactory(string connectionString)
        {
            _ConnectionString = connectionString;
        }

        /// <summary>
        /// 创建连接对象
        /// </summary>
        /// <returns></returns>
        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_ConnectionString);
        }
    }
}
