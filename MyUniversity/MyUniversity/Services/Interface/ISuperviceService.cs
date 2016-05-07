using MyUniversity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyUniversity.Services.Interface
{
    public interface ISuperviceService
    {
        List<Supervice> getMySupervice(int groupNumber, long userId);
        List<Supervice> getMySuperviceByPlanid(long planId);

        Supervice getSuperviceByPlanidUserid(long planId, long userId);

        bool addSupervice(Supervice supervice);
    }
}
