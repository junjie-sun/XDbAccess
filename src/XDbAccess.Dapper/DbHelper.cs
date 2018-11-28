// Copyright (c) junjie sun. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Dapper;
using XDbAccess.AutoTrans;
using XDbAccess.Common;

namespace XDbAccess.Dapper
{
    public abstract class DbHelper<DbContextImpl> : IDbHelper<DbContextImpl> where DbContextImpl : IDbContext
    {
        private ConcurrentDictionary<Type, PropertyInfo[]> typeProperties = new ConcurrentDictionary<Type, PropertyInfo[]>();

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

        public virtual Task<long> InsertAsync<T>(T entity, int? commandTimeout = default(int?))
        {
            var meta = MapParser.GetMapMetaInfo(typeof(T));
            var sql = SQLBuilder.BuildInsertSql(meta);
            return this.ExecuteScalarAsync<long>(sql, entity, commandTimeout);
        }

        public virtual int Update<T>(T entity, bool isUpdateByPrimaryKey = true, string sqlConditionPart = null, object condition = null, int? commandTimeout = default(int?))
        {
            if (entity == null)
            {
                throw new InvalidOperationException("The parameter entity should not null.");
            }

            if (!isUpdateByPrimaryKey && string.IsNullOrWhiteSpace(sqlConditionPart))
            {
                throw new InvalidOperationException("The parameter sqlConditionPart should not null or empty when the parameter isUpdateByPrimaryKey is false.");
            }

            var meta = MapParser.GetMapMetaInfo(typeof(T));
            var sql = SQLBuilder.BuildUpdateSql(meta, isUpdateByPrimaryKey, sqlConditionPart);
            if (isUpdateByPrimaryKey)
            {
                return this.Execute(sql, entity, commandTimeout);
            }
            else
            {
                var param = CreateUpdateParameters(entity, condition, true);
                return this.Execute(sql, param, commandTimeout);
            }
        }

        public virtual Task<int> UpdateAsync<T>(T entity, bool isUpdateByPrimaryKey = true, string sqlConditionPart = null, object condition = null, int? commandTimeout = default(int?))
        {
            if (entity == null)
            {
                throw new InvalidOperationException("The parameter entity should not null.");
            }

            if (!isUpdateByPrimaryKey && string.IsNullOrWhiteSpace(sqlConditionPart))
            {
                throw new InvalidOperationException("The parameter sqlConditionPart should not null or empty when the parameter isUpdateByPrimaryKey is false.");
            }

            var meta = MapParser.GetMapMetaInfo(typeof(T));
            var sql = SQLBuilder.BuildUpdateSql(meta, isUpdateByPrimaryKey, sqlConditionPart);
            if (isUpdateByPrimaryKey)
            {
                return this.ExecuteAsync(sql, entity, commandTimeout);
            }
            else
            {
                var param = CreateUpdateParameters(entity, condition, true);
                return this.ExecuteAsync(sql, param, commandTimeout);
            }
        }

        public virtual int Delete<T>(object condition, bool isDeleteByPrimaryKey = true, string sqlConditionPart = null, int? commandTimeout = default(int?))
        {
            if (!isDeleteByPrimaryKey && string.IsNullOrWhiteSpace(sqlConditionPart))
            {
                throw new InvalidOperationException("The parameter sqlConditionPart should not null or empty when the parameter isDeleteByPrimaryKey is false.");
            }
            var meta = MapParser.GetMapMetaInfo(typeof(T));
            var sql = SQLBuilder.BuildDeleteSql(meta, isDeleteByPrimaryKey, sqlConditionPart);
            return this.Execute(sql, condition, commandTimeout);
        }

        public virtual Task<int> DeleteAsync<T>(object condition, bool isDeleteByPrimaryKey = true, string sqlConditionPart = null, int? commandTimeout = default(int?))
        {
            if (!isDeleteByPrimaryKey && string.IsNullOrWhiteSpace(sqlConditionPart))
            {
                throw new InvalidOperationException("The parameter sqlConditionPart should not null or empty when the parameter isDeleteByPrimaryKey is false.");
            }
            var meta = MapParser.GetMapMetaInfo(typeof(T));
            var sql = SQLBuilder.BuildDeleteSql(meta, isDeleteByPrimaryKey, sqlConditionPart);
            return this.ExecuteAsync(sql, condition, commandTimeout);
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

        public virtual IEnumerable<T> QuerySingleTable<T>(string sqlConditionPart = null, object condition = null, string sqlOrderByPart = null, bool buffered = true, int? commandTimeout = default(int?))
        {
            var meta = MapParser.GetMapMetaInfo(typeof(T));

            var sql = SQLBuilder.BuildSelectSql(meta, true, sqlConditionPart, sqlOrderByPart);

            return this.Query<T>(sql, condition, buffered, commandTimeout);
        }

        public virtual Task<IEnumerable<T>> QuerySingleTableAsync<T>(string sqlConditionPart = null, object condition = null, string sqlOrderByPart = null, int? commandTimeout = default(int?))
        {
            var meta = MapParser.GetMapMetaInfo(typeof(T));

            var sql = SQLBuilder.BuildSelectSql(meta, true, sqlConditionPart, sqlOrderByPart);

            return this.QueryAsync<T>(sql, condition, commandTimeout);
        }

        private DynamicParameters CreateUpdateParameters<T>(T entity, object condition, bool useValuePropertyPrefix)
        {
            var parameters = new DynamicParameters();

            if (entity != null)
            {
                var entityType = entity.GetType();
                var entityProperties = GetProperties(entityType);
                AddToDynamicParameters(parameters, entityProperties, entity, useValuePropertyPrefix ? SQLBuilderConstants.ValuePropertyPrefix : null);
            }

            if (condition != null)
            {
                var conditionType = condition.GetType();
                var conditionProperties = GetProperties(conditionType);
                AddToDynamicParameters(parameters, conditionProperties, condition);
            }

            return parameters;
        }

        private PropertyInfo[] GetProperties(Type type)
        {
            if (typeProperties.TryGetValue(type, out PropertyInfo[] properties))
            {
                return properties;
            }
            properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty);
            typeProperties.TryAdd(type, properties);
            return properties;
        }

        private void AddToDynamicParameters(DynamicParameters parameters, PropertyInfo[] properties, object obj, string prefix = null)
        {
            foreach (var p in properties)
            {
                parameters.Add(string.IsNullOrWhiteSpace(prefix) ? p.Name : prefix + p.Name, p.GetValue(obj));
            }
        }

        #endregion
    }
}
