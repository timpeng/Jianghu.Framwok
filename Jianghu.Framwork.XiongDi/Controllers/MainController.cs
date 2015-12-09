using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Jianghu.Framwork.Core;
using Jianghu.Framwork.Repository.Repository;

namespace Jianghu.Framwork.XiongDi.Controllers
{
    public class MainController : BaseController
    {
        //
        // GET: /Main/

        public ActionResult Index()
        {
            if (SessionInfo!=null)
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
            }
            return View();
        }

    }
}
