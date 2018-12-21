// Copyright (c) junjie sun. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace XDbAccess.AutoTrans
{
    /// <summary>
    /// DbContext参数对象构造器
    /// </summary>
    public class DbContextOptionsBuilder
    {
        private DbContextOptions _Options;

        /// <summary>
        /// 构造函数
        /// </summary>
        public DbContextOptionsBuilder()
        {
            _Options = new DbContextOptions();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="options"></param>
        public DbContextOptionsBuilder(DbContextOptions options)
        {
            _Options = options;
        }

        /// <summary>
        /// DbContext参数对象
        /// </summary>
        public DbContextOptions Options
        {
            get
            {
                return _Options;
            }
        }

        /// <summary>
        /// 配置日志工厂
        /// </summary>
        /// <param name="loggerFactory"></param>
        /// <returns></returns>
        public virtual DbContextOptionsBuilder UseLoggerFactory(ILoggerFactory loggerFactory)
        {
            Options.LoggerFactory = loggerFactory;
            return this;
        }
    }
}
