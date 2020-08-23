using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace BMotionReporting.Logic
{
    public class InfoLogic
    {

        private static string fcm = ConfigurationManager.AppSettings["FCM"];
        public static String SendNotificationFromFirebaseCloud()
        {
            var result = "-1";
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(fcm);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Headers.Add(HttpRequestHeader.Authorization, "key=AAAAFPBBRHU:APA91bHACuno9RdWBYmQeg5_kVfgNOM3pEM5hzRW6g9J1AQkCDYTPUAnVEkDT7FszF5XX8DFch-t_yj99SOGU6BO16PjEIZoEK_lDT4t-z90ASlIlQBQzAcBzHYTX24netn994kt8QLA");
            httpWebRequest.Method = "POST";
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string strNJson = @"{
                    ""to"": ""/topics/ServiceNow"",
                    ""data"": {
                        ""ShortDesc"": ""Some short desc"",
                        ""IncidentNo"": ""any number"",
                        ""Description"": ""detail desc""
  },
  ""notification"": {
                ""title"": ""ServiceNow: Incident No. number"",
    ""text"": ""This is Notification"",
""sound"":""default""
  }
        }";
                streamWriter.Write(strNJson);
                streamWriter.Flush();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();
            }
            return result;
        }
    }
}