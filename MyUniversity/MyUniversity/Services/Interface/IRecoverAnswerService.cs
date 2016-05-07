using MyUniversity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyUniversity.Services.Interface
{
    public interface IRecoverAnswerService 
    {
        List<RecoverAnswer> getRecoverByAnswerId(long answerId);

        bool recoverAnswer(RecoverAnswer recoverAnswer);

        //RecoverAnswer getRecoverAnswerByTime(DateTime reAnswerTime);
    }
}
