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
    public class QuestionService : IQuestionService
    {
        UniversityEntities db = new UniversityEntities();

        public IQuestionRepository _questionRepository { get; private set; }

        public QuestionService(IQuestionRepository questionRepository)
        {
            this._questionRepository = questionRepository;
        }

        #region 发表问题
        public bool publishQuestion(Question question, long userId)
        {
            question.questionTime = DateTime.Now;
            question.userId = userId;
            try
            {
                _questionRepository.Add(question);
                return true;
            }
            catch
            {
                return false;
            }
        } 
        #endregion

        #region 获取最新的问题
        public List<Question> getQuestion(int groupNumber)
        {
            return _questionRepository.Get(null, groupNumber, 5, p => p.questionTime, false).ToList();
        } 
        #endregion

        #region 获取热门的问题
        public List<Question> getHotQuestion(int groupNumber)
        {
            return _questionRepository.Get(null, groupNumber, 5, p => p.questionReplyNum, false).ToList();
        } 
        #endregion

        #region 获取等待回复的问题
        public List<Question> getWaitReplyQuestion(int groupNumber)
        {
            return _questionRepository.Get(p => p.newReplyUserId == null, groupNumber, 5, p => p.questionTime, false).ToList();
        } 
        #endregion

        #region 获取分页总数量
        public int[] getTotalGroups()
        {
            int[] totalGroups = new int[3];
            totalGroups[0] = _questionRepository.Count(null);
            totalGroups[1] = _questionRepository.Count(null);
            totalGroups[2] = _questionRepository.Count(p => p.newReplyUserId == null);
            return totalGroups;
        }
        #endregion

        #region 根据搜索条件获取分页总数量
        public int[] getTotalGroupsByWords(string words)
        {
            int[] totalGroups = new int[3];
            totalGroups[0] = _questionRepository.Count(o => o.questionTitle.Contains(words));
            totalGroups[1] = _questionRepository.Count(o => o.questionTitle.Contains(words));
            totalGroups[2] = _questionRepository.Count(o => o.newReplyUserId == null && o.questionTitle.Contains(words));
            return totalGroups;
        } 
        #endregion

        public void updateNewestReply(string answerContent, long questionId)
        {
            Question question = _questionRepository.Get(o => o.questionId == questionId).SingleOrDefault();
            question.questionNewestReply = answerContent;
            _questionRepository.Update(question);
        }

        #region 条件检索
        public IQueryable<Question> getQuestionById(long questionId)
        {
            return _questionRepository.Get(o => o.questionId == questionId).AsQueryable();
        }

        public List<Question> getQuestionByWrite(getDataModel DataModel)
        {
            return _questionRepository.Get(p => p.questionTitle.Contains(DataModel.searchWrite), DataModel.groupNumber, 5, p => p.questionTime, false).ToList();
        }

        public List<Question> getHotQuestionByWrite(getDataModel DataModel)
        {
            return _questionRepository.Get(o => o.questionTitle.Contains(DataModel.searchWrite), DataModel.groupNumber, 5, o => o.questionReplyNum, false).ToList();
        }

        public List<Question> getWaitReplyQuestionByWrite(getDataModel DataModel)
        {
            return _questionRepository.Get(o => o.questionTitle.Contains(DataModel.searchWrite) && o.newReplyUserId == null, DataModel.groupNumber, 5, o => o.questionTime, false).ToList();
        }

        public List<string> getSuggestQuestionByWrite(string words)
        {
            return db.Question.Where(o => o.questionTitle.Contains(words)).Take(5).Select(o => o.questionTitle).ToList();
        }

        public List<Question> getQuestionByWords(string words, int urlNum)
        {
            return _questionRepository.Get(o => o.questionTitle.Contains(words), urlNum, 5, o => o.questionTime, false).ToList();
        }

        public List<Question> getHotQuestionByWords(string words, int urlNum)
        {
            return _questionRepository.Get(o => o.questionTitle.Contains(words), urlNum, 5, o => o.questionReplyNum, false).ToList();
        }

        public List<Question> getWaitReplyQuestionByWords(string words, int urlNum)
        {
            return _questionRepository.Get(o => o.questionTitle.Contains(words) && o.newReplyUserId == null, urlNum, 5, o => o.questionTime, false).ToList();
        }

        //方案高手分享
        public IQueryable<Question> getQuestionUserId(long userId)
        {
            return _questionRepository.Get(o => o.userId == userId, 1, 10, o => o.questionTime, false).AsQueryable();
        }
        #endregion

        #region 筛选出问题必要信息
        public List<QuestionModel> selectQuestionData(List<Question> question)
        {
            List<QuestionModel> result = new List<QuestionModel>();
            foreach (var s in question)
            {
                if (s.newReplyUserName == null) {
                    s.newReplyUserName = "";
                }
                result.Add(new QuestionModel
                {
                    userId = s.User.userId,
                    userImg = s.User.userImg,
                    userName = s.User.userName,
                    userSex = s.User.userSex,
                    questionReplyNum = Convert.ToInt32(s.questionReplyNum),
                    questionViewNum = Convert.ToInt32(s.questionViewNum),
                    questionTitle = s.questionTitle,
                    questionId = s.questionId,
                    newReplyUserId = Convert.ToInt32(s.newReplyUserId),
                    newReplyUserImg = Convert.ToInt32(s.newReplyUserImg),
                    newReplyUserName = s.newReplyUserName,
                    questionNewestReply = s.questionNewestReply,
                    userDepart = s.User.userDepart,
                    questionPublishedTime = s.questionPublishedTime
                });
            }
            return result;
        } 
        #endregion
    }
}
