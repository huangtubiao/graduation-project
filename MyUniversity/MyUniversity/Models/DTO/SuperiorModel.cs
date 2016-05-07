using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyUniversity.Models.DTO
{
    public class SuperiorModel
    {
        public long userId { get; set; }
        public string userImg { get; set; }
        public string userName { get; set; }
        public string userSex { get; set; }
        public string userMerit { get; set; }
        public string userMeritIntro { get; set; }
        public string userIntro { get; set; }
        public string userSchool { get; set; }
        public string userDepart { get; set; }
        public long userRank { get; set; }
        public int userVisitedNum { get; set; }
        public int userWeekExps { get; set; }
        public int userFans { get; set; }
    }
}
