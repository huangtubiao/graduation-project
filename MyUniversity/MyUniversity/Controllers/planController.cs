using MyUniversity.Models;
using MyUniversity.Models.DTO;
using MyUniversity.Models.Help;
using MyUniversity.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyUniversity.Controllers
{
    public class planController : baseController
    {
        private loginUser loginUser { get; set; }
        private IPlanService _planService { get; set; }
        private ISuperviceService _superviceService { get; set; }
        private IClockLogService _clockLogService { get; set; }
        private IPlanCommentService _planCommentService { get; set; }
        private IUserService _userService { get; set; }
        private IPlanComeOnService _planComeOnService { get; set; }

        public planController(IPlanService planService, ISuperviceService superviceService, IClockLogService clockLogService, IPlanCommentService planCommentService,
            IUserService userService, IPlanComeOnService planComeOnService)
        {
            _planService = planService;
            _superviceService = superviceService;
            _clockLogService = clockLogService;
            _planCommentService = planCommentService;
            _userService = userService;
            _planComeOnService = planComeOnService;
            loginUser = Models.Help.loginUser.getLoginUser();
        }

        #region 返回视图
        public ActionResult index()
        {
            if (System.Web.HttpContext.Current.Session["loginUser"] == null)
            {
                JsonResult["boo_success"] = false;
            }

            //全部计划
            List<Plan> plans = _planService.getAllNewestPlans(1);
            for (int q = 0; q < plans.Count; q++)
            {
                plans[q].planPublishedTime = Util.CommonMethod.getQuestionPublishedTime(plans[q].planTime);
            }
            ViewData["AllNewestPlans"] = plans;

            //我监督的
            List<Supervice> mySupervice = _superviceService.getMySupervice(1, loginUser.userId);
            List<PlanModel> mySupervicePlans = _planService.selectMySupervicePlans(mySupervice);
            for (int q = 0; q < mySupervicePlans.Count; q++)
            {
                mySupervicePlans[q].planPublishedTime = Util.CommonMethod.getQuestionPublishedTime(mySupervicePlans[q].planTime);
            }
            ViewData["mySupervicePlans"] = mySupervicePlans;

            int[] totalGroups = new int[2];
            totalGroups = _planService.getTotalGroups(loginUser.userId); //查询每个选项有多少页，每一页5条数据；
            ViewData["totalGroups"] = totalGroups[0];
            ViewData["totalSecondGroups"] = totalGroups[1];

            List<User> finishPlanUser = _userService.getFinishPlanUser();
            ViewData["finishPlanUser"] = finishPlanUser;
            return View();
        }

        //计划详情
        public ActionResult detail(string id, string group)
        {
            long planId;
            if (id == null)
            {
                planId = Convert.ToInt32(group);
            }
            else
            {
                planId = Convert.ToInt32(id); ;
            }

            IQueryable<Plan> plan = _planService.getPlanById(planId);
            plan.ToList()[0].planPublishedTime = Util.CommonMethod.getQuestionPublishedTime(plan.ToList()[0].planTime);
            ViewData["plan"] = plan;

            List<Clocklog> clockLogs = _clockLogService.getClockLogsByPlanId(planId, 1);
            List<ClockLogModel> clockLogModel = _clockLogService.selectClockLogModel(clockLogs);
            ViewData["clockLogModel"] = clockLogModel;

            List<Plancomment> planCommments = _planCommentService.getPlanCommmentsByPlanId(planId);
            List<PlanCommentModel> planCommentModel = _planCommentService.selectPlanCommentModels(planCommments);
            ViewData["planCommentModel"] = planCommentModel;
            return View();
        }

        //写新计划
        public ActionResult write()
        {
            return View();
        }

        //邀请监督员
        public ActionResult invite()
        {
            ViewData["active"] = "邀请监督";
            ViewBag.active = "邀请监督";
            return View();
        }

        //我的计划
        public ActionResult myplan()
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

            List<User> finishPlanUser = _userService.getFinishPlanUser();
            ViewData["finishPlanUser"] = finishPlanUser;
            return View();
        }

        //我监督的
        public ActionResult mysupervice()
        {
            List<Supervice> mySupervice = _superviceService.getMySupervice(1, loginUser.userId);
            List<PlanModel> mySupervicePlans = _planService.selectMySupervicePlans(mySupervice);

            //判断今天是否完成打卡日志
            foreach (var p in mySupervicePlans)
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
                }
            }
            for (int q = 0; q < mySupervicePlans.Count; q++)
            {
                mySupervicePlans[q].planPublishedTime = Util.CommonMethod.getQuestionPublishedTime(mySupervicePlans[q].planTime);
            }
            ViewData["mySupervicePlans"] = mySupervicePlans;

            List<User> finishPlanUser = _userService.getFinishPlanUser();
            ViewData["finishPlanUser"] = finishPlanUser;

            int[] totalGroups = new int[2];
            totalGroups = _planService.getTotalGroups(loginUser.userId); //查询每个选项有多少页，每一页5条数据；
            ViewData["totalGroups"] = totalGroups[1];
            return View();
        }

        //查看计划监督的人
        public ActionResult superviceuser(long id)
        {
            IQueryable<Plan> plan = _planService.getPlanById(id);
            ViewData["planTitle"] = plan.ToList()[0].planTitle;
            List<Supervice> superviceUser = _superviceService.getMySuperviceByPlanid(id);
            ViewData["superviceUser"] = superviceUser;

            List<User> finishPlanUser = _userService.getFinishPlanUser();
            ViewData["finishPlanUser"] = finishPlanUser;
            return View();
        }

        //查看为我加油的成员
        public ActionResult cheerforme(long id)
        {
            IQueryable<Plan> plan = _planService.getPlanById(id);
            ViewData["planTitle"] = plan.ToList()[0].planTitle;
            List<PlanComeOn> planComeOns = _planComeOnService.getPlanComeOnByPlanid(id);
            List<PlanComeOnModel> planComeOnModel = _planComeOnService.selectPlanComeOnModel(planComeOns);
            ViewData["planComeOns"] = planComeOnModel;

            List<User> finishPlanUser = _userService.getFinishPlanUser();
            ViewData["finishPlanUser"] = finishPlanUser;
            return View();
        }

        //查看计划加油的人
        public ActionResult cheerforplan(long id)
        {
            IQueryable<Plan> plan = _planService.getPlanById(id);
            ViewData["planTitle"] = plan.ToList()[0].planTitle;
            List<PlanComeOn> planComeOns = _planComeOnService.getPlanComeOnByPlanid(id);
            List<PlanComeOnModel> planComeOnModel = _planComeOnService.selectPlanComeOnModel(planComeOns);
            ViewData["planComeOns"] = planComeOnModel;

            List<User> finishPlanUser = _userService.getFinishPlanUser();
            ViewData["finishPlanUser"] = finishPlanUser;
            return View();
        }

        //监督我的人
        public ActionResult superviceme(long id)
        {
            IQueryable<Plan> plan = _planService.getPlanById(id);
            ViewData["planTitle"] = plan.ToList()[0].planTitle;
            List<Supervice> superviceUser = _superviceService.getMySuperviceByPlanid(id);
            ViewData["superviceUser"] = superviceUser;

            List<User> finishPlanUser = _userService.getFinishPlanUser();
            ViewData["finishPlanUser"] = finishPlanUser;
            return View();
        }

        //写打卡日志
        public ActionResult clocklog(long id)
        {
            IQueryable<Plan> plan = _planService.getPlanById(id);
            ViewData["planId"] = plan.ToList()[0].planId;
            ViewData["planTitle"] = plan.ToList()[0].planTitle;

            List<Clocklog> clockLogs = _clockLogService.getClockLogs();
            ViewData["clockLogs"] = clockLogs;
            return View();
        }

        //提交计划打卡日志成功
        public ActionResult success(long id)
        {
            ViewData["planId"] = id;
            return View();
        }

        #endregion

        #region 获取最新、最热的计划监督
        public ActionResult getNewHotPlan(int groupNumber, string hotOrNew, string active)
        {
            List<Plan> plans = null;
            var jsonResult = "";
            List<PlanModel> result = null;
            if (active == "全部计划")
            {
                if (hotOrNew == "最新")
                {
                    plans = _planService.getAllNewestPlans(groupNumber);
                }
                else if (hotOrNew == "最热")
                {
                    List<Plan> allHotPlans = _planService.getAllHotPlans();
                    plans = allHotPlans.Skip(5 * (groupNumber - 1)).Take(5).ToList();   //获取某一页的数据
                }
                for (int q = 0; q < plans.Count; q++)
                {
                    plans[q].planPublishedTime = Util.CommonMethod.getQuestionPublishedTime(plans[q].planTime);
                }
                result = _planService.selectPlansData(plans);
                jsonResult = Util.JsonHelper.Jso_ToJSON(result);
            }
            else if (active == "我监督的")
            {
                if (hotOrNew == "最新")
                {

                    List<Supervice> mySupervice = _superviceService.getMySupervice(groupNumber, loginUser.userId);
                    List<PlanModel> mySupervicePlans = _planService.selectMySupervicePlans(mySupervice);
                    List<PlanModel> newSupervicePlans = mySupervicePlans.OrderByDescending(o => o.planTime).ToList();
                    //判断今天是否完成打卡日志
                    foreach (var p in mySupervicePlans)
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
                        }
                    }
                    for (int q = 0; q < newSupervicePlans.Count; q++)
                    {
                        newSupervicePlans[q]._superviceTime = newSupervicePlans[q].superviceTime.ToShortDateString();
                        newSupervicePlans[q].planPublishedTime = Util.CommonMethod.getQuestionPublishedTime(newSupervicePlans[q].planTime);
                    }
                    result = newSupervicePlans;
                }
                else if (hotOrNew == "最热")
                {
                    List<Supervice> mySupervice = _superviceService.getMySupervice(groupNumber, loginUser.userId);
                    List<PlanModel> mySupervicePlans = _planService.selectMySupervicePlans(mySupervice);
                    List<PlanModel> hotSupervicePlans = mySupervicePlans.OrderByDescending(o => o.planSuperviseNum).ToList();

                    for (int q = 0; q < hotSupervicePlans.Count; q++)
                    {
                        hotSupervicePlans[q]._superviceTime = hotSupervicePlans[q].superviceTime.ToShortDateString();
                        hotSupervicePlans[q].planPublishedTime = Util.CommonMethod.getQuestionPublishedTime(hotSupervicePlans[q].planTime);
                    }
                    result = hotSupervicePlans;
                }
                jsonResult = Util.JsonHelper.Jso_ToJSON(result);
            }

            return Json(jsonResult);

        }
        #endregion

        #region 写新的计划
        [HttpPost]
        public ActionResult write(Plan plan)
        {
            if (System.Web.HttpContext.Current.Session["loginUser"] == null)
            {
                JsonResult["boo_success"] = "unlogin";
                return Json(JsonResult);
            }
            else
            {
                if (_planService.wirtePlan(plan, loginUser.userId) == true)
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

        #region 不显示已完成的计划>只显示没有完成的计划
        public ActionResult noShowFinishPlan(string ifShowFinish)
        {
            if (ifShowFinish == "false")  
            {

                List<Supervice> mySupervice = _superviceService.getMySupervice(1, loginUser.userId);
                List<PlanModel> mySupervicePlans = _planService.selectMySupervicePlans(mySupervice);
                List<PlanModel> unFinishPlans = mySupervicePlans.Where(o => o.planIsFinish == false).ToList();
                //判断今天是否完成打卡日志
                foreach (var p in unFinishPlans)
                {
                    List<Clocklog> clockLog = _clockLogService.getClockLogsByPlanId(p.planId, 1);
                    string d = clockLog[0].clocklogTime.ToString();
                    string today = DateTime.Now.ToShortDateString();
                    string tomorrow = DateTime.Now.AddDays(1).ToShortDateString();
                    if (DateTime.Parse(d) > DateTime.Parse(today) && DateTime.Parse(d) < DateTime.Parse(tomorrow))
                    {
                        p.todayIfClockLog = 1;
                    }
                }
                for (int q = 0; q < unFinishPlans.Count; q++)
                {
                    unFinishPlans[q]._superviceTime = unFinishPlans[q].superviceTime.ToShortDateString();
                    unFinishPlans[q].planPublishedTime = Util.CommonMethod.getQuestionPublishedTime(unFinishPlans[q].planTime);
                }
                var jsonResult = Util.JsonHelper.Jso_ToJSON(unFinishPlans);
                return Json(jsonResult);
            }
            else
            {
                List<Supervice> mySupervice = _superviceService.getMySupervice(1, loginUser.userId);
                List<PlanModel> mySupervicePlans = _planService.selectMySupervicePlans(mySupervice);
                //判断今天是否完成打卡日志
                foreach (var p in mySupervicePlans)
                {
                    List<Clocklog> clockLog = _clockLogService.getClockLogsByPlanId(p.planId, 1);
                    string d = clockLog[0].clocklogTime.ToString();
                    string today = DateTime.Now.ToShortDateString();
                    string tomorrow = DateTime.Now.AddDays(1).ToShortDateString();
                    if (DateTime.Parse(d) > DateTime.Parse(today) && DateTime.Parse(d) < DateTime.Parse(tomorrow))
                    {
                        p.todayIfClockLog = 1;
                    }
                }
                for (int q = 0; q < mySupervicePlans.Count; q++)
                {
                    mySupervicePlans[q]._superviceTime = mySupervicePlans[q].superviceTime.ToShortDateString();
                    mySupervicePlans[q].planPublishedTime = Util.CommonMethod.getQuestionPublishedTime(mySupervicePlans[q].planTime);
                }
                var jsonResult = Util.JsonHelper.Jso_ToJSON(mySupervicePlans);
                return Json(jsonResult);
            }
        }
        #endregion

        #region 更新计划和添加计划加油
        public ActionResult cheer(long planId)
        {
            IQueryable<Plan> plan = _planService.getPlanById(planId);
            plan.ToList()[0].planComeOnNum = plan.ToList()[0].planComeOnNum + 1;
            bool boo_plan = _planService.updatePlan(plan.FirstOrDefault());

            PlanComeOn planComeOn = new PlanComeOn();
            planComeOn.planId = planId;
            planComeOn.planComeOnTime = DateTime.Now;
            planComeOn.userId = loginUser.userId;
            bool boo_comeOn = _planComeOnService.addPlanComeOn(planComeOn);
            if (boo_plan == true && boo_comeOn == true)
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

        #region 检查是否已经为该计划加油过
        public ActionResult checkIfCheered(long planId)
        {
            PlanComeOn planComeOn = _planComeOnService.getPlanComeOnByPlanidUserid(planId, loginUser.userId);
            if (planComeOn == null)
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

        #region 更新计划加油》添加加油的内容
        public ActionResult updatePlanComeOn(string planComeOnWrite, long planId)
        {
            PlanComeOn planComeOn = _planComeOnService.getPlanComeOnByPlanidUserid(planId, loginUser.userId);
            planComeOn.planComeOnWrite = planComeOnWrite;
            if (_planComeOnService.upatePlanComeOn(planComeOn))
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

        #region 获取最近加油的数据
        public ActionResult getNewlyCheerUser(long planId)
        {
            List<PlanComeOn> NewlyCheer = _planComeOnService.getNewlyCheerByPlanid(planId);
            List<NewlyCheerModel> newlyCheerModel = _planComeOnService.selectNewlyCheerModel(NewlyCheer);
            var jsonResult = Util.JsonHelper.Jso_ToJSON(newlyCheerModel);
            return Json(jsonResult);
        } 
        #endregion

        #region 计划浏览次数+1
        public ActionResult addPlanView(long planId)
        {
            IQueryable<Plan> plan = _planService.getPlanById(planId);
            plan.ToList()[0].planViewNum = plan.ToList()[0].planViewNum + 1;
            if (_planService.updatePlan(plan.FirstOrDefault()))
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

        #region 检查是否已经监督了该计划
        public ActionResult checkIfSupervice(long planId)
        {
            Supervice supervice = _superviceService.getSuperviceByPlanidUserid(planId, loginUser.userId);
            if (supervice == null)
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

        #region 监督计划
        public ActionResult supervice(long planId)
        {
            Supervice supervice = new Supervice();
            supervice.planId = planId;
            supervice.userId = loginUser.userId;
            supervice.superviceTime = DateTime.Now;
            if (_superviceService.addSupervice(supervice))
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

        #region 计划评论》添加新的计划评论
        public ActionResult planComment(Plancomment plancomment)
        {
            plancomment.plancommentTime = DateTime.Now;
            plancomment.userId = loginUser.userId;
            if (_planCommentService.addPlanComment(plancomment))
            {
                User user = _userService.getUserById(loginUser.userId);
                PlanCommentModel planCommentModel = _planCommentService.selectPlanCommentModel(plancomment, user);
                var jsonResult = Util.JsonHelper.Jso_ToJSON(planCommentModel);
                return Json(jsonResult);
            }
            else
            {
                JsonResult["boo_success"] = false;
                return Json(JsonResult);
            }
        } 
        #endregion

        #region 加载更多打卡日志
        public ActionResult loadingMoreClockLog(long planId, int groupNum)
        {
            List<Clocklog> clockLogs = _clockLogService.getClockLogsByPlanId(planId, groupNum);
            List<ClockLogModel> clockLogModel = _clockLogService.selectClockLogModel(clockLogs);
            var jsonResult = Util.JsonHelper.Jso_ToJSON(clockLogModel);
            return Json(jsonResult);
        } 
        #endregion

        #region 增加新的打卡日志
        public ActionResult writeClockLog(Clocklog clockLog)
        {
            clockLog.clocklogTime = DateTime.Now;
            clockLog.userId = loginUser.userId;
            if (_clockLogService.addClockLog(clockLog) == true)
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
    }
}
