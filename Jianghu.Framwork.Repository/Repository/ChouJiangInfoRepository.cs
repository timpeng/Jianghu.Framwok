using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Transactions;
using Jianghu.Framwork.Repository.Model;

namespace Jianghu.Framwork.Repository.Repository
{
    public class ChouJiangInfoRepository
    {
        private readonly SqlHelper _sqlHelper = new SqlHelper();
        public ChouJiangInfoRepository()
        {
            _sqlHelper.ConnectionString = FieldConfiguration.Game;
        }
        /// <summary>
        /// 插入抽奖物品
        /// </summary>
        /// <returns></returns>
        public bool Insert(ChouJiangInfo model, string uid)
        {
            new MemberInfoRepository().Update(uid, 368);
            _sqlHelper.ConnectionString = FieldConfiguration.Game;
            bool s = _sqlHelper.ExecuteNonQuery(
                "insert into dbo.ChouJiangInfo(aName,aItem,aItemValue) values(@aName,@aItem,@aItemValue)",
                new SqlParameter("@aName", model.aName),
                new SqlParameter("@aItem", model.aItem),
                new SqlParameter("@aItemValue", model.aItemValue)) > 0;
            return s;
        }
        /// <summary>
        /// 更新物品领取状态
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Messager<ChouJiangInfo> Update(ChouJiangInfo model)
        {
            var message = new Messager<ChouJiangInfo>();
            var info = new AvatarInfoRepository().GetNameByUName(model.aName);
            var list = new List<string>();
            for (int i = 8; i <= info.aStoreItem2.Length; i += 8)
            {
                list.Add(info.aStoreItem2.Substring(i - 8, 8));
            }
            int num = 0;
            for (int j = 0; j < list.Count; j++)
            {
                if (list[j] == "00000000")
                {
                    num = j;
                    break;
                }
            }
            if (list.Count<28)
            {
                num = list.Count;
            }
            string item = info.aStoreItem2.Substring(0, num * 8) + model.aItem+"000";
            string itemValue = info.aStoreItemValue2.Substring(0, num * 9) + model.aItemValue;
            if (list.Count>=28&&!list.Contains("00000000"))
            {
                message.Message = "仓库已满，请清空后在领取！";
            }
            else
            {
                if (GetEntity(model.Id).aState==1)
                {
                    message.Message = "已经领取过,请勿再尝试!";
                }
                else
                {
                    bool s = new AvatarInfoRepository().GetUpDateChoujiang(model.aName, item, itemValue);
                    if (s)
                    {
                        if (_sqlHelper.ExecuteNonQuery("update ChouJiangInfo set aState=1 where Id=@Id",
                    new SqlParameter("@Id", model.Id)) > 0)
                        {
                            message.Message = "领取成功!";
                            message.IsSuccess = true;
                        }
                    }
                }
            }
            return message;
        }

        public ChouJiangInfo GetEntity(int id)
        {
            return _sqlHelper.ExecuteObject<ChouJiangInfo>(
                "select aState from ChouJiangInfo where Id=@Id",
                new SqlParameter("@Id", id));
        }
        public IEnumerable<ChouJiangInfo> GetEntities(string aName,int rows,int page,out int count)
        {
            var data = _sqlHelper.ExecuteObjects<ChouJiangInfo>(
                "select * from ChouJiangInfo where aName=@aName",
                new SqlParameter("@aName", aName));
            count = data.Count();
            return data.Skip(rows * (page - 1)).Take(rows);
        }
    }
}
