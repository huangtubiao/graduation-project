using MyUniversity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyUniversity.Services.Interface
{
    public interface IAnswerService
    {
        int getAnswerNumByQuestionId(long questionId);

        bool answerQuestion(Answer answer);

        List<Answer> getAnswerByQuestionId(long questionId);
    }
}
