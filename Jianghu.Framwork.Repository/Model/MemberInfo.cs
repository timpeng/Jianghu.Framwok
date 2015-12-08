using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jianghu.Framwork.Repository.Model
{
    /// <summary>
    /// 用户表
    /// </summary>
    public class MemberInfo
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string uID { get; set; }
        /// <summary>
        /// 用户密码，大写MD5加密
        /// </summary>
        public string uPassword { get; set; }
        public int mUserSort { get; set; }
        public int uBlockInfo { get; set; }
        /// <summary>
        /// 商城币
        /// </summary>
        public int uTCash { get; set; }
        /// <summary>
        /// 是否在线 1在线，0不在线
        /// </summary>
        public int uOnlineCheck { get; set; }
        public int uYoungUser { get; set; }
        public int uOnlineTime { get; set; }
        public string uOfflineTime { get; set; }
        public string uSecretNumber { get; set; }

    }
}
