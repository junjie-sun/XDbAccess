// Copyright (c) junjie sun. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XDbAccess.Dapper
{
    public class PagedQueryOptions
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        /// <summary>
        /// 示例：[Field1],[Field2],[Field3]
        /// </summary>
        public string SqlFieldsPart { get; set; }

        /// <summary>
        /// 示例1：[Table]
        /// 示例2：[Table1] a,[Table2] b
        /// 示例3：[Table1] a JOIN [Table2] b ON a.Field=b.Field
        /// </summary>
        public string SqlFromPart { get; set; }

        /// <summary>
        /// 示例1：[Field1]=@Param1 AND [Field2]=@Param2
        /// 示例2：a.[Field1]=@Param1 AND b.[Field2]=@Param2
        /// </summary>
        public string SqlConditionPart { get; set; }

        /// <summary>
        /// 示例：[Field1],[Field2] desc
        /// </summary>
        public string SqlOrderPart { get; set; }
    }
}
