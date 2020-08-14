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
    public class UsersController : Controller
    {
        // GET: Users
        //[CheckAuthorizeAttribute(Roles = "Users")]
        [CheckAuthorizeAttribute()]
        public ActionResult Index()
        {
            var model = new UserModels();
            List<UserModels> list = new List<UserModels>();
            var page = new PagedList<UserModels>();
            try
            {
                var userList = UserLogic.getInstance().getAllUsers();
                foreach(var item in userList)
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
                if(UserLogic.getInstance().CheckExistingUser(model))
                {
                    UserLogic.getInstance().Add(model);
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
                model.DocumentList = Document(id);
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
                UserLogic.getInstance().Verification(model.NIP);
                TempData["Success"] = "Success verification for " + model.NIP;
            }
            catch (Exception e)
            {
                TempData["Error"] = "Error verification for " + model.NIP;
                Logging.Log.getInstance().CreateLogError(e);
            }

            return RedirectToAction("Detail", "Users", new { id = model.NIP });
        }

        private PagedList<DocumentModels> Document(string id)
        { 
            List<DocumentModels> list = DocumentLogic.getInstance().getAllDocumentByUser(id);
            var page = new PagedList<DocumentModels>();
            try
            {
                page.Content = list;
            }
            catch (Exception e)
            {
                Logging.Log.getInstance().CreateLogError(e);
            }
            return page;
        }

        [HttpPost]
        public ActionResult Verification(string[] arrValue)
        {
            try
            {
                DocumentLogic.getInstance().Verification(arrValue[0]);
                TempData["Success"] = "Success verification for " + arrValue[0];
            }
            catch (Exception e)
            {
                TempData["Error"] = "Error verification for " + arrValue[0];
                Logging.Log.getInstance().CreateLogError(e);
            }

            return RedirectToAction("Detail", "Users", new { id = arrValue[1] });
        }
    }
}