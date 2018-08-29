// Copyright (c) junjie sun. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XDbAccess.Dapper
{
    public interface ISQLBuilder
    {
        string BuildInsertSql(MapInfo meta);

        string BuildUpdateSql(MapInfo meta);

        string BuidlPagedQuerySql(PagedQueryOptions options);

        string BuildQueryCountSql(string sqlFromPart, string sqlConditionPart = null);
    }
}
