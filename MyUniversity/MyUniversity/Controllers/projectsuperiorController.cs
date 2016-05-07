using MyUniversity.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyUniversity.Models;
using MyUniversity.Models.DTO;
using MyUniversity.Models.Help;
using System.Web.Security;

namespace MyUniversity.Controllers
{
    public class projectsuperiorController : baseController
    {
        UniversityEntities db = new UniversityEntities();

        private loginUser loginUser { get; set; }
        private IUserService _userService { get; set; }
        private IHotSuperiorService _hotSuperiorService { get; set; }
        private IRecentVisitorService _recentVisitorService { get; set; }
        private ISuperiorLetterService _superiorLetterService { get; set; }
        private IFollowsService _followsService { get; set; }
        private IArticleService _articleService { get; set; }
        private IDepartService _departService { get; set; }
        private ISchoolService _schoolService { get; set; }
        private IProjectRecordService _projectRecordService { get; set; }

        public projectsuperiorController(IUserService userService, IHotSuperiorService hotSuperiorService, IRecentVisitorService recentVisitorService,
            ISuperiorLetterService superiorLetterService, IFollowsService followsService, IArticleService articleService, IDepartService departService,
            ISchoolService schoolService, IProjectRecordService projectRecordService)
        {
            _schoolService = schoolService;
            _departService = departService;
            _userService = userService;
            _hotSuperiorService = hotSuperiorService;
            _recentVisitorService = recentVisitorService;
            _superiorLetterService = superiorLetterService;
            _followsService = followsService;
            _articleService = articleService;
            _projectRecordService = projectRecordService;
            loginUser = Models.Help.loginUser.getLoginUser();
        }

        #region 返回视图页
        public ActionResult index()
        {
            //如果session过期就把用户cookies信息清空
            if (System.Web.HttpContext.Current.Session["loginUser"] == null)
            {
                FormsAuthentication.SignOut();
            }
            //ViewBag.UserName = loginUser.userId;
            //ViewData["info"] = Session["info"]; //私信通知条数

            //获取全部高校的方案高手
            IQueryable<User> allSuperiors = _userService.getAllSuperiors(1);
            ViewData["allSuperiors"] = allSuperiors;

            int totalGroups;
            totalGroups = _userService.getTotalGroups(null); //查询每个选项有多少页，每一页5条数据；
            ViewData["allGroups"] = totalGroups;
            

            List<hotSuperior> hotSuperior = _hotSuperiorService.getHotSuperior();  //热门搜索
            ViewData["hotSuperior"] = hotSuperior;

            //获取私信通知条数
            if (System.Web.HttpContext.Current.Session["loginUser"] != null)
            {
                List<SuperiorLetter> superiorLetters = _superiorLetterService.getSuperiorLetterByUserIdUnread(loginUser.userId);
                ViewData["letterCount"] = superiorLetters.Count;
                //校内的方案高手的总数量
                ViewData["mySchoolGroups"] = _userService.getTotalGroupsBySchool(loginUser.userSchool);
            }

            //一周方案高手榜
            List<User> user = _userService.getWeekExpUser();
            List<SuperiorModel> superiorModel = _userService.selectWeekExpUser(user);
            List<SuperiorModel> s = superiorModel.OrderByDescending(o => o.userWeekExps).Skip(1).Take(6).ToList();
            ViewData["WeekExpSperior"] = s;
            return View();
        }

        //搜索的结果
        public ActionResult list()
        {
            string search = Request.QueryString["search"];
            ViewData["search"] = search;

            if (System.Web.HttpContext.Current.Session["loginUser"] != null)
            {
                //校内的方案高手
                List<User> superior = _userService.getMySchoolSuperiorsByWrite(search);
                ViewData["superior"] = superior.AsQueryable();
                ViewData["mySuperiorGroups"] = _userService.getMySuperiorGroupsByWrite(search);
            }
            else
            {
                //搜索校内方案高数显示为空
                IQueryable<User> superior = _userService.getAllSuperiorsByWrite(null);
                ViewData["superior"] = superior;
                ViewData["mySuperiorGroups"] = 0;
            }

            //全部
            IQueryable<User> allSuperiors = _userService.getAllSuperiorsByWrite(search);
            ViewData["allSuperiors"] = allSuperiors;
            ViewData["totalGroups"] = _userService.getTotalGroupsByWrite(search);

            List<hotSuperior> hotSuperior = _hotSuperiorService.getHotSuperior();  //热门搜索
            ViewData["hotSuperior"] = hotSuperior;

            //一周方案高手榜
            List<User> user = _userService.getWeekExpUser();
            List<SuperiorModel> superiorModel = _userService.selectWeekExpUser(user);
            List<SuperiorModel> s = superiorModel.OrderByDescending(o => o.userWeekExps).Skip(1).Take(6).ToList();
            ViewData["WeekExpSperior"] = s;
            return View();
        }

