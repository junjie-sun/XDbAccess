// Copyright (c) junjie sun. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XDbAccess.AutoTrans
{
    /// <summary>
    /// TransScope状态
    /// </summary>
    public enum TransScopeState
    {
        /// <summary>
        /// 初始
        /// </summary>
        Init,

        /// <summary>
        /// 事务开启
        /// </summary>
        Begin,

        /// <summary>
        /// 事务提交
        /// </summary>
        Commit,

        /// <summary>
        /// 事务回滚
        /// </summary>
        Rollback,

        /// <summary>
        /// 已释放
        /// </summary>
        Dispose
    }
}
