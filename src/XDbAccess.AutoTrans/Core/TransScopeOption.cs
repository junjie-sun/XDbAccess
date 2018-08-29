// Copyright (c) junjie sun. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XDbAccess.AutoTrans
{
    /// <summary>
    /// 事务范围选项。
    /// </summary>
    public enum TransScopeOption
    {
        /// <summary>
        /// 如果当前范围内已有事务则延用该事务，否则创建新事务。这是默认值。
        /// </summary>
        Required,

        /// <summary>
        /// 无论当前范围内是否存在事件都创建新事务。
        /// </summary>
        RequireNew
    }
}
