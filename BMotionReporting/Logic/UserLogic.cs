using BMotionReporting.Entity;
using BMotionReporting.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;


using static System.Net.Mime.MediaTypeNames;

namespace BMotionReporting.Logic
{
    public class UserLogic
    {
        private static string pathUpload = ConfigurationManager.AppSettings["PathUploads"];

        BMotionDBEntities db = new BMotionDBEntities();
        List<User> userList;
        static UserLogic usrRgs = null;
        public static UserLogic getInstance()
        {
            if (usrRgs == null)
            {
                usrRgs = new UserLogic();
                return usrRgs;
            }
            else
            {
                return usrRgs;
            }
        }

        public List<sp_UserPengguna_Result> GetAllUsers()
        {
            var list = db.sp_UserPengguna("").ToList();
            return list;
        }

        public List<User> getAllUserAdmin()
        {
            return userList = db.Users.Where(u => u.IsAdmin == "Y").ToList();
        }

        public UserModels GetUserById(string id)
        {
            UserModels usr = new UserModels();
            var list = db.Users.Where(us => us.NIP.Equals(id)).ToList();
            foreach (var item in list)
            {
                usr.NIP = item.NIP == null?"":item.NIP;
                usr.Email = item.Email == null?"":item.Email;
                usr.IsVerify = item.IsVerify==null?"":item.IsVerify;
                usr.Name = item.Name == null?"":item.Name;
                usr.Password = item.Password==null?"":item.Password;
                usr.Profesi = item.Profession==null?"":item.Profession;
                usr.Telp = item.Phone==null?"":item.Phone;
            }
            return usr;
        }

        public bool CheckExistingUser(UserModels model)
        {
            if (db.Users.Where(usr => usr.NIP.Equals(model.NIP)).Count() == 0 && db.Users.Where(usr => usr.Email.Equals(model.Email)).Count() == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Add(UserModels model)
        {
            try
            {
                string dateTimeDayNow = DateTime.Now.ToString("ddMMyyyyHHmmss");
                string dateDayNow = DateTime.Now.ToString("ddMMyyyy");

                db = new BMotionDBEntities();
                User userEntity = new User();
                userEntity.NIP = model.NIP;
                userEntity.Email = model.Email;
                userEntity.Name = model.Name;
                userEntity.Phone = model.Telp;
                userEntity.KTP = model.KTP.FileName;
                userEntity.Password = model.Password;
                userEntity.CreatedDate = DateTime.Now;
                userEntity.CreatedBy = SessionManager.NIP();
                userEntity.IsVerify = model.IsVerify;
                userEntity.Profession = model.Profesi;
                db.Users.Add(userEntity);
                db.SaveChanges();

                var pathImgKtp = Path.Combine(HttpContext.Current.Server.MapPath(pathUpload), dateDayNow, "KTP");

                if (!Directory.Exists(pathImgKtp))
                {
                    System.IO.Directory.CreateDirectory(pathImgKtp);
                }
                using (var fileStream = new System.IO.FileStream(pathImgKtp + "\\" + model.KTP.FileName, System.IO.FileMode.Create, System.IO.FileAccess.Write))
                {
                    model.KTP.InputStream.CopyTo(fileStream);
                }
                if (model.PDF != null)
                {
                    var pathFilePdf = Path.Combine(HttpContext.Current.Server.MapPath(pathUpload), dateDayNow, "Document");

                    db = new BMotionDBEntities();
                    Document docEntity = new Document();
                    docEntity.DocumentNo = "Doc_" + dateTimeDayNow;
                    docEntity.NIP = model.NIP;
                    docEntity.Quota = model.Quota;
                    docEntity.DocumentFile = model.PDF.FileName;
                    docEntity.ExpDate = Convert.ToDateTime(model.ExpiredDate);
                    docEntity.CreatedDate = DateTime.Now;
                    docEntity.CreatedBy = SessionManager.NIP();
                    db.Documents.Add(docEntity);
                    db.SaveChanges();

                    if (!Directory.Exists(pathFilePdf))
                    {
                        System.IO.Directory.CreateDirectory(pathFilePdf);
                    }
                    using (var fileStream = new System.IO.FileStream(pathFilePdf + "\\" + model.PDF.FileName, System.IO.FileMode.Create, System.IO.FileAccess.Write))
                    {
                        model.PDF.InputStream.CopyTo(fileStream);
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void AddUserAdmin(UserModels model)
        {
            try
            {
                string dateTimeDayNow = DateTime.Now.ToString("ddMMyyyyHHmmss");
                string dateDayNow = DateTime.Now.ToString("ddMMyyyy");

                db = new BMotionDBEntities();
                User userEntity = new User();

                userEntity.NIP = dateTimeDayNow;
                userEntity.Email = model.Email;
                userEntity.Name = model.Name;
                userEntity.Password = model.Password;
                userEntity.CreatedDate = DateTime.Now;
                userEntity.CreatedBy = SessionManager.NIP();
                userEntity.IsAdmin = "Y";
                db.Users.Add(userEntity);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Verification(UserModels model)
        {
            try
            {
                db = new BMotionDBEntities();
                User usr = (from u in db.Users
                                where u.NIP.Equals(model.NIP)
                                select u).First();
                usr.IsVerify = "Y";
                usr.Password = model.Password;
                db.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void UpdateUserAdmin(UserModels model)
        {
            try
            {
                db = new BMotionDBEntities();
                User usr = (from u in db.Users
                            where u.NIP.Equals(model.NIP)
                            select u).First();
                usr.IsVerify = "Y";
                usr.Name = model.Name;
                usr.Password = model.Password;
                db.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

      
    }
}