        //访问TA
        public ActionResult space(long id)
        {
            IQueryable<User> user = _userService.getIUserById(id);
            ViewData["user"] = user;
            ViewData["title"] = user.ToList()[0].userMerit;

            //我的分享
            IQueryable<Article> myShare = _articleService.getArticleByUserId(id);
            ViewData["myShare"] = myShare;

            //方案记录
            IQueryable<ProjectRecord> ProjectRecords = _projectRecordService.getRecordsByUserId(id);
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

            //最近访客
            List<RecentVisitor> recentVisitors = _recentVisitorService.getRecentVisitorsByUserId(id);
            ViewData["recentVisitors"] = recentVisitors;
            if (System.Web.HttpContext.Current.Session["loginUser"] != null)
            {
                if (recentVisitors.Count() != 0)
                {
                    if (recentVisitors[0].visitorId != loginUser.userId) //如果最新访问的不是自己，则添加自己
                    {
                        //添加最近访问的人
                        RecentVisitor newVisitor = new RecentVisitor();
                        newVisitor.userId = id;
                        newVisitor.visitorId = loginUser.userId;
                        newVisitor.visitTime = DateTime.Now;
                        _recentVisitorService.addRecentVisitor(newVisitor);
                    }
                }
                else
                {
                    //添加最近访问的人
                    RecentVisitor newVisitor = new RecentVisitor();
                    newVisitor.userId = id;
                    newVisitor.visitorId = loginUser.userId;
                    newVisitor.visitTime = DateTime.Now;
                    _recentVisitorService.addRecentVisitor(newVisitor);
                }
                Follows f = _followsService.getFollowByUseridFuserid(loginUser.userId, id);
                if (f != null)
                {
                    ViewData["focus"] = "已关注";
                }
                else
                {
                    ViewData["focus"] = "关注Ta";
                }
            }
            else
            {
                ViewData["focus"] = "关注Ta";
            }
            return View();
        }

        //我的关注
        public ActionResult follow()
        {
            //获取全部的关注
            IQueryable<Follows> follows = _followsService.getMyFollows(loginUser.userId);
            ViewData["follows"] = follows;

            //获取校内的关注
            IQueryable<Follows> mySchoolFollows = _followsService.getMySchoolFollows(loginUser.userId, loginUser.userSchool);
            ViewData["mySchoolFollows"] = mySchoolFollows;

            List<hotSuperior> hotSuperior = _hotSuperiorService.getHotSuperior();  //热门搜索
            ViewData["hotSuperior"] = hotSuperior;

            //一周方案高手榜
            List<User> user = _userService.getWeekExpUser();
            List<SuperiorModel> superiorModel = _userService.selectWeekExpUser(user);
            List<SuperiorModel> s = superiorModel.OrderByDescending(o => o.userWeekExps).Skip(1).Take(6).ToList();
            ViewData["WeekExpSperior"] = s;
            return View();
        }

        public ActionResult letter()
        {
            ViewData["info"] = Session["info"];

            List<hotSuperior> hotSuperior = _hotSuperiorService.getHotSuperior();  //热门搜索
            ViewData["hotSuperior"] = hotSuperior;

            //全部私信的方案高手
            List<SuperiorLetter> superiorLetter = _superiorLetterService.getSuperiorLettersByUserId(loginUser.userId);
            ViewData["superiorLetter"] = superiorLetter;

            //校内私信的方案高手
            IQueryable<SuperiorLetter> mySchoolsuperiorLetter = _superiorLetterService.getMSSuperiorLettersByUserId(loginUser.userId, loginUser.userSchool);
            ViewData["mySchoolsuperiorLetter"] = mySchoolsuperiorLetter;

            //方案高手私信改为已读
            List<SuperiorLetter> superiorLetters = _superiorLetterService.getSuperiorLetterByUserIdUnread(loginUser.userId);
            _superiorLetterService.updateSuperiorLetters(superiorLetters);

            //一周方案高手榜
            List<User> user = _userService.getWeekExpUser();
            List<SuperiorModel> superiorModel = _userService.selectWeekExpUser(user);
            List<SuperiorModel> s = superiorModel.OrderByDescending(o => o.userWeekExps).Skip(1).Take(6).ToList();
            ViewData["WeekExpSperior"] = s;

            return View();
        }

