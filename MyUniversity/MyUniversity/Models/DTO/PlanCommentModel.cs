using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyUniversity.Models.DTO
{
    public class PlanCommentModel
    {
        public string planCommentContent { get; set; }
        public string planCommentPublishedTime { get; set; }
        public string userImg { get; set; }
        public string userName { get; set; }
        public string userSex { get; set; }
        public long userId { get; set; }
    }
}
