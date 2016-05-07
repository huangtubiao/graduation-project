using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyUniversity.Models.DTO
{
    public class PlanComeOnModel
    {
        public string planComeOnWrite { get; set; }
        public DateTime planComeOnTime { get; set; }
        public long userId { get; set; }
        public string userName { get; set; }
        public string userSex { get; set; }
        public string userImg { get; set; }
    }
}
