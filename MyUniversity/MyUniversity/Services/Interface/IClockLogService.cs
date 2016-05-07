using MyUniversity.Models;
using MyUniversity.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyUniversity.Services.Interface
{
    public interface IClockLogService
    {
        List<Clocklog> getClockLogsByPlanId(long planId, int groupNumber);

        List<ClockLogModel> selectClockLogModel(List<Clocklog> clockLogs);

        List<Clocklog> getClockLogs();

        bool addClockLog(Clocklog clockLog);
    }
}
