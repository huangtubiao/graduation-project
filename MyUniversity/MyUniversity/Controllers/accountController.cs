using MyUniversity.Models;
using MyUniversity.Models.DTO;
using MyUniversity.Models.Help;
using MyUniversity.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MyUniversity.Controllers
{
    public class accountController : baseController
    {
        private loginUser loginUser { get; set; }
        private IUserService _userService { get; set; }
        private IRecentVisitorService _recentVisitorService { get; set; }
        private IPlanService _planService { get; set; }
        private IClockLogService _clockLogService { get; set; }
        private ISchoolService _schoolService { get; set; }
        private IDepartService _departService { get; set; }
        private ICityService _cityService { get; set; }
        private IProjectRecordService _projectRecordService { get; set; }
        private IArticleService _articleService { get; set; }

        public accountController(IUserService userService, IRecentVisitorService recentVisitorService, IPlanService planService, IClockLogService clockLogService,
            ISchoolService schoolService, IDepartService departService, ICityService cityService, IProjectRecordService projectRecordService, IArticleService articleService)
        {
            loginUser = Models.Help.loginUser.getLoginUser();
            _cityService = cityService;
            _departService = departService;
            _schoolService = schoolService;
            _clockLogService = clockLogService;
            _planService = planService;
            _userService = userService;
            _recentVisitorService = recentVisitorService;
            _projectRecordService = projectRecordService;
            _articleService = articleService;
        }

        #region 返回视图页
        //个人中心
        public ActionResult person()
        {
            IQueryable<User> user = _userService.getIUserById(loginUser.userId);
            ViewData["user"] = user;

            //方案记录
            IQueryable<ProjectRecord> ProjectRecords = _projectRecordService.getRecordsByUserId(loginUser.userId);
            ViewData["projectNum"] = ProjectRecords.Count();
            if (ProjectRecords.Where(o => o.pRIfSuccess == true).Count() == 0)
            {
                ViewData["successRate"] = "0%";
            }
            else
            {
                ViewData["successRate"] = ((ProjectRecords.Where(o => o.pRIfSuccess == true).Count() / ProjectRecords.Count()) * 100) + "%";
            }
            ViewData["pRComment"] = ProjectRecords.Where(o => o.pRComment != "" || o.pRComment != null).Count();
            List<ProjectRecordsModel> ProjectRecordsModel = _projectRecordService.selectProjectRecords(ProjectRecords);
            ViewData["ProjectRecords"] = ProjectRecordsModel;

            List<RecentVisitor> recentVisitors = _recentVisitorService.getRecentVisitorsByUserId(loginUser.userId);
            ViewData["recentVisitors"] = recentVisitors;

            //我的分享
            IQueryable<Article> myShare = _articleService.getArticleByUserId(loginUser.userId);
            ViewData["myShare"] = myShare;
            return View();
        }

        //注册
        public ActionResult register()
        {
            //获取热门学校
            IQueryable<School> hotSchools = _schoolService.getHotSchools();
            ViewData["hotSchools"] = hotSchools;
            return View();
        }

        public ActionResult plan()
        {
            List<Plan> myplans = _planService.getMyplansByUserid(loginUser.userId);
            List<PlanModel> myplansModel = _planService.selectPlansData(myplans);
            //判断今天是否完成打卡日志
            foreach (var p in myplansModel)
            {
                List<Clocklog> clockLog = _clockLogService.getClockLogsByPlanId(p.planId, 1);
                if (clockLog.Count == 0)
                {
                    p.todayIfClockLog = 0;
                }
                else
                {
                    string d = clockLog[0].clocklogTime.ToString();
                    string today = DateTime.Now.ToShortDateString();
                    string tomorrow = DateTime.Now.AddDays(1).ToShortDateString();
                    if (DateTime.Parse(d) > DateTime.Parse(today) && DateTime.Parse(d) < DateTime.Parse(tomorrow))
                    {
                        p.todayIfClockLog = 1;
                    }
                    else
                    {
                        p.todayIfClockLog = 0;
                    }
                }
            }
            for (int q = 0; q < myplansModel.Count; q++)
            {
                myplansModel[q].planPublishedTime = Util.CommonMethod.getQuestionPublishedTime(myplansModel[q].planTime);
            }
            ViewData["myplansModel"] = myplansModel;

            List<RecentVisitor> recentVisitors = _recentVisitorService.getRecentVisitorsByUserId(loginUser.userId);
            ViewData["recentVisitors"] = recentVisitors;
            return View();
        }

        public ActionResult setprofile()
        {
            IQueryable<User> user = _userService.getIUserById(loginUser.userId);
            ViewData["user"] = user;

            //获取热门学校
            IQueryable<School> hotSchools = _schoolService.getHotSchools();
            ViewData["hotSchools"] = hotSchools;
            return View();
        }
        #endregion

        #region 获取附近的学校
        public ActionResult GetNearSchools()
        {
            List<string> nearSchools = null;
            //根据用户ip地址获取用户所在地，然后根据用户所在地查询附近的学校
            string ip = Util.CommonMethod.GetHostAddress();
            var strUrl = "http://int.dpool.sina.com.cn/iplookup/iplookup.php?format=json&ip=" + "59.39.128.0";

            WebClient client = new WebClient();
            string reply = client.DownloadString(strUrl);
            IpAddressModel i = Util.JsonHelper.Jso_DeJSON<IpAddressModel>(reply);
            List<City> city = _cityService.getCityByCityName(i.city);
            if (city.Count == 1)
            {
                nearSchools = _schoolService.getSchoolNamesByCityId(city[0].cityId);
            }
            return Json(nearSchools, JsonRequestBehavior.AllowGet);
        } 
        #endregion

        #region 搜索学校
        public ActionResult SchoolSearch()
        {
            var searchText = Request.QueryString["searchText"];
            if (searchText == "")
            {
                var JsonResults = "";
                return Json(JsonResults, JsonRequestBehavior.AllowGet);
            }
            else
            {
                List<string> result = _schoolService.getSchoolsByWrite(searchText);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region 搜索院系
        public ActionResult SearchDepart()
        {
            var searchText = Request.QueryString["searchText"];
            if (searchText == "")
            {
                var JsonResults = "";
                return Json(JsonResults, JsonRequestBehavior.AllowGet);
            }
            else
            {
                List<string> result = _departService.getDepartsBySearchWrite(searchText);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        } 
        #endregion

        #region 根据学校搜索院系
        public ActionResult DepartSearch()
        {
            var searchText = Request.QueryString["searchDepart"];
            var mySchool = Request.QueryString["mySchool"];
            if (searchText == "")
            {
                var JsonResults = "";
                return Json(JsonResults, JsonRequestBehavior.AllowGet);
            }
            else
            {
                School school = _schoolService.getSchoolBySchoolName(mySchool);
                List<string> result = _departService.getDepartsByWrite(searchText, school.schoolId);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region 根据学校获取相应的院系
        public ActionResult GetDepartBySchool()
        {
            var schoolName = Request.QueryString["schoolName"];
            School school = _schoolService.getSchoolBySchoolName(schoolName);
            List<string> departs = _departService.getDepartsBySchoolId(school.schoolId);
            return Json(departs, JsonRequestBehavior.AllowGet);
        } 
        #endregion

        

        #region 用户注册
        [HttpPost]
        public ActionResult register(User user)
        {
            try
            {
                //检查账号是否已经被注册
                if (_userService.getUserByAccount(user.userAccount) != null)
                {
                    results.message = "手机号码已被注册";
                    return Json(results);
                }
                else
                {
                    Random rad = new Random();//实例化随机数产生器rad；
                    int value = rad.Next(1000, 10000);
                    user.userName = DateTime.Now.DayOfWeek.ToString() + value.ToString();
                    user.userImg = "/image/pite.jpg";
                    user.userIntro = "这位学森很懒，什么也没有留下～～！";
                    user.userMerit = "普通大学森";
                    user.userMeritIntro = "我只是一个普普通通的大学森~~！";
                    if (_userService.register(user) == true)
                    {
                        User _user = _userService.getUserByAccount(user.userAccount);
                        _userService.setLoginUser(user.userAccount, _user.userId, _user.userSchool);
                        FormsAuthentication.SetAuthCookie(user.userImg, true);

                        HttpCookie cookie = new HttpCookie("userName", user.userName); //创建cookie的实例。 
                        Response.Cookies.Add(cookie);//将创建的cookie文件输入到浏览器端
                        HttpCookie cookie2 = new HttpCookie("userRank", user.userRank.ToString());
                        Response.Cookies.Add(cookie2);
                        HttpCookie cookie3 = new HttpCookie("userFans", user.userFans.ToString());
                        Response.Cookies.Add(cookie3);

                        results.success = true;
                        return Json(results);
                    }
                    else
                    {
                        results.message = "注册失败";
                        return Json(results);
                    }
                }
            }
            catch
            {
                results.message = "注册失败";
                return Json(results);
            }
        }
        #endregion

        #region 登录
        public ActionResult login(LoginModel user)
        {
            var results = new ResultsModel() { success = false, message = "" };
            try
            {
                //用户是否存在,存在返回true
                if (_userService.isNotUser(user) == false)
                {
                    results.message = "账号不存在";
                    return Json(results, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    if (_userService.login(user) == true)
                    {
                        User _user = _userService.getUserByAccount(user.userAccount);
                        _userService.setLoginUser(user.userAccount, _user.userId, _user.userSchool);
                        FormsAuthentication.SetAuthCookie(_user.userImg, true);

                        HttpCookie cookie = new HttpCookie("userName", _user.userName); //创建cookie的实例。 
                        Response.Cookies.Add(cookie);//将创建的cookie文件输入到浏览器端
                        HttpCookie cookie2 = new HttpCookie("userRank", _user.userRank.ToString());
                        Response.Cookies.Add(cookie2);
                        HttpCookie cookie3 = new HttpCookie("userFans", _user.userFans.ToString());
                        Response.Cookies.Add(cookie3);

                        //活跃度+1
                        _user.userVitality = _user.userVitality + 1;
                        _userService.updateUser(_user);

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
            return Json(results, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 退出登录
        public ActionResult loginOff()
        {
            FormsAuthentication.SignOut();
            System.Web.HttpContext.Current.Session["loginUser"] = null;
            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region 更新用户》增加经验值20
        public ActionResult UpdateUser(long userId)
        {
            User user = _userService.getUserById(userId);
            user.userRank = user.userRank + 20;
            if (_userService.updateUser(user))
            {
                JsonResult["boo_success"] = true;
                return Json(JsonResult);
            }
            else
            {
                JsonResult["boo_success"] = false;
                return Json(JsonResult);
            }
        }
        #endregion

        #region 更改真实姓名
        public ActionResult UpdateRealName(string realName)
        {
            User user = _userService.getUserById(loginUser.userId);
            user.userRealName = realName;
            if (_userService.updateUser(user))
            {
                JsonResult["boo_success"] = true;
                return Json(JsonResult);
            }
            else
            {
                JsonResult["boo_success"] = false;
                return Json(JsonResult);
            }
        }
        #endregion

        #region 更改班级
        public ActionResult UpdateUserClass(string userClass)
        {
            User user = _userService.getUserById(loginUser.userId);
            user.userClass = userClass;
            if (_userService.updateUser(user))
            {
                JsonResult["boo_success"] = true;
                return Json(JsonResult);
            }
            else
            {
                JsonResult["boo_success"] = false;
                return Json(JsonResult);
            }
        }
        #endregion

        #region 更改宿舍
        public ActionResult UpdateUserDorm(string userDorm)
        {
            User user = _userService.getUserById(loginUser.userId);
            user.userDorm = userDorm;
            if (_userService.updateUser(user))
            {
                JsonResult["boo_success"] = true;
                return Json(JsonResult);
            }
            else
            {
                JsonResult["boo_success"] = false;
                return Json(JsonResult);
            }
        }
        #endregion

        #region 更改性别
        public ActionResult UpdateUserSex(string userSex)
        {
            User user = _userService.getUserById(loginUser.userId);
            user.userSex = userSex;
            if (_userService.updateUser(user))
            {
                JsonResult["boo_success"] = true;
                return Json(JsonResult);
            }
            else
            {
                JsonResult["boo_success"] = false;
                return Json(JsonResult);
            }
        }
        #endregion

        #region 更改恋爱状态
        public ActionResult UpdateUserLove(string userLove)
        {
            User user = _userService.getUserById(loginUser.userId);
            user.userLove = userLove;
            if (_userService.updateUser(user))
            {
                JsonResult["boo_success"] = true;
                return Json(JsonResult);
            }
            else
            {
                JsonResult["boo_success"] = false;
                return Json(JsonResult);
            }
        }
        #endregion

        #region 更改方案个性签名
        public ActionResult UpdateSperior(string userIntro)
        {
            User user = _userService.getUserById(loginUser.userId);
            user.userIntro = userIntro;
            if (_userService.updateUser(user))
            {
                JsonResult["boo_success"] = true;
                return Json(JsonResult);
            }
            else
            {
                JsonResult["boo_success"] = false;
                return Json(JsonResult);
            }
        }
        #endregion

        #region 修改方案信息
        [HttpPost]
        public ActionResult saveSuperiorInfo()
        {
            User user = _userService.getUserById(loginUser.userId);
            user.userMerit = Request.Form["ques-title"];
            user.userIntro = Request.Form["superior-text"];
            if (_userService.updateUser(user))
            {
                JsonResult["boo_success"] = true;
                return Json(JsonResult);
            }
            else
            {
                JsonResult["boo_success"] = false;
                return Json(JsonResult);
            }
        } 
        #endregion

        #region 修改基本信息
        public ActionResult saveGeneralInfo()
        {
            User user = _userService.getUserById(loginUser.userId);
            user.userName = Request.Form["userName"];
            user.userSex = Request.Form["sex"];
            if (_userService.updateUser(user))
            {
                JsonResult["boo_success"] = true;
                return Json(JsonResult);
            }
            else
            {
                JsonResult["boo_success"] = false;
                return Json(JsonResult);
            }
        } 
        #endregion

        #region 修改教育信息
        public ActionResult saveEducationInfo()
        {
            User user = _userService.getUserById(loginUser.userId);
            user.userSchool = Request.Form["school"];
            user.userDepart = Request.Form["depart"];
            user.userStartYear = Request.Form["enrollyear"];
            user.userClass = Request.Form["class"];
            user.userDorm = Request.Form["dorm"];
            user.userMiddleSchool = Request.Form["middelSchool"];
            if (_userService.updateUser(user))
            {
                JsonResult["boo_success"] = true;
                return Json(JsonResult);
            }
            else
            {
                JsonResult["boo_success"] = false;
                return Json(JsonResult);
            }
        } 
        #endregion

        #region 获取用户信息
        public ActionResult getUserInfo()
        {
            if (System.Web.HttpContext.Current.Session["loginUser"] != null)
            {
                User user = _userService.getUserById(loginUser.userId);
                SuperiorModel SuperiorModel = _userService.selectUserData(user);
                var JsonResult = Util.JsonHelper.Jso_ToJSON(SuperiorModel);
                return Json(JsonResult);
            }
            else
            {
                return Json(null);
            }
        } 
        #endregion
    }
}
