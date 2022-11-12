// Copyright (c) junjie sun. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;
using XDbAccess.AutoTrans;
using XDbAccess.Common;

namespace XDbAccess.Dapper
{
    /// <summary>
    /// PostgreSQLDbHelper
    /// </summary>
    /// <typeparam name="DbContextImpl"></typeparam>
    public class PostgreSQLDbHelper<DbContextImpl> : DbHelper<DbContextImpl> where DbContextImpl : IDbContext
    {
        private PostgreSQLBuilder _SQLBuilder = new PostgreSQLBuilder();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbContext"></param>
        public PostgreSQLDbHelper(DbContextImpl dbContext) : base(dbContext)
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
