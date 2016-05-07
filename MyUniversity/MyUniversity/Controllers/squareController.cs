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
    public class squareController : Controller
    {
        private loginUser loginUser { get; set; }
        private IPlanService _planService { get; set; }
        private ISuperviceService _superviceService { get; set; }

        public squareController(IPlanService planService, ISuperviceService superviceService)
        {
            _planService = planService;
            _superviceService = superviceService;
            loginUser = Models.Help.loginUser.getLoginUser();
        }

        #region 返回视图
        public ActionResult index()
        {
            //List<Plan> plans = _planService.getAllNewestPlans();
            //for (int q = 0; q < plans.Count; q++)
            //{
            //    plans[q].planPublishedTime = Util.CommonMethod.getQuestionPublishedTime(plans[q].planTime);
            //}
            //ViewData["AllNewestPlans"] = plans;

            //List<Supervice> mySupervice = _superviceService.getMySupervice(10);
            //List<PlanModel> mySupervicePlans = _planService.getMySupervicePlans(mySupervice);
            //for (int q = 0; q < mySupervicePlans.Count; q++)
            //{
            //    mySupervicePlans[q].planPublishedTime = Util.CommonMethod.getQuestionPublishedTime(mySupervicePlans[q].planTime);
            //}
            //ViewData["mySupervicePlans"] = mySupervicePlans;

            return View();
        } 
        #endregion

    }
}
