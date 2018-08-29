// Copyright (c) junjie sun. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace XDbAccess.Dapper
{
    public class MapInfo
    {
        private IList<FieldInfo> _Fields = new List<FieldInfo>();

        public string TableName { get; set; }

        public bool HasIdentity { get; set; }

        public bool HasPrimaryKey { get; set; }

        public bool HasCondition { get; set; }

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

        public bool IsCondition { get; set; }
    }
}
