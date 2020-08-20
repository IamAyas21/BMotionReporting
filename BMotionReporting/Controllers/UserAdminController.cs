using BMotionReporting.App_Start;
using BMotionReporting.Logic;
using BMotionReporting.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BMotionReporting.Controllers
{
    public class UserAdminController : Controller
    {
        // GET: UserAdmin
        [CheckAuthorizeAttribute()]
        public ActionResult Index()
        {
            var model = new UserModels();
            List<UserModels> list = new List<UserModels>();
            var page = new PagedList<UserModels>();
            try
            {
                var userList = UserLogic.getInstance().getAllUserAdmin();
                foreach (var item in userList)
                {
                    model = new UserModels();
                    model.NIP = item.NIP;
                    model.Name = item.Name;
                    model.Profesi = item.Profession;
                    model.Email = item.Email;
                    model.Telp = item.Phone;
                    list.Add(model);
                }
                page.Content = list;
            }
            catch (Exception e)
            {
                Logging.Log.getInstance().CreateLogError(e);
            }
            return View(page);
        }

        [CheckAuthorizeAttribute()]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(UserModels model)
        {
            try
            {
                if (UserLogic.getInstance().CheckExistingUser(model))
                {
                    UserLogic.getInstance().AddUserAdmin(model);
                    TempData["Success"] = "Success saving Data for " + model.Name;
                }
                else
                {
                    TempData["Error"] = "User " + model.Name + " is exist";
                }
            }
            catch (Exception e)
            {
                Logging.Log.getInstance().CreateLogError(e);
            }

            return RedirectToAction("Index");
        }

        [CheckAuthorizeAttribute()]
        public ActionResult Detail(string id)
        {
            UserModels model = new UserModels();
            try
            {
                model = UserLogic.getInstance().GetUserById(id);
            }
            catch (Exception e)
            {
                Logging.Log.getInstance().CreateLogError(e);
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Detail(UserModels model)
        {
            try
            {
                UserLogic.getInstance().Update_UserAdmin(model);
                TempData["Success"] = "Success verification for " + model.Email;
            }
            catch (Exception e)
            {
                TempData["Error"] = "Error verification for " + model.Email;
                Logging.Log.getInstance().CreateLogError(e);
            }

            return RedirectToAction("Index", "UserAdmin", new { id = model.Email });
        }
    }
}