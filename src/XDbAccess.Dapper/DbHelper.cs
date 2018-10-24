// Copyright (c) junjie sun. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using XDbAccess.AutoTrans;
using XDbAccess.Common;

namespace XDbAccess.Dapper
{
    public abstract class DbHelper<DbContextImpl> : IDbHelper<DbContextImpl> where DbContextImpl : IDbContext
    {
        public DbHelper(DbContextImpl dbContext)
        {
            DbContext = dbContext;
        }

        protected IDbContext DbContext { get; }

        protected abstract ISQLBuilder SQLBuilder { get; }

        #region Dapper接口封装

        public virtual int Execute(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            using (var conn = DbContext.GetOpenedConnection())
            {
                var trans = conn.TransScope?.Trans;
                return conn.Execute(sql, param, trans, commandTimeout, commandType);
            }
        }

        public virtual async Task<int> ExecuteAsync(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            using (var conn = await DbContext.GetOpenedConnectionAsync())
            {
                var trans = conn.TransScope?.Trans;
                return await conn.ExecuteAsync(sql, param, trans, commandTimeout, commandType);
            }
        }

        public virtual IDataReader ExecuteReader(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            var conn = DbContext.GetOpenedConnection();
            var trans = conn.TransScope?.Trans;
            var reader = conn.ExecuteReader(sql, param, trans, commandTimeout, commandType);
            return new DataReaderWrap(reader, conn);
        }

        public virtual async Task<IDataReader> ExecuteReaderAsync(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            var conn = await DbContext.GetOpenedConnectionAsync();
            var trans = conn.TransScope?.Trans;
            var reader = await conn.ExecuteReaderAsync(sql, param, trans, commandTimeout, commandType);
            return new DataReaderWrap(reader, conn);
        }

        public virtual object ExecuteScalar(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            using (var conn = DbContext.GetOpenedConnection())
            {
                var trans = conn.TransScope?.Trans;
                return conn.ExecuteScalar(sql, param, trans, commandTimeout, commandType);
            }
        }

        public virtual T ExecuteScalar<T>(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            using (var conn = DbContext.GetOpenedConnection())
            {
                var trans = conn.TransScope?.Trans;
                return conn.ExecuteScalar<T>(sql, param, trans, commandTimeout, commandType);
            }
        }

        public virtual async Task<object> ExecuteScalarAsync(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            using (var conn = await DbContext.GetOpenedConnectionAsync())
            {
                var trans = conn.TransScope?.Trans;
                return await conn.ExecuteScalarAsync(sql, param, trans, commandTimeout, commandType);
            }
        }

        public virtual async Task<T> ExecuteScalarAsync<T>(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            using (var conn = await DbContext.GetOpenedConnectionAsync())
            {
                var trans = conn.TransScope?.Trans;
                return await conn.ExecuteScalarAsync<T>(sql, param, trans, commandTimeout, commandType);
            }
        }

        public virtual IEnumerable<dynamic> Query(string sql, object param = null, bool buffered = true, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            using (var conn = DbContext.GetOpenedConnection())
            {
                var trans = conn.TransScope?.Trans;
                return conn.Query(sql, param, trans, buffered, commandTimeout, commandType);
            }
        }

        public virtual IEnumerable<T> Query<T>(string sql, object param = null, bool buffered = true, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            using (var conn = DbContext.GetOpenedConnection())
            {
                var trans = conn.TransScope?.Trans;
                return conn.Query<T>(sql, param, trans, buffered, commandTimeout, commandType);
            }
        }

        public virtual async Task<IEnumerable<dynamic>> QueryAsync(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            using (var conn = await DbContext.GetOpenedConnectionAsync())
            {
                var trans = conn.TransScope?.Trans;
                return await conn.QueryAsync(sql, param, trans, commandTimeout, commandType);
            }
        }

