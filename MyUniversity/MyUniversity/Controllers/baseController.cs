using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyUniversity.Models.Help;
using MyUniversity.Models.DTO;

namespace MyUniversity.Controllers
{
    public class baseController : Controller
    {
         /// <summary>
        /// Controller操作后，返回的操纵结果
        /// </summary>
        protected Dictionary<string, object> JsonResult { get; set; }
        protected ResultsModel results { get; set; }

        public baseController()
        {
            JsonResult = new Dictionary<string, object>();
            JsonResult["boo_success"] = false;
            results = new ResultsModel() { success = false, message = "" };
        }
    }
}
