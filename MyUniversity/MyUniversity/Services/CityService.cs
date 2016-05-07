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
    public class CityService : ICityService
    {
        public ICityRepository _cityRepository { get; private set; }

        public CityService(ICityRepository cityRepository)
        {
            this._cityRepository = cityRepository;
        }

        public List<City> getCityByCityName(string cityName)
        {
            return _cityRepository.Get(o => o.cityName == cityName).ToList();
        }
    }
}
