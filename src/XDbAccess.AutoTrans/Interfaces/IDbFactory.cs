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
    /// 数据库对象工厂
    /// </summary>
    public interface IDbFactory
    {
        /// <summary>
        /// 创建数据库连接对象
        /// </summary>
        /// <returns></returns>
        IDbConnection CreateConnection();
    }
}
