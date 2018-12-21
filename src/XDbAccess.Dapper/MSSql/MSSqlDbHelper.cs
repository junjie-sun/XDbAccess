// Copyright (c) junjie sun. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XDbAccess.AutoTrans;
using XDbAccess.Common;

namespace XDbAccess.Dapper
{
    /// <summary>
    /// MSSqlDbHelper
    /// </summary>
    /// <typeparam name="DbContextImpl"></typeparam>
    public class MSSqlDbHelper<DbContextImpl> : DbHelper<DbContextImpl> where DbContextImpl : IDbContext
    {
        private MSSqlSQLBuilder _SQLBuilder = new MSSqlSQLBuilder();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbContext"></param>
        public MSSqlDbHelper(DbContextImpl dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// SQLBuilder
        /// </summary>
        protected override ISQLBuilder SQLBuilder
        {
            get
            {
                return _SQLBuilder;
            }
        }
    }
}
