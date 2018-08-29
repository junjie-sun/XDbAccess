// Copyright (c) junjie sun. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XDbAccess.AutoTrans
{
    public class DbContextOptions
    {
        public string ConnectionString { get; set; }

        public IDbFactory DbFactory { get; set; }

        public ILoggerFactory LoggerFactory { get; set; }
    }
}
