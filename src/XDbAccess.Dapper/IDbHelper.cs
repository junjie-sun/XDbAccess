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
    /// <summary>
    /// DbHelper接口
    /// </summary>
    /// <typeparam name="DbContextImpl"></typeparam>
    public interface IDbHelper<DbContextImpl> where DbContextImpl : IDbContext
    {
        #region Dapper接口封装

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        int Execute(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        Task<int> ExecuteAsync(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        IDataReader ExecuteReader(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        Task<IDataReader> ExecuteReaderAsync(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        object ExecuteScalar(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        T ExecuteScalar<T>(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        Task<object> ExecuteScalarAsync(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        Task<T> ExecuteScalarAsync<T>(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="buffered"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        IEnumerable<dynamic> Query(string sql, object param = null, bool buffered = true, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));

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
        IEnumerable<T> Query<T>(string sql, object param = null, bool buffered = true, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        Task<IEnumerable<dynamic>> QueryAsync(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        dynamic QueryFirst(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        T QueryFirst<T>(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        Task<object> QueryFirstAsync(Type type, string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        Task<T> QueryFirstAsync<T>(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        dynamic QueryFirstOrDefault(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        T QueryFirstOrDefault<T>(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        Task<T> QueryFirstOrDefaultAsync<T>(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        GridReaderWrap QueryMultiple(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        Task<GridReaderWrap> QueryMultipleAsync(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        dynamic QuerySingle(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        T QuerySingle<T>(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        Task<T> QuerySingleAsync<T>(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        dynamic QuerySingleOrDefault(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        T QuerySingleOrDefault<T>(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        Task<object> QuerySingleOrDefaultAsync(Type type, string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="commandTimeout"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        Task<T> QuerySingleOrDefaultAsync<T>(string sql, object param = null, int? commandTimeout = default(int?), CommandType? commandType = default(CommandType?));

        #endregion

        #region 扩展接口

        /// <summary>
        /// 执行INSERT操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">实体对象</param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        long Insert<T>(T entity, int? commandTimeout = default(int?));

        /// <summary>
        /// 执行INSERT操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">实体对象</param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        Task<long> InsertAsync<T>(T entity, int? commandTimeout = default(int?));

        /// <summary>
        /// 执行批量INSERT操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">实体对象集合</param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        int BatchInsert<T>(IEnumerable<T> list, int? commandTimeout = default(int?));

        /// <summary>
        /// 执行批量INSERT操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">实体对象集合</param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        Task<int> BatchInsertAsync<T>(IEnumerable<T> list, int? commandTimeout = default(int?));

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
        int Update<T>(T entity, bool isUpdateByPrimaryKey = true, string sqlConditionPart = null, object condition = null, int? commandTimeout = default(int?));


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
        Task<int> UpdateAsync<T>(T entity, bool isUpdateByPrimaryKey = true, string sqlConditionPart = null, object condition = null, int? commandTimeout = default(int?));

        /// <summary>
        /// 执行批量UPDATE操作，只支持根据主键更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">实体对象集合</param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        int BatchUpdate<T>(IEnumerable<T> list, int? commandTimeout = default(int?));

        /// <summary>
        /// 执行批量UPDATE操作，只支持根据主键更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">实体对象集合</param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        Task<int> BatchUpdateAsync<T>(IEnumerable<T> list, int? commandTimeout = default(int?));

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="options">分页查询参数</param>
        /// <param name="param">条件对象</param>
        /// <param name="buffered"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        PagedQueryResult<T> PagedQuery<T>(PagedQueryOptions options, object param = null, bool buffered = true, int? commandTimeout = default(int?));

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="options">分页查询参数</param>
        /// <param name="param">条件对象</param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        Task<PagedQueryResult<T>> PagedQueryAsync<T>(PagedQueryOptions options, object param = null, int? commandTimeout = default(int?));

        /// <summary>
        /// 单表查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlConditionPart">WHERE部分的SQL</param>
        /// <param name="condition">条件对象</param>
        /// <param name="sqlOrderByPart">ORDER部分的SQL</param>
        /// <param name="buffered"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        IEnumerable<T> QuerySingleTable<T>(string sqlConditionPart = null, object condition = null, string sqlOrderByPart = null, bool buffered = true, int? commandTimeout = default(int?));

        /// <summary>
        /// 单表查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlConditionPart">WHERE部分的SQL</param>
        /// <param name="condition">条件对象</param>
        /// <param name="sqlOrderByPart">ORDER部分的SQL</param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        Task<IEnumerable<T>> QuerySingleTableAsync<T>(string sqlConditionPart = null, object condition = null, string sqlOrderByPart = null, int? commandTimeout = default(int?));

        /// <summary>
        /// 执行DELETE操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="condition">条件对象</param>
        /// <param name="isDeleteByPrimaryKey">是否以主键作为条件进行删除</param>
        /// <param name="sqlConditionPart">WHERE部分的SQL，当isDeleteByPrimaryKey=false时有效</param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        int Delete<T>(object condition, bool isDeleteByPrimaryKey = true, string sqlConditionPart = null, int? commandTimeout = default(int?));

        /// <summary>
        /// 执行DELETE操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="condition">条件对象</param>
        /// <param name="isDeleteByPrimaryKey">是否以主键作为条件进行删除</param>
        /// <param name="sqlConditionPart">WHERE部分的SQL，当isDeleteByPrimaryKey=false时有效</param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        Task<int> DeleteAsync<T>(object condition, bool isDeleteByPrimaryKey = true, string sqlConditionPart = null, int? commandTimeout = default(int?));

        /// <summary>
        /// 执行批量DELETE操作，只支持根据主键删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="conditions">条件对象集合</param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        int BatchDelete<T>(IEnumerable<object> conditions, int? commandTimeout = default(int?));

        /// <summary>
        /// 执行批量DELETE操作，只支持根据主键删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="conditions">条件对象集合</param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        Task<int> BatchDeleteAsync<T>(IEnumerable<object> conditions, int? commandTimeout = default(int?));

        #endregion
    }
}
