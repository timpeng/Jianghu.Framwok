using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Jianghu.Framwork.Core;
using Jianghu.Framwork.Repository.Model;
using Jianghu.Framwork.Repository.Repository;

namespace Jianghu.Framwork.Controllers
{
    public class AccountController : Controller
    {
        private readonly MemberInfoRepository _memberInfoRepository = new MemberInfoRepository();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ChangePwd()
        {
            return Json("");
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string uId, string uPassword, string yzm)
        {
            var model = new Messager<MemberInfo>();
            if (Session["ValidateCode"] != null)
            {
                if (Session[FieldConfiguration.ValidateCodeKey].ToString() != yzm)
                {
                    model.Message = "验证码错误!";
                }
                else
                {
                    model = _memberInfoRepository.Login(uId, uPassword);
                    if (model.IsSuccess)
                    {
                        SessionManager.Current[FieldConfiguration.SessionKey] = model.Model;
                        SessionManager.RemoveSession(FieldConfiguration.ValidateCodeKey);
                    }
                }
            }
            else
            {
                model.Message = "未读取到验证码,请稍后重试!";
            }
            return Json(model);
        }
        [HttpPost]
        public ActionResult Register(MemberInfo model, string yzm)
        {
            var s = new Messager<MemberInfo>();
            if (Session["ValidateCode"] != null)
            {
                if (Session[FieldConfiguration.ValidateCodeKey].ToString() != yzm)
                {
                    s.Message = "验证码错误!";
                }
                else
                {
                    s = _memberInfoRepository.Register(model);
                    if (s.IsSuccess)
                    {
                        SessionManager.Current[FieldConfiguration.SessionKey] = s.Model;
                        SessionManager.RemoveSession(FieldConfiguration.ValidateCodeKey);
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            else
            {
                s.Message = "未读取到验证码,请稍后重试!";
            };
            ViewData["s"] = s;
            return View();
        }
        public ActionResult Register()
        {
            return View();
        }
    }
}
