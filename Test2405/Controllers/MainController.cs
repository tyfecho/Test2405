using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Test2405.Models;
using System.Web;

namespace Test2405.Controllers
{
    public class MainController : Controller
    {
        // GET: /Main/
        public IActionResult Index()
        {
           return View();
        }

        public Boolean SessionCheck()
        {
            var tmp = new Byte[20];
            HttpContext.Session.TryGetValue("Username", out tmp);
            if (tmp == null) return false;
            else return true;
        }

        public IActionResult Dashboard()
        {
            if(SessionCheck()) return View();
            else return RedirectToAction("Index", "Main");
        }

        public ActionResult ContentTemplates()
        {
            return View();
        }

        public ActionResult Contents()
        {
            return View();
        }

        public ActionResult ContentsChangeLog()
        {
            return View();
        }

        public ActionResult Notifications_New()
        {
            if (SessionCheck()) return View();
            else return RedirectToAction("Index", "Main");
        }

        public ActionResult UpdateNotification(Test2405.Models.NotificationModel notificationModel, Guid id, Boolean apnsIOS, Boolean gcmAndroid)
        {
            string connectionString = @"Data Source = localhost; Initial Catalog = LoginDatabase; Integrated Security = True;";
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                //SqlCommand StrQuer = new SqlCommand("UPDATE [Notification] (Platform,send_On,Icon,Notifications,action_Activity,Expiry,Priority,updated_On)"+
                //                                    "values(@platform,@sendOn,@icon,@notifications,@action_Activity,@expiry,@priority,@updateOn) WHERE ID = @id", sqlCon);

               SqlCommand StrQuer = new SqlCommand("UPDATE [Notification] SET Platform = @platform," +
                                                    "send_On = @sendOn, Icon = @icon, Notifications = @notifications," +
                                                    "action_Activity = @action_Activity, Expiry = @expiry, Priority = @priority, updated_On = @updatedOn WHERE ID = @id", sqlCon);
                string platformString = "Null";
                if (apnsIOS == true && gcmAndroid == true) platformString = "APNS(iOS)/GCM(Android)";
                else if (apnsIOS == true) platformString = "APNS(iOS)";
                else if (gcmAndroid == true) platformString = "GCM(Android)";
                
                SqlParameter pPlatform = new SqlParameter("@platform", platformString);
                SqlParameter pSendOn = new SqlParameter("@sendOn", notificationModel.NotificationSendOn);
                SqlParameter pIcon = new SqlParameter("@icon", notificationModel.NotificationIcon);
                SqlParameter pNotification = new SqlParameter("@notifications", notificationModel.NotificationMsg);
                SqlParameter pActionActivity = new SqlParameter("@action_Activity", notificationModel.NotificationActionActivity);
                SqlParameter pExpiry = new SqlParameter("@expiry", notificationModel.NotificationExpiry);
                SqlParameter pPriority = new SqlParameter("@priority", notificationModel.NotificationPriority);
                SqlParameter pUpdatedOn = new SqlParameter("@updatedOn", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.ff"));

                SqlParameter pID = new SqlParameter("@id", id);
                
                StrQuer.Parameters.Add(pID);
                StrQuer.Parameters.Add(pPlatform);
                StrQuer.Parameters.Add(pSendOn);
                StrQuer.Parameters.Add(pIcon);
                StrQuer.Parameters.Add(pNotification);
                StrQuer.Parameters.Add(pActionActivity);
                StrQuer.Parameters.Add(pExpiry);
                StrQuer.Parameters.Add(pPriority);
                StrQuer.Parameters.Add(pUpdatedOn);

                SqlDataReader dr = StrQuer.ExecuteReader();
                dr.Close();
                sqlCon.Close();
            }
            return RedirectToAction("Notifications", "Main");
        }

