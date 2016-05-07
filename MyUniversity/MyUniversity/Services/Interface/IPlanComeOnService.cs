using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyUniversity.Models;
using MyUniversity.Models.DTO;

namespace MyUniversity.Services.Interface
{
    public interface IPlanComeOnService
    {
        List<PlanComeOn> getPlanComeOnByPlanid(long planId);
        List<PlanComeOn> getNewlyCheerByPlanid(long planId);

        List<PlanComeOnModel> selectPlanComeOnModel(List<PlanComeOn> planComeOns);
        List<NewlyCheerModel> selectNewlyCheerModel(List<PlanComeOn> NewlyCheer); 

        bool addPlanComeOn(PlanComeOn planComeOn);
        bool upatePlanComeOn(PlanComeOn planComeOn);

        PlanComeOn getPlanComeOnByPlanidUserid(long planId, long userId);

    }
}
