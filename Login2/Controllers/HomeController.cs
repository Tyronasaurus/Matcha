using System;
using System.Collections.Generic;
using Login2.Models;
using System.Data.SqlClient;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Login2.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {

                if (Session["Username"] == null)
                {
                    Session["Username"] = User.Identity.Name;
                }
                getLocation();
                ConnectionStatus(true, Session["Username"].ToString());
            }
            return View();
        }

        public ActionResult Browse()
        {
            SqlDataReader reader = null;
            SqlConnection cn = null;
            SqlCommand cmd = null;
            var userProfile = new Profile();
            var ProfList = new List<Profile>();
            ProfileController profcontroller = new ProfileController();

            userProfile = profcontroller.ReturnProfile(Session["Username"].ToString());
            System.Diagnostics.Debug.WriteLine("Inside Browse Action");

            try
            {
                string _sql = @"SELECT * FROM Profile WHERE username != @username AND sexPref = @sexPref";
                cn = new SqlConnection(Constants.ConnString);
                cmd = new SqlCommand(_sql, cn);
                cmd.Parameters.AddWithValue("@username", userProfile.Username);
                cmd.Parameters.AddWithValue("@sexPref", userProfile.SexPref);
                cn.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string username = reader["username"].ToString();
                    ProfList.Add(profcontroller.ReturnProfile(username));
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
            finally 
            {
                if (ProfList != null)
                {
                    ViewBag.ProfileList = ProfList;
                }
                else
                {
                    ViewBag.ProfileList = null;
                    ViewBag.ErrorMEssage = "No users";
                }
                cn.Close();
            }
            return View();
        }

        [HttpPost]
        public ActionResult Browse(string Filter)
        {
            return (View());
        }

        public void getLocation()
        {
            string location;
            using (var webClient = new System.Net.WebClient())
            {
                var data = webClient.DownloadString("https://geoip-db.com/json");
                JavaScriptSerializer jss = new JavaScriptSerializer();
                var d = jss.Deserialize<dynamic>(data);

                //string country_name = d["country_name"];
                string city = d["city"];
                string state = d["state"];
                //string ipv4 = d["IPv4"];
                location = city + ":" + state;
            }
            if (location != null)
            {
                updateLocation(location);
            }
        }

        public void updateLocation(string location)
        {
            SqlConnection cn = null;
            SqlCommand cmd = null;

            string username = Session["Username"].ToString();
            try
            {
                cn = new SqlConnection(Constants.ConnString);
                string _sql = @"UPDATE Profile SET location = @location WHERE username = @username";
                cmd = new SqlCommand(_sql, cn);
                cmd.Parameters.AddWithValue("@location", location);
                cmd.Parameters.AddWithValue("@username", username);
                cn.Open();
                cmd.ExecuteReader();
                System.Diagnostics.Debug.WriteLine("Successfully updated user location for " + username);
                cn.Close();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
        }

        public void ConnectionStatus(bool IO, string Username)
        {
            SqlConnection cn = null;
            SqlCommand cmd = null;
            string _sql = null;

            if (IO == true)
                _sql = @"UPDATE Profile SET online = 1 WHERE username = @username";
            else if (IO == false)
                _sql = @"UPDATE Profile SET online = 0 WHERE username = @username";

            try
            {
                cn = new SqlConnection(Constants.ConnString);
                cmd = new SqlCommand(_sql, cn);
                cmd.Parameters.AddWithValue("@username", Username);
                cn.Open();
                cmd.ExecuteReader();
                System.Diagnostics.Debug.WriteLine("Set online to " + IO);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
            
        }
    }
}