        public ActionResult become()
        {
            User user = _userService.getUserById(loginUser.userId);
            ViewData["userMerit"] = user.userMerit;
            ViewData["userIntro"] = user.userIntro;
            return View();
        }

        public ActionResult success()
        {
            return View();
        }

        public ActionResult share(long id)
        {
            IQueryable<Article> article = _articleService.getArticleById(id);
            ViewData["article"] = article;
            ViewData["articleTitle"] = article.ToList()[0].articleTitle;
            return View();
        }

        public ActionResult draft()
        {
            return View();
        }

        public ActionResult top()
        {
            //方案提供总排行
            IQueryable<User> rankUser = _userService.getRanklistByProjectNum();
            List<RankUserModel> RankUserModel = _userService.selectRankUserData(rankUser);
            ViewData["rankUser"] = RankUserModel;

            //活跃度排行榜
            IQueryable<User> rankVitalityUser = _userService.getRanklistByVitality();
            List<RankUserModel> rankVitalityModel = _userService.selectRankUserData(rankVitalityUser);
            ViewData["rankVitality"] = rankVitalityModel;

            //我的方案数以及活跃度
            IQueryable<User> myRank = _userService.getIUserById(loginUser.userId);
            ViewData["myRank"] = myRank;

            //一周方案高手榜
            List<User> user = _userService.getWeekExpUser();
            List<SuperiorModel> superiorModel = _userService.selectWeekExpUser(user);
            List<SuperiorModel> s = superiorModel.OrderByDescending(o => o.userWeekExps).Skip(1).Take(6).ToList();
            ViewData["WeekExpSperior"] = s;
            return View();
        }
        #endregion

        #region 查找院系或查找弄个大学
        public ActionResult selectDepartment(string userDepart, string nowActive, string searchWrite, string userSchool)
        {
            string[] JsonResults = new string[2];
            var JsonResult = "";
            List<User> Superiors = null;
            if (nowActive == "校内")
            {
                if (searchWrite == null || searchWrite == "")
                {
                    Superiors = _userService.getMySchoolSuperiorsByDepart(userDepart);
                    JsonResults[1] = Convert.ToString(_userService.getMSSuperiorsGroupsByDepart(userDepart));
                }
                else
                {
                    Superiors = _userService.getMySchoolSuperiorsByDepartWrite(userDepart, searchWrite);
                    JsonResults[1] = Convert.ToString(_userService.getMSSuperiorsGroupsByDepartWrite(userDepart, searchWrite));
                }
            }
            else if (nowActive == "全部")
            {
                if (searchWrite == null || searchWrite == "")
                {
                    if (userSchool == "全部大学")
                    {
                        Superiors = _userService.getAllSuperiorsByDepart(userDepart);
                        JsonResults[1] = Convert.ToString(_userService.getTotalGroupsByDepart(userDepart));
                    }
                    else
                    {
                        if (userDepart == "全部院系")
                        {
                            Superiors = _userService.getAllSuperiorsBySchool(userSchool);
                            JsonResults[1] = Convert.ToString(_userService.getTotalGroupsBySchool(userSchool));
                        }
                        else
                        {
                            Superiors = _userService.getAllSuperiorsBySchoolDepart(userSchool, userDepart);
                            JsonResults[1] = Convert.ToString(_userService.getTotalGroupsBySchoolDepart(userSchool, userDepart));
                        }
                    }
                }
                else
                {
                    if (userSchool == "全部大学")
                    {
                        if (userDepart == "全部院系")
                        {
                            Superiors = _userService.getAllSuperiorsByWrite(searchWrite).ToList();
                            JsonResults[1] = Convert.ToString(_userService.getTotalGroupsByWrite(searchWrite));
                        }
                        else
                        {
                            Superiors = _userService.getAllSuperiorsByDepartWrite(userDepart, searchWrite);
                            JsonResults[1] = Convert.ToString(_userService.getTotalGroupsByDepartWrite(userDepart, searchWrite));
                        }
                    }
                    else
                    {
                        if (userDepart == "全部院系")
                        {
                            Superiors = _userService.getAllSuperiorsByWriteSchool(searchWrite, userSchool);
                            JsonResults[1] = Convert.ToString(_userService.getTotalGroupsBySchoolWrite(searchWrite, userSchool));
                        }
                        else
                        {
                            Superiors = _userService.getAllSuperiorsByWriteSchoolDepart(searchWrite, userSchool, userDepart);
                            JsonResults[1] = Convert.ToString(_userService.getTotalGroupsByWriteSchoolDepart(searchWrite,userSchool, userDepart));
                        }
                    }
                }
            }
            var result = _userService.selectSuperiorData(Superiors);
            JsonResult = Util.JsonHelper.Jso_ToJSON(result);
            JsonResults[0] = JsonResult;
            return Json(JsonResults);
        }
        #endregion

