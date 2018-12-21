// Copyright (c) junjie sun. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace XDbAccess.Common
{
    /// <summary>
    /// 映射数据库表
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class TableAttribute : Attribute
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="tableName">数据库表名</param>
        public TableAttribute(string tableName)
        {
            TableName = tableName;
        }

        /// <summary>
        /// 数据库表名
        /// </summary>
        public string TableName { get; }
    }
}
