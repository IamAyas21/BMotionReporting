using BMotionReporting.App_Start;
using BMotionReporting.Entity;
using BMotionReporting.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BMotionReporting.Controllers
{
    public class FeedbackController : Controller
    {
        BMotionDBEntities db = new BMotionDBEntities();
        // GET: Feedback
        [CheckAuthorizeAttribute()]
        public ActionResult Index()
        {
            List<sp_Feedback_Result> Data = db.sp_Feedback().ToList();
            PagedList<sp_Feedback_Result> page = new PagedList<sp_Feedback_Result>();
            page.Content = Data;
            return View(page);
        }
    }
}