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
    public class RecoverAnswerService : IRecoverAnswerService
    {
        public IRecoverAnswerRepository _recoverAnswerRepository { get; private set; }

        public RecoverAnswerService(IRecoverAnswerRepository recoverAnswerRepository)
        {
            this._recoverAnswerRepository = recoverAnswerRepository;
        }

        #region 条件检索
        public List<RecoverAnswer> getRecoverByAnswerId(long answerId)
        {
            return _recoverAnswerRepository.Get(o => o.answerId == answerId).ToList();
        }

        //public RecoverAnswer getRecoverAnswerByTime(DateTime reAnswerTime)
        //{
        //    return _recoverAnswerRepository.Get(o => o.reAnswerTime == reAnswerTime).SingleOrDefault();
        //}
        #endregion

        #region 对帮助者的评论内容进行回复
        public bool recoverAnswer(RecoverAnswer recoverAnswer)
        {
            try
            {
                _recoverAnswerRepository.Add(recoverAnswer);
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
