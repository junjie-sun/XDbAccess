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

        public DbConnectionWrap(IDbConnection realConnection, ILoggerFactory loggerFactory)
        {
            _Conn = realConnection;
            Guid = System.Guid.NewGuid().ToString();
            if (loggerFactory != null)
            {
                _Logger = loggerFactory.CreateLogger<DbConnectionWrap>();
            }
        }

        public string Guid { get; }

        public IDbConnection RealConnection
        {
            get
            {
                return _Conn;
            }
        }

        public TransScope TransScope { get; set; }

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

        public int ConnectionTimeout
        {
            get
            {
                return _Conn.ConnectionTimeout;
            }
        }

        public string Database
        {
            get
            {
                return _Conn.Database;
            }
        }

        public ConnectionState State
        {
            get
            {
                return _Conn.State;
            }
        }

        public IDbTransaction BeginTransaction()
        {
            return _Conn.BeginTransaction();
        }

        public IDbTransaction BeginTransaction(IsolationLevel il)
        {
            return _Conn.BeginTransaction(il);
        }

        public void ChangeDatabase(string databaseName)
        {
            _Conn.ChangeDatabase(databaseName);
        }

        public void Close()
        {
            if (TransScope == null)
            {
                _Conn.Close();
                LogDebug("TransScope Connection closed. DbConnectionWrap.Guid={0}", Guid);
            }
        }

        public IDbCommand CreateCommand()
        {
            return _Conn.CreateCommand();
        }

        public void Dispose()
        {
            if (TransScope == null)
            {
                _Conn.Dispose();
                LogDebug("TransScope Connection disposed. DbConnectionWrap.Guid={0}", Guid);
            }
        }

        public void Open()
        {
            if (State == ConnectionState.Closed)
            {
                _Conn.Open();
                LogDebug("Connection opened. DbConnectionWrap.Guid={0}", Guid);
            }
        }

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
