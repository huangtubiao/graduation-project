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
    public class PlanService : IPlanService
    {
        public IPlanRepository _planRepository { get; private set; }
        public ISuperviceRepository _superviceRepository { get; private set; }

        public PlanService(IPlanRepository planRepository, ISuperviceRepository superviceRepository)
        {
            this._planRepository = planRepository;
            this._superviceRepository = superviceRepository;
        }

        #region 条件检索
        public IQueryable<Plan> getPlanById(long planId)
        {
            return _planRepository.Get(o => o.planId == planId).AsQueryable();
        }

        public List<Plan> getMyplansByUserid(long userId)
        {
            return _planRepository.Get(o => o.userId == userId).OrderByDescending(o=>o.planTime).ToList();
        }
        #endregion

        #region 获取一定数量的最新的计划，并且按照最新排序
        public List<Plan> getAllNewestPlans(int groupNumber)
        {
            return _planRepository.Get(null, groupNumber, 5, o => o.planTime, false).ToList();
        }
        #endregion

        #region 获取一定数量的计划，并且按照热度排序
        public List<Plan> getHotPlans(int groupNumber)
        {
            return _planRepository.Get(null, groupNumber, 5, o => o.planCommentNum, false).ToList();
        } 
        #endregion

        #region 获取所有的计划，并且按照热度排序
        public List<Plan> getAllHotPlans()
        {
            return _planRepository.Get().OrderByDescending(o => o.planSuperviseNum).ToList();
        } 
        #endregion

        #region 获取数据的总条数
        public int[] getTotalGroups(long userId)
        {
            int[] totalGroups = new int[3];
            totalGroups[0] = _planRepository.Count(null);
            totalGroups[1] = _superviceRepository.Count(o => o.userId == userId);
            return totalGroups;
        } 
        #endregion

        #region 提取我监督的计划
        public List<PlanModel> selectMySupervicePlans(List<Supervice> mySupervice)
        {
            List<PlanModel> MySupervicePlans = new List<PlanModel>();
            foreach (var p in mySupervice)
            {
                MySupervicePlans.Add(new PlanModel
                {
                    planId = p.planId,
                    planTilte = p.Plan.planTitle,
                    planContent = p.Plan.planContent,
                    planViewNum = Convert.ToInt32(p.Plan.planViewNum),
                    planTime = p.Plan.planTime,
                    planCommentNum = Convert.ToInt32(p.Plan.planCommentNum),
                    planComeOnNum = Convert.ToInt32(p.Plan.planComeOnNum),
                    planSuperviseNum = Convert.ToInt32(p.Plan.planSuperviseNum),
                    planPublishedTime = p.Plan.planPublishedTime,
                    planIsFinish = Convert.ToBoolean(p.Plan.planIsFinish),
                    userId = p.User.userId,
                    userImg = p.User.userImg,
                    userName = p.User.userName,
                    userSex = p.User.userSex,
                    userDepart = p.User.userDepart,
                    superviceTime = Convert.ToDateTime(p.superviceTime)
                    
                });
            }
            return MySupervicePlans;
        }
        #endregion

        #region 提取计划的显示数据
        public List<PlanModel> selectPlansData(List<Plan> plans)
        {
            List<PlanModel> PlansData = new List<PlanModel>();
            foreach (var p in plans)
            {
                PlansData.Add(new PlanModel
                {
                    planId = p.planId,
                    planTilte = p.planTitle,
                    planContent = p.planContent,
                    planViewNum = Convert.ToInt32(p.planViewNum),
                    planTime = p.planTime,
                    planCommentNum = Convert.ToInt32(p.planCommentNum),
                    planComeOnNum = Convert.ToInt32(p.planComeOnNum),
                    planSuperviseNum = Convert.ToInt32(p.planSuperviseNum),
                    planPublishedTime = p.planPublishedTime,
                    planIsFinish = Convert.ToBoolean(p.planIsFinish),
                    userId = p.User.userId,
                    userImg = p.User.userImg,
                    userName = p.User.userName,
                    userSex = p.User.userSex,
                    userDepart = p.User.userDepart
                });
            }
            return PlansData;
        } 
        #endregion

        #region 写新的计划
        public bool wirtePlan(Plan plan, long userId)
        {
            plan.planTime = DateTime.Now;
            plan.planViewNum = 0;
            plan.planSuperviseNum = 0;
            plan.planCommentNum = 0;
            plan.planComeOnNum = 0;
            plan.userId = userId;
            try
            {
                _planRepository.Add(plan);
                return true;
            }
            catch
            {
                return false;
            }
        } 
        #endregion

        #region 更新计划
        public bool updatePlan(Plan plan)
        {
            try
            {
                _planRepository.Update(plan);
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
