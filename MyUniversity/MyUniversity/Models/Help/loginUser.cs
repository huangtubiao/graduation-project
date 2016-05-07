using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyUniversity.Models.Help
{
    [Serializable]
    /// <summary>
    /// 登陆用户信息
    /// </summary>
    public class loginUser
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public long userId { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string userAccount { get; set; }

        /// <summary>
        /// 学校
        /// </summary>
        public string userSchool { get; set; }

        /// <summary>
        /// 获取登录用户信息
        /// </summary>
        /// <returns></returns>
        public static loginUser getLoginUser()
        {
            return (loginUser)HttpContext.Current.Session["loginUser"] ?? null;
        }

        /// <summary>
        /// 判断是否已经登录（存在Cookie）
        /// </summary>
        /// <returns></returns>
        public static bool IsAuthenticated()
        {
            return HttpContext.Current.Request.IsAuthenticated;
        }
    }
}
