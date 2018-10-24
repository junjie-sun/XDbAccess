// Copyright (c) junjie sun. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace XDbAccess.Common
{
    [AttributeUsage(AttributeTargets.Property)]
    public class FieldAttribute : Attribute
    {
        public FieldAttribute(string fieldName, bool isPrimaryKey = false, bool isIdentity = false, bool isCondition = false)
        {
            FieldName = fieldName;
            IsPrimaryKey = isPrimaryKey;
            IsIdentity = isIdentity;
            IsCondition = isCondition;
        }

        public string FieldName { get; }

        public bool IsPrimaryKey { get; }

        public bool IsIdentity { get; }

        public bool IsCondition { get; set; }
    }
}
