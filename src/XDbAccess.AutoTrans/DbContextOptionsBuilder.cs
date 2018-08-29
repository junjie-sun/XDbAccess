// Copyright (c) junjie sun. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace XDbAccess.AutoTrans
{
    public class DbContextOptionsBuilder
    {
        private DbContextOptions _Options;

        public DbContextOptionsBuilder()
        {
            _Options = new DbContextOptions();
        }

        public DbContextOptionsBuilder(DbContextOptions options)
        {
            _Options = options;
        }

        public DbContextOptions Options
        {
            get
            {
                return _Options;
            }
        }

        public virtual DbContextOptionsBuilder UseLoggerFactory(ILoggerFactory loggerFactory)
        {
            Options.LoggerFactory = loggerFactory;
            return this;
        }
    }
}
