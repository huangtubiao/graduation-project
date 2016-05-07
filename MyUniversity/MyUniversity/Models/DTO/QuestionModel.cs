using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyUniversity.Models.DTO
{
    public class QuestionModel
    {
        public long userId { get; set; }
        public string userImg { get; set; }
        public string userName { get; set; }
        public string userSex { get; set; }
        public int questionReplyNum { get; set; }
        public int questionViewNum { get; set; }
        public string questionTitle { get; set; }
        public long questionId { get; set; }
        public long newReplyUserId { get; set; }
        public long newReplyUserImg { get; set; }
        public string newReplyUserName { get; set; }
        public string questionNewestReply { get; set; }
        public string userDepart { get; set; }
        public string questionPublishedTime { get; set; }
    }
}
