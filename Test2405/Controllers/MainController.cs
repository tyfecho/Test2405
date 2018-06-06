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

        [HttpPost]
        public ActionResult SubmitSettings(List<OptionModel> optionModelList)
        {
            string connectionString = @"Data Source = localhost; Initial Catalog = LoginDatabase; Integrated Security = True;";
            string DataLog = "";
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                Console.WriteLine(optionModelList);
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
                        string dateTime2String = optionModelList[i].OptionValueDate.ToString("yyyy-MM-ddTHH:mm:ss.fffffff");
                        SqlParameter pValue = new SqlParameter("@value", dateTime2String);
                        DataLog += "ID" + i + ":" + dateTime2String + ";";
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
                SqlParameter pPage = new SqlParameter("@page", "ApplicationSettings");
                StrQuerLog.Parameters.Add(pTime);
                StrQuerLog.Parameters.Add(pDataLog);
                StrQuerLog.Parameters.Add(pPage);
                SqlDataReader dr2 = StrQuerLog.ExecuteReader();
                dr2.Close();

                sqlCon.Close();

            }
            return RedirectToAction("ApplicationSettings", "Main");
        }

        public ActionResult ApplicationSettings()
        {
            var OptionModelList = new List<OptionModel>();
            string connectionString = @"Data Source = localhost; Initial Catalog = LoginDatabase; Integrated Security = True;";
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                Console.WriteLine("Connected");
                sqlCon.Open();
                SqlCommand StrQuer = new SqlCommand("SELECT * FROM [Option] WHERE option_page = @Pagename", sqlCon);
                SqlParameter pPagename = new SqlParameter("@Pagename", "ApplicationSettings");
                StrQuer.Parameters.Add(pPagename);
                SqlDataReader dr = StrQuer.ExecuteReader();
                if ( dr.HasRows )
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
                            OptionValueDate = DateTime.MinValue
                        };
                        if (dr.GetString(2) == "boolean")
                        {
                            if (dr.GetString(3) == "True") temp.OptionValueBool = true;
                        }
                        else if (dr.GetString(2) == "string")
                            temp.OptionValueString = dr.GetString(3);
                        else if (dr.GetString(2) == "calender")
                            temp.OptionValueDate = DateTime.Parse(dr.GetString(3));
                        OptionModelList.Add(temp);
                    }
                }
                return View(OptionModelList);
            }
        }
        public IActionResult ApplicationUpdates()
        {
            return View();
        }
    }

}