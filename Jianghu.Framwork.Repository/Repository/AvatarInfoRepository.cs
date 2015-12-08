using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Jianghu.Framwork.Repository.Fields;
using Jianghu.Framwork.Repository.Model;

namespace Jianghu.Framwork.Repository.Repository
{
    public class AvatarInfoRepository
    {
        private readonly SqlHelper _sqlHelper = new SqlHelper();
        public AvatarInfoRepository()
        {
            _sqlHelper.ConnectionString = FieldConfiguration.Game;
        }
        /// <summary>
        /// 更新用户等级
        /// </summary>
        /// <param name="model"></param>
        public bool UpdateLevel(AvatarInfo model)
        {
            if (GetNameByUName(model.aName).aLevel > model.aLevel)
            {
                return false;
            }
            return _sqlHelper.ExecuteNonQuery(@"update AvatarInfo set aStateBonusPoint=@aStateBonusPoint,
                aSkillPoint=@aSkillPoint,aGeneralExperience=@aGeneralExperience,aLevel=@aLevel,
                aStrength=1,aki=1,aVitality=1,aWisdom=1,aEatLifePotion=@aEatLifePotion,aEatManaPotion=@aEatManaPotion where aName=@aName"
                , new SqlParameter("@aStateBonusPoint", model.aStateBonusPoint)
                , new SqlParameter("@aSkillPoint", model.aSkillPoint)
                , new SqlParameter("@aGeneralExperience", model.aGeneralExperience)
                , new SqlParameter("@aLevel", model.aLevel)
                , new SqlParameter("@aName", model.aName)
                , new SqlParameter("@aEatLifePotion", model.aEatLifePotion)
                , new SqlParameter("@aEatManaPotion", model.aEatManaPotion)) > 0;
        }
        /// <summary>
        /// 获取用户角色
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public IEnumerable<string> GetNameByUid(string uid)
        {
            return _sqlHelper.ExecuteObjects<AvatarInfo>
                ("select * from AvatarInfo where uID=@uID", new SqlParameter("@uID", uid)).Select(u => u.aName);
        }
        public AvatarInfo GetNameByUName(string aName)
        {
            return _sqlHelper.ExecuteObject<AvatarInfo>
                ("select aMoneyHONG,aLevel,aStoreItem,aPlayTime,aEatLifePotion,aKillOtherTribe,aStoreItemValue2,aStoreItem2,aEatStrengthPotion,aEatDexterityPotion from AvatarInfo where aName=@aName", new SqlParameter("@aName", aName));
        }
        public bool UpdateStateBonusPoint(string aName)
        {
            if (GetNameByUName(aName).aLevel < 100 || GetNameByUName(aName).aLevel > 112)
            {
                return false;
            }
            return _sqlHelper.ExecuteNonQuery("update AvatarInfo set aStateBonusPoint=490,aVitality=1,aStrength=1,aKi=1,aWisdom=1 where aName=@aName",
                new SqlParameter("@aName", aName)) > 0;
        }
        public bool GetLiBao(TribeEnum tribe, string aName, long aMoneyHong = 1000000)
        {
            if (!IsEmpty(aName))
            {
                return false;
            }
            switch (tribe)
            {
                case TribeEnum.正:
                    return GetUpDate(aMoneyHong, aName, "99000000990010009922600013397000134240001345100013226000132530001328000061073000610760006107900061082000610850006108800013478000134790001348000099221000", "289931120289931120289931120000003112000003112000003112000003112000003112000003112000003112000003112000003112000003112000003112000003112");
                case TribeEnum.邪:
                    return GetUpDate(aMoneyHong, aName, "99000000990010009922600033145000331720003319900033226000332530003328000071073000710760007107900071082000710850007108800033478000334790003348000099221000", "289931120289931120289931120000003112000003112000003112000003112000003112000003112000003112000003112000003112000003112000003112000003112");
                case TribeEnum.魔:
                    return GetUpDate(aMoneyHong, aName, "99000000990010009922600053145000531720005319900053226000532530005328000081073000810760008107900081082000810850008108800053478000534790005348000099221000", "289931120289931120289931120000003112000003112000003112000003112000003112000003112000003112000003112000003112000003112000003112000003112");
            }
            return false;
        }
        public bool GetLiBao(string aName, long aMoneyHong = 1000000)
        {
            if (!IsEmpty(aName))
            {
                return false;
            }
            return GetUpDate(aMoneyHong, aName,
                @"99000000990010001318800013233000132570001328900033161000332420003326000033293000531880005324200053263000532870001502500015033000",
                @"289931120289931120000003112000003112000003112000003112000003112000003112000003112000003112000003112000003112000003112000003112", 500, 500);
        }
        public bool GetLiBaoXiongDi(string aName, long aMoneyHong=2000000)
        {
            if (!IsEmpty(aName))
            {
                return false;
            }
            return GetUpDate(aMoneyHong, aName,
                @"990000009900100099226000150030001500700015011000150150001501900015023000150270001502900035003000350070003501100035015000350190003502300035027000350290005500300055007000550110005501500055019000550230005502700055029000992170009922300099224000",
                @"289931120289931120289931120000003112000003112000003112000003112000003112000003112000003112000003112000003112000003112000003112000003112000003112000003112000003112000003112000003112000003112000003112000003112000003112000003112000003112000003112", 500, 700);
        }
        /// <summary>
        /// 时间兑换
        /// </summary>
        /// <param name="item">物品代码</param>
        /// <param name="playtime">耗费时间</param>
        /// <param name="aName">角色名</param>
        /// <param name="potion">丹的数量</param>
        /// <returns></returns>
        public Messager<AvatarInfo> GetItem(string item, int playtime, string aName, int potion=50)
        {
            var m = new Messager<AvatarInfo>();
            if (playtime < 1000 || CheckTime(1000, aName))
            {
                m.Message = "兑换时间小于1000分钟!";
            }
            else if (item.Equals("aEatStrengthPotion"))//20力
            {
                if (CheckaEatStrengthPotion(600,aName))
                {
                    m.Message = "力量丹已达上限!";
                }
                else if (_sqlHelper.ExecuteNonQuery("update AvatarInfo set aEatStrengthPotion=aEatStrengthPotion+20,aPlayTime=aPlayTime-1000 where aName=@aName", new SqlParameter("@aName", aName)) > 0)
                {
                    if (GetNameByUName(aName).aEatStrengthPotion > 600)
                    {
                        _sqlHelper.ExecuteNonQuery(
                            "update AvatarInfo set aEatStrengthPotion=600 where aName=@aName",
                            new SqlParameter("@aName", aName));
                    }
                    m.IsSuccess = true;
                    m.Message = "兑换力量丹成功!";
                }
            }
            else if (item.Equals("aEatDexterityPotion"))//20敏
            {

                if (CheckaEatDexterityPotion(600, aName))
                {
                    m.Message = "敏捷丹已达上限!";
                }
                else if (_sqlHelper.ExecuteNonQuery("update AvatarInfo set aEatDexterityPotion=aEatDexterityPotion+20,aPlayTime=aPlayTime-1000 where aName=@aName", new SqlParameter("@aName", aName)) > 0)
                {
                    if (GetNameByUName(aName).aEatDexterityPotion >600)
                    {
                        _sqlHelper.ExecuteNonQuery(
                            "update AvatarInfo set aEatDexterityPotion=600 where aName=@aName",
                            new SqlParameter("@aName", aName));
                    }
                    m.IsSuccess = true;
                    m.Message = "兑换敏捷丹成功!";
                }
            }
            else if (item.Equals("100"))//100贡献
            {
                if (_sqlHelper.ExecuteNonQuery("update AvatarInfo set aKillOtherTribe=aKillOtherTribe+100,aPlayTime=aPlayTime-1000 where aName=@aName", new SqlParameter("@aName", aName)) > 0)
                {
                    m.IsSuccess = true;
                    m.Message = "兑换贡献成功!";
                }
            }
            else if (item.Equals(potion.ToString()))//50雪丹
            {
                int val = GetNameByUName(aName).aEatLifePotion;
                if (val >= 1000)
                {
                    m.Message = "您的雪丹数量已达上限!";
                }
                else
                {
                    if (_sqlHelper.ExecuteNonQuery("update AvatarInfo set aEatLifePotion=aEatLifePotion+"+potion+",aPlayTime=aPlayTime-1000 where aName=@aName", new SqlParameter("@aName", aName)) > 0)
                    {
                        if (GetNameByUName(aName).aEatLifePotion > 1000)
                        {
                            _sqlHelper.ExecuteNonQuery("update AvatarInfo set aEatLifePotion=1000 where aName=@aName",
                                new SqlParameter("@aName", aName));
                        }
                        m.IsSuccess = true;
                        m.Message = "兑换雪丹成功!";
                    }
                }
            }
            else if (!IsEmpty(aName))
            {
                m.Message = "请先清空仓库!";
            }
            else
            {
                var result = item.Length / 8;
                string s = string.Empty;
                for (int i = 0; i < result; i++)
                {
                    s += "000003112";
                }
                if (_sqlHelper.ExecuteNonQuery(@"update AvatarInfo set aStoreItem=@aStoreItem,aStoreItemValue=@aStoreItemValue,aPlayTime=aPlayTime-1000 where aName=@aName"
                    , new SqlParameter("@aStoreItem", item)
                    , new SqlParameter("@aName", aName)
                    , new SqlParameter("@aStoreItemValue", s)) > 0)
                {
                    m.IsSuccess = true;
                    m.Message = "兑换物品成功!";
                }
            }
            return m;
        }
        public Messager<AvatarInfo> GetItem(string item, int playtime, string aName, string dataValue, string uid)
        {
            var m = new Messager<AvatarInfo>();
            if (playtime < 1000 || CheckTime(1000, aName))
            {
                m.Message = "兑换时间小于1000分钟!";
            }
            if (!string.IsNullOrEmpty(dataValue))
            {
                if (dataValue == "100000000")
                {
                    if (_sqlHelper.ExecuteNonQuery("update AvatarInfo set aMoneyHONG=aMoneyHONG+100000000,aPlayTime=aPlayTime-1000 where aName=@aName", new SqlParameter("@aName", aName)) > 0)
                    {
                        m.IsSuccess = true;
                        m.Message = "兑换游戏币成功!";
                    }
                }
                else if (dataValue == "100")
                {
                    m.Message = "抱歉，请联系群主兑换！";
                }
                else if (dataValue == "5000")
                {
                    #region 冗余
                    //if (new MemberInfoRepository().UpdateCash(uid, 5000))
                    //{
                    //    _sqlHelper.ConnectionString = FieldConfiguration.Game;
                    //    if (_sqlHelper.ExecuteNonQuery("update AvatarInfo set aPlayTime=aPlayTime-1000 where aName=@aName", new SqlParameter("@aName", aName)) > 0)
                    //    {
                    //        m.IsSuccess = true;
                    //        m.Message = "兑换商城币成功!";
                    //    }
                    //} 
                    #endregion
                    m.Message = "抱歉，请联系群主兑换";
                }
                else
                {
                    if (!IsEmpty(aName))
                    {
                        m.Message = "请先清空仓库!";
                    }
                    else if (GetUpDate(aName, item, "289931120"))
                    {
                        m.IsSuccess = true;
                        m.Message = "兑换宠物成功!";
                    }
                }
            }
            else if (!IsEmpty(aName))
            {
                m.Message = "请先清空仓库!";
            }
            else
            {
                if (GetUpDate(aName, item))
                {
                    m.IsSuccess = true;
                    m.Message = "兑换物品成功!";
                }
            }
            return m;
        }
        public Messager<AvatarInfo> CheckGongXian(int gongxian, string aName)
        {
            var m = new Messager<AvatarInfo>();
            if (gongxian < 800 || CheckGongxian(800, aName))
            {
                m.Message = "您的贡献值小于800!";
            }
            else if (CheckMoney(aName))
            {
                m.Message = "请保证包裹中的金币低于15E!";
            }
            else if (!IsEmpty(aName))
            {
                m.Message = "请先清空仓库!";
            }
            else
            {
                m.IsSuccess = true;
            }
            return m;
        }
        public Messager<AvatarInfo> GetChouJiang(string item, string itemValue, int gongxian, string aName)
        {
            gongxian = gongxian == 0 ? 800 : gongxian;
            var m = new Messager<AvatarInfo>();
            if (string.IsNullOrEmpty(item)) //金币
            {
                if (_sqlHelper.ExecuteNonQuery("update AvatarInfo set aMoneyHONG=aMoneyHONG+500000000,aKillOtherTribe=aKillOtherTribe-" + gongxian + " where aName=@aName",
                    new SqlParameter("@aName", aName)) > 0)
                {
                    m.IsSuccess = true;
                    m.Message = "抽取物品成功!";
                }
            }
            else if (string.IsNullOrEmpty(itemValue))//披风酒壶
            {
                if (_sqlHelper.ExecuteNonQuery("update AvatarInfo set aStoreItem=@aStoreItem,aKillOtherTribe=aKillOtherTribe-" + gongxian + " where aName=@aName",
                new SqlParameter("@aStoreItem", item),
                new SqlParameter("@aName", aName)) > 0)
                {
                    m.IsSuccess = true;
                    m.Message = "抽取物品成功!";
                }
            }
            else//其他物品
            {
                if (_sqlHelper.ExecuteNonQuery(@"update AvatarInfo set aStoreItem=@aStoreItem,
                aStoreItemValue=@aStoreItemValue,aKillOtherTribe=aKillOtherTribe-" + gongxian + " where aName=@aName",
                 new SqlParameter("@aStoreItem", item),
                 new SqlParameter("@aStoreItemValue", itemValue),
                 new SqlParameter("@aName", aName)) > 0)
                {
                    m.IsSuccess = true;
                    m.Message = "抽取物品成功!";
                }
            }
            m.Model = GetNameByUName(aName);
            return m;
        }
        /// <summary>
        /// 检查在线时间
        /// </summary>
        /// <param name="value"></param>
        /// <param name="aName"></param>
        /// <returns></returns>
        private bool CheckTime(int value, string aName)
        {
            return GetNameByUName(aName).aPlayTime < value;
        }
        /// <summary>
        /// 检查敏捷丹
        /// </summary>
        private bool CheckaEatDexterityPotion(int value, string aName)
        {
            return GetNameByUName(aName).aEatDexterityPotion >= value;
        }
        /// <summary>
        /// 检查力量丹
        /// </summary>
        private bool CheckaEatStrengthPotion(int value, string aName)
        {
            return GetNameByUName(aName).aEatStrengthPotion >= value;
        }
        /// <summary>
        /// 检查贡献值
        /// </summary>
        /// <param name="value"></param>
        /// <param name="aName"></param>
        /// <returns></returns>
        private bool CheckGongxian(int value, string aName)
        {
            var s = GetNameByUName(aName);
            return s.aKillOtherTribe < value;
        }
        private bool CheckMoney(string aName)
        {
            return GetNameByUName(aName).aMoneyHONG > 1500000000;
        }
        /// <summary>
        /// 检查仓库是否为空
        /// </summary>
        internal bool IsEmpty(string aName)
        {
            var s = new StringBuilder();
            for (int i = 1; i <= 224; i++)
            {
                s.Append("0");
            }
            string result = GetNameByUName(aName).aStoreItem;
            if (result != s.ToString() && result != string.Empty)
            {
                return false;
            }
            return true;
        }
        private bool GetUpDate(long aMoneyHong, string aName, string aStoreItem, string aStoreItemValue)
        {
            return _sqlHelper.ExecuteNonQuery(@"update AvatarInfo set aStoreItem=@aStoreItem,
                aStoreItemValue=@aStoreItemValue,aMoneyHONG=@aMoneyHONG where aName=@aName"
                 , new SqlParameter("@aStoreItem", aStoreItem)
                 , new SqlParameter("@aStoreItemValue", aStoreItemValue)
                 , new SqlParameter("@aMoneyHONG", aMoneyHong)
                 , new SqlParameter("@aName", aName)) > 0;
        }
        internal bool GetUpDate(string aName, string aStoreItem, string aStoreItemValue)
        {
            return _sqlHelper.ExecuteNonQuery(@"update AvatarInfo set aStoreItem=@aStoreItem,
                aStoreItemValue=@aStoreItemValue,aPlayTime=aPlayTime-1000 where aName=@aName"
                 , new SqlParameter("@aStoreItem", aStoreItem)
                 , new SqlParameter("@aStoreItemValue", aStoreItemValue)
                 , new SqlParameter("@aName", aName)) > 0;
        }
        internal bool GetUpDateChoujiang(string aName, string aStoreItem, string aStoreItemValue)
        {
            return _sqlHelper.ExecuteNonQuery(@"update AvatarInfo set aStoreItem2=@aStoreItem2,
                aStoreItemValue2=@aStoreItemValue2 where aName=@aName"
                 , new SqlParameter("@aStoreItem2", aStoreItem)
                 , new SqlParameter("@aStoreItemValue2", aStoreItemValue)
                 , new SqlParameter("@aName", aName)) > 0;
        }
        private bool GetUpDate(string aName, string aStoreItem)
        {
            return _sqlHelper.ExecuteNonQuery(@"update AvatarInfo set aStoreItem=@aStoreItem,aPlayTime=aPlayTime-1000 where aName=@aName"
                 , new SqlParameter("@aStoreItem", aStoreItem)
                 , new SqlParameter("@aName", aName)) > 0;
        }
        private bool GetUpDate(long aMoneyHong, string aName, string aStoreItem, string aStoreItemValue, int potion,int dan)
        {
            return _sqlHelper.ExecuteNonQuery(@"update AvatarInfo set aStoreItem=@aStoreItem,
                aStoreItemValue=@aStoreItemValue,aMoneyHONG=@aMoneyHONG,aEatLifePotion=@aEatLifePotion,aEatManaPotion=@aEatManaPotion,aNewWorldExitTime=@aNewWorldExitTime where aName=@aName"
                 , new SqlParameter("@aStoreItem", aStoreItem)
                 , new SqlParameter("@aStoreItemValue", aStoreItemValue)
                 , new SqlParameter("@aMoneyHONG", aMoneyHong)
                 , new SqlParameter("@aEatLifePotion", dan)
                 , new SqlParameter("@aEatManaPotion", dan)
                 , new SqlParameter("@aNewWorldExitTime", potion)
                 , new SqlParameter("@aName", aName)) > 0;
        }

