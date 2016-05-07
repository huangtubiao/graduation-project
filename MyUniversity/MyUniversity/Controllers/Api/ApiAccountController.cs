using MyUniversity.Models;
using MyUniversity.Models.DTO;
using MyUniversity.Models.Help;
using MyUniversity.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MyUniversity.Controllers.Api
{
    public class ApiAccountController : ApiBaseController
    {
        private IUserService _userService { get; set; }

        public ApiAccountController(IUserService userService)
        {
            _userService = userService;
        }

        #region 登录

        public HttpResponseMessage Post([FromBody] LoginModel user)
        {
            try
            {
                if (_userService.isNotUser(user) == false)
                {
                    results.message = "未注册";
                    return Request.CreateResponse(HttpStatusCode.OK, results);
                }
                else
                {
                    if (_userService.login(user) == true)
                    {
                        results.success = true;
                    }
                    else
                    {
                        results.message = "密码错误";
                    }
                }
            }
            catch
            {
                results.message = "登录失败";
            }
            return Request.CreateResponse(HttpStatusCode.OK, results);
        } 
        #endregion

        #region 注册
        public HttpResponseMessage Put([FromBody] User user)
        {
            try
            {
                if (_userService.register(user) == true)
                {
                    results.success = true;
                }
            }
            catch
            {
                results.message = "注册失败";
            }
            return Request.CreateResponse(HttpStatusCode.OK, results);
        }
        #endregion
    }
}
