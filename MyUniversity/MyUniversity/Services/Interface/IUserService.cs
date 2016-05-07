using MyUniversity.Models;
using MyUniversity.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyUniversity.Services.Interface
{
    public interface IUserService 
    {
        bool register(User user);
        bool login(LoginModel user);
        bool updateUser(User user);

        bool isNotUser(LoginModel user);

        User getUserByAccount(string userAccount);
        User getUserById(long userId);

        IQueryable<User> getIUserById(long userId);
        IQueryable<User> getAllSuperiors(int groupNumber);

        List<User> getMySchoolSuperiorsByDepart(string userDepart);
        List<User> getAllSuperiorsByDepart(string userDepart);
        //List<User> getLoveBoyPeoplesByDepart(string userDepart);

        List<User> getMySchoolSuperiorsByWrite(string searchWrite);
        IQueryable<User> getAllSuperiorsByWrite(string searchWrite);

        List<User> getMySchoolSuperiorsByGetData(getDataModel getDataModel);

        List<User> getAllSuperiorsByGetData(getDataModel getDataModel);

        //List<User> getMostLoveBoyPeoplesByGetData(getDataModel getDataModel);
        List<User> getMySchoolSuperiorsByDepartWrite(string userDepart, string searchWrite);
        List<User> getAllSuperiorsByDepartWrite(string userDepart, string searchWrite);
        //List<User> getLoveBoyPeoplesByDepartWrite(string userDepart, string searchWrite);
        List<User> getWeekExpUser();

        void setLoginUser(string userAccount, long userId, string userSchool);

        List<User> getMySchoolSuperiors(int groupNumber, string userDepartment, string userSchool);
        //List<User> getMostLoveGirlPeoples(int groupNumber, string userDepartment);
        //List<User> getMostLoveBoyPeoples(int groupNumber, string userDepartment);
        List<User> getFinishPlanUser();

        List<SuperiorModel> selectWeekExpUser(List<User> user);
        List<SuperiorModel> selectSuperiorData(List<User> lovePeoples);
        SuperiorModel selectUserData(User user);
        List<RankUserModel> selectRankUserData(IQueryable<User> rankUser);

        List<string> getSuperiorByWrite(string searchText);

        int getTotalGroups(string userSchool);
        int getMSSuperiorsGroupsByDepart(string userDepart);
        int getTotalGroupsByDepart(string userDepart);
        int getTotalGroupsByWrite(string searchWrite);
        int getMySuperiorGroupsByWrite(string searchWrite);
        int getMSSuperiorsGroupsByDepartWrite(string userDepart, string searchWrite);
        int getTotalGroupsByDepartWrite(string userDepart, string searchWrite);
        int getTotalGroupsBySchool(string userSchool);
        int getTotalGroupsBySchoolDepart(string userSchool, string userDepart);
        int getTotalGroupsBySchoolWrite(string searchWrite, string userSchool);
        int getTotalGroupsByWriteSchoolDepart(string searchWrite, string userSchool, string userDepart);

        IQueryable<User> getRanklistByProjectNum();
        IQueryable<User> getRanklistByVitality();

        List<User> getAllSuperiorsBySchool(string userSchool);
        List<User> getAllSuperiorsBySchoolDepart(string userSchool, string userDepartment);
        List<User> getAllSuperiorsByWriteDepart(string searchWrite, string userDepartment);
        List<User> getAllSuperiorsByWriteSchool(string searchWrite, string userSchool);
        List<User> getAllSuperiorsByWriteSchoolDepart(string searchWrite, string userSchool, string userDepartment);

        List<User> getFamilys();
    }
}
