using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Web;
using System.Web.SessionState;
using Jianghu.Framwork.Repository.Model;
using Jianghu.Framwork.Repository.Repository;

namespace Jianghu.Framwork.Core
{
    public class SessionManager
    {
        public static SessionManager Current
        {
            get
            {
                return new SessionManager();
            }
        }
        private static readonly string Key = FieldConfiguration.SessionKey;
        private static HttpSessionState Context
        {
            get
            {
                if (HttpContext.Current != null)
                {
                    return HttpContext.Current.Session;
                }
                return null;
            }
        }
        /// <summary>
        /// 设定session信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public  MemberInfo this[string key]
        {
            set
            {
                if (Context != null)
                {
                    Context[key] = value;
                }
            }
        }
        /// <summary>
        /// 获取session信息
        /// </summary>
        public static MemberInfo SessionInfo
        {
            get
            {
                if (Context != null)
                {
                    return Context[Key] as MemberInfo;
                }
                return null;
            }
        }
        /// <summary>
        /// 删除session信息
        /// </summary>

        public static void RemoveSession(string key=null)
        {
            if (Context != null)
            {
                if (key==null)
                {
                    Context.Remove(Key);
                }
                else
                {
                    Context.Remove(key);
                }
            }
        }

    }
}
