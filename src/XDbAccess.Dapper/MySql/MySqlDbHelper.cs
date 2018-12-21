// Copyright (c) junjie sun. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using XDbAccess.AutoTrans;
using Dapper;
using XDbAccess.Common;
using Microsoft.Extensions.Logging;

namespace XDbAccess.Dapper
{
    /// <summary>
    /// MySqlDbHelper
    /// </summary>
    /// <typeparam name="DbContextImpl"></typeparam>
    public class MySqlDbHelper<DbContextImpl> : DbHelper<DbContextImpl> where DbContextImpl : IDbContext
    {
        private MySqlSQLBuilder _SQLBuilder = new MySqlSQLBuilder();

        private ILogger logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbContext"></param>
        public MySqlDbHelper(DbContextImpl dbContext) : base(dbContext)
        {
            if (DbContext.LoggerFactory != null)
            {
                logger = DbContext.LoggerFactory.CreateLogger<MySqlDbHelper<DbContextImpl>>();
            }
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 执行REPLACE操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public int Replace<T>(T entity, int? commandTimeout = default(int?))
        {
            var meta = MapParser.GetMapMetaInfo(typeof(T));
            var sql = _SQLBuilder.BuildReplaceSql(meta);
            LogDebug($"Replace generate SQL: {sql}");
            return this.Execute(sql, entity, commandTimeout);
        }

        /// <summary>
        /// 执行REPLACE操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public Task<int> ReplaceAsync<T>(T entity, int? commandTimeout = default(int?))
        {
            var meta = MapParser.GetMapMetaInfo(typeof(T));
            var sql = _SQLBuilder.BuildReplaceSql(meta);
            LogDebug($"ReplaceAsync generate SQL: {sql}");
            return this.ExecuteAsync(sql, entity, commandTimeout);
        }

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
