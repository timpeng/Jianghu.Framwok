using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using Jianghu.Framwork.Repository.Model;

namespace Jianghu.Framwork.Core
{
    public class BaseController : Controller
    {
        public bool IsRedirect { get; set; }
        public MemberInfo SessionInfo { get; private set; }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (SessionManager.SessionInfo != null)
            {
                this.SessionInfo = SessionManager.SessionInfo;
            }
            else
            {
                //filterContext.Result =
                //new RedirectToRouteResult(
                //    new RouteValueDictionary(
                //    new { action = "Login", controller="Account" }));
            }
            base.OnActionExecuting(filterContext);
        }
    }
}
