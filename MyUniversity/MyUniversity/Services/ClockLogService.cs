using MyUniversity.Models;
using MyUniversity.Models.DTO;
using MyUniversity.Models.Repositories.Interface;
using MyUniversity.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyUniversity.Services
{
    public class ClockLogService : IClockLogService
    {
        public IClockLogRepository _clockLogRepository { get; private set; }

        public ClockLogService(IClockLogRepository clockLogRepository)
        {
            this._clockLogRepository = clockLogRepository;
        }

        #region 条件检索
        public List<Clocklog> getClockLogsByPlanId(long planId, int groupNumber)
        {
            return _clockLogRepository.Get(o => o.planId == planId, groupNumber, 3, o => o.clocklogTime, false).ToList();
        } 
        #endregion

        #region 提出计划打卡日志的必要信息
        public List<ClockLogModel> selectClockLogModel(List<Clocklog> clockLogs)
        {
            List<ClockLogModel> clockLogModel = new List<ClockLogModel>();
            foreach (var c in clockLogs)
            {
                clockLogModel.Add(new ClockLogModel {
                    clockLogContent = c.clocklogContent,
                    clockLogYearMonth = c.clocklogTime.GetDateTimeFormats('y')[0].ToString(),  
                    clockLogDay = c.clocklogTime.Day
                });
            }
            return clockLogModel;
        } 
        #endregion

        #region 获取打卡日志》按照时间排序
        public List<Clocklog> getClockLogs()
        {
            return _clockLogRepository.Get(null, 1, 4, o => o.clocklogTime, false).ToList();
        } 
        #endregion

        #region 增加新的打卡日志
        public bool addClockLog(Clocklog clockLog)
        {
            try
            {
                _clockLogRepository.Add(clockLog);
                return true;
            }
            catch
            {
                return false;
            }
        } 
        #endregion
    }
}
