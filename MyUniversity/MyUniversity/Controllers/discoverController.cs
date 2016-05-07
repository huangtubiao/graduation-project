using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyUniversity.Controllers
{
    public class discoverController : Controller
    {
        //
        // GET: /find/

        public ActionResult index()
        {
            return Redirect("~/disc/index.html");
        }

    }
}