        public ActionResult DeleteNotification(Guid id)
        {
            string connectionString = @"Data Source = localhost; Initial Catalog = LoginDatabase; Integrated Security = True;";
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                SqlCommand StrQuer = new SqlCommand("UPDATE [Notification] SET STATUS = @status WHERE ID = @id", sqlCon);
                SqlParameter pID = new SqlParameter("@id", id);
                SqlParameter pStatus = new SqlParameter("@status", "deleted");
                StrQuer.Parameters.Add(pID);
                StrQuer.Parameters.Add(pStatus);
                SqlDataReader dr = StrQuer.ExecuteReader();
                dr.Close();
                sqlCon.Close();
            }
            return RedirectToAction("Notifications", "Main");
        }

        public ActionResult Notifications()
        {
            if (SessionCheck())
            {
                var NotificationList = new List<NotificationModel>();
                NotificationList = retrieveNotificationData("Notifications");

                return View(NotificationList);
            }
            else return RedirectToAction("Index", "Main");
        }

        public ActionResult Announcements_Logs()
        {
            if (SessionCheck())
            {
                var NotificationList = new List<NotificationModel>();
                NotificationList = retrieveNotificationData("Announcements_Logs");

                return View(NotificationList);
            }
            else return RedirectToAction("Index", "Main");
        }

        public ActionResult Announcements()
        {
            if (SessionCheck())
            {
                //var NotificationList = new List<NotificationModel>();
                //NotificationList = retrieveNotificationData("Announcements_Logs");

                return View();
            }
            else return RedirectToAction("Index", "Main");
        }

        public ActionResult Notifications_Edit(Guid id)
        {
            if (SessionCheck())
            {
                var NotificationSingle = new NotificationModel();
                NotificationSingle = retrieveNotificationSingular(id);
                return View(NotificationSingle);
            }
            else return RedirectToAction("Index", "Main");
        }

