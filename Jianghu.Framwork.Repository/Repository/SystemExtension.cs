using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Jianghu.Framwork.Repository.Repository
{
    public static class SystemExtension
    {
        public static string CreateMd5(this string val)
        {
            var md5 = MD5.Create();
            var bytes = Encoding.UTF8.GetBytes(val);
            var buffer = md5.ComputeHash(bytes);
            var sb = new StringBuilder();
            buffer.ForEach(s => sb.Append(s.ToString("X2")));
            md5.Dispose();
            return sb.ToString();
        }

        public static void ForEach<T>(this IEnumerable<T> val, Action<T> action)
        {
            foreach (var key in val)
            {
                if (action != null)
                {
                    action(key);
                }
            }
        }
    }
}
