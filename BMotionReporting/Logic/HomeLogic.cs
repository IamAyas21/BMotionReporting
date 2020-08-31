using BMotionReporting.Entity;
using BMotionReporting.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BMotionReporting.Logic
{
    public class HomeLogic
    {
        BMotionDBEntities db = new BMotionDBEntities();
        static HomeLogic _FuelLogic = null;

        public static HomeLogic getInstance()
        {
            if (_FuelLogic == null)
            {
                _FuelLogic = new HomeLogic();
                return _FuelLogic;
            }
            else
            {
                return _FuelLogic;
            }
        }

        public HomeModels GetDashboard()
        {
            HomeModels data = new HomeModels();
            data.Fuel = GetTotalFuel();
            data.UserPengguna = GetUserPengguna();
            data.Pengumuman = GetLastPengumuman();
            return data;
        }

        public List<sp_HomeTotalFuel_Result> GetTotalFuel()
        {
            var list = db.sp_HomeTotalFuel("").ToList();
            return list;
        }

        public List<sp_HomeUserPengguna_Result> GetUserPengguna()
        {
            var list = db.sp_HomeUserPengguna("").ToList();
            return list;
        }

        public sp_Notification_Result GetLastPengumuman()
        {
            var list = db.sp_Notification().ToList();
            return list.FirstOrDefault();
        }
    }
}