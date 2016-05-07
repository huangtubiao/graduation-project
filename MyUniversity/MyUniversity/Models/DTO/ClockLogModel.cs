using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyUniversity.Models.DTO
{
    public class ClockLogModel
    {
        public string clockLogContent { get; set; }
        public int clockLogDay { get; set; }
        public string clockLogYearMonth { get; set; }
    }
}