        public virtual async Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            using (var conn = await DbContext.GetOpenedConnectionAsync())
            {
                var trans = conn.TransScope?.Trans;
                return await conn.QueryAsync<T>(sql, param, trans, commandTimeout, commandType);
            }
        }

        public virtual dynamic QueryFirst(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            using (var conn = DbContext.GetOpenedConnection())
            {
                var trans = conn.TransScope?.Trans;
                return conn.QueryFirst(sql, param, trans, commandTimeout, commandType);
            }
        }

        public virtual T QueryFirst<T>(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            using (var conn = DbContext.GetOpenedConnection())
            {
                var trans = conn.TransScope?.Trans;
                return conn.QueryFirst<T>(sql, param, trans, commandTimeout, commandType);
            }
        }

        public virtual async Task<object> QueryFirstAsync(Type type, string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            using (var conn = await DbContext.GetOpenedConnectionAsync())
            {
                var trans = conn.TransScope?.Trans;
                return await conn.QueryFirstAsync(type, sql, param, trans, commandTimeout, commandType);
            }
        }

        public virtual async Task<T> QueryFirstAsync<T>(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            using (var conn = await DbContext.GetOpenedConnectionAsync())
            {
                var trans = conn.TransScope?.Trans;
                return await conn.QueryFirstAsync<T>(sql, param, trans, commandTimeout, commandType);
            }
        }

        public virtual dynamic QueryFirstOrDefault(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            using (var conn = DbContext.GetOpenedConnection())
            {
                var trans = conn.TransScope?.Trans;
                return conn.QueryFirstOrDefault(sql, param, trans, commandTimeout, commandType);
            }
        }

        public virtual T QueryFirstOrDefault<T>(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            using (var conn = DbContext.GetOpenedConnection())
            {
                var trans = conn.TransScope?.Trans;
                return conn.QueryFirstOrDefault<T>(sql, param, trans, commandTimeout, commandType);
            }
        }

        public virtual async Task<T> QueryFirstOrDefaultAsync<T>(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            using (var conn = await DbContext.GetOpenedConnectionAsync())
            {
                var trans = conn.TransScope?.Trans;
                return await conn.QueryFirstOrDefaultAsync<T>(sql, param, trans, commandTimeout, commandType);
            }
        }

        public virtual GridReaderWrap QueryMultiple(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            var conn = DbContext.GetOpenedConnection();
            var trans = conn.TransScope?.Trans;
            var reader = conn.QueryMultiple(sql, param, trans, commandTimeout, commandType);
            return new GridReaderWrap(reader, conn);
        }

        public virtual async Task<GridReaderWrap> QueryMultipleAsync(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            var conn = await DbContext.GetOpenedConnectionAsync();
            var trans = conn.TransScope?.Trans;
            var reader = await conn.QueryMultipleAsync(sql, param, trans, commandTimeout, commandType);
            return new GridReaderWrap(reader, conn);
        }

        public virtual dynamic QuerySingle(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            using (var conn = DbContext.GetOpenedConnection())
            {
                var trans = conn.TransScope?.Trans;
                return conn.QuerySingle(sql, param, trans, commandTimeout, commandType);
            }
        }

        public virtual T QuerySingle<T>(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            using (var conn = DbContext.GetOpenedConnection())
            {
                var trans = conn.TransScope?.Trans;
                return conn.QuerySingle<T>(sql, param, trans, commandTimeout, commandType);
            }
        }

        public virtual async Task<T> QuerySingleAsync<T>(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            using (var conn = await DbContext.GetOpenedConnectionAsync())
            {
                var trans = conn.TransScope?.Trans;
                return await conn.QuerySingleAsync<T>(sql, param, trans, commandTimeout, commandType);
            }
        }

        public virtual dynamic QuerySingleOrDefault(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            using (var conn = DbContext.GetOpenedConnection())
            {
                var trans = conn.TransScope?.Trans;
                return conn.QuerySingleOrDefault(sql, param, trans, commandTimeout, commandType);
            }
        }

