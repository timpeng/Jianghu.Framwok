using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Jianghu.Framwork.Repository.Model;

namespace Jianghu.Framwork.Repository.Repository
{
    public class MemberInfoRepository
    {
        private readonly SqlHelper _sqlHelper = new SqlHelper();
        public MemberInfoRepository()
        {
            _sqlHelper.ConnectionString = FieldConfiguration.Account;
        }
        /// <summary>
        /// 用户注册
        /// </summary>
        public Messager<MemberInfo> Register(MemberInfo model)
        {
            var message = new Messager<MemberInfo>();
            if (GetMemberInfoByUid(model.uID) != null)
            {
                message.Message = "用户名重复!";
            }
            else
            {
                if (_sqlHelper.ExecuteNonQuery("insert into MemberInfo(uID,uPassword) values(@uID,@uPassword)",
    new SqlParameter("@uID", model.uID),
    new SqlParameter("@uPassword", model.uPassword.CreateMd5())) > 0)
                {
                    message.Message = "注册成功!";
                    message.IsSuccess = true;
                    message.Model = GetMemberInfoByUid(model.uID);
                }
                else
                {
                    message.Message = "系统繁忙，请稍后重试!";
                }
            }
            return message;
        }
        /// <summary>
        /// 用户登录
        /// </summary>
        public Messager<MemberInfo> Login(string uid, string pwd)
        {
            var message = new Messager<MemberInfo>();
            var info = GetMemberInfoByUid(uid);
            if (info == null)
            {
                message.Message = "用户名不存在!";
            }
            else if (info.uPassword != pwd.CreateMd5())
            {
                message.Message = "用户名或密码错误!";
            }
            else
            {
                message.IsSuccess = true;
                message.Message = "登录成功!";
                message.Model = info;
            }
            return message;
        }
        /// <summary>
        /// 根据用户名查找用户信息
        /// </summary>
        /// <param name="uid">用户名</param>
        /// <returns></returns>
        public MemberInfo GetMemberInfoByUid(string uid)
        {
            return _sqlHelper.ExecuteObject<MemberInfo>("select * from MemberInfo where uID=@uID",
                new SqlParameter("@uID", uid));
        }
        /// <summary>
        /// 修改密码
        /// </summary>
        public Messager<MemberInfo> ChangePwd(string uid, string newPwd,string oldPwd)
        {
            var message = new Messager<MemberInfo>();
            var data = GetMemberInfoByUid(uid);
            if (data == null)
            {
                message.Message = "用户名不存在!";
            }
            else if (data.uPassword != oldPwd.CreateMd5())
            {
                message.Message = "旧密码错误!";
            }
            else
            {
                if (_sqlHelper.ExecuteNonQuery("update MemberInfo set uPassword=@uPassword where uID=@uID",
                    new SqlParameter("@uID", uid),
                    new SqlParameter("@uPassword", newPwd.CreateMd5())) > 0)
                {
                    message.Message = "修改密码成功!";
                    message.IsSuccess = true;
                }
                else
                {
                    message.Message = "系统繁忙，请稍后重试!";
                }
            }
            return message;
        }
        /// <summary>
        /// 增加商城币
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="uTCash"></param>
        /// <returns></returns>
        public bool UpdateCash(string uid, int uTCash)
        {
            return _sqlHelper.ExecuteNonQuery("update MemberInfo set uTCash=uTCash+@uTCash  where  uID=@uID",
                 new SqlParameter("@uTCash", uTCash)
                , new SqlParameter("@uID", uid)) > 0;
        }
        /// <summary>
        /// 减少商城币
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="uTCash"></param>
        /// <returns></returns>
        public bool Update(string uid, int uTCash)
        {
            return _sqlHelper.ExecuteNonQuery("update MemberInfo set uTCash=uTCash-@uTCash  where  uID=@uID",
                 new SqlParameter("@uTCash", uTCash)
                , new SqlParameter("@uID", uid)) > 0;
        }
    }
}
