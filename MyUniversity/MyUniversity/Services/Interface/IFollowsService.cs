using MyUniversity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyUniversity.Services.Interface
{
    public interface IFollowsService
    {
        Follows getFollowByUseridFuserid(long userId, long followUserId);

        IQueryable<Follows> getMyFollows(long userId);
        IQueryable<Follows> getMySchoolFollows(long userId, string userSchool);

        bool addNewFollow(Follows f);
    }
}
