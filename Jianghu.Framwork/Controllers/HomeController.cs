using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Jianghu.Framwork.Core;
using Jianghu.Framwork.Repository.Fields;
using Jianghu.Framwork.Repository.Model;
using Jianghu.Framwork.Repository.Repository;

namespace Jianghu.Framwork.Controllers
{
    public class HomeController : BaseController
    {
        private readonly AvatarInfoRepository _avatarInfoRepository = new AvatarInfoRepository();
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult GetLevel(AvatarInfo model)
        {
            return Json(_avatarInfoRepository.UpdateLevel(model));
        }
        public ActionResult GetLevel()
        {
            ViewBag.NameList = _avatarInfoRepository
                .GetNameByUid(SessionInfo.uID)
                .Select(u => new SelectListItem
                {
                    Text = u,
                    Value = u
                });
            return View();
        }
        public ActionResult GetLiBao(TribeEnum tribe, string aName)
        {
            return Json(_avatarInfoRepository.GetLiBao(tribe, aName));
        }
        public ActionResult LiBao()
        {
            ViewBag.NameList = _avatarInfoRepository
                .GetNameByUid(SessionInfo.uID)
                .Select(u => new SelectListItem
                {
                    Text = u,
                    Value = u
                });
            return View();
        }

        public ActionResult ExChange()
        {
            ViewBag.NameList = _avatarInfoRepository
                    .GetNameByUid(SessionInfo.uID)
                    .Select(u => new SelectListItem
                    {
                        Text = u,
                        Value = u
                    });
            return View();
        }
        [HttpPost]
        public ActionResult ExChange(string item, int playtime, string aName)
        {
            return Json(_avatarInfoRepository.GetItem(item, playtime, aName));
        }
        public ActionResult GetPlayTime(string aName)
        {
            return Json(_avatarInfoRepository.GetNameByUName(aName).aPlayTime, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 垃圾神装兑换贡献1件=20贡献
        /// </summary>
        /// <param name="aName">用户角色名</param>
        /// <returns></returns>
        public ActionResult ExchangeItem(string aName)
        {
            return Json(_avatarInfoRepository.ExchangeItem(aName));
        }

        public ActionResult GetPaiHang(int type)
        {
            return Json(new AvatarInfoRepository().GetPaiHangs(type));
        }
    }
}
