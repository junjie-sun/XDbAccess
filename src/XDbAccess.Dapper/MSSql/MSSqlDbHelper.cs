// Copyright (c) junjie sun. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XDbAccess.AutoTrans;

namespace XDbAccess.Dapper
{
    public class MSSqlDbHelper<DbContextImpl> : DbHelper<DbContextImpl> where DbContextImpl : IDbContext
    {
        private MSSqlSQLBuilder _SQLBuilder = new MSSqlSQLBuilder();

        public MSSqlDbHelper(DbContextImpl dbContext) : base(dbContext)
        {
        }

        protected override ISQLBuilder SQLBuilder
        {
            get
            {
                return _SQLBuilder;
            }
        }
    }
}
