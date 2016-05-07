using MyUniversity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyUniversity.Services.Interface
{
    public interface ISchoolService
    {
        IQueryable<School> getHotSchools();
        
        List<string> getAllSchool();
        List<string> getSchoolsByWrite(string searchText);

        School getSchoolBySchoolName(string schoolName);

        List<string> getSchoolNamesByCityId(int cityId);
    }
}
