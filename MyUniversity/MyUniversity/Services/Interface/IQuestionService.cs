using MyUniversity.Models;
using MyUniversity.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyUniversity.Services.Interface
{
    public interface IQuestionService
    {
        bool publishQuestion(Question question, long userId);

        List<Question> getQuestion(int groupNumber);
        List<Question> getHotQuestion(int groupNumber);
        List<Question> getWaitReplyQuestion(int groupNumber);

        void updateNewestReply(string answerContent, long questionId);

        IQueryable<Question> getQuestionById(long questionId);
     
        List<Question> getQuestionByWords(string words, int urlNum);
        List<Question> getHotQuestionByWords(string words, int urlNum);
        List<Question> getWaitReplyQuestionByWords(string words, int urlNum);

        List<string> getSuggestQuestionByWrite(string words);

        int[] getTotalGroups();
        int[] getTotalGroupsByWords(string words);

        List<QuestionModel> selectQuestionData(List<Question> question);

        //方案高手的分享
        IQueryable<Question> getQuestionUserId(long userId);
     }
}
