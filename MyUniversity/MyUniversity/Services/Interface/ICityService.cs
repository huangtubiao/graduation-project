﻿using MyUniversity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyUniversity.Services.Interface
{
    public interface ICityService
    {
        List<City> getCityByCityName(string cityName);
    }
}
