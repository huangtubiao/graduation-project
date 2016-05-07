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
    public class RecentVisitorService : IRecentVisitorService
    {
        public IRecentVisitorRepository _recentVisitorRepository { get; private set; }

        public RecentVisitorService(IRecentVisitorRepository recentVisitorRepository)
        {
            this._recentVisitorRepository = recentVisitorRepository;
        }

        #region 条件检索
        public List<RecentVisitor> getRecentVisitorsByUserId(long userId)
        {
            return _recentVisitorRepository.Get(o => o.userId == userId, 1, 6, o => o.visitTime, false).ToList();
        } 
        #endregion

        public bool addRecentVisitor(RecentVisitor newVisitor)
        {
            try
            {
                _recentVisitorRepository.Add(newVisitor);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
