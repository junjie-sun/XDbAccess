// Copyright (c) junjie sun. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XDbAccess.AutoTrans;

namespace XDbAccess.MySql
{
    public static class MySqlDbContextOptionsExtensions
    {
        public static DbContextOptionsBuilder UseMySql(this DbContextOptionsBuilder optionsBuilder, string connectionString)
        {
            optionsBuilder.Options.ConnectionString = connectionString;
            optionsBuilder.Options.DbFactory = new MySqlDbFactory(connectionString);
            return optionsBuilder;
        }
    }
}
