// Copyright (c) junjie sun. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace XDbAccess.Common
{
    public interface ISQLBuilder
    {
        string BuildInsertSql(MapInfo meta);

        string BuildUpdateSql(MapInfo meta);

        string BuidlPagedQuerySql(PagedQueryOptions options);

        string BuildQueryCountSql(string sqlFromPart, string sqlConditionPart = null);

        string BuildSelectSql(MapInfo meta, bool hasFromPart = false, string sqlConditionPart = null, string sqlOrderByPart = null);
    }
}
