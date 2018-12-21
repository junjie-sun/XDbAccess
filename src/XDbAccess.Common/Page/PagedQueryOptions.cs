// Copyright (c) junjie sun. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace XDbAccess.Common
{
    /// <summary>
    /// 分页查询参数
    /// </summary>
    public class PagedQueryOptions
    {
        /// <summary>
        /// 当前页号，从0开始
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 每页记录数
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// SELECT部分的SQL
        /// 示例：[Field1],[Field2],[Field3]
        /// </summary>
        public string SqlFieldsPart { get; set; }

        /// <summary>
        /// FROM部分的SQL
        /// 示例1：[Table]
        /// 示例2：[Table1] a,[Table2] b
        /// 示例3：[Table1] a JOIN [Table2] b ON a.Field=b.Field
        /// </summary>
        public string SqlFromPart { get; set; }

        /// <summary>
        /// WHERE部分的SQL
        /// 示例1：[Field1]=@Param1 AND [Field2]=@Param2
        /// 示例2：a.[Field1]=@Param1 AND b.[Field2]=@Param2
        /// </summary>
        public string SqlConditionPart { get; set; }

        /// <summary>
        /// ORDER部分的SQL
        /// 示例：[Field1],[Field2] desc
        /// </summary>
        public string SqlOrderPart { get; set; }
    }
}
