// Copyright (c) junjie sun. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using XDbAccess.AutoTrans;

namespace XDbAccess.MySql
{
    public class MySqlDbFactory : IDbFactory
    {
        private string _ConnectionString;

        public MySqlDbFactory(string connectionString)
        {
            _ConnectionString = connectionString;
        }

        public IDbConnection CreateConnection()
        {
            return new MySqlConnection(_ConnectionString);
        }
    }
}
