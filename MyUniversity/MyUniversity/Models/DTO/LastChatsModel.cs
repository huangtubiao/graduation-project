using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyUniversity.Models.DTO
{
    public class LastChatsModel
    {
        public int lastChatId { get; set; }
        public string userImg { get; set; }
        public string userName { get; set; }
        public string lastChatContent { get; set; }
        public long lastChatfriendId { get; set; }
        public int messageUnreadNum { get; set; }
    }
}
