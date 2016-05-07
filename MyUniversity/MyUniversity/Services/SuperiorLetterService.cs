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
    public class SuperiorLetterService : ISuperiorLetterService
    {
        public ISuperiorLetterRepository _superiorLetterRepository { get; private set; }

        public SuperiorLetterService(ISuperiorLetterRepository superiorLetterRepository)
        {
            this._superiorLetterRepository = superiorLetterRepository;
        }

        #region 条件检索
        public List<SuperiorLetter> getSuperiorLettersByUserId(long userId)
        {
            return _superiorLetterRepository.Get(o => o.superiorLetterIfReply == true && o.superiorLetterSendUId == userId).ToList();
        }

        public IQueryable<SuperiorLetter> getMSSuperiorLettersByUserId(long userId, string userSchool)
        {
            return _superiorLetterRepository.Get(o => o.superiorLetterIfReply == true && o.superiorLetterReceiveUId == userId && o.User1.userSchool == userSchool).AsQueryable();
        }

        public List<SuperiorLetter> getSuperiorLetterByUserIdUnread(long userId)
        {
            return _superiorLetterRepository.Get(o => o.superiorLetterIfReply == true && o.superiorLetterSendUId == userId && o.superiorLetterIfRead == false).ToList();
        }

        public SuperiorLetter getSuperiorLettersBySuidRuid(long ReceiveUId, long SendUId)
        {
            return _superiorLetterRepository.Get(o => o.superiorLetterSendUId == SendUId && o.superiorLetterReceiveUId == ReceiveUId).FirstOrDefault();
        }

        public SuperiorLetter getSuperiorLettersBySuidRuidC(long ReceiveUId, long SendUId)
        {
            return _superiorLetterRepository.Get(o => o.superiorLetterSendUId == SendUId && o.superiorLetterReceiveUId == ReceiveUId && o.superiorLetterIfComment == 2).FirstOrDefault();
        }

        public SuperiorLetter getSuperiorLettersById(long superiorLetterId)
        {
            return _superiorLetterRepository.Get(o => o.superiorLetterId == superiorLetterId).FirstOrDefault();
        }
        #endregion

        #region 更新方案高手私信
        public bool updateSuperiorLetters(List<SuperiorLetter> superiorLetters)
        {
            try
            {
                foreach (var s in superiorLetters)
                {
                    s.superiorLetterIfRead = true;
                    _superiorLetterRepository.Update(s);
                }
                return true;
            }
            catch
            {
                return false;
            }
        } 

        public bool updateSperiorLetter(SuperiorLetter superiorLetter)
        {
            try
            {
                _superiorLetterRepository.Update(superiorLetter);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region 添加新的方案高手私信
        public bool addSuperiorLetter(SuperiorLetter superiorLetter)
        {
            try
            {
                _superiorLetterRepository.Add(superiorLetter);
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
