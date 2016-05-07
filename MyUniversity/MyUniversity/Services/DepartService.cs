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
    public class DepartService : IDepartService
    {
        UniversityEntities db = new UniversityEntities();

        public IDepartRepository _departRepository { get; private set; }

        public DepartService(IDepartRepository departRepository)
        {
            this._departRepository = departRepository;
        }

        #region 条件检索
        public List<string> getDepartsBySchoolId(int schoolId)
        {
            return db.Depart.Where(o => o.schoolId == schoolId).Select(o => o.departName).ToList();
        }

        public List<string> getDepartsByWrite(string searchText, int schoolId)
        {
            return _departRepository.Get(o => o.departName.Contains(searchText) && o.schoolId == schoolId).Select(o => o.departName).ToList();
        }

        public List<string> getDepartsBySearchWrite(string searchText)
        {
            return _departRepository.Get(o => o.departName.Contains(searchText)).Select(o => o.departName).Distinct().ToList();
        }
        #endregion

        #region 获取全部大学的院系
        public List<string> getAllDepart()
        {
            return _departRepository.Get().Select(o => o.departName).Distinct().ToList();
        } 
        #endregion
    }
}
