using MyUniversity.Models;
using MyUniversity.Models.DTO;
using MyUniversity.Models.Help;
using MyUniversity.Models.Repositories.Interface;
using MyUniversity.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyUniversity.Services
{
    public class UserService : IUserService
    {
        UniversityEntities db = new UniversityEntities();

        private loginUser loginUser { get; set; }
        public IUserRepository _userRepository { get; private set; }

        public UserService(IUserRepository userRepository)
        {
            loginUser = Models.Help.loginUser.getLoginUser();
            this._userRepository = userRepository;
        }

        #region 注册
        public bool register(User user)
        {
            //user.user_registerTime = DateTime.Now;
            try
            {
                _userRepository.Add(user);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region 登录

        //用户是否存在,存在返回true
        public bool isNotUser(LoginModel user)
        {
            User userIfExit = getUserByAccount(user.userAccount);
            if (userIfExit != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool login(LoginModel user)
        {
            User _user = getUserByAccountAndPaw(user);
            if (_user == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion

        #region 保存用户登录信息
        public void setLoginUser(string userAccount, long userId, string userSchool)
        {
            loginUser loginUser = new loginUser
            {
                userId = userId,
                userAccount = userAccount,
                userSchool = userSchool
            };
            HttpContext.Current.Session["loginUser"] = loginUser;
        }
        #endregion

        #region 获取校内的方案高手
        public List<User> getMySchoolSuperiors(int groupNumber, string userDepartment, string userSchool)
        {
            if (userDepartment == "全部院系")
            {
                return _userRepository.Get(p => p.userSchool == userSchool && p.userMerit != null, groupNumber, 10, p => p.userRank, false).ToList();
            }
            else
            {
                return _userRepository.Get(p => p.userDepart == userDepartment && p.userSchool == userSchool && p.userMerit != null, groupNumber, 10, p => p.userRank, false).ToList();
            }
        }
        #endregion

        #region 获取全部高校的方案高手
        public IQueryable<User> getAllSuperiors(int groupNumber)
        {
            return _userRepository.Get(o =>o.userMerit != null, groupNumber, 10, p => p.userRank, false).AsQueryable();
        }
        #endregion

        //#region 获取最热心的大学女同学
        //public List<User> getMostLoveGirlPeoples(int groupNumber, string userDepartment)
        //{
        //    if (userDepartment == "全部院系")
        //    {
        //        return _userRepository.Get(p => p.userSex == "女", groupNumber, 10, p => p.userRank, false).ToList();
        //    }
        //    else
        //    {
        //        return _userRepository.Get(p => p.userSex == "女" && p.userDepart == userDepartment, groupNumber, 10, p => p.userRank, false).ToList();
        //    }
        //}
        //#endregion

        //#region 获取最热心的大学男同学
        //public List<User> getMostLoveBoyPeoples(int groupNumber, string userDepartment)
        //{
        //    if (userDepartment == "全部院系")
        //    {
        //        return _userRepository.Get(p => p.userSex == "男", groupNumber, 10, p => p.userRank, false).ToList();
        //    }
        //    else
        //    {
        //        return _userRepository.Get(p => p.userSex == "男" && p.userDepart == userDepartment, groupNumber, 10, p => p.userRank, false).ToList();
        //    }
        //}
        //#endregion

        #region 条件检索
        public User getUserByAccountAndPaw(LoginModel user)
        {
            return _userRepository.Get(o => o.userAccount == user.userAccount && o.userPaw == user.userPaw).FirstOrDefault();
        }

        public User getUserByAccount(string userAccount)
        {
            return _userRepository.Get(o => o.userAccount == userAccount).FirstOrDefault();
        }

        public User getUserById(long userId)
        {
            return _userRepository.Get(o => o.userId == userId).SingleOrDefault();
        }

        public List<User> getMySchoolSuperiorsByDepart(string userDepart)
        {
            if (userDepart == "全部院系")
            {
                return _userRepository.Get(o => o.userSchool == loginUser.userSchool, 1, 10, o => o.userRank, false).ToList();
            }
            else
            {
                return _userRepository.Get(o => o.userDepart == userDepart && o.userSchool == loginUser.userSchool, 1, 10, o => o.userRank, false).ToList();
            }
        }

        public List<User> getAllSuperiorsByDepart(string userDepart)
        {
            if (userDepart == "全部院系")
            {
                return _userRepository.Get(null, 1, 10, o => o.userRank, false).ToList();
            }
            else
            {
                return _userRepository.Get(o => o.userDepart == userDepart, 1, 10, o => o.userRank, false).ToList();
            }
        }

        //public List<User> getLoveBoyPeoplesByDepart(string userDepart)
        //{
        //    if (userDepart == "全部院系")
        //    {
        //        return _userRepository.Get(o => o.userSex == "男", 1, 10, o => o.userRank, false).ToList();
        //    }
        //    else
        //    {
        //        return _userRepository.Get(o => o.userDepart == userDepart && o.userSex == "男", 1, 10, o => o.userRank, false).ToList();
        //    }
        //}

        public List<string> getSuperiorByWrite(string searchText)
        {

            return db.User.Where(o => o.userMerit .Contains(searchText)).Take(6).Select(o => o.userMerit).ToList();
        }

        public List<User> getMySchoolSuperiorsByWrite(string searchWrite)
        {
            return _userRepository.Get(p => p.userMerit.Contains(searchWrite) && p.userSchool == loginUser.userSchool, 1, 10, p => p.userRank, false).ToList();
        }

        public IQueryable<User> getAllSuperiorsByWrite(string searchWrite)
        {
            return _userRepository.Get(p => p.userMerit.Contains(searchWrite), 1, 10, p => p.userRank, false).AsQueryable();
        }
        
        public int getMSSuperiorsGroupsByDepart(string userDepart)
        {
            if (userDepart == "全部院系")
            {
                return _userRepository.Count(o => o.userSchool == loginUser.userSchool);
            }
            else
            {
                return _userRepository.Count(o => o.userDepart == userDepart && o.userSchool == loginUser.userSchool);
            }
        }

        public int getTotalGroupsByDepart(string userDepart)
        {
            if (userDepart == "全部院系")
            {
                return _userRepository.Count(null);
            }
            else
            {
                return _userRepository.Count(o => o.userDepart == userDepart);
            }
        }

        public int getTotalGroupsByWrite(string searchWrite)
        {
            return _userRepository.Count(o => o.userMerit.Contains(searchWrite));
        }

        public int getMySuperiorGroupsByWrite(string searchWrite)
        {
            return _userRepository.Count(o => o.userMerit.Contains(searchWrite) && o.userSchool == loginUser.userSchool);
        }

        public int getMSSuperiorsGroupsByDepartWrite(string userDepart, string searchWrite)
        {
            if (userDepart == "全部院系")
            {
                return _userRepository.Count(o => o.userMerit.Contains(searchWrite) && o.userSchool == loginUser.userSchool);
            }
            else
            {
                return _userRepository.Count(o => o.userDepart == userDepart && o.userMerit.Contains(searchWrite) && o.userSchool == loginUser.userSchool);
            }
        }

        public int getTotalGroupsByDepartWrite(string userDepart, string searchWrite)
        {
            if (userDepart == "全部院系")
            {
                return _userRepository.Count(o => o.userMerit.Contains(searchWrite));
            }
            else
            {
                return _userRepository.Count(o => o.userDepart == userDepart && o.userMerit.Contains(searchWrite));
            }
        }

        public int getTotalGroupsBySchool(string userSchool)
        {
            return _userRepository.Count(o => o.userSchool == userSchool);
        }

        public int getTotalGroupsBySchoolDepart(string userSchool, string userDepart)
        {
            return _userRepository.Count(o => o.userSchool == userSchool && o.userDepart == userDepart);
        }

        public int getTotalGroupsBySchoolWrite(string searchWrite, string userSchool)
        {
            return _userRepository.Count(o => o.userSchool == userSchool && o.userMerit.Contains(searchWrite));
        }

        public int getTotalGroupsByWriteSchoolDepart(string searchWrite, string userSchool, string userDepart)
        {
            return _userRepository.Count(o => o.userMerit.Contains(searchWrite) && o.userSchool == userSchool && o.userDepart == userDepart);
        }

        public List<User> getMySchoolSuperiorsByGetData(getDataModel getDataModel)
        {
            if (getDataModel.userDepartment == "全部院系")
            {
                return _userRepository.Get(p => p.userMerit.Contains(getDataModel.searchWrite) && p.userSchool == loginUser.userSchool, getDataModel.groupNumber, 10, p => p.userRank, false).ToList();
            }
            else
            {
                return _userRepository.Get(p => p.userMerit.Contains(getDataModel.searchWrite) && p.userDepart == getDataModel.userDepartment && p.userSchool == loginUser.userSchool, getDataModel.groupNumber, 10, p => p.userRank, false).ToList();
            }
        }

        public List<User> getAllSuperiorsByGetData(getDataModel getDataModel)
        {
            return _userRepository.Get(p => p.userMerit.Contains(getDataModel.searchWrite), getDataModel.groupNumber, 10, p => p.userRank, false).ToList();
        }

        //public List<User> getMostLoveBoyPeoplesByGetData(getDataModel getDataModel)
        //{
        //    if (getDataModel.userDepartment == "全部院系")
        //    {
        //        return _userRepository.Get(p => p.userMerit.Contains(getDataModel.searchWrite) && p.userSex == "男", getDataModel.groupNumber, 10, p => p.userRank, false).ToList();
        //    }
        //    else
        //    {
        //        return _userRepository.Get(p => p.userMerit.Contains(getDataModel.searchWrite) && p.userDepart == getDataModel.userDepartment && p.userSex == "男", getDataModel.groupNumber, 10, p => p.userRank, false).ToList();
        //    }

        //}

        public List<User> getMySchoolSuperiorsByDepartWrite(string userDepart, string searchWrite)
        {
            if (userDepart == "全部院系")
            {
                return _userRepository.Get(o => o.userMerit.Contains(searchWrite) && o.userSchool == loginUser.userSchool, 1, 10, o => o.userRank, false).ToList();
            }
            else
            {
                return _userRepository.Get(o => o.userDepart == userDepart && o.userMerit.Contains(searchWrite) && o.userSchool == loginUser.userSchool, 1, 10, o => o.userRank, false).ToList();
            }
        }

        public List<User> getAllSuperiorsByDepartWrite(string userDepart, string searchWrite)
        {
            if (userDepart == "全部院系")
            {
                return _userRepository.Get(o => o.userMerit.Contains(searchWrite), 1, 10, o => o.userRank, false).ToList();
            }
            else
            {
                return _userRepository.Get(o => o.userDepart == userDepart && o.userMerit.Contains(searchWrite), 1, 10, o => o.userRank, false).ToList();
            }
        }

        //public List<User> getLoveBoyPeoplesByDepartWrite(string userDepart, string searchWrite)
        //{
        //    if (userDepart == "全部院系")
        //    {
        //        return _userRepository.Get(o => o.userMerit.Contains(searchWrite) && o.userSex == "男", 1, 10, o => o.userRank, false).ToList();
        //    }
        //    else
        //    {
        //        return _userRepository.Get(o => o.userDepart == userDepart && o.userMerit.Contains(searchWrite) && o.userSex == "男", 1, 10, o => o.userRank, false).ToList();
        //    }
        //}

        public IQueryable<User> getIUserById(long userId)
        {
            return _userRepository.Get(o => o.userId == userId).AsQueryable();
        }

        public List<User> getAllSuperiorsBySchool(string userSchool)
        {
            return _userRepository.Get(o => o.userSchool == userSchool, 1, 10, o => o.userRank, false).ToList();
        }

        public List<User> getAllSuperiorsBySchoolDepart(string userSchool, string userDepartment)
        {
            return _userRepository.Get(o => o.userSchool == userSchool && o.userDepart == userDepartment, 1, 10, o => o.userRank, false).ToList();
        }

        public List<User> getAllSuperiorsByWriteDepart(string searchWrite, string userDepartment)
        {
            return _userRepository.Get(o => o.userMerit.Contains(searchWrite) && o.userDepart == userDepartment, 1, 10, o => o.userRank, false).ToList();
        }

        public List<User> getAllSuperiorsByWriteSchool(string searchWrite, string userSchool)
        {
            return _userRepository.Get(o => o.userMerit.Contains(searchWrite) && o.userSchool == userSchool, 1, 10, o => o.userRank, false).ToList();
        }

        public List<User> getAllSuperiorsByWriteSchoolDepart(string searchWrite, string userSchool, string userDepartment)
        {
            return _userRepository.Get(o => o.userMerit.Contains(searchWrite) && o.userSchool == userSchool && o.userDepart == userDepartment, 1, 10, o => o.userRank, false).ToList();
        }
        #endregion

        #region 获取分页总数量
        public int getTotalGroups(string userSchool)
        {
            if (userSchool != null)
            {
                return _userRepository.Count(p => p.userSchool == userSchool);  //校内 
            }
            else
            {
                return _userRepository.Count(null); //全部
            }
        }
        #endregion

        #region 获取已经完成计划的人
        public List<User> getFinishPlanUser()
        {
            return _userRepository.Get(null, 1, 5, o => o.planFinishNum, false).ToList();
        }
        #endregion

        #region 更新用户信息
        public bool updateUser(User user)
        {
            try
            {
                _userRepository.Update(user);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region 获取获得经验值的方案高手
        public List<User> getWeekExpUser()
        {
            return _userRepository.Get(o => o.userWeekExps != null).ToList();
        }
        #endregion

        #region 实现算法
        #region 筛选出一周方案高手榜
        public List<SuperiorModel> selectWeekExpUser(List<User> user)
        {
            List<SuperiorModel> s = new List<SuperiorModel>();
            foreach (var u in user)
            {
                DateTime d = Convert.ToDateTime(u.userWeekExpsStartTime);
                if ((DateTime.Now - d).TotalDays <= 70)
                {
                    s.Add(new SuperiorModel
                    {
                        userId = u.userId,
                        userName = u.userName,
                        userImg = u.userImg,
                        userMerit = u.userMerit,
                        userRank = Convert.ToInt64(u.userRank),
                        userSex = u.userSex,
                        userWeekExps = Convert.ToInt32(u.userWeekExps)
                    });
                }
            }
            return s;
        }
        #endregion

        #region 筛选出方案高手的数据
        public List<SuperiorModel> selectSuperiorData(List<User> lovePeoples)
        {
            List<SuperiorModel> result = new List<SuperiorModel>();
            foreach (var s in lovePeoples)
            {
                result.Add(new SuperiorModel
                {
                    userId = s.userId,
                    userImg = s.userImg,
                    userName = s.userName,
                    userSex = s.userSex,
                    userMerit = s.userMerit,
                    userMeritIntro = s.userMeritIntro,
                    userIntro = s.userIntro,
                    userSchool = s.userSchool,
                    userDepart = s.userDepart,
                    userRank = Convert.ToInt64(s.userRank),
                    userVisitedNum = Convert.ToInt32(s.userVisitedNum)
                });
            }
            return result;
        }

        public SuperiorModel selectUserData(User user)
        {
            SuperiorModel result = new SuperiorModel();
            result.userId = user.userId;
            result.userImg = user.userImg;
            result.userName = user.userName;
            result.userMeritIntro = user.userMeritIntro;
            result.userMerit = user.userMerit;
            result.userSchool = user.userSchool;
            result.userDepart = user.userDepart;
            result.userSex = user.userSex;
            result.userRank = Convert.ToInt64(user.userRank);
            result.userFans = Convert.ToInt32(user.userFans);
            return result;
        }
        #endregion

        public List<RankUserModel> selectRankUserData(IQueryable<User> rankUser)
        {
            int num = 0;
            List<RankUserModel> result = new List<RankUserModel>();
            foreach (var r in rankUser)
            {
                num++;
                result.Add(new RankUserModel
                {
                    userId = r.userId,
                    userImg = r.userImg,
                    userName = r.userName,
                    rankNum = num,
                    userRank = Convert.ToInt64(r.userRank),
                    userFans = Convert.ToInt32(r.userFans),
                    userProjectNum = Convert.ToInt32(r.userProjectNum),
                    userVitality = Convert.ToInt32(r.userVitality)
                });
            }
            return result;
        } 
        #endregion

        #region 方案提供总排行
        public IQueryable<User> getRanklistByProjectNum()
        {
            return _userRepository.Get(null, 1, 12, o => o.userProjectNum, false).AsQueryable();
        } 
        #endregion

        #region 活跃度声望总排行
        public IQueryable<User> getRanklistByVitality()
        {
            return _userRepository.Get(null, 1, 12, o => o.userVitality, false).AsQueryable();
        } 
        #endregion

        #region 获取大家庭成员信息
        public List<User> getFamilys()
        {
            Random rad = new Random();//实例化随机数产生器rad；
            int num = getAllFamilysCount();
            double groups = Math.Ceiling(Convert.ToDouble(num) / 30);
            int value = rad.Next(1, Convert.ToInt32(groups) + 1);
            return _userRepository.Get(o => o.userMerit != "普通大学森", value, 30, o => o.userRank, false).ToList();
        } 
        #endregion

        #region 获取方案高手的数量
        public int getAllFamilysCount()
        {
            return _userRepository.Get(o => o.userMerit != "普通大学森").Count();
        } 
        #endregion
    }
}
