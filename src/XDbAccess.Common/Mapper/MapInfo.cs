// Copyright (c) junjie sun. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace XDbAccess.Common
{
    /// <summary>
    /// 数据库表映射信息
    /// </summary>
    public class MapInfo
    {
        private IList<FieldInfo> _Fields = new List<FieldInfo>();

        /// <summary>
        /// 数据库表名
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 是否有自增长字段
        /// </summary>
        public bool HasIdentity { get; set; }

        /// <summary>
        /// 是否有主键字段
        /// </summary>
        public bool HasPrimaryKey { get; set; }

        /// <summary>
        /// 映射的字段集合
        /// </summary>
        public IList<FieldInfo> Fields
        {
            get
            {
                return _Fields;
            }
            set
            {
                _Fields = value;
            }
        }
    }

    /// <summary>
    /// 数据库字段映射信息
    /// </summary>
    public class FieldInfo
    {
        /// <summary>
        /// 字段名
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// 属性名
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// 是否为主键字段
        /// </summary>
        public bool IsPrimaryKey { get; set; }

        /// <summary>
        /// 是否为自增长字段
        /// </summary>
        public bool IsIdentity { get; set; }
    }
}
