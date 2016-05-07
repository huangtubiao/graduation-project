using MyUniversity.Models;
using MyUniversity.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyUniversity.Services.Interface
{
    public interface IPlanCommentService
    {
        List<Plancomment> getPlanCommmentsByPlanId(long planId);

        List<PlanCommentModel> selectPlanCommentModels(List<Plancomment> planCommments);

        PlanCommentModel selectPlanCommentModel(Plancomment plancomment, User user);

        bool addPlanComment(Plancomment plancomment);
    }
}
