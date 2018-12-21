// Copyright (c) junjie sun. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace XDbAccess.AutoTrans
{
    /// <summary>
    /// 对数据库连接对象进行封装
    /// </summary>
    public class DbConnectionWrap : IDbConnection
    {
        private IDbConnection _Conn;

        private ILogger _Logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="realConnection"></param>
        /// <param name="loggerFactory"></param>
        public DbConnectionWrap(IDbConnection realConnection, ILoggerFactory loggerFactory)
        {
            _Conn = realConnection;
            Guid = System.Guid.NewGuid().ToString();
            if (loggerFactory != null)
            {
                _Logger = loggerFactory.CreateLogger<DbConnectionWrap>();
            }
        }

        /// <summary>
        /// 连接唯一Id
        /// </summary>
        public string Guid { get; }

        /// <summary>
        /// 实际数据库连接对象
        /// </summary>
        public IDbConnection RealConnection
        {
            get
            {
                return _Conn;
            }
        }

        /// <summary>
        /// 事务对象
        /// </summary>
        public TransScope TransScope { get; set; }

        /// <summary>
        /// 连接字符串
        /// </summary>
        public string ConnectionString
        {
            get
            {
                return _Conn.ConnectionString;
            }

            set
            {
                _Conn.ConnectionString = value;
            }
        }

        /// <summary>
        /// 连接超时
        /// </summary>
        public int ConnectionTimeout
        {
            get
            {
                return _Conn.ConnectionTimeout;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Database
        {
            get
            {
                return _Conn.Database;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ConnectionState State
        {
            get
            {
                return _Conn.State;
            }
        }

        /// <summary>
        /// 开启事务
        /// </summary>
        /// <returns></returns>
        public IDbTransaction BeginTransaction()
        {
            return _Conn.BeginTransaction();
        }

        /// <summary>
        /// 开启事务
        /// </summary>
        /// <param name="il"></param>
        /// <returns></returns>
        public IDbTransaction BeginTransaction(IsolationLevel il)
        {
            return _Conn.BeginTransaction(il);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="databaseName"></param>
        public void ChangeDatabase(string databaseName)
        {
            _Conn.ChangeDatabase(databaseName);
        }

        /// <summary>
        /// 关闭连接 
        /// </summary>
        public void Close()
        {
            if (TransScope == null)
            {
                _Conn.Close();
                LogDebug("TransScope Connection closed. DbConnectionWrap.Guid={0}", Guid);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IDbCommand CreateCommand()
        {
            return _Conn.CreateCommand();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            if (TransScope == null)
            {
                _Conn.Dispose();
                LogDebug("TransScope Connection disposed. DbConnectionWrap.Guid={0}", Guid);
            }
        }

        /// <summary>
        /// 打开连接
        /// </summary>
        public void Open()
        {
            if (State == ConnectionState.Closed)
            {
                _Conn.Open();
                LogDebug("Connection opened. DbConnectionWrap.Guid={0}", Guid);
            }
        }

        /// <summary>
        /// 打开连接
        /// </summary>
        /// <returns></returns>
        public async Task OpenAsync()
        {
            if (State == ConnectionState.Closed)
            {
                await((DbConnection)_Conn).OpenAsync();
                LogDebug("Connection opened. DbConnectionWrap.Guid={0}", Guid);
            }
        }

        private void LogDebug(string message, params object[] args)
        {
            if (_Logger != null)
            {
                _Logger.LogDebug(message, args);
            }
        }
    }
}
