using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;

namespace Jianghu.Framwork.Core
{
    public class CacheManager
    {
        public Cache CacheContext
        {
            get { return HttpRuntime.Cache; }
        }
        /// <summary>
        /// 设定缓存
        /// </summary>
        public object this[string key]
        {
            set
            {
                CacheContext[key] = value;
            }
        }
        /// <summary>
        /// 获取缓存
        /// </summary>
        public T Get<T>(string key) where T : class
        {
            return CacheContext.Get(key) as T;
        }
        /// <summary>
        /// 移除缓存
        /// </summary>
        public bool Remove(string key)
        {
            if (Get<object>(key) != null)
            {
                try
                {
                    CacheContext.Remove(key);
                }
                catch
                {
                    return false;
                }
            }
            return true;
        }
    }
}
