using MyUniversity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyUniversity.Services.Interface
{
    public interface ISuperiorLetterService
    {
        List<SuperiorLetter> getSuperiorLettersByUserId(long usrId);
        List<SuperiorLetter> getSuperiorLetterByUserIdUnread(long userId);
        IQueryable<SuperiorLetter> getMSSuperiorLettersByUserId(long userId, string userSchool);

        SuperiorLetter getSuperiorLettersBySuidRuid(long ReceiveUId, long SendUId);
        SuperiorLetter getSuperiorLettersBySuidRuidC(long ReceiveUId, long SendUId);
        SuperiorLetter getSuperiorLettersById(long superiorLetterId);

        bool updateSuperiorLetters(List<SuperiorLetter> superiorLetters);
        bool addSuperiorLetter(SuperiorLetter superiorLetter);
        bool updateSperiorLetter(SuperiorLetter superiorLetter);
    }
}
