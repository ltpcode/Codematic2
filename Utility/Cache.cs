using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Remoting.Messaging;

namespace Maticsoft.Utility
{
    /// <summary>
    /// 数据缓存保存信息异步处理委托
    /// </summary>
    delegate void EventSaveCache(object key, object value);
    /// <summary>
    /// 对象缓存类
    /// </summary>
    public class Cache
    {        
        // 用于缓存数据的Hashtable       
        protected System.Collections.Hashtable _Cache = new System.Collections.Hashtable();
        protected Object _LockObj = new object();              
        public int Count
        {
            get
            {return _Cache.Count;}
        }

        /// <summary>
        /// 构造缓存对象
        /// </summary>
        public Cache()
        {
        }

        #region 获取指定键值的对象GetObject()

        /// <summary>
        /// 获取指定键值的对象
        /// </summary>
        /// <param name="key">键值</param>
        /// <returns>object</returns>
        public virtual object GetObject(object key)
        {
            if (_Cache.ContainsKey(key))
            {
                return _Cache[key];
            }
            return null;
        }

        #endregion

        #region 把对象按指定的键值保存到缓存中SaveCaech()
        /// <summary>
        /// 把对象按指定的键值保存到缓存中
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="value">保存的对象</param>
        public void SaveCache(object key, object value)
        {
            EventSaveCache save = new EventSaveCache(SetCache);
            IAsyncResult ar = save.BeginInvoke(key, value, new System.AsyncCallback(Results), null);
        }

        /// <summary>
        /// 把对象按指定的键值保存到缓存中
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="value">保存的对象</param>
        protected virtual void SetCache(object key, object value)
        {
            lock (_LockObj)
            {
                if (!_Cache.ContainsKey(key))
                    _Cache.Add(key, value);
            }
        }

        private void Results(IAsyncResult ar)
        {
            EventSaveCache fd = (EventSaveCache)((AsyncResult)ar).AsyncDelegate;
            fd.EndInvoke(ar);
        }
        #endregion

        #region 在缓存中删除指定键值的对象DelObject()
        /// <summary>
        /// 在缓存中删除指定键值的对象
        /// </summary>
        /// <param name="key">键值</param>
        public virtual void DelObject(object key)
        {
            lock (_Cache.SyncRoot)
            {
                _Cache.Remove(key);
            }
        }
        #endregion

        #region 清除缓存中所有对象Clear()
        /// <summary>
        /// 清除缓存中所有对象
        /// </summary>
        public virtual void Clear()
        {
            lock (_Cache.SyncRoot)
            {
                _Cache.Clear();
            }
        }
        #endregion


    }



}
