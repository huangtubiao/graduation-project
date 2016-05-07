using MyUniversity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyUniversity.Services.Interface
{
    public interface IHotSuperiorService
    {
        List<hotSuperior> getHotSuperior();

        int checkIfHadSearch(string word);

        hotSuperior getHotSuperiorByWrite(string word);

        bool updateHotSuperior(hotSuperior h);

        bool addHotSuperior(hotSuperior h);
    }
}
