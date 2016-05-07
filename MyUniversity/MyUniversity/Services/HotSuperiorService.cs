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
    public class HotSuperiorService : IHotSuperiorService
    {
        public IHotSuperiorRepository _hotSuperiorRepository { get; private set; }

        public HotSuperiorService(IHotSuperiorRepository hotSuperiorRepository)
        {
            this._hotSuperiorRepository = hotSuperiorRepository;
        }

        #region 条件检索
        public hotSuperior getHotSuperiorByWrite(string word)
        {
            return _hotSuperiorRepository.Get(o => o.hotSuperiorWrite == word).FirstOrDefault();
        } 
        #endregion

        public List<hotSuperior> getHotSuperior()
        {
            return _hotSuperiorRepository.Get(null, 1, 10, o => o.hotSuperiorNums, false).ToList();
        }

        #region 查询是否有搜索过
        public int checkIfHadSearch(string word)
        {
            return _hotSuperiorRepository.Get(o => o.hotSuperiorWrite == word).Count();
        } 
        #endregion

        #region 更新热门搜索
        public bool updateHotSuperior(hotSuperior h)
        {
            try
            {
                _hotSuperiorRepository.Update(h);
                return true;
            }
            catch
            {
                return false;
            }

        } 
        #endregion

        public bool addHotSuperior(hotSuperior h)
        {
            try
            {
                _hotSuperiorRepository.Add(h);
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}
