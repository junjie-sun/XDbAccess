﻿// Copyright (c) junjie sun. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Logging;
using XDbAccess.AutoTrans;
using XDbAccess.Common;

namespace XDbAccess.Dapper
{
    /// <summary>
    /// DbHelper
    /// </summary>
    /// <typeparam name="DbContextImpl"></typeparam>
    public abstract class DbHelper<DbContextImpl> : IDbHelper<DbContextImpl> where DbContextImpl : IDbContext
    {
        private ConcurrentDictionary<Type, PropertyInfo[]> typeProperties = new ConcurrentDictionary<Type, PropertyInfo[]>();

        private ILogger logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbContext"></param>
        public DbHelper(DbContextImpl dbContext)
        {
            DbContext = dbContext;
            if (DbContext.LoggerFactory != null)
            {
                logger = DbContext.LoggerFactory.CreateLogger<DbHelper<DbContextImpl>>();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected IDbContext DbContext { get; }

        /// <summary>
        /// 
        /// </summary>
        protected abstract ISQLBuilder SQLBuilder { get; }

        #region Dapper接口封装

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public virtual int Execute(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            using (var conn = DbContext.GetOpenedConnection())
            {
                var trans = conn.TransScope?.Trans;
                return conn.Execute(sql, param, trans, commandTimeout, commandType);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public virtual async Task<int> ExecuteAsync(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            using (var conn = await DbContext.GetOpenedConnectionAsync())
            {
                var trans = conn.TransScope?.Trans;
                return await conn.ExecuteAsync(sql, param, trans, commandTimeout, commandType);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public virtual IDataReader ExecuteReader(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            var conn = DbContext.GetOpenedConnection();
            var trans = conn.TransScope?.Trans;
            var reader = conn.ExecuteReader(sql, param, trans, commandTimeout, commandType);
            return new DataReaderWrap(reader, conn);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public virtual async Task<IDataReader> ExecuteReaderAsync(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            var conn = await DbContext.GetOpenedConnectionAsync();
            var trans = conn.TransScope?.Trans;
            var reader = await conn.ExecuteReaderAsync(sql, param, trans, commandTimeout, commandType);
            return new DataReaderWrap(reader, conn);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public virtual object ExecuteScalar(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            using (var conn = DbContext.GetOpenedConnection())
            {
                var trans = conn.TransScope?.Trans;
                return conn.ExecuteScalar(sql, param, trans, commandTimeout, commandType);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public virtual T ExecuteScalar<T>(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            using (var conn = DbContext.GetOpenedConnection())
            {
                var trans = conn.TransScope?.Trans;
                return conn.ExecuteScalar<T>(sql, param, trans, commandTimeout, commandType);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public virtual async Task<object> ExecuteScalarAsync(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            using (var conn = await DbContext.GetOpenedConnectionAsync())
            {
                var trans = conn.TransScope?.Trans;
                return await conn.ExecuteScalarAsync(sql, param, trans, commandTimeout, commandType);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public virtual async Task<T> ExecuteScalarAsync<T>(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            using (var conn = await DbContext.GetOpenedConnectionAsync())
            {
                var trans = conn.TransScope?.Trans;
                return await conn.ExecuteScalarAsync<T>(sql, param, trans, commandTimeout, commandType);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="buffered"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public virtual IEnumerable<dynamic> Query(string sql, object param = null, bool buffered = true, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            using (var conn = DbContext.GetOpenedConnection())
            {
                var trans = conn.TransScope?.Trans;
                return conn.Query(sql, param, trans, buffered, commandTimeout, commandType);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="buffered"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public virtual IEnumerable<T> Query<T>(string sql, object param = null, bool buffered = true, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            using (var conn = DbContext.GetOpenedConnection())
            {
                var trans = conn.TransScope?.Trans;
                return conn.Query<T>(sql, param, trans, buffered, commandTimeout, commandType);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public virtual async Task<IEnumerable<dynamic>> QueryAsync(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            using (var conn = await DbContext.GetOpenedConnectionAsync())
            {
                var trans = conn.TransScope?.Trans;
                return await conn.QueryAsync(sql, param, trans, commandTimeout, commandType);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public virtual async Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            using (var conn = await DbContext.GetOpenedConnectionAsync())
            {
                var trans = conn.TransScope?.Trans;
                return await conn.QueryAsync<T>(sql, param, trans, commandTimeout, commandType);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public virtual dynamic QueryFirst(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            using (var conn = DbContext.GetOpenedConnection())
            {
                var trans = conn.TransScope?.Trans;
                return conn.QueryFirst(sql, param, trans, commandTimeout, commandType);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public virtual T QueryFirst<T>(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            using (var conn = DbContext.GetOpenedConnection())
            {
                var trans = conn.TransScope?.Trans;
                return conn.QueryFirst<T>(sql, param, trans, commandTimeout, commandType);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public virtual async Task<object> QueryFirstAsync(Type type, string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            using (var conn = await DbContext.GetOpenedConnectionAsync())
            {
                var trans = conn.TransScope?.Trans;
                return await conn.QueryFirstAsync(type, sql, param, trans, commandTimeout, commandType);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public virtual async Task<T> QueryFirstAsync<T>(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            using (var conn = await DbContext.GetOpenedConnectionAsync())
            {
                var trans = conn.TransScope?.Trans;
                return await conn.QueryFirstAsync<T>(sql, param, trans, commandTimeout, commandType);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public virtual dynamic QueryFirstOrDefault(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            using (var conn = DbContext.GetOpenedConnection())
            {
                var trans = conn.TransScope?.Trans;
                return conn.QueryFirstOrDefault(sql, param, trans, commandTimeout, commandType);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public virtual T QueryFirstOrDefault<T>(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            using (var conn = DbContext.GetOpenedConnection())
            {
                var trans = conn.TransScope?.Trans;
                return conn.QueryFirstOrDefault<T>(sql, param, trans, commandTimeout, commandType);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public virtual async Task<T> QueryFirstOrDefaultAsync<T>(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            using (var conn = await DbContext.GetOpenedConnectionAsync())
            {
                var trans = conn.TransScope?.Trans;
                return await conn.QueryFirstOrDefaultAsync<T>(sql, param, trans, commandTimeout, commandType);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public virtual GridReaderWrap QueryMultiple(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            var conn = DbContext.GetOpenedConnection();
            var trans = conn.TransScope?.Trans;
            var reader = conn.QueryMultiple(sql, param, trans, commandTimeout, commandType);
            return new GridReaderWrap(reader, conn);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public virtual async Task<GridReaderWrap> QueryMultipleAsync(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            var conn = await DbContext.GetOpenedConnectionAsync();
            var trans = conn.TransScope?.Trans;
            var reader = await conn.QueryMultipleAsync(sql, param, trans, commandTimeout, commandType);
            return new GridReaderWrap(reader, conn);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public virtual dynamic QuerySingle(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            using (var conn = DbContext.GetOpenedConnection())
            {
                var trans = conn.TransScope?.Trans;
                return conn.QuerySingle(sql, param, trans, commandTimeout, commandType);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public virtual T QuerySingle<T>(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            using (var conn = DbContext.GetOpenedConnection())
            {
                var trans = conn.TransScope?.Trans;
                return conn.QuerySingle<T>(sql, param, trans, commandTimeout, commandType);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public virtual async Task<T> QuerySingleAsync<T>(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            using (var conn = await DbContext.GetOpenedConnectionAsync())
            {
                var trans = conn.TransScope?.Trans;
                return await conn.QuerySingleAsync<T>(sql, param, trans, commandTimeout, commandType);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public virtual dynamic QuerySingleOrDefault(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            using (var conn = DbContext.GetOpenedConnection())
            {
                var trans = conn.TransScope?.Trans;
                return conn.QuerySingleOrDefault(sql, param, trans, commandTimeout, commandType);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public virtual T QuerySingleOrDefault<T>(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            using (var conn = DbContext.GetOpenedConnection())
            {
                var trans = conn.TransScope?.Trans;
                return conn.QuerySingleOrDefault<T>(sql, param, trans, commandTimeout, commandType);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public virtual async Task<object> QuerySingleOrDefaultAsync(Type type, string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?))
        {
            using (var conn = await DbContext.GetOpenedConnectionAsync())
            {
                var trans = conn.TransScope?.Trans;
                return await conn.QuerySingleOrDefaultAsync(type, sql, param, trans, commandTimeout, commandType);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 执行INSERT操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">实体对象</param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public virtual long Insert<T>(T entity, int? commandTimeout = default(int?))
        {
            var meta = MapParser.GetMapMetaInfo(typeof(T));
            var sql = SQLBuilder.BuildInsertSql(meta);
            LogDebug($"Insert generate SQL: {sql}");
            return this.ExecuteScalar<long>(sql, entity, commandTimeout);
        }

        /// <summary>
        /// 执行INSERT操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">实体对象</param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public virtual Task<long> InsertAsync<T>(T entity, int? commandTimeout = default(int?))
        {
            var meta = MapParser.GetMapMetaInfo(typeof(T));
            var sql = SQLBuilder.BuildInsertSql(meta);
            LogDebug($"InsertAsync generate SQL: {sql}");
            return this.ExecuteScalarAsync<long>(sql, entity, commandTimeout);
        }

        /// <summary>
        /// 执行批量INSERT操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">实体对象集合</param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public virtual int BatchInsert<T>(IEnumerable<T> list, int? commandTimeout = default(int?))
        {
            var meta = MapParser.GetMapMetaInfo(typeof(T));
            var sql = SQLBuilder.BuildInsertSql(meta, false);
            LogDebug($"BatchInsert generate SQL: {sql}");
            return this.Execute(sql, list, commandTimeout);
        }

        /// <summary>
        /// 执行批量INSERT操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">实体对象集合</param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public virtual Task<int> BatchInsertAsync<T>(IEnumerable<T> list, int? commandTimeout = default(int?))
        {
            var meta = MapParser.GetMapMetaInfo(typeof(T));
            var sql = SQLBuilder.BuildInsertSql(meta, false);
            LogDebug($"BatchInsert generate SQL: {sql}");
            return this.ExecuteAsync(sql, list, commandTimeout);
        }

        /// <summary>
        /// 执行UPDATE操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">实体对象</param>
        /// <param name="isUpdateByPrimaryKey">是否以主键作为条件进行更新</param>
        /// <param name="sqlConditionPart">WHERE部分的SQL，当isUpdateByPrimaryKey=false时有效</param>
        /// <param name="condition">条件对象</param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
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
            LogDebug($"Update generate SQL: {sql}");
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

        /// <summary>
        /// 执行UPDATE操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">实体对象</param>
        /// <param name="isUpdateByPrimaryKey">是否以主键作为条件进行更新</param>
        /// <param name="sqlConditionPart">WHERE部分的SQL，当isUpdateByPrimaryKey=false时有效</param>
        /// <param name="condition">条件对象</param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
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
            LogDebug($"UpdateAsync generate SQL: {sql}");
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

        /// <summary>
        /// 执行批量UPDATE操作，只支持根据主键更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">实体对象集合</param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public virtual int BatchUpdate<T>(IEnumerable<T> list, int? commandTimeout = default(int?))
        {
            var meta = MapParser.GetMapMetaInfo(typeof(T));
            var sql = SQLBuilder.BuildUpdateSql(meta);
            LogDebug($"Update generate SQL: {sql}");
            return this.Execute(sql, list, commandTimeout);
        }

        /// <summary>
        /// 执行批量UPDATE操作，只支持根据主键更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">实体对象集合</param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public virtual Task<int> BatchUpdateAsync<T>(IEnumerable<T> list, int? commandTimeout = default(int?))
        {
            var meta = MapParser.GetMapMetaInfo(typeof(T));
            var sql = SQLBuilder.BuildUpdateSql(meta);
            LogDebug($"Update generate SQL: {sql}");
            return this.ExecuteAsync(sql, list, commandTimeout);
        }

        /// <summary>
        /// 执行DELETE操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="condition">条件对象</param>
        /// <param name="isDeleteByPrimaryKey">是否以主键作为条件进行删除</param>
        /// <param name="sqlConditionPart">WHERE部分的SQL，当isDeleteByPrimaryKey=false时有效</param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public virtual int Delete<T>(object condition, bool isDeleteByPrimaryKey = true, string sqlConditionPart = null, int? commandTimeout = default(int?))
        {
            if (!isDeleteByPrimaryKey && string.IsNullOrWhiteSpace(sqlConditionPart))
            {
                throw new InvalidOperationException("The parameter sqlConditionPart should not null or empty when the parameter isDeleteByPrimaryKey is false.");
            }
            var meta = MapParser.GetMapMetaInfo(typeof(T));
            var sql = SQLBuilder.BuildDeleteSql(meta, isDeleteByPrimaryKey, sqlConditionPart);
            LogDebug($"Delete generate SQL: {sql}");
            return this.Execute(sql, condition, commandTimeout);
        }

        /// <summary>
        /// 执行DELETE操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="condition">条件对象</param>
        /// <param name="isDeleteByPrimaryKey">是否以主键作为条件进行删除</param>
        /// <param name="sqlConditionPart">WHERE部分的SQL，当isDeleteByPrimaryKey=false时有效</param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public virtual Task<int> DeleteAsync<T>(object condition, bool isDeleteByPrimaryKey = true, string sqlConditionPart = null, int? commandTimeout = default(int?))
        {
            if (!isDeleteByPrimaryKey && string.IsNullOrWhiteSpace(sqlConditionPart))
            {
                throw new InvalidOperationException("The parameter sqlConditionPart should not null or empty when the parameter isDeleteByPrimaryKey is false.");
            }
            var meta = MapParser.GetMapMetaInfo(typeof(T));
            var sql = SQLBuilder.BuildDeleteSql(meta, isDeleteByPrimaryKey, sqlConditionPart);
            LogDebug($"DeleteAsync generate SQL: {sql}");
            return this.ExecuteAsync(sql, condition, commandTimeout);
        }

        /// <summary>
        /// 执行批量DELETE操作，只支持根据主键删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="conditions">条件对象集合</param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public virtual int BatchDelete<T>(IEnumerable<object> conditions, int? commandTimeout = default(int?))
        {
            var meta = MapParser.GetMapMetaInfo(typeof(T));
            var sql = SQLBuilder.BuildDeleteSql(meta);
            LogDebug($"Delete generate SQL: {sql}");
            return this.Execute(sql, conditions, commandTimeout);
        }

        /// <summary>
        /// 执行批量DELETE操作，只支持根据主键删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="conditions">条件对象集合</param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public virtual Task<int> BatchDeleteAsync<T>(IEnumerable<object> conditions, int? commandTimeout = default(int?))
        {
            var meta = MapParser.GetMapMetaInfo(typeof(T));
            var sql = SQLBuilder.BuildDeleteSql(meta);
            LogDebug($"Delete generate SQL: {sql}");
            return this.ExecuteAsync(sql, conditions, commandTimeout);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="options">分页查询参数</param>
        /// <param name="param">条件对象</param>
        /// <param name="buffered"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public virtual PagedQueryResult<T> PagedQuery<T>(PagedQueryOptions options, object param = null, bool buffered = true, int? commandTimeout = default(int?))
        {
            PagedQueryResult<T> result = new PagedQueryResult<T>();
            result.PageIndex = options.PageIndex;
            result.PageSize = options.PageSize;

            if (options.PageIndex == 0 || options.AlwayQueryCount)
            {
                var countSql = SQLBuilder.BuildQueryCountSql(options.SqlFromPart, options.SqlConditionPart, options.SqlGroupPart);
                result.Total = this.ExecuteScalar<long>(countSql, param, commandTimeout);
            }

            var sql = SQLBuilder.BuildPagedQuerySql(options);
            LogDebug($"PagedQuery generate SQL: {sql}");
            result.Data = this.Query<T>(sql, param, buffered, commandTimeout).ToList();

            return result;
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="options">分页查询参数</param>
        /// <param name="param">条件对象</param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public virtual async Task<PagedQueryResult<T>> PagedQueryAsync<T>(PagedQueryOptions options, object param = null, int? commandTimeout = default(int?))
        {
            PagedQueryResult<T> result = new PagedQueryResult<T>();
            result.PageIndex = options.PageIndex;
            result.PageSize = options.PageSize;

            if (options.PageIndex == 0 || options.AlwayQueryCount)
            {
                var countSql = SQLBuilder.BuildQueryCountSql(options.SqlFromPart, options.SqlConditionPart, options.SqlGroupPart);
                result.Total = await this.ExecuteScalarAsync<long>(countSql, param, commandTimeout);
            }

            var sql = SQLBuilder.BuildPagedQuerySql(options);
            LogDebug($"PagedQueryAsync generate SQL: {sql}");
            var data = await this.QueryAsync<T>(sql, param, commandTimeout);
            result.Data = data.ToList();

            return result;
        }

        /// <summary>
        /// 单表查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlConditionPart">WHERE部分的SQL</param>
        /// <param name="condition">条件对象</param>
        /// <param name="sqlOrderPart">ORDER部分的SQL</param>
        /// <param name="buffered"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public virtual IEnumerable<T> QuerySingleTable<T>(string sqlConditionPart = null, object condition = null, string sqlOrderPart = null, bool buffered = true, int? commandTimeout = default(int?))
        {
            var meta = MapParser.GetMapMetaInfo(typeof(T));

            var sql = SQLBuilder.BuildSelectSql(meta, true, sqlConditionPart, sqlOrderPart);

            LogDebug($"QuerySingleTable generate SQL: {sql}");

            return this.Query<T>(sql, condition, buffered, commandTimeout);
        }

        /// <summary>
        /// 单表查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlConditionPart">WHERE部分的SQL</param>
        /// <param name="condition">条件对象</param>
        /// <param name="sqlOrderPart">ORDER部分的SQL</param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public virtual Task<IEnumerable<T>> QuerySingleTableAsync<T>(string sqlConditionPart = null, object condition = null, string sqlOrderPart = null, int? commandTimeout = default(int?))
        {
            var meta = MapParser.GetMapMetaInfo(typeof(T));

            var sql = SQLBuilder.BuildSelectSql(meta, true, sqlConditionPart, sqlOrderPart);

            LogDebug($"QuerySingleTableAsync generate SQL: {sql}");

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

        #region 私有方法

        private void LogDebug(string message, params object[] args)
        {
            if (logger != null)
            {
                logger.LogDebug(message, args);
            }
        }

        #endregion
    }
}
