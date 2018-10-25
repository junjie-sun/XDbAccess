// Copyright (c) junjie sun. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XDbAccess.Common
{
    public class MySqlSQLBuilder : ISQLBuilder
    {
        public string BuildInsertSql(MapInfo meta)
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

            if (meta.HasIdentity)
            {
                sqlBuilder.Append(" SELECT CAST(LAST_INSERT_ID() AS SIGNED);");

            }
            else
            {
                sqlBuilder.Append(" SELECT CAST(0 AS SIGNED);");

            }

            return sqlBuilder.ToString();
        }

        public string BuildUpdateSql(MapInfo meta)
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
                sqlBuilder.Append(field.PropertyName);
            }

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

            sqlBuilder.Append(";");

            return sqlBuilder.ToString();
        }

        public string BuidlPagedQuerySql(PagedQueryOptions options)
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

            var sql = string.Format(@"SELECT {0} FROM {1} WHERE 1=1 {2} {3} LIMIT {4},{5};",
                options.SqlFieldsPart, options.SqlFromPart, string.IsNullOrEmpty(options.SqlConditionPart) ? string.Empty : "AND " + options.SqlConditionPart,
                string.IsNullOrEmpty(options.SqlOrderPart) ? string.Empty : "ORDER BY " + options.SqlOrderPart, pageStartIndex, currentPageCount);

            return sql;
        }

        public string BuildQueryCountSql(string sqlFromPart, string sqlConditionPart = null)
        {
            if (string.IsNullOrEmpty(sqlFromPart))
            {
                throw new ArgumentNullException("Need to specify sqlFromPart");
            }

            var sql = string.Format(" SELECT COUNT(1) FROM {0} where 1=1 {1}  ;"
                , sqlFromPart, string.IsNullOrEmpty(sqlConditionPart) ? string.Empty : "AND " + sqlConditionPart);

            return sql;
        }

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

        public string BuildSelectSql(MapInfo meta, bool hasFromPart = false, string sqlConditionPart = null)
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

            if (hasFromPart)
            {
                sqlBuilder.AppendFormat(" FROM `{0}`", meta.TableName);

                if (!String.IsNullOrWhiteSpace(sqlConditionPart))
                {
                    sqlBuilder.AppendFormat(" WHERE {0}", sqlConditionPart);
                }
            }

            return sqlBuilder.ToString();
        }
    }
}
