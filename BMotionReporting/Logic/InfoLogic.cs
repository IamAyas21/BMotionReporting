using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using RestSharp;
using BMotionReporting.Entity;

namespace BMotionReporting.Logic
{
    public class InfoLogic
    {

        BMotionDBEntities db = new BMotionDBEntities();
        static InfoLogic _InfoLogic = null;

        public static InfoLogic getInstance()
        {
            if (_InfoLogic == null)
            {
                _InfoLogic = new InfoLogic();
                return _InfoLogic;
            }
            else
            {
                return _InfoLogic;
            }
        }

        public List<sp_Notification_Result> getAllInfo()
        {
            return db.sp_Notification().ToList();
        }

        public void Add(sp_Notification_Result model)
        {
            try
            {
                var result = db.sp_NotificationInsert(model.Message, model.Title);
                var client = new RestClient("https://fcm.googleapis.com/fcm/send");
                //var client = new RestClient("https://fcm.googleapis.com//v1/projects/bmotion/messages:send");
                var request = new RestRequest(Method.POST);
                request.AddHeader("postman-token", "e656c729-9be1-8499-67db-270369945eb7");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/json");
                request.AddHeader("authorization", "key=AAAAFPBBRHU:APA91bHACuno9RdWBYmQeg5_kVfgNOM3pEM5hzRW6g9J1AQkCDYTPUAnVEkDT7FszF5XX8DFch-t_yj99SOGU6BO16PjEIZoEK_lDT4t-z90ASlIlQBQzAcBzHYTX24netn994kt8QLA");
                string JSON_Post = "{\r\n\"to\":\"/topics/ServiceNow\",\r\n \"notification\" : {\r\n  \"sound\" : \"default\",\r\n  \"body\" :  \""+model.Title+"\",\r\n  \"title\" : \"B-Motions Info\",\r\n  \"content_available\" : true,\r\n  \"priority\" : \"high\"\r\n },\r\n \"data\" : {\r\n  \"sound\" : \"default\",\r\n  \"body\" :  \"test body\",\r\n  \"title\" : \"test title\",\r\n  \"content_available\" : true,\r\n  \"priority\" : \"high\"\r\n }\r\n}";
                request.AddParameter("application/json", JSON_Post, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        //private static string fcm = ConfigurationManager.AppSettings["FCM"];
        //public static String SendNotificationFromFirebaseCloud()
        //{
        //    var client = new RestClient("https://fcm.googleapis.com/fcm/send");
        //    //var client = new RestClient("https://fcm.googleapis.com//v1/projects/bmotion/messages:send");
        //    var request = new RestRequest(Method.POST);
        //    request.AddHeader("postman-token", "e656c729-9be1-8499-67db-270369945eb7");
        //    request.AddHeader("cache-control", "no-cache");
        //    request.AddHeader("content-type", "application/json");
        //    request.AddHeader("authorization", "key=AAAAFPBBRHU:APA91bHACuno9RdWBYmQeg5_kVfgNOM3pEM5hzRW6g9J1AQkCDYTPUAnVEkDT7FszF5XX8DFch-t_yj99SOGU6BO16PjEIZoEK_lDT4t-z90ASlIlQBQzAcBzHYTX24netn994kt8QLA");
        //    string JSON_Post = "{\r\n\"to\":\"/topics/ServiceNow\",\r\n \"notification\" : {\r\n  \"sound\" : \"default\",\r\n  \"body\" :  \"Pesan Melalui Controller (InfoLogic.cs)\",\r\n  \"title\" : \"Test Services\",\r\n  \"content_available\" : true,\r\n  \"priority\" : \"high\"\r\n },\r\n \"data\" : {\r\n  \"sound\" : \"default\",\r\n  \"body\" :  \"test body\",\r\n  \"title\" : \"test title\",\r\n  \"content_available\" : true,\r\n  \"priority\" : \"high\"\r\n }\r\n}";
        //    request.AddParameter("application/json", JSON_Post, ParameterType.RequestBody);
        //    IRestResponse response = client.Execute(request);

        //    return "ok";
        //}
    }
}