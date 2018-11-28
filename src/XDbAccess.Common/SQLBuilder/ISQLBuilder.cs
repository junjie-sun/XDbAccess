// Copyright (c) junjie sun. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace XDbAccess.Common
{
    public interface ISQLBuilder
    {
        string BuildInsertSql(MapInfo meta);

        string BuildUpdateSql(MapInfo meta, bool isUpdateByPrimaryKey = true, string sqlConditionPart = null, string valuePropertyPrefix = SQLBuilderConstants.ValuePropertyPrefix);

        string BuidlPagedQuerySql(PagedQueryOptions options);

        string BuildQueryCountSql(string sqlFromPart, string sqlConditionPart = null);

        string BuildSelectSql(MapInfo meta, bool hasFromPart = false, string sqlConditionPart = null, string sqlOrderByPart = null);

        string BuildDeleteSql(MapInfo meta, bool isDeleteByPrimaryKey = true, string sqlConditionPart = null);
    }

    public static class SQLBuilderConstants
    {
        public const string ValuePropertyPrefix = "Value";
    }
}
