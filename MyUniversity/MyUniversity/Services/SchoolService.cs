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
    public class SchoolService : ISchoolService
    {
        UniversityEntities db = new UniversityEntities();

        public ISchoolRepository _schoolRepository { get; private set; }

        public SchoolService(ISchoolRepository schoolRepository)
        {
            this._schoolRepository = schoolRepository;
        }

        public IQueryable<School> getHotSchools()
        {
            return _schoolRepository.Get(null, 1, 10, o => o.schoolRegisterNum, false).AsQueryable();
        }

        public School getSchoolBySchoolName(string schoolName)
        {
            return _schoolRepository.Get(o => o.schoolName == schoolName).FirstOrDefault();
        }

        #region 获取全部学校
        public List<string> getAllSchool()
        {
            return _schoolRepository.Get().OrderByDescending(o => o.schoolRegisterNum).Select(o => o.schoolName).ToList();
        } 
        #endregion

        #region 条件检索
        public List<string> getSchoolNamesByCityId(int cityId)
        {
            return _schoolRepository.Get(o => o.cityId == cityId).Select(o => o.schoolName).ToList();
        }

        public List<string> getSchoolsByWrite(string searchText)
        {
            return db.School.Where(o => o.schoolName.Contains(searchText)).Select(o => o.schoolName).ToList();
        }
        #endregion
    }
}
