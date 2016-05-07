using MyUniversity.Models;
using MyUniversity.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyUniversity.Services.Interface
{
    public interface IProjectRecordService 
    {
        IQueryable<ProjectRecord> getRecordsByUserId(long userId);

        List<ProjectRecordsModel> selectProjectRecords(IQueryable<ProjectRecord> ProjectRecords);

        ProjectRecord getPRecordById(long pRId);

        bool updateProjectRecord(ProjectRecord projectRecord);
        bool addRrojectRecord(ProjectRecord projectRecord);
    }
}
