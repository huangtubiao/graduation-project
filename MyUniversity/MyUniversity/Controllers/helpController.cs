using MyUniversity.Models;
using MyUniversity.Models.DTO;
using MyUniversity.Models.Help;
using MyUniversity.Services.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyUniversity.Controllers
{
    public class helpController : baseController
    {
        private loginUser loginUser { get; set; }
        private IQuestionService _questionService { get; set; }
        private IAnswerService _answerService { get; set; }
        private IRecoverAnswerService _recoverAnswerService { get; set; }
        private IUserService _userService { get; set; }

        public helpController(IQuestionService questionService, IAnswerService answerService, IRecoverAnswerService recoverAnswerService,
            IUserService userService)
        {
            _questionService = questionService;
            _answerService = answerService;
            _recoverAnswerService = recoverAnswerService;
            _userService = userService;
            loginUser = Models.Help.loginUser.getLoginUser();
        }

        #region 返回视图页
        public ActionResult index(string group, string num)
        {
            var words = Request.QueryString["words"];
            ViewData["words"] = words;
            int urlGroup = Convert.ToInt32(group);   //是来自那个选项卡
            int urlNum = Convert.ToInt32(num);   //是那一页
            int[] totalGroups = new int[3];
            if (words == "" || words == null)   //如果没有搜索内容，则判断是否有分页
            {
                if (group == null)   //首次加载页面
                {
                    List<Question> Questions = _questionService.getQuestion(1);
                    for (int q = 0; q < Questions.Count; q++)
                    {
                        Questions[q].questionPublishedTime = getQuestionPublishedTime(Questions[q].questionTime);
                    }
                    ViewData["Questions"] = Questions;

                    List<Question> hotQuestion = _questionService.getHotQuestion(1);
                    for (int i = 0; i < hotQuestion.Count; i++)
                    {
                        hotQuestion[i].questionPublishedTime = getQuestionPublishedTime(hotQuestion[i].questionTime);
                    }
                    ViewData["hotQuestion"] = hotQuestion;

                    List<Question> waitReplyQuestion = _questionService.getWaitReplyQuestion(1);
                    for (int j = 0; j < waitReplyQuestion.Count; j++)
                    {
                        waitReplyQuestion[j].questionPublishedTime = getQuestionPublishedTime(waitReplyQuestion[j].questionTime);
                    }
                    ViewData["waitReplyQuestion"] = waitReplyQuestion;

                }
                else
                {
                    if (urlGroup == 0)  //最新
                    {
                        List<Question> Questions = _questionService.getQuestion(urlNum);
                        for (int q = 0; q < Questions.Count; q++)
                        {
                            Questions[q].questionPublishedTime = getQuestionPublishedTime(Questions[q].questionTime);
                        }
                        ViewData["Questions"] = Questions;

                        List<Question> hotQuestion = _questionService.getHotQuestion(1);
                        for (int i = 0; i < hotQuestion.Count; i++)
                        {
                            hotQuestion[i].questionPublishedTime = getQuestionPublishedTime(hotQuestion[i].questionTime);
                        }
                        ViewData["hotQuestion"] = hotQuestion;

                        List<Question> waitReplyQuestion = _questionService.getWaitReplyQuestion(1);
                        for (int j = 0; j < waitReplyQuestion.Count; j++)
                        {
                            waitReplyQuestion[j].questionPublishedTime = getQuestionPublishedTime(waitReplyQuestion[j].questionTime);
                        }
                        ViewData["waitReplyQuestion"] = waitReplyQuestion;
                    }
                    if (urlGroup == 1)  //热门
                    {
                        List<Question> Questions = _questionService.getQuestion(1);
                        for (int q = 0; q < Questions.Count; q++)
                        {
                            Questions[q].questionPublishedTime = getQuestionPublishedTime(Questions[q].questionTime);
                        }
                        ViewData["Questions"] = Questions;

                        List<Question> hotQuestion = _questionService.getHotQuestion(urlNum);
                        for (int i = 0; i < hotQuestion.Count; i++)
                        {
                            hotQuestion[i].questionPublishedTime = getQuestionPublishedTime(hotQuestion[i].questionTime);
                        }
                        ViewData["hotQuestion"] = hotQuestion;

                        List<Question> waitReplyQuestion = _questionService.getWaitReplyQuestion(1);
                        for (int j = 0; j < waitReplyQuestion.Count; j++)
                        {
                            waitReplyQuestion[j].questionPublishedTime = getQuestionPublishedTime(waitReplyQuestion[j].questionTime);
                        }
                        ViewData["waitReplyQuestion"] = waitReplyQuestion;
                    }
                    else if (urlGroup == 2)   //等待回复
                    {
                        List<Question> Questions = _questionService.getQuestion(1);
                        for (int q = 0; q < Questions.Count; q++)
                        {
                            Questions[q].questionPublishedTime = getQuestionPublishedTime(Questions[q].questionTime);
                        }
                        ViewData["Questions"] = Questions;

                        List<Question> hotQuestion = _questionService.getHotQuestion(1);
                        for (int i = 0; i < hotQuestion.Count; i++)
                        {
                            hotQuestion[i].questionPublishedTime = getQuestionPublishedTime(hotQuestion[i].questionTime);
                        }
                        ViewData["hotQuestion"] = hotQuestion;

                        List<Question> waitReplyQuestion = _questionService.getWaitReplyQuestion(urlNum);
                        for (int j = 0; j < waitReplyQuestion.Count; j++)
                        {
                            waitReplyQuestion[j].questionPublishedTime = getQuestionPublishedTime(waitReplyQuestion[j].questionTime);
                        }
                        ViewData["waitReplyQuestion"] = waitReplyQuestion;
                    }
                }
                totalGroups = _questionService.getTotalGroups(); //查询每个选项有多少页，每一页5条数据；
                ViewData["totalGroups"] = totalGroups[0];
                ViewData["totalSecondGroups"] = totalGroups[1];
                ViewData["totalThirdGroups"] = totalGroups[2];
            }
            else
            {
                if (group == null)   //只有搜索，没有目标页面
                {
                    List<Question> Questions = _questionService.getQuestionByWords(words, 1);
                    for (int q = 0; q < Questions.Count; q++)
                    {
                        Questions[q].questionPublishedTime = getQuestionPublishedTime(Questions[q].questionTime);
                    }
                    ViewData["Questions"] = Questions;

                    List<Question> hotQuestion = _questionService.getHotQuestionByWords(words, 1);
                    for (int i = 0; i < hotQuestion.Count; i++)
                    {
                        hotQuestion[i].questionPublishedTime = getQuestionPublishedTime(hotQuestion[i].questionTime);
                    }
                    ViewData["hotQuestion"] = hotQuestion;

                    List<Question> waitReplyQuestion = _questionService.getWaitReplyQuestionByWords(words, 1);
                    for (int j = 0; j < waitReplyQuestion.Count; j++)
                    {
                        waitReplyQuestion[j].questionPublishedTime = getQuestionPublishedTime(waitReplyQuestion[j].questionTime);
                    }
                    ViewData["waitReplyQuestion"] = waitReplyQuestion;
                }
                else  //查找搜索的内容和目标页
                {
                    if (urlGroup == 0)  //最新
                    {
                        List<Question> Questions = _questionService.getQuestionByWords(words, urlNum);
                        for (int q = 0; q < Questions.Count; q++)
                        {
                            Questions[q].questionPublishedTime = getQuestionPublishedTime(Questions[q].questionTime);
                        }
                        ViewData["Questions"] = Questions;

                        List<Question> hotQuestion = _questionService.getHotQuestionByWords(words, 1);
                        for (int i = 0; i < hotQuestion.Count; i++)
                        {
                            hotQuestion[i].questionPublishedTime = getQuestionPublishedTime(hotQuestion[i].questionTime);
                        }
                        ViewData["hotQuestion"] = hotQuestion;

                        List<Question> waitReplyQuestion = _questionService.getWaitReplyQuestionByWords(words, 1);
                        for (int j = 0; j < waitReplyQuestion.Count; j++)
                        {
                            waitReplyQuestion[j].questionPublishedTime = getQuestionPublishedTime(waitReplyQuestion[j].questionTime);
                        }
                        ViewData["waitReplyQuestion"] = waitReplyQuestion;
                    }
                    if (urlGroup == 1)  //热门
                    {
                        List<Question> Questions = _questionService.getQuestionByWords(words, 1);
                        for (int q = 0; q < Questions.Count; q++)
                        {
                            Questions[q].questionPublishedTime = getQuestionPublishedTime(Questions[q].questionTime);
                        }
                        ViewData["Questions"] = Questions;

                        List<Question> hotQuestion = _questionService.getHotQuestionByWords(words, urlNum);
                        for (int i = 0; i < hotQuestion.Count; i++)
                        {
                            hotQuestion[i].questionPublishedTime = getQuestionPublishedTime(hotQuestion[i].questionTime);
                        }
                        ViewData["hotQuestion"] = hotQuestion;

                        List<Question> waitReplyQuestion = _questionService.getWaitReplyQuestionByWords(words, 1);
                        for (int j = 0; j < waitReplyQuestion.Count; j++)
                        {
                            waitReplyQuestion[j].questionPublishedTime = getQuestionPublishedTime(waitReplyQuestion[j].questionTime);
                        }
                        ViewData["waitReplyQuestion"] = waitReplyQuestion;
                    }
                    else if (urlGroup == 2)   //等待回复
                    {
                        List<Question> Questions = _questionService.getQuestionByWords(words, 1);
                        for (int q = 0; q < Questions.Count; q++)
                        {
                            Questions[q].questionPublishedTime = getQuestionPublishedTime(Questions[q].questionTime);
                        }
                        ViewData["Questions"] = Questions;

                        List<Question> hotQuestion = _questionService.getHotQuestionByWords(words, 1);
                        for (int i = 0; i < hotQuestion.Count; i++)
                        {
                            hotQuestion[i].questionPublishedTime = getQuestionPublishedTime(hotQuestion[i].questionTime);
                        }
                        ViewData["hotQuestion"] = hotQuestion;

                        List<Question> waitReplyQuestion = _questionService.getWaitReplyQuestionByWords(words, urlNum);
                        for (int j = 0; j < waitReplyQuestion.Count; j++)
                        {
                            waitReplyQuestion[j].questionPublishedTime = getQuestionPublishedTime(waitReplyQuestion[j].questionTime);
                        }
                        ViewData["waitReplyQuestion"] = waitReplyQuestion;
                    }
                }
                totalGroups = _questionService.getTotalGroupsByWords(words); //查询每个选项有多少页，每一页5条数据；
                ViewData["totalGroups"] = totalGroups[0];
                ViewData["totalSecondGroups"] = totalGroups[1];
                ViewData["totalThirdGroups"] = totalGroups[2];
            }
            return View();
        }

        public ActionResult publish()
        {
            return View();
        }

        public ActionResult savesuccess()
        {
            return View();
        }

        //问题详情
        public ActionResult detail(long id)
        {
            try
            {
                IQueryable<Question> questionDetail = _questionService.getQuestionById(id);
                foreach (var q in questionDetail)
                {
                    q.questionPublishedTime = getQuestionPublishedTime(q.questionTime);
                    q.questionReplyNum = _answerService.getAnswerNumByQuestionId(q.questionId);
                    ViewData["questionDetail"] = questionDetail;
                    List<Answer> questionComments = _answerService.getAnswerByQuestionId(q.questionId);
                    for (int i = 0; i < questionComments.Count; i++)
                    {
                        questionComments[i].answerPublishedTime = getQuestionPublishedTime(questionComments[i].answerTime);
                    }
                    ViewData["questionComments"] = questionComments;

                    ArrayList RecoverAnswerList = new ArrayList();
                    for (int r = 0; r < questionComments.Count; r++)
                    {
                        List<RecoverAnswer> RecoverAnswer = _recoverAnswerService.getRecoverByAnswerId(questionComments[r].answerId);
                        for (int t = 0; t < RecoverAnswer.Count; t++)
                        {
                            RecoverAnswer[t].reAnswerPublishedTime = getQuestionPublishedTime(RecoverAnswer[t].reAnswerTime);
                        }
                        RecoverAnswerList.Add(RecoverAnswer);
                    }

                    for (int r = 0; r < RecoverAnswerList.Count; r++)
                    {
                        string p = Convert.ToString(r);
                        ViewData[p] = RecoverAnswerList[r];
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 发表问题
        [HttpPost]
        public ActionResult publish(Question question)
        {
            if (System.Web.HttpContext.Current.Session["loginUser"] == null)
            {
                return RedirectToAction("register", "account");
            }
            else
            {
                if (_questionService.publishQuestion(question, loginUser.userId) == true)
                {
                    JsonResult["boo_success"] = true;
                    return Json(JsonResult);
                }
                else
                {
                    JsonResult["boo_success"] = false;
                    return Json(JsonResult);
                }
            }
        }
        #endregion

        #region 回答问题
        public ActionResult answer(Answer answer)
        {
            if (System.Web.HttpContext.Current.Session["loginUser"] == null)
            {
                JsonResult["boo_success"] = "pleaseLogin";
                return Json(JsonResult);
            }
            else
            {
                answer.answerTime = DateTime.Now;
                answer.userId = loginUser.userId;
                if (_answerService.answerQuestion(answer) == true)
                {

                    _questionService.updateNewestReply(answer.answerContent, answer.questionId);
                    JsonResult["boo_success"] = true;
                    return Json(JsonResult);
                }
                else
                {
                    JsonResult["boo_success"] = false;
                    return Json(JsonResult);
                }
            }
        }
        #endregion

        #region 对帮助者的评论内容进行回复
        public ActionResult recoverAnswer(RecoverAnswer recoverAnswer, string recoverWho)
        {
            if (System.Web.HttpContext.Current.Session["loginUser"] == null)
            {
                JsonResult["boo_success"] = "pleaseLogin";
                return Json(JsonResult);
            }
            else
            {
                recoverAnswer.reAnswerTime = DateTime.Now;
                recoverAnswer.reAnswerUserId = loginUser.userId;
                if (recoverWho == null)
                {
                    if (_recoverAnswerService.recoverAnswer(recoverAnswer) == true)
                    {
                        User user = _userService.getUserById(recoverAnswer.reAnswerUserId);
                        JsonResult["reAnswerUserImg"] = user.userImg;
                        JsonResult["reAnswerUserName"] = user.userName;
                        JsonResult["answerUserName"] = "";
                        JsonResult["boo_success"] = true;
                        return Json(JsonResult);
                    }
                    else
                    {
                        JsonResult["boo_success"] = false;
                        return Json(JsonResult);
                    }
                }
                else
                {
                    User u = _userService.getUserById(Convert.ToInt64(recoverAnswer.questionUserId));
                    recoverAnswer.answerUserName = u.userName;
                    recoverAnswer.reAnswerContent = recoverAnswer.reAnswerContent.Replace(recoverWho, "");
                    if (_recoverAnswerService.recoverAnswer(recoverAnswer) == true)
                    {
                        User user = _userService.getUserById(recoverAnswer.reAnswerUserId);
                        JsonResult["reAnswerUserImg"] = user.userImg;
                        JsonResult["reAnswerUserName"] = user.userName;
                        JsonResult["answerUserName"] = recoverAnswer.answerUserName;
                        JsonResult["boo_success"] = true;
                        return Json(JsonResult);
                    }
                    else
                    {
                        JsonResult["boo_success"] = false;
                        return Json(JsonResult);
                    }
                }
            }
        }
        #endregion

        #region 计算问题发布了多少时间
        public string getQuestionPublishedTime(DateTime questionTime)
        {
            int publishedTimes;
            string questionPublishedTime = null;
            double totalSecond = (DateTime.Now - questionTime).TotalSeconds;
            int _totalSecond = Convert.ToInt32(totalSecond);
            if (_totalSecond >= 86400)
            {
                publishedTimes = Convert.ToInt32(_totalSecond / 86400);
                questionPublishedTime = publishedTimes + "天前";
            }
            if (_totalSecond >= 3600 && _totalSecond < 86400)
            {
                publishedTimes = Convert.ToInt32(_totalSecond / 3600);
                questionPublishedTime = publishedTimes + "小时前";
            }
            if (_totalSecond >= 60 && _totalSecond < 3600)
            {
                publishedTimes = Convert.ToInt32(_totalSecond / 60);
                questionPublishedTime = publishedTimes + "分钟前";
            }
            else if (_totalSecond >= 0 && _totalSecond < 60)
            {
                questionPublishedTime = _totalSecond + "秒前";
            }
            return questionPublishedTime;
        }
        #endregion

        #region 搜索智能提醒
        public ActionResult searchSuggest()
        {
            var words = Request.QueryString["words"];
            if (words == "")
            {
                var JsonResults = "";
                return Json(JsonResults, JsonRequestBehavior.AllowGet);
            }
            else
            {
                List<string> result = _questionService.getSuggestQuestionByWrite(words);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        public ActionResult ifLogin()
        {
            if (System.Web.HttpContext.Current.Session["loginUser"] == null)
            {
                JsonResult["boo_success"] = false;
            }
            else
            {
                JsonResult["boo_success"] = true;
            }
            return Json(JsonResult);
        }
    }
}