        public Messager<AvatarInfo> ExchangeItem(string aName)
        {
            var message = new Messager<AvatarInfo>();
            var item = GetNameByUName(aName);
            if (!string.IsNullOrEmpty(item.aStoreItem))
            {
                var list = new List<string>();
                var newlist = new List<string>();
                for (int i = 8; i <= item.aStoreItem.Length; i += 8)
                {
                    list.Add(item.aStoreItem.Substring(i - 8, 8));
                }
                var oldlist = new List<string>();
                list.ForEach(s =>
                {
                    if (!_itemList.Contains(s) || !_itemListXiongdi.Contains(s))
                    {
                        if (s != "00000000")
                        {
                            newlist.Add(s);
                        }
                    }
                    else
                    {
                        if (s == "99231000")//贡献卷,回收100贡献
                        {
                            for (int i = 0; i < 4; i++)
                            {
                                oldlist.Add(s);
                            }
                        }
                        oldlist.Add(s);
                    }
                });
                if (!oldlist.Any())
                {
                    message.Message = "没有可以兑换的神装!";
                }
                else
                {
                    var s = new StringBuilder();
                    newlist.ForEach(r => s.Append(r));
                    if (_sqlHelper.ExecuteNonQuery("update AvatarInfo set aKillOtherTribe=aKillOtherTribe+@aKillOtherTribe,aStoreItem=@aStoreItem where aName=@aName",
                        new SqlParameter("@aStoreItem", s.ToString()),
                        new SqlParameter("@aKillOtherTribe", 20 * oldlist.Count),
                        new SqlParameter("@aName", aName)) > 0)
                    {
                        message.IsSuccess = true;
                        message.Message = "恭喜你,兑换贡献成功!";
                    }
                }
            }
            else
            {
                message.Message = "仓库为空!";
            }
            return message;
        }

