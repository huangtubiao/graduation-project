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
    public class ProjectRecordService : IProjectRecordService
    {
        public IProjectRecordRepository _projectRecordRepository { get; private set; }

        public ProjectRecordService(IProjectRecordRepository projectRecordRepository)
        {
            this._projectRecordRepository = projectRecordRepository;
        }

        #region 条件检索
        public IQueryable<ProjectRecord> getRecordsByUserId(long userId)
        {
            return _projectRecordRepository.Get(o => o.pRUserId == userId).AsQueryable();
        }

        public ProjectRecord getPRecordById(long pRId)
        {
            return _projectRecordRepository.Get(o => o.pRId == pRId).FirstOrDefault();
        }
        #endregion

        #region 更新方案记录
        public bool updateProjectRecord(ProjectRecord projectRecord)
        {
            try
            {
                _projectRecordRepository.Update(projectRecord);
                return true;
            }
            catch
            {
                return false;
            }
        } 
        #endregion

        #region 添加新的方案记录
        public bool addRrojectRecord(ProjectRecord projectRecord)
        {
            try
            {
                _projectRecordRepository.Add(projectRecord);
                return true;
            }
            catch
            {
                return false;
            }
        } 
        #endregion

        #region 实现算法
        public List<ProjectRecordsModel> selectProjectRecords(IQueryable<ProjectRecord> ProjectRecords)
        {
            List<ProjectRecordsModel> ProjectRecordsModel = new List<ProjectRecordsModel>();
            foreach (var p in ProjectRecords.ToList())
            {
                if (p.pRContent != null)
                {
                    ProjectRecordsModel.Add(new ProjectRecordsModel
                    {
                        pRId = p.pRId,
                        pRContent = p.pRContent,
                        pRComment = p.pRComment,
                        pRPublishedTime = Util.CommonMethod.getQuestionPublishedTime(p.pRTime),
                        userLoveNum = Convert.ToInt32(p.userLoveNum),
                        userCommentNum = Convert.ToInt32(p.userCommentNum),
                        userId = p.userId,
                        userImg = p.User1.userImg,
                        userName = p.User1.userName
                    });
                }
            }
            return ProjectRecordsModel;
        }
        #endregion
    }
}
