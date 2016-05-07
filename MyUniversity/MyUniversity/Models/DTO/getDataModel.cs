using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyUniversity.Models.DTO
{
    public class getDataModel
    {
        public int groupNumber { get; set; }
        public string whatOnactive { get; set; }
        public string userDepartment { get; set; }
        public string searchWrite { get; set; }
        public string userSchool { get; set; }
    }
}
