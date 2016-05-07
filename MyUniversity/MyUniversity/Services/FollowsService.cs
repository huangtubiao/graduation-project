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
    public class FollowsService : IFollowsService
    {
        public IFollowsRepository _followsRepository { get; private set; }

        public FollowsService(IFollowsRepository followsRepository)
        {
            this._followsRepository = followsRepository;
        }

        #region 条件检索
        public Follows getFollowByUseridFuserid(long userId, long followUserId)
        {
            return _followsRepository.Get(o => o.userId == userId && o.followUserId == followUserId).FirstOrDefault();
        }

        public IQueryable<Follows> getMyFollows(long userId)
        {
            return _followsRepository.Get(o => o.userId == userId).OrderByDescending(o => o.followTime).AsQueryable();
        }

        public IQueryable<Follows> getMySchoolFollows(long userId, string userSchool)
        {
            return _followsRepository.Get(o => o.userId == userId && o.User.userSchool == userSchool).OrderByDescending(o => o.followTime).AsQueryable();
        }
        #endregion

        #region 添加关注
        public bool addNewFollow(Follows f)
        {
            try
            {
                _followsRepository.Add(f);
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
