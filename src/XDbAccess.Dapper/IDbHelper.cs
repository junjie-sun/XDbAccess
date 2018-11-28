// Copyright (c) junjie sun. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using XDbAccess.AutoTrans;
using XDbAccess.Common;

namespace XDbAccess.Dapper
{
    public interface IDbHelper<DbContextImpl> where DbContextImpl : IDbContext
    {
        #region Dapper接口封装

        int Execute(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));
        Task<int> ExecuteAsync(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));
        IDataReader ExecuteReader(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));
        Task<IDataReader> ExecuteReaderAsync(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));
        object ExecuteScalar(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));
        T ExecuteScalar<T>(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));
        Task<object> ExecuteScalarAsync(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));
        Task<T> ExecuteScalarAsync<T>(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));
        IEnumerable<dynamic> Query(string sql, object param = null, bool buffered = true, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));
        IEnumerable<T> Query<T>(string sql, object param = null, bool buffered = true, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));
        Task<IEnumerable<dynamic>> QueryAsync(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));
        Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));
        dynamic QueryFirst(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));
        T QueryFirst<T>(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));
        Task<object> QueryFirstAsync(Type type, string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));
        Task<T> QueryFirstAsync<T>(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));
        dynamic QueryFirstOrDefault(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));
        T QueryFirstOrDefault<T>(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));
        Task<T> QueryFirstOrDefaultAsync<T>(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));
        GridReaderWrap QueryMultiple(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));
        Task<GridReaderWrap> QueryMultipleAsync(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));
        dynamic QuerySingle(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));
        T QuerySingle<T>(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));
        Task<T> QuerySingleAsync<T>(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));
        dynamic QuerySingleOrDefault(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));
        T QuerySingleOrDefault<T>(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));
        Task<object> QuerySingleOrDefaultAsync(Type type, string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));
        Task<T> QuerySingleOrDefaultAsync<T>(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));

        #endregion

        #region 扩展接口

        long Insert<T>(T entity, int? commandTimeout = default(int?));

        Task<long> InsertAsync<T>(T entity, int? commandTimeout = default(int?));

        int Update<T>(T entity, int? commandTimeout = default(int?));

        Task<int> UpdateAsync<T>(T entity, int? commandTimeout = default(int?));

        PagedQueryResult<T> PagedQuery<T>(PagedQueryOptions options, object param = null, bool buffered = true, int? commandTimeout = default(int?));

        Task<PagedQueryResult<T>> PagedQueryAsync<T>(PagedQueryOptions options, object param = null, int? commandTimeout = default(int?));

        IEnumerable<T> QuerySingleTable<T>(string sqlConditionPart = null, object condition = null, string sqlOrderByPart = null, bool buffered = true, int? commandTimeout = default(int?));

        Task<IEnumerable<T>> QuerySingleTableAsync<T>(string sqlConditionPart = null, object condition = null, string sqlOrderByPart = null, int? commandTimeout = default(int?));

        #endregion
    }
}
