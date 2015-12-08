using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Jianghu.Framwork.Core
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class ExceptionFilterAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);
            string message = filterContext.Exception.Message;
            string source = filterContext.Exception.Source;
            string stacktrace = filterContext.Exception.StackTrace;
            string path = AppDomain.CurrentDomain.BaseDirectory + @"logs\exception\";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            File.AppendAllText(path+DateTime.Now.ToString("yyyyMMdd") + ".txt", string.Format(@"{3}:source:{0};\r\message:{1};\r\nstacktrace:{2};\r\n\r\n", source, message, stacktrace, DateTime.Now));
        }
    }
}