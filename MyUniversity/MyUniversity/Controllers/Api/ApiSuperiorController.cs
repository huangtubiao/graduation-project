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
    public class ApiSuperiorController : ApiController
    {
        private loginUser loginUser { get; set; }
        private IUserService _userService { get; set; }
        private IArticleService _articleService { get; set; }

        public ApiSuperiorController(IUserService userService, IArticleService articleService)
        {
            _articleService = articleService;
            _userService = userService;
            loginUser = Models.Help.loginUser.getLoginUser();
        }

        //Get api/ApiSuperior
        //获取全部院系的方案高手
        [HttpGet]
        public HttpResponseMessage GetAllSuperior()
        {
            List<User> a = _userService.getMySchoolSuperiors(1, "全部院系", loginUser.userSchool);
            List<SuperiorModel> s = _userService.selectSuperiorData(a);
            var result = Util.JsonHelper.Jso_ToJSON(s);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        //获取方案高手的相关信息(去看看方案高手)
        [HttpGet]
        public HttpResponseMessage GetSuperiorInfoById(long id)
        {
            IQueryable<Article> myShare = _articleService.getArticleByUserId(id);
            var result = Util.JsonHelper.Jso_ToJSON(myShare);
            
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
