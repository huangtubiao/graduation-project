using MyUniversity.Models;
using MyUniversity.Models.Repositories.Interface;
using MyUniversity.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyUniversity.Services
{
    public class SuperviceService : ISuperviceService
    {
        public ISuperviceRepository _superviceRepository { get; private set; }

        public SuperviceService(ISuperviceRepository superviceRepository)
        {
            this._superviceRepository = superviceRepository;
        }

        #region 条件检索
        public List<Supervice> getMySupervice(int groupNumber, long userId)
        {
            return _superviceRepository.Get(o => o.userId == userId, groupNumber, 5, o => o.superviceTime, false).ToList();
        }

        public List<Supervice> getMySuperviceByPlanid(long planId)
        {
            return _superviceRepository.Get(o => o.planId == planId).ToList();
        }

        public Supervice getSuperviceByPlanidUserid(long planId, long userId)
        {
            return _superviceRepository.Get(o => o.planId == planId && o.userId == userId).FirstOrDefault();
        } 
        #endregion

        #region 添加新的计划监督
        public bool addSupervice(Supervice supervice)
        {
            try
            {
                _superviceRepository.Add(supervice);
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
