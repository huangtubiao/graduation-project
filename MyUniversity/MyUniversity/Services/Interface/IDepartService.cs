using MyUniversity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyUniversity.Services.Interface
{
    public interface IDepartService 
    {
        List<string> getDepartsBySchoolId(int schoolId);
        List<string> getDepartsByWrite(string searchText, int schoolId);
        List<string> getAllDepart();
        List<string> getDepartsBySearchWrite(string searchText);

    }
}