        public List<NotificationModel> retrieveNotificationData(string pageName)
        {
            var NotificationModelList = new List<NotificationModel>();
            string connectionString = @"Data Source = localhost; Initial Catalog = LoginDatabase; Integrated Security = True;";
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                SqlCommand StrQuer = new SqlCommand("SELECT * FROM [Notification]", sqlCon);
               
                SqlDataReader dr = StrQuer.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        NotificationModel temp = new NotificationModel()
                        {
                            NotificationID = dr.GetGuid(0),
                            NotificationSendOn = dr.GetString(1),
                            NotificationPlatform = dr.GetString(2),
                            NotificationMsg = dr.GetString(3),
                            NotificationStatus = dr.GetString(4),
                            NotificationBy = dr.GetString(5),
                            NotificationCreatedOn = dr.GetString(10)
                        };

                        NotificationModelList.Add(temp);
                    }
                }
                dr.Close();
            }
            return NotificationModelList;
        }

        [HttpPost]
        public ActionResult SubmitSettings(List<OptionModel> optionModelList, string pageName)
        {
            string connectionString = @"Data Source = localhost; Initial Catalog = LoginDatabase; Integrated Security = True;";
            string DataLog = "";
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                Console.WriteLine(pageName);
                for ( var i = 0; i < optionModelList.Count(); i++ )
                {
                    SqlCommand StrQuer = new SqlCommand("UPDATE [Option] SET option_value = @value WHERE option_name = @name", sqlCon);
                    if (optionModelList[i].OptionType == "boolean")
                    {
                        SqlParameter pValue = new SqlParameter("@value",optionModelList[i].OptionValueBool.ToString());

                        DataLog += "ID" + i + ":" + optionModelList[i].OptionValueBool.ToString() + ";";
                        StrQuer.Parameters.Add(pValue);
                    }
                    else if (optionModelList[i].OptionType == "string")
                    {
                        SqlParameter pValue = new SqlParameter("@value", optionModelList[i].OptionValueString);
                        DataLog += "ID" + i + ":" + optionModelList[i].OptionValueString + ";";
                        StrQuer.Parameters.Add(pValue);
                    }
                    else if (optionModelList[i].OptionType == "calender")
                    {
                        //string dateTime2String = optionModelList[i].OptionValueDate.ToString("yyyy-MM-ddTHH:mm:ss.fffffff");
                        SqlParameter pValue = new SqlParameter("@value", optionModelList[i].OptionValueDate);
                        DataLog += "ID" + i + ":" + optionModelList[i].OptionValueDate + ";";
                        StrQuer.Parameters.Add(pValue);
                    }
                    SqlParameter pID = new SqlParameter("@name", optionModelList[i].OptionName);
                    StrQuer.Parameters.Add(pID);
                    SqlDataReader dr = StrQuer.ExecuteReader();
                    dr.Close();
                }
                //  Key in LogData into ChangeLog
                SqlCommand StrQuerLog = new SqlCommand("INSERT INTO [ChangeLog] (LogID,Time,LogData,Page) values(NEWID(),@time,@logdata,@page)", sqlCon);
                SqlParameter pTime = new SqlParameter("@time", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffffff"));
                SqlParameter pDataLog = new SqlParameter("@logdata", DataLog);
                SqlParameter pPage = new SqlParameter("@page", pageName);
                StrQuerLog.Parameters.Add(pTime);
                StrQuerLog.Parameters.Add(pDataLog);
                StrQuerLog.Parameters.Add(pPage);
                SqlDataReader dr2 = StrQuerLog.ExecuteReader();
                dr2.Close();

                sqlCon.Close();

            }
            return RedirectToAction("ApplicationSettings", "Main");
        }

        public List<OptionModel> retrieveOptionData(string pageName)
        {
            var OptionModelList = new List<OptionModel>();
            string connectionString = @"Data Source = localhost; Initial Catalog = LoginDatabase; Integrated Security = True;";
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                Console.WriteLine("Connected");
                sqlCon.Open();
                SqlCommand StrQuer = new SqlCommand("SELECT * FROM [Option] WHERE option_page = @Pagename", sqlCon);
                SqlParameter pPagename = new SqlParameter("@Pagename", pageName);
                StrQuer.Parameters.Add(pPagename);
                SqlDataReader dr = StrQuer.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        OptionModel temp = new OptionModel()
                        {
                            OptionId = dr.GetInt32(0),
                            OptionName = dr.GetString(1),
                            OptionType = dr.GetString(2),
                            OptionValueString = null,
                            OptionValueBool = false,
                            OptionValueDate = null,
                        };
                        if (dr.GetString(2) == "boolean")
                        {
                            if (dr.GetString(3) == "True") temp.OptionValueBool = true;
                        }
                        else if (dr.GetString(2) == "string")
                            temp.OptionValueString = dr.GetString(3);
                        else if (dr.GetString(2) == "calender")
                        {
                            DateTime tempTime = Convert.ToDateTime(dr.GetString(3)); 
                            string timeString = tempTime.ToString("yyyy-MM-ddTHH:mm:ss");
                            temp.OptionValueDate = timeString;
                        }
                       
                        OptionModelList.Add(temp);
                    }
                }
                dr.Close();
            }
            return OptionModelList;
        }

        public ActionResult ApplicationSettings()
        {
            if (SessionCheck())
            {
                var OptionModelList = new List<OptionModel>();
                OptionModelList = retrieveOptionData("ApplicationSettings");
                return View(OptionModelList);
            }
            else return RedirectToAction("Index", "Main");
        }
        public ActionResult ApplicationUpdates()
        {
            if (SessionCheck())
            {
                var OptionModelList = new List<OptionModel>();
                OptionModelList = retrieveOptionData("ApplicationUpdates");
                return View(OptionModelList);
            }
            else return RedirectToAction("Index", "Main");
        }

        //  notification_new.cshtml
        public ActionResult SaveNotification(Test2405.Models.NotificationModel notificationModel,Boolean apnsIOS, Boolean gcmAndroid)
        {
            string platformString = "";
            string connectionString = @"Data Source = localhost; Initial Catalog = LoginDatabase; Integrated Security = True;";
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                Console.WriteLine("Connected");
                sqlCon.Open();
                SqlCommand StrQuer = new SqlCommand("INSERT INTO [Notification] (ID,send_On,Platform,Notifications,Status,posted_By,action_Activity,Expiry,Priority,Icon,created_On)" +
                                                        "values(NEWID(),@sendon,@platform,@message,@status,@postedby,@action_activity,@expiry,@priority,@icon,@created_on)", sqlCon);
                SqlParameter pNotificationSendOn = new SqlParameter("@sendon", notificationModel.NotificationSendOn);
                if (apnsIOS == true) platformString += "APNS(iOS)";
                if (apnsIOS == true && gcmAndroid == true) platformString += "/GCM(Android)";
                else if (gcmAndroid == true) platformString += "GCM(Android)";

                SqlParameter pNotificationPlatform = new SqlParameter("@platform", platformString);
                SqlParameter pNotificationMsg = new SqlParameter("@message", notificationModel.NotificationMsg);
                SqlParameter pNotificationStatus = new SqlParameter("@status", "new");
                SqlParameter pNotificationPostedBy = new SqlParameter("@postedby", "admin");
                SqlParameter pNotificationActionActivity = new SqlParameter("@action_activity", notificationModel.NotificationActionActivity);
                SqlParameter pNotificationExpiry = new SqlParameter("@expiry",notificationModel.NotificationExpiry);
                SqlParameter pNotificationPriority = new SqlParameter("@priority", notificationModel.NotificationPriority);
                SqlParameter pNotificationIcon = new SqlParameter("@icon", notificationModel.NotificationIcon);
                SqlParameter pCreatedOn = new SqlParameter("@created_on", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.ff"));

                StrQuer.Parameters.Add(pNotificationSendOn);
                StrQuer.Parameters.Add(pNotificationPlatform);
                StrQuer.Parameters.Add(pNotificationMsg);
                StrQuer.Parameters.Add(pNotificationStatus);
                StrQuer.Parameters.Add(pNotificationPostedBy);
                StrQuer.Parameters.Add(pNotificationActionActivity);
                StrQuer.Parameters.Add(pNotificationExpiry);
                StrQuer.Parameters.Add(pNotificationPriority);
                StrQuer.Parameters.Add(pNotificationIcon);
                StrQuer.Parameters.Add(pCreatedOn);

                SqlDataReader dr = StrQuer.ExecuteReader();
                dr.Close();
                sqlCon.Close();

            }
            return RedirectToAction("Notifications", "Main");
        }

        public NotificationModel retrieveNotificationSingular(Guid pID)
        {
            NotificationModel temp = new NotificationModel();
            string connectionString = @"Data Source = localhost; Initial Catalog = LoginDatabase; Integrated Security = True;";
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                SqlCommand StrQuer = new SqlCommand("SELECT * FROM [Notification] WHERE ID = @id", sqlCon);
                SqlParameter newID = new SqlParameter("@id", pID);
                StrQuer.Parameters.Add(newID);
                SqlDataReader dr = StrQuer.ExecuteReader();
                if ( dr.HasRows )
                {
                    if (dr.Read())
                    {
                        temp = new NotificationModel()
                        {
                            NotificationID = dr.GetGuid(0),
                            NotificationSendOn = dr.GetString(1),
                            NotificationPlatform = dr.GetString(2),
                            NotificationMsg = dr.GetString(3),
                            NotificationStatus = dr.GetString(4),
                            NotificationBy = dr.GetString(5),
                            NotificationActionActivity = dr.GetString(6),
                            NotificationExpiry = dr.GetString(7),
                            NotificationPriority = dr.GetString(8),
                            NotificationIcon = dr.GetString(9),
                            NotificationCreatedOn = dr.GetString(10)
                        };
                    }
                }
                dr.Close();
                sqlCon.Close();
            }

            return temp;

        }
    }

}