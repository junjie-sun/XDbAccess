// Copyright (c) junjie sun. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace XDbAccess.AutoTrans
{
    /// <summary>
    /// 管理数据库连接、事务，并根据配置提供相应的DbHelper的实例
    /// </summary>
    public interface IDbContext
    {
        /// <summary>
        /// 连接字符串
        /// </summary>
        string ConnectionString { get; }

        /// <summary>
        /// 获取已打开的数据库连接对象
        /// </summary>
        /// <returns></returns>
        DbConnectionWrap GetOpenedConnection();

        /// <summary>
        /// 获取已打开的数据库连接对象(异步版本)
        /// </summary>
        /// <returns></returns>
        Task<DbConnectionWrap> GetOpenedConnectionAsync();

        /// <summary>
        /// 开启事务范围
        /// </summary>
        /// <param name="option"></param>
        /// <param name="il"></param>
        /// <returns></returns>
        TransScope TransScope(TransScopeOption option, IsolationLevel il = IsolationLevel.ReadCommitted);
    }
}
