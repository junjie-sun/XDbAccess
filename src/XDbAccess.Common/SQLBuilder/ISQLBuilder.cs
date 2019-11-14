// Copyright (c) junjie sun. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace XDbAccess.Common
{
    /// <summary>
    /// SQL构造器接口
    /// </summary>
    public interface ISQLBuilder
    {
        /// <summary>
        /// 构造INSERT语句
        /// </summary>
        /// <param name="meta">映射信息</param>
        /// <param name="isBuildIdentitySql">是否生成查询自动生成ID的SQL</param>
        /// <returns></returns>
        string BuildInsertSql(MapInfo meta, bool isBuildIdentitySql = true);

        /// <summary>
        /// 构造UPDATE语句
        /// </summary>
        /// <param name="meta">映射信息</param>
        /// <param name="isUpdateByPrimaryKey">是否以主键作为条件进行更新</param>
        /// <param name="sqlConditionPart">WHERE部分的SQL，当isUpdateByPrimaryKey=false时有效</param>
        /// <param name="valuePropertyPrefix">为了避免与WHERE部分的参数有相同名称冲突，在SET部分的参数所添加的前缀，当isUpdateByPrimaryKey=false时有效</param>
        /// <returns></returns>
        string BuildUpdateSql(MapInfo meta, bool isUpdateByPrimaryKey = true, string sqlConditionPart = null, string valuePropertyPrefix = SQLBuilderConstants.ValuePropertyPrefix);

        /// <summary>
        /// 构造分页查询语句
        /// </summary>
        /// <param name="options">分页查询参数</param>
        /// <returns></returns>
        string BuidlPagedQuerySql(PagedQueryOptions options);

        /// <summary>
        /// 构造SELECT COUNT语句
        /// </summary>
        /// <param name="sqlFromPart">FROM部分的SQL</param>
        /// <param name="sqlConditionPart">WHERE部分的SQL</param>
        /// <param name="sqlGroupPart">GROUP部分的SQL</param>
        /// <returns></returns>
        string BuildQueryCountSql(string sqlFromPart, string sqlConditionPart = null, string sqlGroupPart = null);

        /// <summary>
        /// 构造SELECT语句
        /// </summary>
        /// <param name="meta">映射信息</param>
        /// <param name="isBuildFullSql">是否构造完整的SQL，如果为false则只构造SELECT部分的语句</param>
        /// <param name="sqlConditionPart">WHERE部分的SQL</param>
        /// <param name="sqlOrderPart">ORDER部分的SQL</param>
        /// <returns></returns>
        string BuildSelectSql(MapInfo meta, bool isBuildFullSql = false, string sqlConditionPart = null, string sqlOrderPart = null);

        /// <summary>
        /// 构造DELETE语句
        /// </summary>
        /// <param name="meta">映射信息</param>
        /// <param name="isDeleteByPrimaryKey">是否以主键作为条件进行删除</param>
        /// <param name="sqlConditionPart">WHERE部分的SQL，当isDeleteByPrimaryKey=false时有效</param>
        /// <returns></returns>
        string BuildDeleteSql(MapInfo meta, bool isDeleteByPrimaryKey = true, string sqlConditionPart = null);
    }

    /// <summary>
    /// SQLBuilder常量
    /// </summary>
    public static class SQLBuilderConstants
    {
        /// <summary>
        /// 值属性前缀
        /// </summary>
        public const string ValuePropertyPrefix = "Value";
    }
}
