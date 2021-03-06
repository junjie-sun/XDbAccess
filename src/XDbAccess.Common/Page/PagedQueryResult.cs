﻿// Copyright (c) junjie sun. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace XDbAccess.Common
{
    /// <summary>
    /// 分页查询结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PagedQueryResult<T>
    {
        /// <summary>
        /// 每页记录数
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 当前页码，从0开始计算
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 总记录数，只有当PageIndex=0时才返回
        /// </summary>
        public long Total { get; set; }

        /// <summary>
        /// 数据结果集
        /// </summary>
        public List<T> Data { get; set; }
    }
}
