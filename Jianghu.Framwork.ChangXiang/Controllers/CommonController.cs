using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Web.Mvc;
using Jianghu.Framwork.Core;
using Jianghu.Framwork.Repository.Repository;
using Newtonsoft.Json;

namespace Jianghu.Framwork.ChangXiang.Controllers
{
    public class CommonController : Controller
    {
        public ActionResult ValidateCode()
        {
            string code = CreateValidateCode(5);
            Session[FieldConfiguration.ValidateCodeKey] = code;
            byte[] bytes = CreateValidateGraphic(code);
            return File(bytes, @"image/jpeg");
        }
        /// <summary>
        /// 创建验证码的图片
        /// </summary>
        public byte[] CreateValidateGraphic(string validateCode)
        {
            Bitmap image = new Bitmap((int)Math.Ceiling(validateCode.Length * 12.0), 22);
            Graphics g = Graphics.FromImage(image);
            try
            {
                //生成随机生成器
                Random random = new Random();
                //清空图片背景色
                g.Clear(Color.White);
                //画图片的干扰线
                for (int i = 0; i < 25; i++)
                {
                    int x1 = random.Next(image.Width);
                    int x2 = random.Next(image.Width);
                    int y1 = random.Next(image.Height);
                    int y2 = random.Next(image.Height);
                    g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
                }
                Font font = new Font("Arial", 12, (FontStyle.Bold | FontStyle.Italic));
                LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height),
                 Color.Blue, Color.DarkRed, 1.2f, true);
                g.DrawString(validateCode, font, brush, 3, 2);
                //画图片的前景干扰点
                for (int i = 0; i < 100; i++)
                {
                    int x = random.Next(image.Width);
                    int y = random.Next(image.Height);
                    image.SetPixel(x, y, Color.FromArgb(random.Next()));
                }
                //画图片的边框线
                g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);
                //保存图片数据
                MemoryStream stream = new MemoryStream();
                image.Save(stream, ImageFormat.Jpeg);
                //输出图片流
                return stream.ToArray();
            }
            finally
            {
                g.Dispose();
                image.Dispose();
            }
        }
        ///<summary>
        /// 生成验证码
        /// </summary>
        /// <param name="length">指定验证码的长度</param>
        /// <returns></returns>
        public string CreateValidateCode(int length)
        {
            int[] randMembers = new int[length];
            int[] validateNums = new int[length];
            string validateNumberStr = "";
            //生成起始序列值
            int seekSeek = unchecked((int)DateTime.Now.Ticks);
            Random seekRand = new Random(seekSeek);
            int beginSeek = (int)seekRand.Next(0, Int32.MaxValue - length * 10000);
            int[] seeks = new int[length];
            for (int i = 0; i < length; i++)
            {
                beginSeek += 10000;
                seeks[i] = beginSeek;
            }
            //生成随机数字
            for (int i = 0; i < length; i++)
            {
                Random rand = new Random(seeks[i]);
                int pownum = 1 * (int)Math.Pow(10, length);
                randMembers[i] = rand.Next(pownum, Int32.MaxValue);
            }
            //抽取随机数字
            for (int i = 0; i < length; i++)
            {
                string numStr = randMembers[i].ToString();
                int numLength = numStr.Length;
                Random rand = new Random();
                int numPosition = rand.Next(0, numLength - 1);
                validateNums[i] = Int32.Parse(numStr.Substring(numPosition, 1));
            }
            //生成验证码
            for (int i = 0; i < length; i++)
            {
                validateNumberStr += validateNums[i].ToString();
            }
            return validateNumberStr;
        }

        public ActionResult CreateCache(string admin)
        {
            if (admin == "tompeng")
            {
                CreateData();
                return Content("ok");
            }
            return Content("你无权限进行此操作!");
        }
        private void CreateData()
        {
            var s = new List<Dictionary<string, string>>();
            GetData(s, 5, "99203");
            GetData(s, 10, "99218");
            GetData(s, 30, "99217");
            GetData(s, 10, "99197");
            GetData(s, 50, "99198");
            GetData(s, 100, "99267", "119931120");
            GetData(s, 30, "99266", "119931120");
            GetData(s, 20, "99229", "289931120");
            GetData(s, 300, "99226", "289931120");
            GetData(s, 300, "99227", "289931120");
            GetData(s, 300, "99228", "289931120");
            GetData(s, 1000, "99148");
            GetData(s, 1000, "00723");
            GetData(s, 1000, "99194");
            GetData(s, 6000, "00136");
            GetData(s, 6000, "00137");
            GetData(s, 30, "02953", "000003112");
            GetData(s, 30, "02503", "000003112");
            GetData(s, 30, "03403", "000003112");
            GetData(s, 30, "03853", "000003112");
            GetData(s, 30, "04303", "000003112");
            GetData(s, 30, "04753", "000003112");
            GetData(s, 30, "22503", "000003112");
            GetData(s, 30, "22953", "000003112");
            GetData(s, 30, "23403", "000003112");
            GetData(s, 30, "23853", "000003112");
            GetData(s, 30, "24303", "000003112");
            GetData(s, 30, "24753", "000003112");
            GetData(s, 30, "42503", "000003112");
            GetData(s, 30, "42953", "000003112");
            GetData(s, 30, "43403", "000003112");
            GetData(s, 30, "43853", "000003112");
            GetData(s, 30, "44303", "000003112");
            GetData(s, 30, "44753", "000003112");
            string file = FieldConfiguration.ChouJiangFile;
            if (!Directory.Exists(file))
            {
                Directory.CreateDirectory(file);
            }
            Arrrandom(ref s);
            System.IO.File.WriteAllText(file + "s.json", JsonConvert.SerializeObject(s));
        }
        private void Arrrandom(ref List<Dictionary<string, string>> arr)
        {
            Random ran = new Random();
            int k = 0;
            var strtmp = new Dictionary<string, string>();
            for (int i = 0; i < arr.Count; i++)
            {
                k = ran.Next(0, 20);
                if (k != i)
                {
                    strtmp = arr[i];
                    arr[i] = arr[k];
                    arr[k] = strtmp;
                }
            }
        }
        private void GetData(List<Dictionary<string, string>> list, int length, string val, string isValue = "000000000")
        {
            for (int i = 0; i < length; i++)
            {
                list.Add(new Dictionary<string, string>
                {
                    {val,isValue}
                });
            }
        }
    }
}
