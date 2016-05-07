using MyUniversity.Models;
using MyUniversity.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyUniversity.Services.Interface
{
    public interface IPlanService
    {
        List<Plan> getAllNewestPlans(int groupNumber);
        List<Plan> getHotPlans(int groupNumber);
        List<Plan> getAllHotPlans();
        List<Plan> getMyplansByUserid(long userId);

        List<PlanModel> selectMySupervicePlans(List<Supervice> mySupervice);
        List<PlanModel> selectPlansData(List<Plan> plans);

        int[] getTotalGroups(long userId);

        IQueryable<Plan> getPlanById(long planId);

        bool wirtePlan(Plan plan, long userId);
        bool updatePlan(Plan plan);
    } 
}
