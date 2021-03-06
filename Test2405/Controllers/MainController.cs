﻿using System;
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
                var announcementList = new List<AnnouncementModel>();
                announcementList = retrieveAnnouncementData();

                return View(announcementList);
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

        public ActionResult SaveAnnouncement(AnnouncementModel announcementModel)
        {
            string connectionString = @"Data Source = localhost; Initial Catalog = LoginDatabase; Integrated Security = True;";
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                SqlCommand StrQuer = new SqlCommand("INSERT INTO [Announcement] (ID,created_On,send_On,stop_On,Status,Type,Message,updated_On,posted_By)" +
                                                        "values(NEWID(),@createdOn,@sendOn,@stopOn,@status,@type,@message,@updatedOn,@postedBy)", sqlCon);
                SqlParameter pCreatedOn = new SqlParameter("@createdOn", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.ff"));
                SqlParameter pSendOn = new SqlParameter("@sendOn",announcementModel.AnnouncementSendOn);
                SqlParameter pStopOn = new SqlParameter("@stopOn", announcementModel.AnnouncementStopOn);
                SqlParameter pStatus = new SqlParameter("@status", "new");
                SqlParameter pType = new SqlParameter("@type", announcementModel.AnnouncementType);
                SqlParameter pMessage = new SqlParameter("@message", announcementModel.AnnouncementMessage);
                SqlParameter pUpdatedOn = new SqlParameter("@updatedOn", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.ff"));

                var tmp = new Byte[20];
                HttpContext.Session.TryGetValue("Username", out tmp);
                string name = System.Text.Encoding.UTF8.GetString(tmp);
                name = name.Replace("\0", "");
                SqlParameter postedBy = new SqlParameter("@postedBy", name);

                StrQuer.Parameters.Add(pCreatedOn);
                StrQuer.Parameters.Add(pSendOn);
                StrQuer.Parameters.Add(pStopOn);
                StrQuer.Parameters.Add(pStatus);
                StrQuer.Parameters.Add(pType);
                StrQuer.Parameters.Add(pMessage);
                StrQuer.Parameters.Add(pUpdatedOn);
                StrQuer.Parameters.Add(postedBy);


                SqlDataReader dr = StrQuer.ExecuteReader();
                dr.Close();
                sqlCon.Close();
            }
            return RedirectToAction("Announcements_Logs", "Main");
        }

        public List<AnnouncementModel> retrieveAnnouncementData()
        {
            var AnnouncementModelList = new List<AnnouncementModel>();
            string connectionString = @"Data Source = localhost; Initial Catalog = LoginDatabase; Integrated Security = True;";
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                SqlCommand StrQuer = new SqlCommand("SELECT * FROM [Announcement]", sqlCon);

                SqlDataReader dr = StrQuer.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        AnnouncementModel temp = new AnnouncementModel()
                        {
                            AnnouncementID = dr.GetGuid(0),
                            AnnouncementCreatedOn = dr.GetString(1),
                            AnnouncementSendOn = dr.GetString(2),
                            AnnouncementStopOn = dr.GetString(3),
                            AnnouncementStatus = dr.GetString(4),
                            AnnouncementType = (AnnouncementType)Convert.ToInt16(dr.GetString(5)),
                            AnnouncementMessage = dr.GetString(6),
                            AnnouncementBy = dr.GetString(8)
                        };

                        //temp.AnnouncementBy = temp.AnnouncementBy.Replace("\0", "");
                        AnnouncementModelList.Add(temp);
                    }
                }
                dr.Close();
            }
            return AnnouncementModelList;
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
            return RedirectToAction(pageName, "Main");
        }



        [HttpPost]
        public ActionResult SubmitUpdates(List<CombinedOptionModel> combinedOptionModel, string pageName)
        {
            string connectionString = @"Data Source = localhost; Initial Catalog = LoginDatabase; Integrated Security = True;";
            string DataLog = "";
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
               

                for (var i = 0; i < 5; i++)
                {
                    SqlCommand StrQuer = new SqlCommand("UPDATE [Option] SET option_value = @value WHERE option_name = @name", sqlCon);

                    String valString = "";
                    String optionName = "";
                    if (i == 0) {
                        optionName = "File";
                        valString = combinedOptionModel[0].CombinedFile;
                    }
                    else if (i == 1) {
                        optionName = "Version";
                        valString = combinedOptionModel[0].CombinedVersion;
                    }
                    else if (i == 2) {
                        optionName = "Change Description";
                        valString = combinedOptionModel[0].CombinedDescription;
                    }
                    else if (i == 3) {
                        optionName = "Size";
                        valString = combinedOptionModel[0].CombinedSize;
                    }
                    else if (i == 4) {
                        optionName = "Publish On";
                        valString = combinedOptionModel[0].CombinedPublishedOn;
                    }

                    SqlParameter pValue = new SqlParameter("@value", valString);
                    DataLog += "ID" + i + ":" + valString + ";";
                    StrQuer.Parameters.Add(pValue);
                    
                   
                    SqlParameter pName = new SqlParameter("@name", optionName);
                    StrQuer.Parameters.Add(pName);
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
            return RedirectToAction(pageName, "Main");
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
                var combinedModelList = new List<CombinedOptionModel>();
                OptionModelList = retrieveOptionData("ApplicationUpdates");

                CombinedOptionModel combinedOptionModel = new CombinedOptionModel()
                {
                    CombinedFile = OptionModelList[0].OptionValueString,
                    CombinedVersion = OptionModelList[1].OptionValueString,
                    CombinedDescription = OptionModelList[2].OptionValueString,
                    CombinedSize = OptionModelList[3].OptionValueString,
                    CombinedPublishedOn = OptionModelList[4].OptionValueDate
                };


                combinedModelList = retrieveChangeLog("ApplicationUpdates");
                combinedModelList.Insert(0, combinedOptionModel);
                return View(combinedModelList);
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

                var tmp = new Byte[20];
                HttpContext.Session.TryGetValue("Username", out tmp);
                string name = System.Text.Encoding.UTF8.GetString(tmp);
                name = name.Replace("\0", "");
                SqlParameter pNotificationPostedBy = new SqlParameter("@postedby", name);

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

        public List<CombinedOptionModel> retrieveChangeLog(string pageName)
        {
            List<CombinedOptionModel> combinedOptionModelsList = new List<CombinedOptionModel>();

            string connectionString = @"Data Source = localhost; Initial Catalog = LoginDatabase; Integrated Security = True;";
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                SqlCommand StrQuer = new SqlCommand("SELECT * FROM [ChangeLog] WHERE Page = @Pagename", sqlCon);
                SqlParameter pPagename = new SqlParameter("@Pagename", pageName);
                StrQuer.Parameters.Add(pPagename);
                SqlDataReader dr = StrQuer.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        String ChangeLog = dr.GetString(2);
                        //  Get Array of strings, split by ;
                        String[] arrayString = ChangeLog.Split(';');

                        //temp.AnnouncementBy = temp.AnnouncementBy.Replace("\0", "");

                        CombinedOptionModel combinedOptionModel = new CombinedOptionModel()
                        {
                            CombinedFile = arrayString[0].Replace("ID0:",""),
                            CombinedVersion = arrayString[1].Replace("ID1:", ""),
                            CombinedDescription = arrayString[2].Replace("ID2:", ""),
                            CombinedSize = arrayString[3].Replace("ID3:", ""),
                            CombinedPublishedOn = arrayString[4].Replace("ID4:", ""),
                        };

                        combinedOptionModelsList.Add(combinedOptionModel);

                    }
                }
                dr.Close();
            }

            return combinedOptionModelsList;
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