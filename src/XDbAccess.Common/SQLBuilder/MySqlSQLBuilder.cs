// Copyright (c) junjie sun. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XDbAccess.Common
{
    /// <summary>
    /// MYSQL语句构造器接口
    /// </summary>
    public class MySqlSQLBuilder : ISQLBuilder
    {
        /// <summary>
        /// 构造INSERT语句
        /// </summary>
        /// <param name="meta">映射信息</param>
        /// <param name="isBuildIdentitySql">是否生成查询自动生成ID的SQL</param>
        /// <returns></returns>
        public string BuildInsertSql(MapInfo meta, bool isBuildIdentitySql = true)
        {
            if (meta == null || meta.Fields.Count == 0)
            {
                throw new ArgumentException("Need to specify fields.");
            }

            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.AppendFormat("INSERT INTO `{0}` (", meta.TableName);

            bool isFirst = true;
            for (var i = 0; i < meta.Fields.Count; i++)
            {
                var field = meta.Fields[i];
                if (field.IsIdentity)
                {
                    continue;
                }

                if (!isFirst)
                {
                    sqlBuilder.Append(",");
                }
                else
                {
                    isFirst = false;
                }

                sqlBuilder.Append("`");
                sqlBuilder.Append(field.FieldName);
                sqlBuilder.Append("`");
            }

            isFirst = true;
            sqlBuilder.Append(") VALUES (");
            for (var i = 0; i < meta.Fields.Count; i++)
            {
                var field = meta.Fields[i];
                if (field.IsIdentity)
                {
                    continue;
                }

                if (!isFirst)
                {
                    sqlBuilder.Append(",");
                }
                else
                {
                    isFirst = false;
                }

                sqlBuilder.Append("@");
                sqlBuilder.Append(field.PropertyName);
            }

            sqlBuilder.Append(");");

            if (isBuildIdentitySql)
            {
                if (meta.HasIdentity)
                {
                    sqlBuilder.Append(" SELECT CAST(LAST_INSERT_ID() AS SIGNED);");
                }
                else
                {
                    sqlBuilder.Append(" SELECT CAST(0 AS SIGNED);");
                }
            }

            return sqlBuilder.ToString();
        }

        /// <summary>
        /// 构造UPDATE语句
        /// </summary>
        /// <param name="meta">映射信息</param>
        /// <param name="isUpdateByPrimaryKey">是否以主键作为条件进行更新</param>
        /// <param name="sqlConditionPart">WHERE部分的SQL，当isUpdateByPrimaryKey=false时有效</param>
        /// <param name="valuePropertyPrefix">为了避免与WHERE部分的参数有相同名称冲突，在SET部分的参数所添加的前缀，当isUpdateByPrimaryKey=false时有效</param>
        /// <returns></returns>
        public string BuildUpdateSql(MapInfo meta, bool isUpdateByPrimaryKey = true, string sqlConditionPart = null, string valuePropertyPrefix = SQLBuilderConstants.ValuePropertyPrefix)
        {
            if (meta == null || meta.Fields.Count == 0)
            {
                throw new ArgumentException("Need to specify fields.");
            }

            if (!meta.HasPrimaryKey)
            {
                throw new ArgumentException("Need to specify a primary key.");
            }

            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.AppendFormat("UPDATE `{0}` SET ", meta.TableName);

            bool isFirst = true;
            for (var i = 0; i < meta.Fields.Count; i++)
            {
                var field = meta.Fields[i];
                if (field.IsPrimaryKey)
                {
                    continue;
                }

                if (!isFirst)
                {
                    sqlBuilder.Append(",");
                }
                else
                {
                    isFirst = false;
                }

                sqlBuilder.Append("`");
                sqlBuilder.Append(field.FieldName);
                sqlBuilder.Append("`=@");
                sqlBuilder.Append(isUpdateByPrimaryKey ? field.PropertyName : valuePropertyPrefix + field.PropertyName);
            }

            if (isUpdateByPrimaryKey)
            {
                sqlBuilder.Append(" WHERE 1=1");
                IEnumerable<FieldInfo> conditionFields = meta.Fields.Where(field => field.IsPrimaryKey);
                foreach (var conditionField in conditionFields)
                {
                    sqlBuilder.Append(" AND ");
                    sqlBuilder.Append("`");
                    sqlBuilder.Append(conditionField.FieldName);
                    sqlBuilder.Append("`=@");
                    sqlBuilder.Append(conditionField.PropertyName);
                }
            }
            else if (!isUpdateByPrimaryKey && !string.IsNullOrWhiteSpace(sqlConditionPart))
            {
                sqlBuilder.AppendFormat(" WHERE {0}", sqlConditionPart);
            }

            sqlBuilder.Append(";");

            return sqlBuilder.ToString();
        }

        /// <summary>
        /// 构造分页查询语句
        /// </summary>
        /// <param name="options">分页查询参数</param>
        /// <returns></returns>
        public string BuildPagedQuerySql(PagedQueryOptions options)
        {
            if (string.IsNullOrEmpty(options.SqlFieldsPart))
            {
                throw new ArgumentNullException("Need to specify SqlFieldsPart");
            }

            if (string.IsNullOrEmpty(options.SqlFromPart))
            {
                throw new ArgumentNullException("Need to specify SqlFromPart");
            }

            if (string.IsNullOrEmpty(options.SqlOrderPart))
            {
                throw new ArgumentNullException("Need to specify SqlOrderPart");
            }

            int pageStartIndex = options.PageSize * options.PageIndex;
            int currentPageCount = options.PageSize;

            var sql = string.Format(@"SELECT {0} FROM {1} {2} {3} {4} LIMIT {5},{6};",
                options.SqlFieldsPart, options.SqlFromPart,
                string.IsNullOrEmpty(options.SqlConditionPart) ? string.Empty : "WHERE " + options.SqlConditionPart,
                string.IsNullOrEmpty(options.SqlGroupPart) ? string.Empty : "GROUP BY " + options.SqlGroupPart,
                string.IsNullOrEmpty(options.SqlOrderPart) ? string.Empty : "ORDER BY " + options.SqlOrderPart,
                pageStartIndex, currentPageCount);

            return sql;
        }

        /// <summary>
        /// 构造SELECT COUNT语句
        /// </summary>
        /// <param name="sqlFromPart">FROM部分的SQL</param>
        /// <param name="sqlConditionPart">WHERE部分的SQL</param>
        /// <param name="sqlGroupPart">GROUP部分的SQL</param>
        /// <returns></returns>
        public string BuildQueryCountSql(string sqlFromPart, string sqlConditionPart = null, string sqlGroupPart = null)
        {
            if (string.IsNullOrEmpty(sqlFromPart))
            {
                throw new ArgumentNullException("Need to specify sqlFromPart");
            }

            var sql = string.Format(@"
                SELECT COUNT(1) FROM
                (
                    SELECT 1 as CountField
                    FROM {0} {1} {2}
                ) as CountTable;
            ", sqlFromPart,
            string.IsNullOrEmpty(sqlConditionPart) ? string.Empty : "WHERE " + sqlConditionPart,
            string.IsNullOrEmpty(sqlGroupPart) ? string.Empty : "GROUP BY " + sqlGroupPart);

            return sql;
        }

        /// <summary>
        /// 构造REPLACE语句
        /// </summary>
        /// <param name="meta">映射信息</param>
        /// <returns></returns>
        public string BuildReplaceSql(MapInfo meta)
        {
            if (meta == null || meta.Fields.Count == 0)
            {
                return string.Empty;
            }

            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.AppendFormat("REPLACE INTO `{0}` (", meta.TableName);

            bool isFirst = true;
            for (var i = 0; i < meta.Fields.Count; i++)
            {
                var field = meta.Fields[i];
                if (field.IsIdentity)
                {
                    continue;
                }

                if (!isFirst)
                {
                    sqlBuilder.Append(",");
                }
                else
                {
                    isFirst = false;
                }

                sqlBuilder.Append("`");
                sqlBuilder.Append(field.FieldName);
                sqlBuilder.Append("`");
            }

            isFirst = true;
            sqlBuilder.Append(") VALUES (");
            for (var i = 0; i < meta.Fields.Count; i++)
            {
                var field = meta.Fields[i];
                if (field.IsIdentity)
                {
                    continue;
                }

                if (!isFirst)
                {
                    sqlBuilder.Append(",");
                }
                else
                {
                    isFirst = false;
                }

                sqlBuilder.Append("@");
                sqlBuilder.Append(field.PropertyName);
            }

            sqlBuilder.Append(");");

            return sqlBuilder.ToString();
        }

        /// <summary>
        /// 构造SELECT语句
        /// </summary>
        /// <param name="meta">映射信息</param>
        /// <param name="isBuildFullSql">是否构造完整的SQL，如果为false则只构造SELECT部分的语句</param>
        /// <param name="sqlConditionPart">WHERE部分的SQL</param>
        /// <param name="sqlOrderPart">ORDER部分的SQL</param>
        /// <returns></returns>
        public string BuildSelectSql(MapInfo meta, bool isBuildFullSql = false, string sqlConditionPart = null, string sqlOrderPart = null)
        {
            if (meta == null || meta.Fields.Count == 0)
            {
                throw new ArgumentException("Need to specify fields.");
            }

            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("SELECT ");

            bool isFirst = true;
            for (var i = 0; i < meta.Fields.Count; i++)
            {
                var field = meta.Fields[i];

                if (!isFirst)
                {
                    sqlBuilder.Append(",");
                }
                else
                {
                    isFirst = false;
                }

                sqlBuilder.Append("`");
                sqlBuilder.Append(field.FieldName);
                sqlBuilder.Append("` ");
                sqlBuilder.Append(field.PropertyName);
            }

            if (isBuildFullSql)
            {
                sqlBuilder.AppendFormat(" FROM `{0}`", meta.TableName);

                if (!string.IsNullOrWhiteSpace(sqlConditionPart))
                {
                    sqlBuilder.AppendFormat(" WHERE {0}", sqlConditionPart);
                }

                if (!string.IsNullOrWhiteSpace(sqlOrderPart))
                {
                    sqlBuilder.AppendFormat(" ORDER BY {0}", sqlOrderPart);
                }
            }

            return sqlBuilder.ToString();
        }

        /// <summary>
        /// 构造DELETE语句
        /// </summary>
        /// <param name="meta">映射信息</param>
        /// <param name="isDeleteByPrimaryKey">是否以主键作为条件进行删除</param>
        /// <param name="sqlConditionPart">WHERE部分的SQL，当isDeleteByPrimaryKey=false时有效</param>
        /// <returns></returns>
        public string BuildDeleteSql(MapInfo meta, bool isDeleteByPrimaryKey = true, string sqlConditionPart = null)
        {
            if (meta == null || meta.Fields.Count == 0)
            {
                throw new ArgumentException("Need to specify fields.");
            }

            if (!meta.HasPrimaryKey)
            {
                throw new ArgumentException("Need to specify a primary key.");
            }

            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.AppendFormat("DELETE FROM `{0}`", meta.TableName);

            if (isDeleteByPrimaryKey)
            {
                sqlBuilder.Append(" WHERE 1=1");
                IEnumerable<FieldInfo> conditionFields = meta.Fields.Where(field => field.IsPrimaryKey);
                foreach (var conditionField in conditionFields)
                {
                    sqlBuilder.Append(" AND ");
                    sqlBuilder.Append("`");
                    sqlBuilder.Append(conditionField.FieldName);
                    sqlBuilder.Append("`=@");
                    sqlBuilder.Append(conditionField.PropertyName);
                }
            }
            else if(!isDeleteByPrimaryKey && !string.IsNullOrWhiteSpace(sqlConditionPart))
            {
                sqlBuilder.AppendFormat(" WHERE {0}", sqlConditionPart);
            }

            sqlBuilder.Append(";");

            return sqlBuilder.ToString();
        }
    }
}
