﻿// Copyright (c) junjie sun. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace XDbAccess.Common
{
    public class MapInfo
    {
        private IList<FieldInfo> _Fields = new List<FieldInfo>();

        public string TableName { get; set; }

        public bool HasIdentity { get; set; }

        public bool HasPrimaryKey { get; set; }

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

    public class FieldInfo
    {
        public string FieldName { get; set; }

        public string PropertyName { get; set; }

        public bool IsPrimaryKey { get; set; }

        public bool IsIdentity { get; set; }
    }
}
