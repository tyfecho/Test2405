using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Test2405.Models;

namespace Test2405.Controllers
{
    public class MainController : Controller
    {
        // GET: /Main/
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Dashboard()
        {
            return View();
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
            return View();
        }

        public ActionResult DeleteNotification(int id)
        {
            string connectionString = @"Data Source = localhost; Initial Catalog = LoginDatabase; Integrated Security = True;";
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                SqlCommand StrQuer = new SqlCommand("DELETE FROM [Notification] WHERE ID = @id", sqlCon);
                SqlParameter pID = new SqlParameter("@id", Convert.ToString(id));
                StrQuer.Parameters.Add(pID);
                SqlDataReader dr = StrQuer.ExecuteReader();
                dr.Close();
                sqlCon.Close();
            }
            return RedirectToAction("Notifications", "Main");
        }

        public ActionResult Notifications()
        {
            var NotificationList = new List<NotificationModel>();
            NotificationList = retrieveNotificationData("Notifications");
            return View(NotificationList);
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
                            NotificationID = dr.GetInt32(0),
                            NotificationPosted = dr.GetString(1),
                            NotificationPlatform = dr.GetString(2),
                            NotificationMsg = dr.GetString(3),
                            NotificationStatus = dr.GetString(4),
                            NotificationBy = dr.GetString(5),
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
            var OptionModelList = new List<OptionModel>();
            OptionModelList = retrieveOptionData("ApplicationSettings");
            return View(OptionModelList);
        }
        public IActionResult ApplicationUpdates()
        {
            var OptionModelList = new List<OptionModel>();
            OptionModelList = retrieveOptionData("ApplicationUpdates");
            return View(OptionModelList);
        }
    }

}