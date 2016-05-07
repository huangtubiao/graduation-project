using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyUniversity.Models.DTO
{
    public class IpAddressModel
    {
        public string ret { get; set; }
        public string start { get; set; }
        public string end { get; set; }
        public string country { get; set; }
        public string province { get; set; }
        public string city { get; set; }
        public string district { get; set; }
        public string isp { get; set; }
        public string type { get; set; }
        public string desc { get; set; }
    }
}
