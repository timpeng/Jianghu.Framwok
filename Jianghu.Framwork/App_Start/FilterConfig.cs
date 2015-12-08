using System.Web;
using System.Web.Mvc;
using Jianghu.Framwork.Core;

namespace Jianghu.Framwork
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new ExceptionFilterAttribute());
        }
    }
}