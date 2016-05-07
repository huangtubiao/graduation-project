using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyUniversity.Models.DTO
{
    public class LoginModel
    {
        public string ip { get; set; }
        public string userPaw { get; set; }
        public string userAccount { get; set; }
    }
}
