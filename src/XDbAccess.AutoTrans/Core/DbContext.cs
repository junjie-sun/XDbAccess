// Copyright (c) junjie sun. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace XDbAccess.AutoTrans
{
    public class DbContext : IDbContext
    {
        private IDbFactory _DbFactory;

        private ILogger _Logger;

        //保存处于事务中的数据库连接对象，该列表支持多线程与异步方法调用
        private AsyncLocal<Stack<DbConnectionWrap>> _TransScopeConnections = new AsyncLocal<Stack<DbConnectionWrap>>();

        public DbContext(DbContextOptions options)
        {
            if(options.DbFactory == null)
            {
                throw new ArgumentException("Not specified DbFactory");
            }

            ConnectionString = options.ConnectionString;
            _DbFactory = options.DbFactory;
            LoggerFactory = options.LoggerFactory;
            if (LoggerFactory != null)
            {
                _Logger = LoggerFactory.CreateLogger<DbContext>();
            }
        }

        #region 接口实现

        public ILoggerFactory LoggerFactory { get; }

        public string ConnectionString { get; }

        public DbConnectionWrap GetOpenedConnection()
        {
            var wrapConn = CreateConnectionWrap(false);
            wrapConn.Open();
            if (wrapConn.TransScope != null)
            {
                wrapConn.TransScope.Begin();
            }
            return wrapConn;
        }

        public async Task<DbConnectionWrap> GetOpenedConnectionAsync()
        {
            var wrapConn = CreateConnectionWrap(false);
            await wrapConn.OpenAsync();
            if (wrapConn.TransScope != null)
            {
                wrapConn.TransScope.Begin();
            }
            return wrapConn;
        }

        public TransScope TransScope(TransScopeOption option = TransScopeOption.Required, IsolationLevel il = IsolationLevel.ReadCommitted)
        {
            var conn = CreateConnectionWrap(option == TransScopeOption.RequireNew);
            RegistTransScopeConnection(conn);
            var scope = new TransScope(conn, conn.TransScope, il, option, LoggerFactory, conn.Guid);
            conn.TransScope = scope;
            scope.OnDisposed += TransScope_OnDisposed;
            return scope;
        }

        #endregion

        #region 私有方法

        private void RegistTransScopeConnection(DbConnectionWrap conn)
        {
            if (_TransScopeConnections.Value == null)
            {
                _TransScopeConnections.Value = new Stack<DbConnectionWrap>();
            }

            if (_TransScopeConnections.Value.Count == 0 || _TransScopeConnections.Value.Peek() != conn)
            {
                _TransScopeConnections.Value.Push(conn);
                LogDebug("Regist TransScope Connection(Root TransScope). DbConnectionWrap.Guid={0}", conn.Guid);
            }
        }

        private void TransScope_OnDisposed(object sender, EventArgs e)
        {
            if (_TransScopeConnections.Value.Count == 0)
            {
                throw new InvalidOperationException("TransScope disposed has error, no connection.");
            }

            var connWrap = _TransScopeConnections.Value.Peek();
            var scope = (TransScope)sender;

            if (connWrap.TransScope != scope)
            {
                throw new InvalidOperationException("TransScope disposed has error, TransScope is not match.");
            }

            //当TransScope处于最上层时才关闭数据库连接
            connWrap.TransScope = scope.Parent;
            if (connWrap.TransScope == null)
            {
                _TransScopeConnections.Value.Pop();
                if (_TransScopeConnections.Value.Count == 0)
                {
                    _TransScopeConnections.Value = null;
                }
                connWrap.Dispose();
            }
        }

        private DbConnectionWrap CreateConnectionWrap(bool isNew)
        {
            DbConnectionWrap wrapConn;

            //在TransScope范围中共用一个数据库连接对象
            if (_TransScopeConnections.Value != null && _TransScopeConnections.Value.Count > 0 && !isNew)
            {
                wrapConn = _TransScopeConnections.Value.Peek();
                LogDebug("Get a exist connection from TransScope. DbConnectionWrap.Guid={0}", wrapConn.Guid);
            }
            else
            {
                var conn = _DbFactory.CreateConnection();
                wrapConn = new DbConnectionWrap(conn, LoggerFactory);
                LogDebug("Create new connection. DbConnectionWrap.Guid={0}", wrapConn.Guid);
            }

            return wrapConn;
        }

        private void LogDebug(string message, params object[] args)
        {
            if (_Logger != null)
            {
                _Logger.LogDebug(message, args);
            }
        }

        #endregion
    }
}
