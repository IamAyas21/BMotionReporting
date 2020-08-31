using BMotionReporting.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BMotionReporting.Models
{
    public class HomeModels
    {
        public List<sp_HomeTotalFuel_Result> Fuel { get; set; }
        public List<sp_HomeUserPengguna_Result> UserPengguna { get; set; }
        public sp_Notification_Result Pengumuman { get; set; }
    }
    
}