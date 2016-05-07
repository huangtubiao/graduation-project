using MyUniversity.Models;
using MyUniversity.Models.DTO;
using MyUniversity.Models.Repositories.Interface;
using MyUniversity.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyUniversity.Services
{
    public class PlanComeOnService : IPlanComeOnService
    {
        public IPlanComeOnRepository _planComeOnRepository { get; private set; }

        public PlanComeOnService(IPlanComeOnRepository planComeOnRepository)
        {
            this._planComeOnRepository = planComeOnRepository;
        }

        #region 条件检索
        public List<PlanComeOn> getPlanComeOnByPlanid(long planId)
        {
            return _planComeOnRepository.Get(o => o.planId == planId).OrderByDescending(o => o.planComeOnTime).ToList();
        }

        public PlanComeOn getPlanComeOnByPlanidUserid(long planId, long userId)
        {
            return _planComeOnRepository.Get(o => o.planId == planId && o.userId == userId).FirstOrDefault();
        }

        public List<PlanComeOn> getNewlyCheerByPlanid(long planId)
        {
            return _planComeOnRepository.Get(o => o.planId == planId, 1, 4, o => o.planComeOnTime, false).ToList();
        }
        #endregion

        #region 提取出必要的计划加油信息
        public List<PlanComeOnModel> selectPlanComeOnModel(List<PlanComeOn> planComeOns)
        {
            List<PlanComeOnModel> planComeOnData = new List<PlanComeOnModel>();
            foreach (var p in planComeOns)
            {
                planComeOnData.Add(new PlanComeOnModel
                {
                    planComeOnWrite = p.planComeOnWrite,
                    planComeOnTime = Convert.ToDateTime(p.planComeOnTime),
                    userId = Convert.ToInt64(p.userId),
                    userImg = p.User.userImg,
                    userName = p.User.userName,
                    userSex = p.User.userSex
                });
            }
            return planComeOnData;
        } 
        #endregion

        #region 提取出必要的最近加油的同学信息
        public List<NewlyCheerModel> selectNewlyCheerModel(List<PlanComeOn> NewlyCheer)
        {
            List<NewlyCheerModel> newlyCheerData = new List<NewlyCheerModel>();
            foreach (var n in NewlyCheer)
            {
                newlyCheerData.Add(new NewlyCheerModel
                {
                    userId = Convert.ToInt64(n.userId),
                    userImg = n.User.userImg
                });
            }
            return newlyCheerData;
        } 
        #endregion

        #region 添加新的计划加油
        public bool addPlanComeOn(PlanComeOn planComeOn)
        {
            try
            {
                _planComeOnRepository.Add(planComeOn);
                return true;
            }
            catch
            {
                return false;
            }
            
        } 
        #endregion

        #region 更新计划加油
        public bool upatePlanComeOn(PlanComeOn planComeOn)
        {
            try
            {
                _planComeOnRepository.Update(planComeOn);
                return true;
            }
            catch
            {
                return false;
            }
        } 
        #endregion

        
    }
}
