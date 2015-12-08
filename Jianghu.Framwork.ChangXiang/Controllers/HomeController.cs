using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Jianghu.Framwork.Core;
using Jianghu.Framwork.Repository.Fields;
using Jianghu.Framwork.Repository.Model;
using Jianghu.Framwork.Repository.Repository;
using Newtonsoft.Json;

namespace Jianghu.Framwork.ChangXiang.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            ViewBag.aName = SessionInfo.uID;
            var data = new List<SelectListItem>();
            data.Add(new SelectListItem { Text = "请选择", Value = string.Empty });
            var result = new AvatarInfoRepository()
                .GetNameByUid(SessionInfo.uID)
                .Select(u => new SelectListItem
                {
                    Text = u,
                    Value = u
                });
            data.AddRange(result);
            ViewBag.NameList = data;
            ViewBag.Cash = new MemberInfoRepository().GetMemberInfoByUid(SessionInfo.uID).uTCash;
            return View();
        }

        public ActionResult UpdateStateBonusPoint(string aName)
        {
            return Json(new AvatarInfoRepository().UpdateStateBonusPoint(aName));
        }
        public ActionResult GetItem(string aName, int page, int rows)
        {
            int count;
            var result = new ChouJiangInfoRepository().GetEntities(aName, rows, page, out count).ToList();
            result.ForEach(s =>
            {
                s.aItemName = _itemName[s.aItem.Trim()];
            });
            return Json(new { total = count, rows = result });
        }
        public ActionResult GetLiBao(string aName)
        {
            return Json(new AvatarInfoRepository().GetLiBao(aName));
        }
        [HttpPost]
        public ActionResult ExChange(string item, int playtime, string aName, string dataValue)
        {
            lock (this)
            {
                return Json(new AvatarInfoRepository().GetItem(Encrpyt.Decode(item), playtime, aName, Encrpyt.Decode(dataValue), SessionInfo.uID));
            }
        }
        public ActionResult GetPlayTime(string aName)
        {
            return Json(new AvatarInfoRepository().GetNameByUName(aName).aPlayTime, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Item(string aName)
        {
            if (!new AvatarInfoRepository().GetNameByUid(SessionInfo.uID).Contains(aName))
            {
                return Content("您没有权限查看!");
            }
            ViewBag.aName = aName;
            ViewBag.Dictionary = _itemName;
            return View();
        }
        public ActionResult Choujiang(string aName)
        {
            if (GetChoujiang() == null)
            {
                return Json("礼包已经抽取空了，请等待更新!");
            }
            var result = GetChoujiang().First();
            int cash = new MemberInfoRepository().GetMemberInfoByUid(SessionInfo.uID).uTCash;
            if (cash < 368)
            {
                return Json(",对不起,您的点数不足368点,请联系客服充值!");
            }
            bool data = new ChouJiangInfoRepository().Insert(new ChouJiangInfo
            {
                aName = aName,
                aItem = result.Key,
                aItemValue = result.Value
            }, SessionInfo.uID);
            if (data)
            {
                return Json("恭喜你获得道具：【" + _itemName[result.Key].Trim() + "】x1");
            }
            return Json("服务器繁忙,请稍后重试!");

        }

        public ActionResult GetChoujiangItem(ChouJiangInfo model)
        {
            return Json(new ChouJiangInfoRepository().Update(model));
        }
        private Dictionary<string, string> GetChoujiang()
        {
            lock (this)
            {
                int nums = 0;
                var random = new Random();
                string json = System.IO.File.ReadAllText(FieldConfiguration.ChouJiangFile + "s.json");
                var result = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(json);
                nums = result.Count + 1;//设定随机数上限为集合的长度
                int index = random.Next(nums);
                Dictionary<string, string> rt = null;
                if (index > 0)
                {
                    rt = result[index];
                }
                if (rt != null)
                {
                    result.Remove(rt);//从当前集合中移除已经抽取到的结果
                    System.IO.File.WriteAllText(FieldConfiguration.ChouJiangFile + "s.json", JsonConvert.SerializeObject(result));//重新写入文件中
                }
                return rt;//返回当前抽奖结果
            }
        }
        private readonly Dictionary<string, string> _itemName =
            new Dictionary<string, string>
        {
            {"99203", "武神秘籍"},
            {"99217", "武神披风"},
            {"99218", "顶级武神披风"},
            {"99197", "武神戒指"},
            {"99198", "武神葫芦"},
            {"99267", "乌鸦    "},
            {"99266", "喜鹊    "},
            {"99229", "蝙蝠    "},
            {"99226", "佛光    "},
            {"99227", "天兆    "},
            {"99228", "金刚    "},
            {"99148", "防破符  "},
            {"00723", "太阴洗  "},
            {"99194", "译名卷  "},
            {"00136", "血丹  	"},
            {"00137", "蓝丹  	"},
            {"02953", "畅享新神刀（正）"},
            {"02503", "畅享新神剑      "},
            {"03403", "畅享新斗玉      "},
            {"03853", "畅享新神衣（正）"},
            {"04303", "畅享新神腕（正）"},
            {"04753", "畅享新神靴（正）"},
            {"22503", "畅享新神刀（邪）"},
            {"22953", "畅享新神钺      "},
            {"23403", "畅享新神琴      "},
            {"23853", "畅享新神衣（邪）"},
            {"24303", "畅享新神腕（邪）"},
            {"24753", "畅享新神靴（邪）"},
            {"42503", "畅享新神刀（魔）"},
            {"42953", "畅享新神枪      "},
            {"43403", "畅享新神杵      "},
            {"43853", "畅享新神衣（魔）"},
            {"44303", "畅享新神腕（魔）"},
            {"44753", "畅享新神靴（魔）"},
        };
    }
}
