// Copyright (c) junjie sun. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Reflection;

namespace XDbAccess.Common
{
    /// <summary>
    /// 数据库映射解析器
    /// </summary>
    public static class MapParser
    {
        private static IDictionary<Type, MapInfo> _MetaInfoContainer = new Dictionary<Type, MapInfo>();

        /// <summary>
        /// 获取映射信息
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static MapInfo GetMapMetaInfo(Type type)
        {
            if (!_MetaInfoContainer.ContainsKey(type))
            {
                lock (type)
                {
                    if (!_MetaInfoContainer.ContainsKey(type))
                    {
                        var metaInfo = BuildMapMetaInfo(type);
                        _MetaInfoContainer.Add(type, metaInfo);
                    }
                }
            }

            return _MetaInfoContainer[type];
        }

        private static MapInfo BuildMapMetaInfo(Type type)
        {
            var metaInfo = new MapInfo();

            var tableNameAttribute = type.GetTypeInfo().GetCustomAttribute<TableAttribute>(true);
            metaInfo.TableName = tableNameAttribute != null ? tableNameAttribute.TableName : type.Name;

            var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty);
            foreach (var prop in properties)
            {
                var ignoreAttribute = prop.GetCustomAttribute<IgnoreAttribute>(true);
                if (ignoreAttribute != null)
                {
                    continue;
                }

                var fieldAttribute = prop.GetCustomAttribute<FieldAttribute>(true);
                var fieldName = fieldAttribute != null ? fieldAttribute.FieldName : prop.Name;

                var fieldInfo = new FieldInfo();

                fieldInfo.FieldName = fieldName;
                fieldInfo.PropertyName = prop.Name;
                if (fieldAttribute != null)
                {
                    fieldInfo.IsIdentity = fieldAttribute.IsIdentity;
                    fieldInfo.IsPrimaryKey = fieldAttribute.IsPrimaryKey;
                }

                if (!metaInfo.HasIdentity && fieldInfo.IsIdentity)
                {
                    metaInfo.HasIdentity = true;
                }

                if (!metaInfo.HasPrimaryKey && fieldInfo.IsPrimaryKey)
                {
                    metaInfo.HasPrimaryKey = true;
                }

                metaInfo.Fields.Add(fieldInfo);
            }

            return metaInfo;
        }
    }
}
