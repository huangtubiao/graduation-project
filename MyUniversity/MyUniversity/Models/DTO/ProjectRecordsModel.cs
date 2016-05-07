using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyUniversity.Models.DTO
{
    public class ProjectRecordsModel : Controller
    {
        public long pRId { get; set; }
        public string pRContent { get; set; }
        public string pRComment { get; set; }
        public string pRPublishedTime { get; set; }
        public int userLoveNum { get; set; }
        public int userCommentNum { get; set; }
        public long userId { get; set; }
        public string userImg { get; set; }
        public string userName { get; set; }
    }
}
