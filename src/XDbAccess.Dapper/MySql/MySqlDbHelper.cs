// Copyright (c) junjie sun. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using XDbAccess.AutoTrans;
using Dapper;

namespace XDbAccess.Dapper
{
    public class MySqlDbHelper<DbContextImpl> : DbHelper<DbContextImpl> where DbContextImpl : IDbContext
    {
        private MySqlSQLBuilder _SQLBuilder = new MySqlSQLBuilder();

        public MySqlDbHelper(DbContextImpl dbContext) : base(dbContext)
        {
        }

        protected override ISQLBuilder SQLBuilder
        {
            get
            {
                return _SQLBuilder;
            }
        }

        public override async Task<IDataReader> ExecuteReaderAsync(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            var conn = await DbContext.GetOpenedConnectionAsync();
            var trans = conn.TransScope?.Trans;
            //貌似Dapper在这里有个bug，导致返回的Reader为Close状态
            //var reader = await conn.ExecuteReaderAsync(sql, param, trans, commandTimeout, commandType);
            //暂时的方案
            var reader = await Task.FromResult(conn.ExecuteReader(sql, param, trans, commandTimeout, commandType));
            return new DataReaderWrap(reader, conn);
        }

        public int Replace<T>(T entity, int? commandTimeout = default(int?))
        {
            var meta = MapParser.GetMapMetaInfo(typeof(T));
            var sql = _SQLBuilder.BuildReplaceSql(meta);
            return this.Execute(sql, entity, commandTimeout);
        }

        public Task<int> ReplaceAsync<T>(T entity, int? commandTimeout = default(int?))
        {
            var meta = MapParser.GetMapMetaInfo(typeof(T));
            var sql = _SQLBuilder.BuildReplaceSql(meta);
            return this.ExecuteAsync(sql, entity, commandTimeout);
        }
    }
}