        #region 搜索智能提醒
        public ActionResult searchSuggest()
        {
            var searchText = Request.QueryString["searchText"];
            if (searchText == "")
            {
                var JsonResults = "";
                return Json(JsonResults, JsonRequestBehavior.AllowGet);
            }
            else
            {
                List<string> result = _userService.getSuperiorByWrite(searchText);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region 搜索获取方案高手列表
        [HttpPost]
        public ActionResult list(searchFormModel searchForm)
        {
            var JsonResult = "";
            List<SuperiorModel> result;
            string[] JsonResults = new string[6];
            List<User> user = _userService.getMySchoolSuperiorsByWrite(searchForm.searchWrite);
            result = _userService.selectSuperiorData(user);
            JsonResult = Util.JsonHelper.Jso_ToJSON(result);
            JsonResults[0] = JsonResult; //获得全部方案高手
            JsonResults[3] = Convert.ToString(_userService.getTotalGroupsByWrite(searchForm.searchWrite));
            return Json(JsonResults);
        }
        #endregion

        #region 自动加载更多数据
        public ActionResult getData(getDataModel getDataModel)
        {
            List<User> user = null;
            if (getDataModel.whatOnactive == "校内")
            {
                if (getDataModel.searchWrite == null)
                {
                    user = _userService.getMySchoolSuperiors(getDataModel.groupNumber, getDataModel.userDepartment, loginUser.userSchool);
                }
                else
                {
                    user = _userService.getMySchoolSuperiorsByGetData(getDataModel);
                }
            }
            else if (getDataModel.whatOnactive == "全部")
            {
                if (getDataModel.searchWrite == null)
                {
                    if (getDataModel.userSchool == "全部大学")
                    {
                        if (getDataModel.userDepartment == "全部院系")
                        {
                            user = _userService.getAllSuperiors(getDataModel.groupNumber).ToList();
                        }
                        else
                        {
                            user = _userService.getAllSuperiorsByDepart(getDataModel.userDepartment);
                        }
                    }
                    else
                    {
                        if (getDataModel.userDepartment == "全部院系")
                        {
                            user = _userService.getAllSuperiorsBySchool(getDataModel.userSchool);
                        }
                        else
                        {
                            user = _userService.getAllSuperiorsBySchoolDepart(getDataModel.userSchool, getDataModel.userDepartment);
                        }
                    }
                }
                else
                {
                    if (getDataModel.userSchool == "全部大学")
                    {
                        if (getDataModel.userDepartment == "全部院系")
                        {
                            user = _userService.getAllSuperiorsByGetData(getDataModel); //byWrite
                        }
                        else
                        {
                            user = _userService.getAllSuperiorsByWriteDepart(getDataModel.searchWrite, getDataModel.userDepartment);
                        }
                    }
                    else
                    {
                        if (getDataModel.userDepartment == "全部院系")
                        {
                            user = _userService.getAllSuperiorsByWriteSchool(getDataModel.searchWrite, getDataModel.userSchool);
                        }
                        else
                        {
                            user = _userService.getAllSuperiorsByWriteSchoolDepart(getDataModel.searchWrite, getDataModel.userSchool, getDataModel.userDepartment);
                        }
                    }
                }
            }
            List<SuperiorModel> result = _userService.selectSuperiorData(user);
            var JsonResult = Util.JsonHelper.Jso_ToJSON(result);
            return Json(JsonResult);
        }
        #endregion

        #region 查看方案高手私信记录
        public ActionResult SelectSuperiorLetter(long userId)
        {
            SuperiorLetter superiorLetter = _superiorLetterService.getSuperiorLettersBySuidRuid(userId, loginUser.userId);
            if (superiorLetter != null)
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

        public ActionResult CheckSperiorLetter(long userId)
        {
            SuperiorLetter superiorLetter = _superiorLetterService.getSuperiorLettersBySuidRuidC(loginUser.userId, userId);
            if (superiorLetter != null)
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

        #region 添加新的方案高手私信
        public ActionResult AddSuperiorLetter(long userId)
        {
            SuperiorLetter superiorLetter = new SuperiorLetter();
            superiorLetter.superiorLetterReceiveUId = userId;
            superiorLetter.superiorLetterSendUId = loginUser.userId;
            if (_superiorLetterService.addSuperiorLetter(superiorLetter))
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

        #region 更新方案高手私信
        public ActionResult UpdateSperiorLetter(long userId, string message)
        {
            SuperiorLetter superiorLetter = _superiorLetterService.getSuperiorLettersBySuidRuid(loginUser.userId, userId);
            superiorLetter.superiorLetterIfReply = true;
            superiorLetter.superiorLetterNewestRly = message;
            if (_superiorLetterService.updateSperiorLetter(superiorLetter))
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

        #region 更新》最佳方案或方案不合
        public ActionResult bestSuperior(long superiorLetterId, long userId, int superiorLetterIfComment)
        {
            SuperiorLetter superiorLetter = _superiorLetterService.getSuperiorLettersById(superiorLetterId);
            superiorLetter.superiorLetterIfComment = superiorLetterIfComment;
            if (_superiorLetterService.updateSperiorLetter(superiorLetter))
            {
                //更新或添加一周方案高手榜的相关数据
                User user = _userService.getUserById(userId);
                if (user.userWeekExpsStartTime == null || user.userWeekExps == null)
                {
                    user.userWeekExps = 1;
                    user.userWeekExpsStartTime = DateTime.Now;
                    _userService.updateUser(user);
                }
                else
                {
                    DateTime d = Convert.ToDateTime(user.userWeekExpsStartTime);
                    if ((DateTime.Now - d).TotalDays > 7)
                    {
                        user.userWeekExps = 1;
                        user.userWeekExpsStartTime = DateTime.Now;
                        _userService.updateUser(user);
                    }
                    else
                    {
                        user.userWeekExps = user.userWeekExps + 1;
                        _userService.updateUser(user);
                    }
                }
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

        #region 关注TA
        public ActionResult AddFollow(long userId)
        {
            Follows f = new Follows();
            f.followeType = 1;
            f.followTime = DateTime.Now;
            f.followUserId = userId;
            f.userId = loginUser.userId;
            if (_followsService.addNewFollow(f))
            {
                User user = _userService.getUserById(userId);
                user.userFans = user.userFans + 1;
                //活跃度+1
                user.userVitality = user.userVitality + 1;
                _userService.updateUser(user);
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

        #region 点赞方案记录
        public ActionResult pRLove(long pRId)
        {
            ProjectRecord projectRecord = _projectRecordService.getPRecordById(pRId);
            projectRecord.userLoveNum = projectRecord.userLoveNum + 1;
            if (_projectRecordService.updateProjectRecord(projectRecord))
            {
                //活跃度+1
                if (System.Web.HttpContext.Current.Session["loginUser"] != null)
                {
                    User user = _userService.getUserById(loginUser.userId);
                    user.userVitality = user.userVitality + 1;
                    _userService.updateUser(user);
                }
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

        #region 成为方案高手
        public ActionResult BecomeSuperior(User user)
        {
            User me = _userService.getUserById(loginUser.userId);
            me.userMerit = Request.Form["ques-title"];
            me.userIntro = Request.Form["superior_text"];
            //活跃度+1
            me.userVitality = me.userVitality + 1;
            if (_userService.updateUser(me))
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

        #region 检查用户是否已经登录
        public ActionResult checkIfLogin()
        {
            if (System.Web.HttpContext.Current.Session["loginUser"] == null)
            {
                JsonResult["boo_success"] = false;
                return Json(JsonResult);
            }
            else
            {
                JsonResult["boo_success"] = true;
                return Json(JsonResult);
            }
        }
        #endregion

        #region 获取全部大学的院系
        public ActionResult getAllDepart()
        {
            List<string> result = _departService.getAllDepart();
            return Json(result);
        }
        #endregion

        #region 获取全部大学的名称
        public ActionResult getAllSchool()
        {
            List<string> result = _schoolService.getAllSchool();
            return Json(result);
        }
        #endregion

        #region 获取本校的院系
        public ActionResult getMySchoolDepart()
        {
            School school = _schoolService.getSchoolBySchoolName(loginUser.userSchool);
            List<string> result = _departService.getDepartsBySchoolId(school.schoolId);
            return Json(result);
        }
        #endregion

        #region 通过游客ip检查用户是否重复点赞方案记录
        public ActionResult checkIfP()
        {
            string ip = Request.QueryString["ip"];
            if (Request.Cookies["userIp"] != null)
            {
                JsonResult["boo_success"] = false;
                return Json(JsonResult, JsonRequestBehavior.AllowGet);
            }
            else
            {
                HttpCookie cookie = new HttpCookie("userIp", ip); //创建cookie的实例。 
                Response.Cookies.Add(cookie);//将创建的cookie文件输入到浏览器端 

                JsonResult["boo_success"] = true;
                return Json(JsonResult, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region 添加或更新热门搜索
        public ActionResult superiorSearchNums(string word)
        {
            if (_hotSuperiorService.checkIfHadSearch(word) != 0)
            {
                hotSuperior h = _hotSuperiorService.getHotSuperiorByWrite(word);
                h.hotSuperiorNums = h.hotSuperiorNums + 1;
                if (_hotSuperiorService.updateHotSuperior(h))
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
            else
            {
                hotSuperior h = new hotSuperior();
                h.hotSuperiorWrite = word;
                h.hotSuperiorNums = 1;
                if (_hotSuperiorService.addHotSuperior(h))
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
        }
        #endregion

        #region 返回我的大学名称
        public ActionResult getMySchoolName()
        {
            string result = loginUser.userSchool;
            return Json(result);
        } 
        #endregion

        #region 返回我的头像信息
        public ActionResult getMyInfo()
        {
            User user = _userService.getUserById(loginUser.userId);
            var result = user.userImg;
            return Json(result);
        } 
        #endregion

        #region 获取大家庭成员信息
        public ActionResult getFamilys()
        {
            string[] JsonResults = new string[4];
            List<User> familys = _userService.getFamilys();

            List<User> familysItemOne = familys.Skip(9 * 0).Take(9).ToList();
            List<User> familysItemTwo = familys.Skip(9 * 1).Take(9).ToList();
            List<User> familysItemthree = familys.Skip(9 * 2).Take(9).ToList();
            List<User> familysItemForth = familys.Skip(9 * 3).Take(9).ToList();

            List<SuperiorModel> result1 = _userService.selectSuperiorData(familysItemOne);
            JsonResults[0] = Util.JsonHelper.Jso_ToJSON(result1);

            List<SuperiorModel> result2 = _userService.selectSuperiorData(familysItemTwo);
            JsonResults[1] = Util.JsonHelper.Jso_ToJSON(result2);

            List<SuperiorModel> result3 = _userService.selectSuperiorData(familysItemthree);
            JsonResults[2] = Util.JsonHelper.Jso_ToJSON(result3);

            List<SuperiorModel> result4 = _userService.selectSuperiorData(familysItemForth);
            JsonResults[3] = Util.JsonHelper.Jso_ToJSON(result4);

            return Json(JsonResults);
        } 
        #endregion

        #region 采纳为最佳方案》对方案高手提供的方案进行描述已经评价
        public ActionResult commitSoluComment()
        {
            ProjectRecord projectRecord = new ProjectRecord();
            projectRecord.pRContent = Request.Form["pRContent"];
            projectRecord.pRComment = Request.Form["pRComment"];
            if (Request.Form["options"] == "0")
            {
                //最家方案
                projectRecord.pRIfSuccess = true;
                JsonResult["boo_success"] = 0;
            }
            else
            {
                //方案不合
                JsonResult["boo_success"] = 1;
            }
            projectRecord.pRTime = DateTime.Now;
            projectRecord.pRUserId = loginUser.userId;
            projectRecord.userId = Convert.ToInt64(Request.Form["userId"]);
            projectRecord.userCommentNum = 0;
            projectRecord.userLoveNum = 0;

            if (_projectRecordService.addRrojectRecord(projectRecord))
            {
                return Json(JsonResult);
            }
            else
            {
                JsonResult["boo_success"] = false;
                return Json(JsonResult);
            }
        } 
        #endregion
    }
}
