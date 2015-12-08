using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Jianghu.Framwork.Repository.Repository
{
    public class FieldConfiguration
    {
        public static string Account
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["account"].ConnectionString;
            }
        }
        public static string Game
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["game"].ConnectionString;
            }
        }

        public static string SessionKey
        {
            get
            {
                return "JIANGHU_SYSTEM_SESSION";
            }
        }
        public static string ChouJiangKey
        {
            get
            {
                return "JIANGHU_SYSTEM_CHOUJIANG";
            }
        }
        public static string ChouJiangFile
        {
            get
            {
                return AppDomain.CurrentDomain.BaseDirectory + "files/";
            }
        }
        public static string ValidateCodeKey
        {
            get
            {
                return "ValidateCode";
            }
        }
    }
}
