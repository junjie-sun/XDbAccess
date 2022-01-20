// Copyright (c) junjie sun. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace XDbAccess.AutoTrans
{
    /// <summary>
    /// 事务范围，默认TransScopeOption为Required
    /// 当TransScopeOption为RequireNew时，TransScope是独立的，内层TransScope进行Commit、Rollback、Disable操作不会对外层的TransScope产生影响。
    /// 当TransScopeOption为Required时，内层的TransScope只能进行Rollback操作，当内层TransScope进行Rollback操作时会对事务产生影响，并且所有外层的TransScope.State将被设置为Rollback状态，
    /// 内层的TransScope进行Commit与Disable操作不会对事务产生影响，只有最外层的TransScope进行Commit与Disable操作时才会对事务产生影响。
    /// 不支持在TransScope范围中开启新线程并在新线程中使用TranScope
    /// </summary>
    public class TransScope : IDisposable
    {
        private TransScopeState _State = TransScopeState.Init;

        private IDbConnection _Conn;

        private IDbTransaction _Trans;

        private IsolationLevel _IsolationLevel;

        private ILoggerFactory _LoggerFactory;

        private ILogger _Logger;

        private string _ConnectionId;

        /// <summary>
        /// Disposed事件
        /// </summary>
        public event EventHandler OnDisposed;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="parent"></param>
        /// <param name="il"></param>
        /// <param name="option"></param>
        /// <param name="loggerFactory"></param>
        /// <param name="connectionId"></param>
        public TransScope(IDbConnection conn, TransScope parent, IsolationLevel il = IsolationLevel.ReadCommitted, TransScopeOption option = TransScopeOption.Required, ILoggerFactory loggerFactory = null, string connectionId = null)
        {
            if (loggerFactory != null)
            {
                _LoggerFactory = loggerFactory;
                _Logger = loggerFactory.CreateLogger<TransScope>();
            }

            Guid = System.Guid.NewGuid().ToString();
            Parent = parent;
            Option = option;
            _Conn = conn;
            _ConnectionId = connectionId;
            //当前事务为内嵌事务
            if (Parent != null && Option == TransScopeOption.Required)
            {
                _Trans = Parent.Trans;
                _State = Parent.State;
                LogDebug("Create Nest TransScope. DbConnectionWrap.Guid={0}, TransScope.Guid={1}", _ConnectionId, Guid);
            }
            else
            {
                LogDebug("Create Root TransScope. DbConnectionWrap.Guid={0}, TransScope.Guid={1}", _ConnectionId, Guid);
            }
            _IsolationLevel = il;
        }

        /// <summary>
        /// 唯一标识
        /// </summary>
        public string Guid { get; }

        /// <summary>
        /// 父级TransScope对象
        /// </summary>
        public TransScope Parent
        {
            get;
        }

        /// <summary>
        /// TransScope参数
        /// </summary>
        public TransScopeOption Option
        {
            get;
        }

        /// <summary>
        /// TransScope状态
        /// </summary>
        public TransScopeState State
        {
            get
            {
                return _State;
            }
        }

        /// <summary>
        /// 数据库事务对象
        /// </summary>
        public IDbTransaction Trans
        {
            get
            {
                return _Trans;
            }
        }

        /// <summary>
        /// 开启事务
        /// </summary>
        public void Begin()
        {
            if (_State == TransScopeState.Init)
            {
                _Trans = _Conn.BeginTransaction(_IsolationLevel);
                _State = TransScopeState.Begin;
                LogDebug("Begin Transaction. DbConnectionWrap.Guid={0}, TransScope.Guid={1}", _ConnectionId, Guid);
                if (Parent != null && Option == TransScopeOption.Required)
                {
                    Parent.SetBeginState(_Trans);
                }
            }
        }

        /// <summary>
        /// 提交事务
        /// </summary>
        public void Commit()
        {
            if ((Option == TransScopeOption.RequireNew || Parent == null) && State == TransScopeState.Begin)
            {
                Trans.Commit();
                _State = TransScopeState.Commit;
                LogDebug("Commit Transaction. DbConnectionWrap.Guid={0}, TransScope.Guid={1}", _ConnectionId, Guid);
            }
        }

        /// <summary>
        /// 回滚事务
        /// </summary>
        public void Rollback()
        {
            if ((Option == TransScopeOption.RequireNew || Parent == null) && State == TransScopeState.Begin)
            {
                Trans.Rollback();
                LogDebug("Rollback Transaction. DbConnectionWrap.Guid={0}, TransScope.Guid={1}", _ConnectionId, Guid);
            }
            _State = TransScopeState.Rollback;
            LogDebug("Set TransScopeState to Rollback. DbConnectionWrap.Guid={0}, TransScope.Guid={1}", _ConnectionId, Guid);
            if (Parent != null && Option == TransScopeOption.Required)
            {
                Parent.SetRollbackState();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            if (Option == TransScopeOption.RequireNew || Parent == null)
            {
                if (Trans != null)
                {
                    Trans.Dispose();
                    LogDebug("Dispose Transaction. DbConnectionWrap.Guid={0}, TransScope.Guid={1}", _ConnectionId, Guid);
                }
                _State = TransScopeState.Dispose;
                LogDebug("Set TransScopeState to Dispose. DbConnectionWrap.Guid={0}, TransScope.Guid={1}", _ConnectionId, Guid);
            }
            OnDisposed?.Invoke(this, new EventArgs());
        }

        private void SetRollbackState()
        {
            _State = TransScopeState.Rollback;
            LogDebug("Set TransScopeState to Rollback. DbConnectionWrap.Guid={0}, TransScope.Guid={1}", _ConnectionId, Guid);
            if (Parent != null)
            {
                Parent.SetRollbackState();
            }
        }

        private void SetBeginState(IDbTransaction trans)
        {
            _Trans = trans;
            _State = TransScopeState.Begin;
            LogDebug("Set TransScopeState to Begin. DbConnectionWrap.Guid={0}, TransScope.Guid={1}", _ConnectionId, Guid);
            if (Parent != null)
            {
                Parent.SetBeginState(trans);
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
