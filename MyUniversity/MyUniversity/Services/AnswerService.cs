using MyUniversity.Models;
using MyUniversity.Models.Repositories.Interface;
using MyUniversity.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyUniversity.Services
{
    public class AnswerService : IAnswerService
    {
        public IAnswerRepository _answerRepository { get; private set; }

        public AnswerService(IAnswerRepository answerRepository)
        {
            this._answerRepository = answerRepository;
        }

        #region 条件检索
        public int getAnswerNumByQuestionId(long questionId)
        {
            return _answerRepository.Get(o => o.questionId == questionId).ToList().Count();
        }

        public List<Answer> getAnswerByQuestionId(long questionId)
        {
            return _answerRepository.Get(o => o.questionId == questionId).ToList();
        } 
        #endregion

        #region 回答问题
        public bool answerQuestion(Answer answer)
        {
            try
            {
                _answerRepository.Add(answer);
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
