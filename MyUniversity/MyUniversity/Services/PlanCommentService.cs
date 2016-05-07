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
    public class PlanCommentService : IPlanCommentService
    {
        public IPlanCommentRepository _planCommentRepository { get; private set; }

        public PlanCommentService(IPlanCommentRepository planCommentRepository)
        {
            this._planCommentRepository = planCommentRepository;
        }

        #region 条件检索
        public List<Plancomment> getPlanCommmentsByPlanId(long planId)
        {
            return _planCommentRepository.Get(o=>o.planId == planId).ToList();
        } 
        #endregion

        #region 提取出计划评论信息
        public List<PlanCommentModel> selectPlanCommentModels(List<Plancomment> planCommments)
        {
            List<PlanCommentModel> planCommentModel = new List<PlanCommentModel>();
            foreach (var p in planCommments)
            {
                planCommentModel.Add(new PlanCommentModel
                {
                    planCommentContent = p.plancommentContent,
                    planCommentPublishedTime = Util.CommonMethod.getQuestionPublishedTime(p.plancommentTime),
                    userId = p.userId,
                    userImg = p.User.userImg,
                    userName = p.User.userName,
                    userSex = p.User.userSex
                });
            }
            return planCommentModel;
        } 
        #endregion

        #region 提取出单个计划评论信息
        public PlanCommentModel selectPlanCommentModel(Plancomment plancomment, User user)
        {
            PlanCommentModel planCommentModel = new PlanCommentModel();
            planCommentModel.userId = user.userId;
            planCommentModel.userName = user.userName;
            planCommentModel.userSex = user.userSex;
            planCommentModel.userImg = user.userImg;
            planCommentModel.planCommentContent = plancomment.plancommentContent;
            planCommentModel.planCommentPublishedTime = Util.CommonMethod.getQuestionPublishedTime(plancomment.plancommentTime);
            return planCommentModel;
        } 
        #endregion

        #region 添加新的计划评论
        public bool addPlanComment(Plancomment plancomment)
        {
            try
            {
                _planCommentRepository.Add(plancomment);
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
