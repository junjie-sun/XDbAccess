// Copyright (c) junjie sun. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XDbAccess.AutoTrans
{
    public enum TransScopeState
    {
        Init,
        Begin,
        Commit,
        Rollback,
        Dispose
    }
}