        private readonly List<string> _itemList = new List<string>
        {
          "15003000" ,"15007000","15011000","15015000","15019000","15023000","15027000","15029000", 
          "35003000" ,"35007000","35011000","35015000","35019000","35023000","35027000","35027000",
          "55003000" ,"55007000","55011000","55015000","55019000","55023000","55027000","55029000"
        };
        private readonly List<string> _itemListXiongdi = new List<string>
        {
         "15045000", "15055000", "15065000", "15085000", 
         "15095000", "15105000", "35035000", "35055000", 
         "35065000", "35075000", "35085000", "35095000",
         "35105000", "55045000", "55055000", "55065000",
         "55075000", "55085000", "55095000", "55105000","99231000"
        };


        /// <summary>
        /// 排行榜列表
        /// </summary>
        public IEnumerable<PaiHang> GetPaiHangs(int type, int index = 30)
        {
            if (type == 0)
            {
                return _sqlHelper.ExecuteObjects<AvatarInfo>("select top " + index + " aEatLifePotion,aName from AvatarInfo order by aEatLifePotion desc").Select(x => new PaiHang
                {
                    Name = x.aName,
                    Numbers = x.aEatLifePotion
                });
            } if (type == 1)
            {
                return _sqlHelper.ExecuteObjects<AvatarInfo>("select top " + index + " aEatStrengthPotion,aName from AvatarInfo order by aEatStrengthPotion desc").Select(x => new PaiHang
                {
                    Name = x.aName,
                    Numbers = x.aEatStrengthPotion
                });
            }
            if (type == 2)
            {
                return _sqlHelper.ExecuteObjects<AvatarInfo>("select top " + index + " aEatDexterityPotion,aName from AvatarInfo order by aEatDexterityPotion desc").Select(x => new PaiHang
                {
                    Name = x.aName,
                    Numbers = x.aEatDexterityPotion
                });
            }
            if (type == 3)
            {
                return _sqlHelper.ExecuteObjects<AvatarInfo>("select top " + index + " aKillOtherTribe,aName from AvatarInfo order by aKillOtherTribe desc").Select(x => new PaiHang
                {
                    Name = x.aName,
                    Numbers = x.aKillOtherTribe
                });
            }
            return _sqlHelper.ExecuteObjects<AvatarInfo>("select top " + index + " aPlayTime,aName from AvatarInfo order by aPlayTime desc").Select(x => new PaiHang
            {
                Name = x.aName,
                Numbers = x.aPlayTime
            });
        }
    }
}