        public virtual T QuerySingleOrDefault<T>(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            using (var conn = DbContext.GetOpenedConnection())
            {
                var trans = conn.TransScope?.Trans;
                return conn.QuerySingleOrDefault<T>(sql, param, trans, commandTimeout, commandType);
            }
        }

        public virtual async Task<object> QuerySingleOrDefaultAsync(Type type, string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            using (var conn = await DbContext.GetOpenedConnectionAsync())
            {
                var trans = conn.TransScope?.Trans;
                return await conn.QuerySingleOrDefaultAsync(type, sql, param, trans, commandTimeout, commandType);
            }
        }

        public virtual async Task<T> QuerySingleOrDefaultAsync<T>(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            using (var conn = await DbContext.GetOpenedConnectionAsync())
            {
                var trans = conn.TransScope?.Trans;
                return await conn.QuerySingleOrDefaultAsync<T>(sql, param, trans, commandTimeout, commandType);
            }
        }

        #endregion

        #region 扩展接口

        public virtual long Insert<T>(T entity, int? commandTimeout = default(int?))
        {
            var meta = MapParser.GetMapMetaInfo(typeof(T));
            var sql = SQLBuilder.BuildInsertSql(meta);
            return this.ExecuteScalar<long>(sql, entity, commandTimeout);
        }

        public virtual async Task<long> InsertAsync<T>(T entity, int? commandTimeout = default(int?))
        {
            var meta = MapParser.GetMapMetaInfo(typeof(T));
            var sql = SQLBuilder.BuildInsertSql(meta);
            return await this.ExecuteScalarAsync<long>(sql, entity, commandTimeout);
        }

        public virtual int Update<T>(T entity, int? commandTimeout = default(int?))
        {
            var meta = MapParser.GetMapMetaInfo(typeof(T));
            var sql = SQLBuilder.BuildUpdateSql(meta);
            return this.Execute(sql, entity, commandTimeout);
        }

        public virtual async Task<int> UpdateAsync<T>(T entity, int? commandTimeout = default(int?))
        {
            var meta = MapParser.GetMapMetaInfo(typeof(T));
            var sql = SQLBuilder.BuildUpdateSql(meta);
            return await this.ExecuteAsync(sql, entity, commandTimeout);
        }

        public virtual PagedQueryResult<T> PagedQuery<T>(PagedQueryOptions options, object param = null, bool buffered = true, int? commandTimeout = default(int?))
        {
            PagedQueryResult<T> result = new PagedQueryResult<T>();
            result.PageIndex = options.PageIndex;
            result.PageSize = options.PageSize;

            if (options.PageIndex == 0)
            {
                var countSql = SQLBuilder.BuildQueryCountSql(options.SqlFromPart, options.SqlConditionPart);
                result.Total = this.ExecuteScalar<long>(countSql, param, commandTimeout);
            }

            var sql = SQLBuilder.BuidlPagedQuerySql(options);
            result.Data = this.Query<T>(sql, param, buffered, commandTimeout).ToList();

            return result;
        }

        public virtual async Task<PagedQueryResult<T>> PagedQueryAsync<T>(PagedQueryOptions options, object param = null, int? commandTimeout = default(int?))
        {
            PagedQueryResult<T> result = new PagedQueryResult<T>();
            result.PageIndex = options.PageIndex;
            result.PageSize = options.PageSize;

            if (options.PageIndex == 0)
            {
                var countSql = SQLBuilder.BuildQueryCountSql(options.SqlFromPart, options.SqlConditionPart);
                result.Total = await this.ExecuteScalarAsync<long>(countSql, param, commandTimeout);
            }

            var sql = SQLBuilder.BuidlPagedQuerySql(options);
            var data = await this.QueryAsync<T>(sql, param, commandTimeout);
            result.Data = data.ToList();

            return result;
        }

        #endregion
    }
}
