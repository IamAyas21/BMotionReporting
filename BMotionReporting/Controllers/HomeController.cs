﻿using BMotionReporting.App_Start;
using BMotionReporting.Logic;
using BMotionReporting.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BMotionReporting.Controllers
{
    public class HomeController : Controller
    {
        [CheckAuthorizeAttribute()]
        public ActionResult Index()
        {
            HomeModels data = new HomeModels();
            data = HomeLogic.getInstance().GetDashboard();
            return View(data);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}