using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Util
{
    public static class CommonMethod
    {
        #region 计算问题发布了多少时间
        public static string getQuestionPublishedTime(DateTime questionTime)
        {
            int publishedTimes;
            string questionPublishedTime = null;
            double totalSecond = (DateTime.Now - questionTime).TotalSeconds;
            int _totalSecond = Convert.ToInt32(totalSecond);
            if (_totalSecond >= 86400)
            {
                publishedTimes = Convert.ToInt32(_totalSecond / 86400);
                questionPublishedTime = publishedTimes + "天前";
            }
            if (_totalSecond >= 3600 && _totalSecond < 86400)
            {
                publishedTimes = Convert.ToInt32(_totalSecond / 3600);
                questionPublishedTime = publishedTimes + "小时前";
            }
            if (_totalSecond >= 60 && _totalSecond < 3600)
            {
                publishedTimes = Convert.ToInt32(_totalSecond / 60);
                questionPublishedTime = publishedTimes + "分钟前";
            }
            else if (_totalSecond >= 0 && _totalSecond < 60)
            {
                questionPublishedTime = _totalSecond + "秒前";
            }
            return questionPublishedTime;
        }
        #endregion

        /// <summary>
        /// 获取客户端IP地址（无视代理）
        /// </summary>
        /// <returns>若失败则返回回送地址</returns>
        public static string GetHostAddress()
        {
            string userHostAddress = HttpContext.Current.Request.UserHostAddress;

            if (string.IsNullOrEmpty(userHostAddress))
            {
                userHostAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }

            //最后判断获取是否成功，并检查IP地址的格式（检查其格式非常重要）
            if (!string.IsNullOrEmpty(userHostAddress) && IsIP(userHostAddress))
            {
                return userHostAddress;
            }
            return "127.0.0.1";
        }

        /// <summary>
        /// 检查IP地址格式
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsIP(string ip)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }

        /// <summary>
        /// UNICODE字符转为中文  
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string unicodeToChinese(string str)
        {
            string outStr = "";
            if (!string.IsNullOrEmpty(str))
            {
                string[] strlist = str.Replace("//", "").Split('u');
                try
                {
                    for (int i = 1; i < strlist.Length; i++)
                    {
                        //将unicode字符转为10进制整数，然后转为char中文字符  
                        outStr += (char)int.Parse(strlist[i], System.Globalization.NumberStyles.HexNumber);
                    }
                }
                catch (FormatException ex)
                {
                    outStr = ex.Message;
                }
            }
            return outStr;
        }

        public static string a(string str)
        {
            byte[] bytes = new byte[2];

            bytes[1] = byte.Parse(int.Parse(str.Substring(0, 2), NumberStyles.HexNumber).ToString());

            bytes[0] = byte.Parse(int.Parse(str.Substring(2), NumberStyles.HexNumber).ToString());

            return Encoding.Unicode.GetString(bytes);
        }
    }
}
