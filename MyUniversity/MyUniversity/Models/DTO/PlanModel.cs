using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyUniversity.Models.DTO
{
    public class PlanModel
    {
        public long planId { get; set; }
        public string planTilte { get; set; }
        public string planContent { get; set; }
        public int planViewNum { get; set; }
        public string planPublishedTime { get; set; }
        public DateTime planTime { get; set; }
        public int planCommentNum { get; set; }
        public int planComeOnNum { get; set; }
        public int planSuperviseNum { get; set; }
        public bool planIsFinish { get; set; }
        public long userId { get; set; }
        public string userImg { get; set; }
        public string userName { get; set; }
        public string userSex { get; set; }
        public string userDepart { get; set; }
        public DateTime superviceTime { get; set; }
        public string _superviceTime { get; set; }
        public int todayIfClockLog { get; set; }
    }
}
