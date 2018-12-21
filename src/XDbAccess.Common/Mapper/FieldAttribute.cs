// Copyright (c) junjie sun. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace XDbAccess.Common
{
    /// <summary>
    /// 映射数据库字段
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class FieldAttribute : Attribute
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="fieldName">数据库字段名</param>
        /// <param name="isPrimaryKey">是否为主键字段</param>
        /// <param name="isIdentity">是否为自增长字段</param>
        public FieldAttribute(string fieldName, bool isPrimaryKey = false, bool isIdentity = false)
        {
            FieldName = fieldName;
            IsPrimaryKey = isPrimaryKey;
            IsIdentity = isIdentity;
        }

        /// <summary>
        /// 数据库字段名
        /// </summary>
        public string FieldName { get; }

        /// <summary>
        /// 是否为主键字段
        /// </summary>
        public bool IsPrimaryKey { get; }

        /// <summary>
        /// 是否为自增长字段
        /// </summary>
        public bool IsIdentity { get; }
    }
}
