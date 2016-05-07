using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyUniversity.Models.DTO
{
    public class RegisterModel
    {
        public string userPaw { get; set; }
        public string userAccount { get; set; }
        public string userSchool { get; set; }
    }
}
