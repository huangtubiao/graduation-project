using MyUniversity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyUniversity.Services.Interface
{
    public interface IRecentVisitorService
    {
        List<RecentVisitor> getRecentVisitorsByUserId(long userId);

        bool addRecentVisitor(RecentVisitor newVisitor);
    }
}
