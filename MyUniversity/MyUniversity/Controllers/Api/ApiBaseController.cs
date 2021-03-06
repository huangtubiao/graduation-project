﻿using MyUniversity.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MyUniversity.Controllers.Api
{
    public class ApiBaseController : ApiController
    {
        protected ResultsModel results { get; set; }
        public ApiBaseController()
        {
            results = new ResultsModel() { success = false, message = "" };
        }
    }
}
