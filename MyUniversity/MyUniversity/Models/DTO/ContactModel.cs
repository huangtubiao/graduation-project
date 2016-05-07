using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyUniversity.Models.DTO
{
    public class ContactModel
    {
        public long contactFriendId { get; set; }
        public string userImg { get; set; }
        public string userName { get; set; }
    }
}
