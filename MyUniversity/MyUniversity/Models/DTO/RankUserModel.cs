using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyUniversity.Models.DTO
{
    public class RankUserModel
    {
        public int rankNum { get; set; }
        public long userId { get; set; }
        public string userName { get; set; }
        public string userImg { get; set; }
        public int userProjectNum { get; set; }
        public int userFans { get; set; }
        public long userRank { get; set; }
        public int userVitality { get; set; }
    }
}
