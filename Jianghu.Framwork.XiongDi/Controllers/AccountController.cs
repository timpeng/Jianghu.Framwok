using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Jianghu.Framwork.Core;
using Jianghu.Framwork.Repository.Model;
using Jianghu.Framwork.Repository.Repository;

namespace Jianghu.Framwork.XiongDi.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /Account/
        private readonly MemberInfoRepository _memberInfoRepository = new MemberInfoRepository();
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(string uId, string uPassword)
        {
            var model = _memberInfoRepository.Login(uId, uPassword);
            if (model.IsSuccess)
            {
                SessionManager.Current[FieldConfiguration.SessionKey] = model.Model;
                SessionManager.RemoveSession(FieldConfiguration.ValidateCodeKey);
            }
            return Json(model);
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(MemberInfo model)
        {
            var s = _memberInfoRepository.Register(model);
            if (s.IsSuccess)
            {
                SessionManager.Current[FieldConfiguration.SessionKey] = s.Model;
                SessionManager.RemoveSession(FieldConfiguration.ValidateCodeKey);
            }
            return Json(s);
        }
        public ActionResult Exit()
        {
            try
            {
                SessionManager.RemoveSession();
            }
            catch
            {
                return Json(false);
            }
            return Json(true);
        }

        public ActionResult ChangePwd()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ChangePwd(string uId, string oldPwd, string newPwd)
        {
            return Json(new MemberInfoRepository().ChangePwd(uId, newPwd, oldPwd));
        }
    }
}
